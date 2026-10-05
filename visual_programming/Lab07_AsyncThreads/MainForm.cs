// ============================================================================
// Практична робота 7. Асинхронне програмування та багатопотоковість у Windows Forms
// Дисципліна: Інструментальні засоби візуального програмування (Костіков О.А.)
// Студент: ТАРАС Вадим, група аІк43
// Основна логіка багатопотоковості та керування UI (MainForm.cs)
// ============================================================================

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab07_AsyncThreads;

/// <summary>
/// Модель звіту про прогрес операції, яка передається між фоновим та інтерфейсним потоками.
/// </summary>
public record TaskProgressReport(
    int ProcessedItems,
    int TotalItems,
    string ItemDescription,
    int WorkerThreadId,
    double ExecutionSpeedItemsPerSec
);

public partial class MainForm : Form
{
    // Ідентифікатор головного UI-потоку (Single-Threaded Apartment)
    private readonly int _uiThreadId;

    // Джерело токена скасування для асинхронних задач (Task)
    private CancellationTokenSource? _cancellationTokenSource;

    // Примітив синхронізації для паузи та продовження виконання (ManualResetEventSlim)
    private readonly ManualResetEventSlim _pauseEvent = new(true);

    // Секундомір для розрахунку витраченого часу та прогнозу завершення (ETA)
    private readonly Stopwatch _stopwatch = new();

    // Компонент класичного підходу багатопотоковості
    private BackgroundWorker? _backgroundWorker;

    // Прапорці стану життєвого циклу задачі
    private bool _isRunning = false;
    private bool _isPaused = false;

    // Лічильник кліків для демонстрації чуйності інтерфейсу (UI Responsiveness)
    private int _uiClickCounter = 0;

    public MainForm()
    {
        InitializeComponent();

        // Фіксуємо ID UI-потоку в момент конструювання форми
        _uiThreadId = Environment.CurrentManagedThreadId;
        statusThreadInfo.Text = $"UI ManagedThreadId: #{_uiThreadId}";
        lblCurrentThreadInfo.Text = $"Потоки: UI = Потік #{_uiThreadId} | Робочий = --";

        // Прив'язка обробників подій керування
        btnStart.Click += async (s, e) => await OnStartClickedAsync();
        btnPause.Click += OnPauseClicked;
        btnCancel.Click += OnCancelClicked;
        btnClearLog.Click += OnClearLogClicked;
        btnTestUi.Click += OnTestUiClicked;

        Log($"Головне вікно ініціалізовано. UI-потік #{_uiThreadId} готовий до роботи.");
    }

    #region Керування чуйністю інтерфейсу (Клік-тест)

    /// <summary>
    /// Обробник натискання демонстраційної кнопки перевірки чуйності UI.
    /// Якщо UI заблокований синхронною операцією, кліки не будуть оброблятися!
    /// При асинхронній роботі лічильник миттєво інкрементується (60+ FPS).
    /// </summary>
    private void OnTestUiClicked(object? sender, EventArgs e)
    {
        _uiClickCounter++;
        lblUiClickCount.Text = $"Кліків: {_uiClickCounter} (UI чуйний і активний!)";
        lblUiClickCount.ForeColor = Color.DarkGreen;
    }

    #endregion

    #region Централізоване логування подій

    /// <summary>
    /// Безпечний міжпотоковий запис події до журналу (ListBox).
    /// Перевіряє властивість InvokeRequired. Якщо виклик прийшов з фонового потоку,
    /// маршалить операцію в чергу повідомлень UI-потоку через BeginInvoke.
    /// </summary>
    /// <param name="message">Текст повідомлення для реєстрації</param>
    private void Log(string message)
    {
        if (InvokeRequired)
        {
            try
            {
                BeginInvoke(new Action(() => Log(message)));
            }
            catch (ObjectDisposedException)
            {
                // Форма закривається користувачем під час фонової роботи
            }
            return;
        }

        int currentThreadId = Environment.CurrentManagedThreadId;
        string threadType = (currentThreadId == _uiThreadId) ? "UI-потік" : "Робочий потік";
        string logEntry = $"[{DateTime.Now:HH:mm:ss.fff}] [#{currentThreadId,2} {threadType,-13}] {message}";

        lstLog.Items.Add(logEntry);
        lstLog.TopIndex = lstLog.Items.Count - 1;
    }

    private void OnClearLogClicked(object? sender, EventArgs e)
    {
        lstLog.Items.Clear();
        Log("Журнал операцій очищено користувачем.");
    }

    #endregion

    #region Запуск операцій за обраною технологією

