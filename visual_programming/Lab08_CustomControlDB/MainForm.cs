using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Lab08_CustomControlDB
{
    /// <summary>
    /// Головна форма додатку «Hardware Telemetry Dashboard & Custom Controls».
    /// Демонструє динамічне розміщення та налаштування компонентів SensorCardControl,
    /// обробку власних подій AlertTriggered, персистентне збереження телеметрії через DatabaseService,
    /// фільтрацію вибірок у DataGridView та експорт звітів.
    /// </summary>
    public partial class MainForm : Form
    {
        private readonly DatabaseService _dbService;
        private readonly Random _random = new();

        // Екземпляри користувацьких компонентів UserControl
        private SensorCardControl _cardCpuTemp = null!;
        private SensorCardControl _cardRamUsage = null!;
        private SensorCardControl _cardNetLoad = null!;
        private SensorCardControl _cardPressure = null!;

        // Поточні модельовані базові значення
        private double _simCpuTemp = 48.0;
        private double _simRamUsage = 14.5;
        private double _simNetLoad = 240.0;
        private double _simPressure = 2.4;

        private int _totalAlertCount = 0;

        public MainForm()
        {
            InitializeComponent();
            _dbService = new DatabaseService();

            InitializeSensorCards();
            ConfigureDataGrid();
            PopulateFilterControls();
            BindEventHandlers();
            RefreshDataGrid();
            UpdateStatusBar();
        }

        /// <summary>
        /// Програмне створення та налаштування екземплярів власного компонента SensorCardControl.
        /// Демонструє повторне використання UserControl з різними конфігураційними параметрами.
        /// </summary>
        private void InitializeSensorCards()
        {
            flowCardsPanel.SuspendLayout();

            // 1. Датчик температури процесора CPU
            _cardCpuTemp = new SensorCardControl
            {
                SensorName = "Температура CPU",
                Unit = "°C",
                MinValue = 0.0,
                MaxValue = 105.0,
                MinThreshold = 20.0,
                MaxThreshold = 78.0,
                CardAccentColor = Color.FromArgb(239, 68, 68),
                Margin = new Padding(6)
            };
            _cardCpuTemp.UpdateTelemetry(_simCpuTemp);
            _cardCpuTemp.AlertTriggered += OnSensorAlertTriggered;

            // 2. Датчик оперативної пам'яті RAM
            _cardRamUsage = new SensorCardControl
            {
                SensorName = "Використання RAM",
                Unit = "ГБ",
                MinValue = 0.0,
                MaxValue = 32.0,
                MinThreshold = 2.0,
                MaxThreshold = 27.5,
                CardAccentColor = Color.FromArgb(16, 185, 129),
                Margin = new Padding(6)
            };
            _cardRamUsage.UpdateTelemetry(_simRamUsage);
            _cardRamUsage.AlertTriggered += OnSensorAlertTriggered;

            // 3. Датчик навантаження мережі
            _cardNetLoad = new SensorCardControl
            {
                SensorName = "Трафік мережі",
                Unit = "Мбіт/с",
                MinValue = 0.0,
                MaxValue = 1000.0,
                MinThreshold = 5.0,
                MaxThreshold = 820.0,
                CardAccentColor = Color.FromArgb(59, 130, 246),
                Margin = new Padding(6)
            };
            _cardNetLoad.UpdateTelemetry(_simNetLoad);
            _cardNetLoad.AlertTriggered += OnSensorAlertTriggered;

            // 4. Датчик тиску контуру охолодження
            _cardPressure = new SensorCardControl
            {
                SensorName = "Тиск охолодження",
                Unit = "Бар",
                MinValue = 0.0,
                MaxValue = 6.0,
                MinThreshold = 1.2,
                MaxThreshold = 4.2,
                CardAccentColor = Color.FromArgb(168, 85, 247),
                Margin = new Padding(6)
            };
            _cardPressure.UpdateTelemetry(_simPressure);
            _cardPressure.AlertTriggered += OnSensorAlertTriggered;

            // Додавання до панелі відображення
            flowCardsPanel.Controls.Add(_cardCpuTemp);
            flowCardsPanel.Controls.Add(_cardRamUsage);
            flowCardsPanel.Controls.Add(_cardNetLoad);
            flowCardsPanel.Controls.Add(_cardPressure);

            flowCardsPanel.ResumeLayout();
        }

        /// <summary>
        /// Конфігурація колонок та форматування таблиці DataGridView для відображення історії телеметрії.
        /// </summary>
        private void ConfigureDataGrid()
        {
            dgvTelemetry.Columns.Clear();
            dgvTelemetry.AutoGenerateColumns = false;

            dgvTelemetry.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Id",
                HeaderText = "ID",
                Width = 60,
                SortMode = DataGridViewColumnSortMode.Automatic
            });

            dgvTelemetry.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "FormattedTimestamp",
                HeaderText = "Час фіксації",
                Width = 150,
                SortMode = DataGridViewColumnSortMode.Automatic
            });

            dgvTelemetry.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "SensorName",
                HeaderText = "Назва сенсора",
                Width = 180,
                SortMode = DataGridViewColumnSortMode.Automatic
            });

            dgvTelemetry.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "FormattedValue",
                HeaderText = "Показник",
                Width = 110,
                SortMode = DataGridViewColumnSortMode.Automatic
            });

            dgvTelemetry.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "StatusDisplay",
                HeaderText = "Статус",
                Width = 100,
                SortMode = DataGridViewColumnSortMode.Automatic
            });

            dgvTelemetry.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Message",
                HeaderText = "Повідомлення / Діагностика",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                SortMode = DataGridViewColumnSortMode.Automatic
            });

            // Підсвічування рядків з аварійними записами
            dgvTelemetry.RowPrePaint += (s, e) =>
            {
                if (e.RowIndex < 0 || e.RowIndex >= dgvTelemetry.Rows.Count) return;
                var row = dgvTelemetry.Rows[e.RowIndex];
                if (row.DataBoundItem is TelemetryRecord record)
                {
                    if (record.Status == SensorStatus.Critical)
                    {
                        row.DefaultCellStyle.BackColor = Color.FromArgb(254, 226, 226);
                        row.DefaultCellStyle.ForeColor = Color.FromArgb(153, 27, 27);
                    }
                    else if (record.Status == SensorStatus.Warning)
                    {
                        row.DefaultCellStyle.BackColor = Color.FromArgb(254, 243, 199);
                        row.DefaultCellStyle.ForeColor = Color.FromArgb(146, 64, 14);
                    }
                    else
                    {
                        row.DefaultCellStyle.BackColor = Color.White;
                        row.DefaultCellStyle.ForeColor = Color.FromArgb(30, 41, 59);
                    }
                }
            };
        }

        private void PopulateFilterControls()
        {
            cmbFilterSensor.Items.Clear();
            cmbFilterSensor.Items.Add("Усі сенсори");
            cmbFilterSensor.Items.Add("Температура CPU");
            cmbFilterSensor.Items.Add("Використання RAM");
            cmbFilterSensor.Items.Add("Трафік мережі");
            cmbFilterSensor.Items.Add("Тиск охолодження");
            cmbFilterSensor.SelectedIndex = 0;

            cmbFilterStatus.SelectedIndex = 0;
        }

        private void BindEventHandlers()
        {
            btnStartSimulation.Click += BtnStartSimulation_Click;
            btnStep.Click += BtnStep_Click;
            btnSimulateSpike.Click += BtnSimulateSpike_Click;
            numInterval.ValueChanged += (s, e) => simulationTimer.Interval = (int)numInterval.Value;
            simulationTimer.Tick += SimulationTimer_Tick;

            btnApplyFilter.Click += (s, e) => ApplyFilters();
            btnResetFilter.Click += (s, e) => ResetFilters();
            btnExportCsv.Click += BtnExportCsv_Click;
            btnClearDb.Click += BtnClearDb_Click;
            btnClearLog.Click += (s, e) => { txtAlertLog.Clear(); _totalAlertCount = 0; UpdateStatusBar(); };
        }

        #region Керування симуляцією надходження телеметрії

        private void BtnStartSimulation_Click(object? sender, EventArgs e)
        {
            if (simulationTimer.Enabled)
            {
                simulationTimer.Stop();
                btnStartSimulation.Text = "▶ Запустити збір";
                btnStartSimulation.BackColor = Color.FromArgb(22, 163, 74);
                statusSimulationLabel.Text = "Статус: Зупинено";
            }
            else
            {
                simulationTimer.Start();
                btnStartSimulation.Text = "⏸ Зупинити збір";
                btnStartSimulation.BackColor = Color.FromArgb(217, 119, 6);
                statusSimulationLabel.Text = "Статус: Активний збір телеметрії";
            }
        }

        private void BtnStep_Click(object? sender, EventArgs e)
        {
            GenerateTelemetryCycle();
        }

        private void BtnSimulateSpike_Click(object? sender, EventArgs e)
        {
            // Штучне навантаження та генерація критичної аварії по CPU та мережі
            _simCpuTemp = 86.4;
            _simNetLoad = 910.0;
            _simPressure = 4.8;
            GenerateTelemetryCycle();
        }

        private void SimulationTimer_Tick(object? sender, EventArgs e)
        {
            // Моделювання поступового випадкового коливання (Random Walk)
            _simCpuTemp += (_random.NextDouble() - 0.48) * 3.5;
            _simCpuTemp = Math.Max(25.0, Math.Min(95.0, _simCpuTemp));

            _simRamUsage += (_random.NextDouble() - 0.48) * 1.2;
            _simRamUsage = Math.Max(4.0, Math.Min(31.5, _simRamUsage));

            _simNetLoad += (_random.NextDouble() - 0.49) * 45.0;
            _simNetLoad = Math.Max(15.0, Math.Min(980.0, _simNetLoad));

            _simPressure += (_random.NextDouble() - 0.50) * 0.25;
            _simPressure = Math.Max(0.8, Math.Min(5.5, _simPressure));

            GenerateTelemetryCycle();
        }

        /// <summary>
        /// Виконання одного такту збору телеметрії: оновлення карток та запис у базу даних.
        /// </summary>
        private void GenerateTelemetryCycle()
        {
            // Оновлюємо картки
            _cardCpuTemp.UpdateTelemetry(_simCpuTemp);
            _cardRamUsage.UpdateTelemetry(_simRamUsage);
            _cardNetLoad.UpdateTelemetry(_simNetLoad);
            _cardPressure.UpdateTelemetry(_simPressure);

            // Зберігаємо результати вимірювання кожного сенсора у репозиторій бази даних
            PersistSensorReading(_cardCpuTemp);
            PersistSensorReading(_cardRamUsage);
            PersistSensorReading(_cardNetLoad);
            PersistSensorReading(_cardPressure);

            RefreshDataGrid();
            UpdateStatusBar();
        }

        private void PersistSensorReading(SensorCardControl card)
        {
            bool isAlert = card.Status == SensorStatus.Critical;
            string message = card.Status switch
            {
                SensorStatus.Normal => "Показники в межах робочої норми",
                SensorStatus.Warning => $"Попередження: показник наближається до критичної межі",
                SensorStatus.Critical => $"АВАРІЯ: вихід за межі [{card.MinThreshold:F1} - {card.MaxThreshold:F1} {card.Unit}]",
                _ => string.Empty
            };

            _dbService.InsertRecord(card.SensorName, card.Value, card.Unit, card.Status, isAlert, message);
        }

        #endregion

        #region Обробка подій AlertTriggered та журналювання

        /// <summary>
        /// Обробник події AlertTriggered користувацького компонента SensorCardControl.
        /// Викликається автоматично компонентом при фіксації небезпечних параметрів.
        /// </summary>
        private void OnSensorAlertTriggered(object? sender, AlertEventArgs e)
        {
            _totalAlertCount++;

            string logEntry = $"[{e.Timestamp:yyyy-MM-dd HH:mm:ss.fff}] [УВАГА / АЛЕРТ] Сенсор: "{e.SensorName}" " +
                             $"| Значення: {e.CurrentValue:F1} {e.Unit} | Порогове значення: {e.Threshold:F1} {e.Unit} " +
                             $"| Опис: {e.Message}\r\n";

            txtAlertLog.SelectionStart = 0;
            txtAlertLog.SelectionLength = 0;
            txtAlertLog.SelectionColor = Color.FromArgb(248, 113, 113); // Світло-червоний
            txtAlertLog.SelectedText = logEntry;

            lblLogCount.Text = $"Зафіксовано подій тривог: {_totalAlertCount}";
            UpdateStatusBar();
        }

        #endregion

        #region Фільтрація, DataGridView та операції з БД

        private void RefreshDataGrid()
        {
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            string? sensorFilter = cmbFilterSensor.SelectedItem?.ToString();
            SensorStatus? statusFilter = cmbFilterStatus.SelectedIndex switch
            {
                1 => SensorStatus.Normal,
                2 => SensorStatus.Warning,
                3 => SensorStatus.Critical,
                _ => null
            };

            bool onlyAlerts = chkOnlyAlerts.Checked;

            var records = _dbService.GetFilteredRecords(sensorFilter, statusFilter, null, null, onlyAlerts);
            dgvTelemetry.DataSource = records;
        }

        private void ResetFilters()
        {
            cmbFilterSensor.SelectedIndex = 0;
            cmbFilterStatus.SelectedIndex = 0;
            chkOnlyAlerts.Checked = false;
            ApplyFilters();
        }

        private void BtnExportCsv_Click(object? sender, EventArgs e)
        {
            using var sfd = new SaveFileDialog
            {
                Filter = "CSV файл (*.csv)|*.csv|Усі файли (*.*)|*.*",
                FileName = $"Telemetry_Report_{DateTime.Now:yyyyMMdd_HHmmss}.csv",
                Title = "Збереження звіту телеметрії"
            };

            if (sfd.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    string? sensorFilter = cmbFilterSensor.SelectedItem?.ToString();
                    SensorStatus? statusFilter = cmbFilterStatus.SelectedIndex switch
                    {
                        1 => SensorStatus.Normal,
                        2 => SensorStatus.Warning,
                        3 => SensorStatus.Critical,
                        _ => null
                    };

                    var records = _dbService.GetFilteredRecords(sensorFilter, statusFilter, null, null, chkOnlyAlerts.Checked);
                    _dbService.ExportToCsv(sfd.FileName, records);

                    MessageBox.Show($"Успішно експортовано {records.Count} записів у файл:\n{sfd.FileName}",
                        "Експорт завершено", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка експорту даних: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnClearDb_Click(object? sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Ви дійсно бажаєте повністю очистити історію журналу телеметрії у базі даних?",
                "Підтвердження очищення",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                _dbService.ClearHistory();
                RefreshDataGrid();
                UpdateStatusBar();
                MessageBox.Show("Базу даних телеметрії успішно очищено.", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void UpdateStatusBar()
        {
            statusSensorsLabel.Text = "Активних сенсорів: 4";
            statusRecordsLabel.Text = $"Записів у БД: {_dbService.TotalRecordsCount}";
            statusAlertsLabel.Text = $"Аварійних записів: {_dbService.TotalAlertsCount}";
        }

        #endregion
    }
}
