using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Lab03_Dialogs
{
    /// <summary>
    /// Головна форма текстового редактора з демонстрацією стандартних діалогових вікон
    /// та безпечної роботи з потоками вводу-виводу System.IO.
    /// Практична робота №3 з дисципліни «Інструментальні засоби візуального програмування».
    /// Виконавець: студент групи аІк43 Тарас Вадим.
    /// </summary>
    public class MainForm : Form
    {
        // Елементи інтерфейсу користувача
        private MenuStrip mainMenu = null!;
        private ToolStripMenuItem fileMenu = null!;
        private ToolStripMenuItem editMenu = null!;
        private ToolStripMenuItem formatMenu = null!;
        private ToolStripMenuItem helpMenu = null!;

        private TextBox txtContent = null!;
        private StatusStrip statusStrip = null!;
        private ToolStripStatusLabel lblFilePath = null!;
        private ToolStripStatusLabel lblFileInfo = null!;
        private ToolStripStatusLabel lblEncoding = null!;
        private ToolStripStatusLabel lblCaretPosition = null!;

        // Змінні стану редактора
        private string? currentFilePath = null;
        private bool isModified = false;
        private readonly Encoding currentEncoding = Encoding.UTF8;

        public MainForm()
        {
            InitializeComponent();
            UpdateTitleAndStatus();
        }

        /// <summary>
        /// Програмна ініціалізація та конфігурація компонентів форми.
        /// </summary>
        private void InitializeComponent()
        {
            this.Text = "Текстовий редактор (Практична робота №3)";
            this.Size = new Size(950, 650);
            this.MinimumSize = new Size(600, 400);
            this.StartPosition = FormStartPosition.CenterScreen;

            // 1. Створення та конфігурація головного меню (MenuStrip)
            mainMenu = new MenuStrip();

            // Меню "Файл"
            fileMenu = new ToolStripMenuItem("&Файл");

            var itemNew = new ToolStripMenuItem("&Створити", null, OnNewFile_Click, Keys.Control | Keys.N);
            var itemOpen = new ToolStripMenuItem("&Відкрити...", null, OnOpenFile_Click, Keys.Control | Keys.O);
            var itemSave = new ToolStripMenuItem("&Зберегти", null, OnSaveFile_Click, Keys.Control | Keys.S);
            var itemSaveAs = new ToolStripMenuItem("Зберегти &як...", null, OnSaveAsFile_Click, Keys.Control | Keys.Shift | Keys.S);
            var itemBrowseFolder = new ToolStripMenuItem("&Огляд каталогу...", null, OnBrowseFolder_Click, Keys.Control | Keys.D);
            var itemSeparator1 = new ToolStripSeparator();
            var itemExit = new ToolStripMenuItem("Ви&хід", null, OnExit_Click, Keys.Alt | Keys.F4);

            fileMenu.DropDownItems.AddRange(new ToolStripItem[] {
                itemNew, itemOpen, itemSave, itemSaveAs, itemBrowseFolder, itemSeparator1, itemExit
            });

            // Меню "Правка"
            editMenu = new ToolStripMenuItem("&Правка");
            var itemClear = new ToolStripMenuItem("&Очистити вміст", null, OnClearContent_Click);
            var itemSelectAll = new ToolStripMenuItem("Виділити &все", null, (s, e) => txtContent.SelectAll(), Keys.Control | Keys.A);
            var itemWordWrap = new ToolStripMenuItem("Перенесення &рядків", null, OnToggleWordWrap_Click)
            {
                CheckOnClick = true,
                Checked = false
            };

            editMenu.DropDownItems.AddRange(new ToolStripItem[] {
                itemClear, itemSelectAll, itemWordWrap
            });

            // Меню "Формат"
            formatMenu = new ToolStripMenuItem("Ф&ормат");
            var itemFont = new ToolStripMenuItem("&Шрифт...", null, OnChooseFont_Click);
            var itemTextColor = new ToolStripMenuItem("Колір &тексту...", null, OnChooseTextColor_Click);
            var itemBgColor = new ToolStripMenuItem("Колір &тла...", null, OnChooseBackgroundColor_Click);

            formatMenu.DropDownItems.AddRange(new ToolStripItem[] {
                itemFont, itemTextColor, itemBgColor
            });

            // Меню "Довідка"
            helpMenu = new ToolStripMenuItem("&Довідка");
            var itemAbout = new ToolStripMenuItem("&Про програму...", null, OnAbout_Click, Keys.F1);
            helpMenu.DropDownItems.Add(itemAbout);

            mainMenu.Items.AddRange(new ToolStripItem[] {
                fileMenu, editMenu, formatMenu, helpMenu
            });

            // 2. Створення та конфігурація рядка стану (StatusStrip)
            statusStrip = new StatusStrip();
            lblFilePath = new ToolStripStatusLabel
            {
                Text = "Новий документ",
                Spring = true,
                TextAlign = ContentAlignment.MiddleLeft
            };
            lblFileInfo = new ToolStripStatusLabel
            {
                Text = "Символів: 0 | Рядків: 1",
                BorderSides = ToolStripStatusLabelBorderSides.Left,
                BorderStyle = Border3DStyle.Etched
            };
            lblEncoding = new ToolStripStatusLabel
            {
                Text = "UTF-8",
                BorderSides = ToolStripStatusLabelBorderSides.Left,
                BorderStyle = Border3DStyle.Etched
            };
            lblCaretPosition = new ToolStripStatusLabel
            {
                Text = "Ряд: 1, Стовп: 1",
                BorderSides = ToolStripStatusLabelBorderSides.Left,
                BorderStyle = Border3DStyle.Etched
            };

            statusStrip.Items.AddRange(new ToolStripItem[] {
                lblFilePath, lblFileInfo, lblEncoding, lblCaretPosition
            });

            // 3. Створення та конфігурація текстової області (TextBox)
            txtContent = new TextBox
            {
                Multiline = true,
                Dock = DockStyle.Fill,
                ScrollBars = ScrollBars.Both,
                WordWrap = false,
                Font = new Font("Consolas", 11f, FontStyle.Regular),
                BackColor = Color.White,
                ForeColor = Color.Black,
                AcceptsTab = true
            };

            txtContent.TextChanged += OnContent_TextChanged;
            txtContent.KeyUp += OnCaretOrContent_Changed;
            txtContent.MouseUp += OnCaretOrContent_Changed;

            // Додавання елементів до форми (порядок DockStyle важливий!)
            this.Controls.Add(txtContent);
            this.Controls.Add(statusStrip);
            this.Controls.Add(mainMenu);
            this.MainMenuStrip = mainMenu;

            this.FormClosing += MainForm_FormClosing;
        }

        #region Обробники меню Файл

        /// <summary>
        /// Створення нового документа. Запитує підтвердження збереження змін.
        /// </summary>
        private void OnNewFile_Click(object? sender, EventArgs e)
        {
            if (PromptSaveIfModified())
            {
                txtContent.Clear();
                currentFilePath = null;
                isModified = false;
                UpdateTitleAndStatus();
            }
        }

        /// <summary>
        /// Відкриття файлу за допомогою діалогового вікна OpenFileDialog
        /// та безпечне читання вмісту через StreamReader у блоці try-catch-finally.
        /// </summary>
        private void OnOpenFile_Click(object? sender, EventArgs e)
        {
            if (!PromptSaveIfModified())
                return;

            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Оберіть текстовий файл для відкриття";
                ofd.Filter = "Текстові файли (*.txt)|*.txt|Файли коду (*.cs;*.json;*.md;*.xml)|*.cs;*.json;*.md;*.xml|Усі файли (*.*)|*.*";
                ofd.FilterIndex = 1;
                ofd.CheckFileExists = true;
                ofd.CheckPathExists = true;
                ofd.Multiselect = false;

                if (ofd.ShowDialog(this) == DialogResult.OK)
                {
                    ReadFileWithStreamReader(ofd.FileName);
                }
            }
        }

        /// <summary>
        /// Збереження файлу у поточний шлях або виклик SaveFileDialog, якщо файл новий.
        /// </summary>
        private void OnSaveFile_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentFilePath))
            {
                OnSaveAsFile_Click(sender, e);
            }
            else
            {
                WriteFileWithStreamWriter(currentFilePath);
            }
        }

        /// <summary>
        /// Збереження файлу за допомогою діалогового вікна SaveFileDialog
        /// та безпечний запис вмісту через StreamWriter у блоці try-catch-finally.
        /// </summary>
        private void OnSaveAsFile_Click(object? sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Title = "Зберегти файл як...";
                sfd.Filter = "Текстові файли (*.txt)|*.txt|Markdown файли (*.md)|*.md|Усі файли (*.*)|*.*";
                sfd.DefaultExt = "txt";
                sfd.OverwritePrompt = true;
                sfd.AddExtension = true;

                if (!string.IsNullOrEmpty(currentFilePath))
                {
                    sfd.InitialDirectory = Path.GetDirectoryName(currentFilePath);
                    sfd.FileName = Path.GetFileName(currentFilePath);
                }

                if (sfd.ShowDialog(this) == DialogResult.OK)
                {
                    WriteFileWithStreamWriter(sfd.FileName);
                }
            }
        }

        /// <summary>
        /// Вибір та аналіз каталогу за допомогою діалогового вікна FolderBrowserDialog.
        /// </summary>
        private void OnBrowseFolder_Click(object? sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Оберіть папку для перегляду вмісту та статистики файлів:";
                fbd.ShowNewFolderButton = true;

                if (fbd.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        string selectedPath = fbd.SelectedPath;
                        string[] files = Directory.GetFiles(selectedPath);
                        string[] dirs = Directory.GetDirectories(selectedPath);

                        StringBuilder report = new StringBuilder();
                        report.AppendLine($"=== ОГЛЯД КАТАЛОГУ: {selectedPath} ===");
                        report.AppendLine($"Кількість підпапок: {dirs.Length}");
                        report.AppendLine($"Кількість файлів: {files.Length}");
                        report.AppendLine(new string('-', 50));
                        report.AppendLine("Файли у вибраному каталозі:");

                        foreach (string file in files)
                        {
                            FileInfo fi = new FileInfo(file);
                            report.AppendLine($"• {fi.Name} ({fi.Length:N0} байт, змінено: {fi.LastWriteTime:yyyy-MM-dd HH:mm})");
                        }

                        var result = MessageBox.Show(
                            $"{report}\n\nБажаєте вставити цей список у редактор?",
                            "Результат огляду папки",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Information);

                        if (result == DialogResult.Yes)
                        {
                            txtContent.AppendText(Environment.NewLine + report.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(
                            $"Помилка під час читання каталогу:\n{ex.Message}",
                            "Помилка доступу",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Безпечний вихід із застосунку з контролем незбережених даних.
        /// </summary>
        private void OnExit_Click(object? sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region Робота з потоками введення/виведення (System.IO)

        /// <summary>
        /// Читання вмісту файлу з використанням StreamReader у суворому академічному блоці try-catch-finally.
        /// </summary>
        /// <param name="path">Абсолютний шлях до файлу.</param>
        private void ReadFileWithStreamReader(string path)
        {
            StreamReader? reader = null;
            try
            {
                // Створення потоку читання з фіксацією кодування UTF-8
                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                reader = new StreamReader(fs, currentEncoding);

                string content = reader.ReadToEnd();
                txtContent.Text = content;

                currentFilePath = path;
                isModified = false;
                UpdateTitleAndStatus();

                MessageBox.Show(
                    $"Файл успішно прочитано:\n{Path.GetFileName(path)}\nРозмір: {new FileInfo(path).Length:N0} байт",
                    "Успіх",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (FileNotFoundException fnfEx)
            {
                MessageBox.Show($"Файл не знайдено на диску:\n{fnfEx.Message}", "Помилка введення-виведення", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (UnauthorizedAccessException uaEx)
            {
                MessageBox.Show($"Недостатньо прав для читання обраного файлу:\n{uaEx.Message}", "Помилка доступу", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (IOException ioEx)
            {
                MessageBox.Show($"Помилка введення/виведення при зчитуванні:\n{ioEx.Message}", "Помилка читання", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Непередбачена помилка:\n{ex.Message}", "Критична помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Обов'язкове закриття та звільнення системних ресурсів у секції finally
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
        }

        /// <summary>
        /// Запис вмісту форми у файл з використанням StreamWriter у суворому блоці try-catch-finally.
        /// </summary>
        /// <param name="path">Абсолютний шлях для збереження файлу.</param>
        private void WriteFileWithStreamWriter(string path)
        {
            StreamWriter? writer = null;
            try
            {
                // Створення потоку запису (перезапис файлу) у кодуванні UTF-8
                FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
                writer = new StreamWriter(fs, currentEncoding);

                writer.Write(txtContent.Text);
                writer.Flush(); // Очищення буфера та гарантований запис на фізичний диск

                currentFilePath = path;
                isModified = false;
                UpdateTitleAndStatus();

                MessageBox.Show(
                    $"Файл успішно збережено:\n{Path.GetFileName(path)}\nРозмір: {new FileInfo(path).Length:N0} байт",
                    "Успіх збереження",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (UnauthorizedAccessException uaEx)
            {
                MessageBox.Show($"Відмовлено в доступі для запису за цим шляхом:\n{uaEx.Message}", "Помилка збереження", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (IOException ioEx)
            {
                MessageBox.Show($"Помилка введення/виведення під час запису файлу:\n{ioEx.Message}", "Помилка I/O", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка збереження файлу:\n{ex.Message}", "Критична помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Гарантоване звільнення файлового дескриптора
                if (writer != null)
                {
                    writer.Close();
                    writer.Dispose();
                }
            }
        }

        #endregion

        #region Діалогові вікна налаштування інтерфейсу (FontDialog, ColorDialog)

        /// <summary>
        /// Вибір шрифту для робочої області редактора через FontDialog.
        /// </summary>
        private void OnChooseFont_Click(object? sender, EventArgs e)
        {
            using (FontDialog fd = new FontDialog())
            {
                fd.Font = txtContent.Font;
                fd.ShowEffects = true;
                fd.ShowColor = false;

                if (fd.ShowDialog(this) == DialogResult.OK)
                {
                    txtContent.Font = fd.Font;
                }
            }
        }

        /// <summary>
        /// Вибір кольору шрифту (тексту) через ColorDialog.
        /// </summary>
        private void OnChooseTextColor_Click(object? sender, EventArgs e)
        {
            using (ColorDialog cd = new ColorDialog())
            {
                cd.Color = txtContent.ForeColor;
                cd.FullOpen = true; // Дозволити вибір довільного відтінку палітри

                if (cd.ShowDialog(this) == DialogResult.OK)
                {
                    txtContent.ForeColor = cd.Color;
                }
            }
        }

        /// <summary>
        /// Вибір кольору тла текстового поля через ColorDialog.
        /// </summary>
        private void OnChooseBackgroundColor_Click(object? sender, EventArgs e)
        {
            using (ColorDialog cd = new ColorDialog())
            {
                cd.Color = txtContent.BackColor;
                cd.FullOpen = true;

                if (cd.ShowDialog(this) == DialogResult.OK)
                {
                    txtContent.BackColor = cd.Color;
                }
            }
        }

        #endregion

        #region Допоміжні методи та обробники інтерфейсу

        private void OnClearContent_Click(object? sender, EventArgs e)
        {
            if (txtContent.TextLength > 0)
            {
                var confirm = MessageBox.Show(
                    "Очистити весь вміст документа?",
                    "Підтвердження очищення",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirm == DialogResult.Yes)
                {
                    txtContent.Clear();
                }
            }
        }

        private void OnToggleWordWrap_Click(object? sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem mi)
            {
                txtContent.WordWrap = mi.Checked;
                txtContent.ScrollBars = mi.Checked ? ScrollBars.Vertical : ScrollBars.Both;
            }
        }

        private void OnAbout_Click(object? sender, EventArgs e)
        {
            string info = "Практична робота №3\n" +
                          "Тема: Діалогові вікна та файли у Windows Forms\n" +
                          "Дисципліна: Інструментальні засоби візуального програмування\n" +
                          "Викладач: Костіков О.А.\n\n" +
                          "Виконавець:\n" +
                          "Студент групи аІк43\n" +
                          "Тарас Вадим\n\n" +
                          "Функціонал:\n" +
                          "• OpenFileDialog & SaveFileDialog з фільтрацією\n" +
                          "• FontDialog & ColorDialog для персоналізації стилю\n" +
                          "• FolderBrowserDialog для аналізу вмісту каталогів\n" +
                          "• Безпечні потоки StreamReader / StreamWriter у блоках try-catch-finally";

            MessageBox.Show(info, "Про програму", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void OnContent_TextChanged(object? sender, EventArgs e)
        {
            isModified = true;
            UpdateTitleAndStatus();
        }

        private void OnCaretOrContent_Changed(object? sender, EventArgs e)
        {
            int index = txtContent.SelectionStart;
            int line = txtContent.GetLineFromCharIndex(index) + 1;
            int col = index - txtContent.GetFirstCharIndexOfCurrentLine() + 1;

            lblCaretPosition.Text = $"Ряд: {line}, Стовп: {col}";
        }

        private void UpdateTitleAndStatus()
        {
            string fileName = string.IsNullOrEmpty(currentFilePath)
                ? "Новий документ"
                : Path.GetFileName(currentFilePath);

            string title = $"{fileName}{(isModified ? " *" : "")} - Текстовий редактор (Лаб 3)";
            this.Text = title;

            lblFilePath.Text = string.IsNullOrEmpty(currentFilePath)
                ? "Новий документ"
                : currentFilePath;

            int linesCount = txtContent.Lines.Length;
            int charsCount = txtContent.TextLength;
            lblFileInfo.Text = $"Символів: {charsCount:N0} | Рядків: {linesCount:N0}";
            lblEncoding.Text = currentEncoding.WebName.ToUpperInvariant();
        }

        /// <summary>
        /// Перевірка наявності незбережених змін та запит користувача на їх збереження.
        /// </summary>
        /// <returns>True, якщо можна продовжувати операцію; False, якщо користувач натиснув «Скасувати».</returns>
        private bool PromptSaveIfModified()
        {
            if (!isModified)
                return true;

            string docName = string.IsNullOrEmpty(currentFilePath) ? "Новий документ" : Path.GetFileName(currentFilePath);
            var result = MessageBox.Show(
                $"Документ «{docName}» містить незбережені зміни.\nЗберегти їх зараз?",
                "Збереження змін",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                OnSaveFile_Click(this, EventArgs.Empty);
                return !isModified; // успішно збережено, якщо прапорець скинуто
            }
            else if (result == DialogResult.No)
            {
                return true; // користувач свідомо ігнорує зміни
            }
            else
            {
                return false; // операцію скасовано
            }
        }

        private void MainForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (!PromptSaveIfModified())
            {
                e.Cancel = true;
            }
        }

        #endregion
    }
}