    private async Task OnStartClickedAsync()
    {
        if (_isRunning) return;

        // Зчитування вхідних параметрів задачі
        int totalItems = (int)numTotalItems.Value;
        int delayMs = (int)numDelayMs.Value;
        bool simulateHeavyWork = chkSimulateHeavyWork.Checked;

        // Скидання елементів візуалізації
        progressBar.Value = 0;
        lblPercentage.Text = "0 %";
        lblProcessedCount.Text = $"Оброблено: 0 з {totalItems} ітемів";
        lblElapsedTime.Text = "Час: 00:00:00.000";
        lblEta.Text = "Оцінка завершення (ETA): Розрахунок...";
        lblStatusMessage.Text = "Статус: Виконується обробка...";
        lblStatusMessage.ForeColor = Color.FromArgb(0, 102, 204);

        // Налаштування прапорців стану
        _isRunning = true;
        _isPaused = false;
        _pauseEvent.Set(); // Дозволяємо виконання
        SetControlsState(isRunning: true);

        _stopwatch.Restart();

        try
        {
            if (radAsyncAwait.Checked)
            {
                Log($"--- ЗАПУСК: Сучасний підхід (async/await + Task.Run + Progress<T>) ---");
                await ExecuteAsyncAwaitTask(totalItems, delayMs, simulateHeavyWork);
            }
            else if (radBackgroundWorker.Checked)
            {
                Log($"--- ЗАПУСК: Класичний підхід (компонент BackgroundWorker) ---");
                ExecuteBackgroundWorker(totalItems, delayMs, simulateHeavyWork);
            }
            else if (radDirectInvoke.Checked)
            {
                Log($"--- ЗАПУСК: Прямий міжпотоковий виклик (Task.Run + InvokeRequired / Invoke) ---");
                await ExecuteDirectInvokeTask(totalItems, delayMs, simulateHeavyWork);
            }
            else if (radSyncFreeze.Checked)
            {
                Log($"--- УВАГА! ЗАПУСК: Синхронний блокуючий виклик (Thread.Sleep у UI-потоці) ---");
                ExecuteSyncBlocking(totalItems, delayMs, simulateHeavyWork);
            }
        }
        catch (OperationCanceledException)
        {
            Log("Операцію перервано токеном скасування (OperationCanceledException).");
            OnTaskCancelled();
        }
        catch (Exception ex)
        {
            Log($"Критична помилка виконання: {ex.Message}");
            MessageBox.Show($"Помилка: {ex.Message}", "Помилка обробки", MessageBoxButtons.OK, MessageBoxIcon.Error);
            OnTaskCompleted(success: false);
        }
    }

    #endregion

    #region Режим 1: Сучасний підхід (async/await + Task.Run + Progress<T>)

    /// <summary>
    /// Сучасний підхід: асинхронне делегування пулу потоків через Task.Run.
    /// Екземпляр Progress<T> фіксує SynchronizationContext UI-потоку та гарантує,
    /// що зворотні виклики методу Report оновлюють візуальні компоненти в UI-потоці!
    /// Скасування координується через CancellationTokenSource.
    /// </summary>
    private async Task ExecuteAsyncAwaitTask(int totalItems, int delayMs, bool simulateHeavy)
    {
        _cancellationTokenSource = new CancellationTokenSource();
        CancellationToken token = _cancellationTokenSource.Token;

        // Progress<T> синхронізується з контекстом форми Windows Forms
        var progressReporter = new Progress<TaskProgressReport>(report =>
        {
            UpdateProgressDisplay(report);
        });

        // Запуск тривалої фонової задачі в пулі потоків
        await Task.Run(() =>
        {
            int workerThreadId = Environment.CurrentManagedThreadId;
            Log($"[Task.Run] Фонова задача стартувала у потоці пулу #{workerThreadId}.");

            for (int i = 1; i <= totalItems; i++)
            {
                // 1. Очікування, якщо користувач натиснув 'Пауза'
                _pauseEvent.Wait(token);

                // 2. Перевірка запиту на скасування операції
                token.ThrowIfCancellationRequested();

                // 3. Імітація корисної роботи
                if (simulateHeavy)
                {
                    PerformCpuIntensiveWork();
                }
                Thread.Sleep(delayMs);

                // 4. Безпечний звіт про прогрес через інтерфейс IProgress<T>
                double speed = i / Math.Max(0.001, _stopwatch.Elapsed.TotalSeconds);
                ((IProgress<TaskProgressReport>)progressReporter).Report(new TaskProgressReport(
                    ProcessedItems: i,
                    TotalItems: totalItems,
                    ItemDescription: $"Обчислено пакет даних #{i:D4}",
                    WorkerThreadId: workerThreadId,
                    ExecutionSpeedItemsPerSec: speed
                ));
            }

            Log($"[Task.Run] Робочий потік #{workerThreadId} завершив усі {totalItems} ітерацій.");
        }, token);

        OnTaskCompleted(success: true);
    }

    #endregion

    #region Режим 2: Класичний підхід (BackgroundWorker)

    /// <summary>
    /// Класичний компонентний підхід (.NET 2.0+).
    /// Використовує подієву модель: DoWork (у фоні), ProgressChanged (у UI), RunWorkerCompleted (у UI).
    /// </summary>
    private void ExecuteBackgroundWorker(int totalItems, int delayMs, bool simulateHeavy)
    {
        _backgroundWorker = new BackgroundWorker
        {
            WorkerReportsProgress = true,
            WorkerSupportsCancellation = true
        };

        _backgroundWorker.DoWork += (s, args) =>
        {
            var worker = (BackgroundWorker)s!;
            int workerThreadId = Environment.CurrentManagedThreadId;
            Log($"[BackgroundWorker.DoWork] Запущено у фоновому потоці #{workerThreadId}.");

            for (int i = 1; i <= totalItems; i++)
            {
                // Пауза
                _pauseEvent.Wait();

                // Перевірка скасування через CancellationPending
                if (worker.CancellationPending)
                {
                    args.Cancel = true;
                    Log($"[BackgroundWorker] Зафіксовано запит CancellationPending на ітерації #{i}.");
                    return;
                }

                if (simulateHeavy)
                {
                    PerformCpuIntensiveWork();
                }
                Thread.Sleep(delayMs);

                int percent = (int)((double)i / totalItems * 100);
                double speed = i / Math.Max(0.001, _stopwatch.Elapsed.TotalSeconds);
                var report = new TaskProgressReport(i, totalItems, $"Пакет BackgroundWorker #{i:D4}", workerThreadId, speed);

                // Звіт про прогрес (автоматично маршалиться в UI-потік)
                worker.ReportProgress(percent, report);
            }
        };

        _backgroundWorker.ProgressChanged += (s, args) =>
        {
            // Обробник виконується у головному UI-потоці!
            if (args.UserState is TaskProgressReport report)
            {
                UpdateProgressDisplay(report);
            }
        };

        _backgroundWorker.RunWorkerCompleted += (s, args) =>
        {
            // Обробник виконується у головному UI-потоці!
            if (args.Cancelled)
            {
                Log("[BackgroundWorker] Операцію успішно скасовано через CancelAsync().");
                OnTaskCancelled();
            }
            else if (args.Error != null)
            {
                Log($"[BackgroundWorker] Помилка: {args.Error.Message}");
                OnTaskCompleted(success: false);
            }
            else
            {
                Log("[BackgroundWorker] Обробка завершена успішно.");
                OnTaskCompleted(success: true);
            }
        };

        _backgroundWorker.RunWorkerAsync();
    }

    #endregion

    #region Режим 3: Прямий міжпотоковий виклик (InvokeRequired / Invoke)

    /// <summary>
    /// Демонстрація прямої низькорівневої синхронізації без високорівневих обгорток.
    /// Робочий потік перевіряє Form.InvokeRequired і явно викликає Invoke / BeginInvoke.
    /// Це захищає від InvalidOperationException ("Cross-thread operation not valid").
    /// </summary>
    private async Task ExecuteDirectInvokeTask(int totalItems, int delayMs, bool simulateHeavy)
    {
        _cancellationTokenSource = new CancellationTokenSource();
        CancellationToken token = _cancellationTokenSource.Token;

        await Task.Run(() =>
        {
            int workerThreadId = Environment.CurrentManagedThreadId;
            Log($"[DirectInvoke] Старт обробки у фоновому потоці #{workerThreadId}.");

            for (int i = 1; i <= totalItems; i++)
            {
                _pauseEvent.Wait(token);
                token.ThrowIfCancellationRequested();

                if (simulateHeavy)
                {
                    PerformCpuIntensiveWork();
                }
                Thread.Sleep(delayMs);

                double speed = i / Math.Max(0.001, _stopwatch.Elapsed.TotalSeconds);
                var report = new TaskProgressReport(i, totalItems, $"Прямий маршалінг #{i:D4}", workerThreadId, speed);

                // Явна перевірка міжпотокового виклику:
                // Якщо викликати UpdateProgressDisplay(report) напряму без Invoke — 
                // середовище згенерує InvalidOperationException!
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new Action(() => UpdateProgressDisplay(report)));
                }
                else
                {
                    UpdateProgressDisplay(report);
                }
            }

            Log($"[DirectInvoke] Потік #{workerThreadId} завершив роботу.");
        }, token);

        OnTaskCompleted(success: true);
    }

    #endregion

    #region Режим 4: Синхронний блокуючий виклик (Thread.Sleep у UI — Демонстрація фрізу)

    /// <summary>
    /// Антипатерн розробки GUI: блокування черги повідомлень (Windows Message Loop)
    /// тривалою синхронною роботою прямо в головному UI-потоці.
    /// Застосунок переходить у стан "Not Responding" (Не відповідає). Клікер, кнопки
    /// та переміщення вікна блокуються системою до завершення повного циклу!
    /// </summary>
    private void ExecuteSyncBlocking(int totalItems, int delayMs, bool simulateHeavy)
    {
        Log("УВАГА: Головний UI-потік заблоковано! Спробуйте натиснути 'Клік-тест' або перемістити вікно...");
        lblStatusMessage.Text = "Статус: UI ЗАБЛОКОВАНО! (Форма не реагує на події)";
        lblStatusMessage.ForeColor = Color.Red;
        statusLabel.Text = "УВАГА: Черга повідомлень Windows заблокована синхронним циклом!";

        // Примусово перемальовуємо форму перед фрізом
        this.Refresh();

        for (int i = 1; i <= totalItems; i++)
        {
            if (simulateHeavy)
            {
                PerformCpuIntensiveWork();
            }

            // Блокуємо головний потік UI
            Thread.Sleep(delayMs);

            double speed = i / Math.Max(0.001, _stopwatch.Elapsed.TotalSeconds);
            var report = new TaskProgressReport(i, totalItems, $"Синхронний блок #{i:D4}", _uiThreadId, speed);

            // Оновлюємо значення в UI прямо (InvokeRequired == false)
            UpdateProgressDisplay(report);

            // Якщо НЕ викликати Application.DoEvents(), вікно навіть не буде перемальовуватись!
            // Але DoEvents є небезпечним "костилем", який порушує стек викликів.
            // Тут ми навмисно демонструємо чистий ефект зависання.
        }

        Log("Синхронний цикл завершено. UI-потік розблоковано та повернуто до нормального стану.");
        OnTaskCompleted(success: true);
    }

    #endregion

    #region Оновлення графічного інтерфейсу та розрахунок ETA

    /// <summary>
    /// Оновлення елементів візуалізації на основі отриманого звіту.
    /// Повинно виконуватися виключно в контексті UI-потоку.
    /// </summary>
    private void UpdateProgressDisplay(TaskProgressReport report)
    {
        int percent = (int)((double)report.ProcessedItems / report.TotalItems * 100);
        percent = Math.Clamp(percent, 0, 100);

        progressBar.Value = percent;
        lblPercentage.Text = $"{percent} %";
        lblProcessedCount.Text = $"Оброблено: {report.ProcessedItems} з {report.TotalItems} ітемів ({report.ItemDescription})";

        TimeSpan elapsed = _stopwatch.Elapsed;
        lblElapsedTime.Text = $"Час: {elapsed:hh\\:mm\\:ss\\.fff}";

        // Розрахунок часу до завершення (ETA - Estimated Time of Arrival)
        if (report.ProcessedItems > 0 && percent < 100)
        {
            double averageTimePerItemMs = elapsed.TotalMilliseconds / report.ProcessedItems;
            int remainingItems = report.TotalItems - report.ProcessedItems;
            double remainingMs = remainingItems * averageTimePerItemMs;
            TimeSpan eta = TimeSpan.FromMilliseconds(remainingMs);

            lblEta.Text = $"Оцінка завершення (ETA): ~{eta:mm\\:ss} ({report.ExecutionSpeedItemsPerSec:F1} оп/сек)";
        }
        else if (percent >= 100)
        {
            lblEta.Text = "Оцінка завершення (ETA): Завершено!";
        }

        lblCurrentThreadInfo.Text = $"Потоки: UI = Потік #{_uiThreadId} | Робочий = Потік #{report.WorkerThreadId}";
        statusLabel.Text = $"Обробка: {percent}% ({report.ProcessedItems}/{report.TotalItems}) у потоці #{report.WorkerThreadId}";
    }

    #endregion

    #region Керування Паузою, Скасуванням та завершенням

    private void OnPauseClicked(object? sender, EventArgs e)
    {
        if (!_isRunning) return;

        _isPaused = !_isPaused;

        if (_isPaused)
        {
            _pauseEvent.Reset(); // Блокує фоновий потік
            _stopwatch.Stop();
            btnPause.Text = "▶ Продовжити";
            btnPause.BackColor = Color.FromArgb(40, 167, 69);
            btnPause.ForeColor = Color.White;
            lblStatusMessage.Text = "Статус: Тимчасово призупинено (Пауза)";
            lblStatusMessage.ForeColor = Color.FromArgb(204, 102, 0);
            Log("Операцію поставлено на паузу користувачем.");
        }
        else
        {
            _pauseEvent.Set(); // Відновлює виконання
            _stopwatch.Start();
            btnPause.Text = "⏸ Пауза";
            btnPause.BackColor = Color.FromArgb(255, 193, 7);
            btnPause.ForeColor = Color.Black;
            lblStatusMessage.Text = "Статус: Виконується обробка...";
            lblStatusMessage.ForeColor = Color.FromArgb(0, 102, 204);
            Log("Виконання операції відновлено.");
        }
    }

    private void OnCancelClicked(object? sender, EventArgs e)
    {
        if (!_isRunning) return;

        Log("Отримано команду скасування. Надсилання сигналів зупинки потокам...");

        // Якщо потік був на паузі — знімаємо блокування, щоб він зміг прочитати токен!
        if (_isPaused)
        {
            _pauseEvent.Set();
        }

        // Скасування Task.Run
        _cancellationTokenSource?.Cancel();

        // Скасування BackgroundWorker
        if (_backgroundWorker != null && _backgroundWorker.IsBusy)
        {
            _backgroundWorker.CancelAsync();
        }
    }

    private void OnTaskCompleted(bool success)
    {
        _isRunning = false;
        _isPaused = false;
        _stopwatch.Stop();
        SetControlsState(isRunning: false);

        if (success)
        {
            progressBar.Value = 100;
            lblPercentage.Text = "100 %";
            lblStatusMessage.Text = "Статус: УСПІШНО ЗАВЕРШЕНО!";
            lblStatusMessage.ForeColor = Color.DarkGreen;
            statusLabel.Text = $"Операцію успішно виконано за {_stopwatch.Elapsed:mm\\:ss\\.fff}";
            Log($"Задачу повністю завершено за {_stopwatch.Elapsed:hh\\:mm\\:ss\\.fff}.");
        }
        else
        {
            lblStatusMessage.Text = "Статус: ЗАВЕРШЕНО З ПОМИЛКОЮ!";
            lblStatusMessage.ForeColor = Color.Red;
            statusLabel.Text = "Виконання перервано з помилкою.";
        }
    }

    private void OnTaskCancelled()
    {
        _isRunning = false;
        _isPaused = false;
        _stopwatch.Stop();
        SetControlsState(isRunning: false);

        lblStatusMessage.Text = "Статус: СКАСОВАНО КОРИСТУВАЧЕМ";
        lblStatusMessage.ForeColor = Color.DarkRed;
        statusLabel.Text = "Операція перервана користувачем.";
        Log("Операція переведена у фінальний стан: СКАСОВАНО.");
    }

    /// <summary>
    /// Блокування/розблокування елементів керування під час виконання
    /// </summary>
    private void SetControlsState(bool isRunning)
    {
        btnStart.Enabled = !isRunning;
        btnPause.Enabled = isRunning;
        btnCancel.Enabled = isRunning;
        grpExecutionMode.Enabled = !isRunning;
        grpTaskParameters.Enabled = !isRunning;

        if (!isRunning)
        {
            btnPause.Text = "⏸ Пауза";
            btnPause.BackColor = Color.FromArgb(255, 193, 7);
            btnPause.ForeColor = Color.Black;
        }
    }

    #endregion

    #region Допоміжні методи математичного навантаження CPU

    /// <summary>
    /// Імітація ресурсомістких математичних обчислень у потоці (наприклад, множення матриць).
    /// </summary>
    private static void PerformCpuIntensiveWork()
    {
        const int size = 45;
        double[,] a = new double[size, size];
        double[,] b = new double[size, size];
        double[,] c = new double[size, size];

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                a[i, j] = Math.Sin(i) * Math.Cos(j);
                b[i, j] = Math.Cos(i) * Math.Sin(j);
            }
        }

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                double sum = 0;
                for (int k = 0; k < size; k++)
                {
                    sum += a[i, k] * b[k, j];
                }
                c[i, j] = sum;
            }
        }
    }

    #endregion
}
