using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;

namespace Lab04_DataGridView
{
    public class MainForm : Form
    {
        // Дані та прив'язка
        private BindingList<ProductItem> _allProducts = new();
        private BindingSource _bindingSource = new();

        // Елементи інтерфейсу
        private ToolStrip _toolStrip = null!;
        private ToolStripButton _btnAdd = null!;
        private ToolStripButton _btnDelete = null!;
        private ToolStripSeparator _sep1 = null!;
        private ToolStripLabel _lblSearch = null!;
        private ToolStripTextBox _txtSearch = null!;
        private ToolStripButton _btnFilter = null!;
        private ToolStripButton _btnResetFilter = null!;
        private ToolStripSeparator _sep2 = null!;
        private ToolStripDropDownButton _btnExport = null!;
        private ToolStripDropDownButton _btnImport = null!;

        private DataGridView _dataGridView = null!;
        private StatusStrip _statusStrip = null!;
        private ToolStripStatusLabel _statusCountLabel = null!;
        private ToolStripStatusLabel _statusSumLabel = null!;
        private ToolStripStatusLabel _statusInfoLabel = null!;

        public MainForm()
        {
            InitializeComponent();
            LoadSampleData();
            UpdateSummary();
        }

        private void InitializeComponent()
        {
            Text = "Практична робота 4: DataGridView та Data Binding (Управління складом)";
            Width = 1000;
            Height = 650;
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);

            // 1. Панель інструментів (ToolStrip)
            _toolStrip = new ToolStrip { ImageList = null, GripStyle = ToolStripGripStyle.Hidden };

            _btnAdd = new ToolStripButton("➕ Додати товар", null, BtnAdd_Click) { DisplayStyle = ToolStripItemDisplayStyle.Text };
            _btnDelete = new ToolStripButton("❌ Видалити", null, BtnDelete_Click) { DisplayStyle = ToolStripItemDisplayStyle.Text };
            _sep1 = new ToolStripSeparator();

            _lblSearch = new ToolStripLabel("🔍 Пошук:");
            _txtSearch = new ToolStripTextBox { Width = 150 };
            _txtSearch.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) ApplyFilter(); };
            _btnFilter = new ToolStripButton("Фільтрувати", null, (s, e) => ApplyFilter()) { DisplayStyle = ToolStripItemDisplayStyle.Text };
            _btnResetFilter = new ToolStripButton("Скинути", null, (s, e) => ResetFilter()) { DisplayStyle = ToolStripItemDisplayStyle.Text };
            _sep2 = new ToolStripSeparator();

            // Меню експорту
            _btnExport = new ToolStripDropDownButton("💾 Експорт");
            _btnExport.DropDownItems.Add("Експорт у CSV...", null, (s, e) => ExportToCsv());
            _btnExport.DropDownItems.Add("Експорт у JSON...", null, (s, e) => ExportToJson());

            // Меню імпорту
            _btnImport = new ToolStripDropDownButton("📂 Імпорт");
            _btnImport.DropDownItems.Add("Імпорт з CSV...", null, (s, e) => ImportFromCsv());
            _btnImport.DropDownItems.Add("Імпорт з JSON...", null, (s, e) => ImportFromJson());

            _toolStrip.Items.AddRange(new ToolStripItem[]
            {
                _btnAdd, _btnDelete, _sep1,
                _lblSearch, _txtSearch, _btnFilter, _btnResetFilter, _sep2,
                _btnExport, _btnImport
            });

            // 2. Статус-рядок (StatusStrip)
            _statusStrip = new StatusStrip();
            _statusCountLabel = new ToolStripStatusLabel("Всього записів: 0") { Margin = new Padding(0, 3, 20, 2) };
            _statusSumLabel = new ToolStripStatusLabel("Загальна вартість: 0.00 грн") { Margin = new Padding(0, 3, 20, 2), Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            _statusInfoLabel = new ToolStripStatusLabel("Готово до роботи") { Spring = true, TextAlign = ContentAlignment.MiddleRight, ForeColor = Color.DarkSlateGray };

            _statusStrip.Items.AddRange(new ToolStripItem[] { _statusCountLabel, _statusSumLabel, _statusInfoLabel });

            // 3. Таблиця DataGridView
            _dataGridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                BackgroundColor = Color.WhiteSmoke,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = true,
                RowHeadersWidth = 35,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                EnableHeadersVisualStyles = false
            };

            // Налаштування стилів заголовків
            _dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 62, 80);
            _dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            _dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            _dataGridView.ColumnHeadersHeight = 35;
            _dataGridView.RowTemplate.Height = 28;
            _dataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 248, 250);

            // Налаштування колонок різних типів
            ConfigureColumns();

            // Події DataGridView
            _dataGridView.CellValidating += DataGridView_CellValidating;
            _dataGridView.CellEndEdit += (s, e) => UpdateSummary();
            _dataGridView.CellContentClick += DataGridView_CellContentClick;
            _dataGridView.DataError += (s, e) =>
            {
                MessageBox.Show($"Помилка у форматі даних клітинки: {e.Exception?.Message}", "Помилка валідації", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.ThrowException = false;
            };

            // Додавання елементів на форму
            Controls.Add(_dataGridView);
            Controls.Add(_toolStrip);
            Controls.Add(_statusStrip);
        }

        private void ConfigureColumns()
        {
            // 1. Колонка ID (Text)
            var colId = new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ProductItem.Id),
                HeaderText = "№",
                Width = 50,
                FillWeight = 30,
                ReadOnly = true,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }
            };

            // 2. Колонка назви (Text)
            var colName = new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ProductItem.Name),
                HeaderText = "Назва товару",
                FillWeight = 120
            };

            // 3. Колонка категорії (ComboBox)
            var colCategory = new DataGridViewComboBoxColumn
            {
                DataPropertyName = nameof(ProductItem.Category),
                HeaderText = "Категорія",
                FillWeight = 80,
                DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox
            };
            colCategory.Items.AddRange("Електроніка", "Комп'ютери", "Периферія", "Мережеве обладнання", "Аксесуари");

            // 4. Колонка ціни (Text з форматуванням)
            var colPrice = new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ProductItem.Price),
                HeaderText = "Ціна (грн)",
                FillWeight = 60,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N2" }
            };

            // 5. Колонка кількості (Text)
            var colQty = new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ProductItem.Quantity),
                HeaderText = "Кількість",
                FillWeight = 50,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }
            };

            // 6. Колонка наявності (CheckBox)
            var colInStock = new DataGridViewCheckBoxColumn
            {
                DataPropertyName = nameof(ProductItem.InStock),
                HeaderText = "В наявності",
                FillWeight = 50
            };

            // 7. Розрахункова вартість (ReadOnly Text)
            var colTotal = new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ProductItem.TotalValue),
                HeaderText = "Вартість разом",
                FillWeight = 70,
                ReadOnly = true,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N2", Font = new Font("Segoe UI", 9F, FontStyle.Bold) }
            };

            // 8. Кнопка швидкої дії (Button)
            var colAction = new DataGridViewButtonColumn
            {
                HeaderText = "Дія",
                Text = "⚡ Списати 1 шт",
                UseColumnTextForButtonValue = true,
                FillWeight = 65
            };

            _dataGridView.Columns.AddRange(colId, colName, colCategory, colPrice, colQty, colInStock, colTotal, colAction);
        }

        private void LoadSampleData()
        {
            _allProducts = new BindingList<ProductItem>
            {
                new() { Id = 1, Name = "Монітор 27\" IPS 144Hz", Category = "Периферія", Price = 8499.00m, Quantity = 12, InStock = true },
                new() { Id = 2, Name = "Механічна клавіатура RGB", Category = "Периферія", Price = 2599.50m, Quantity = 25, InStock = true },
                new() { Id = 3, Name = "Маршрутизатор Wi-Fi 6 Gigabit", Category = "Мережеве обладнання", Price = 3199.00m, Quantity = 8, InStock = true },
                new() { Id = 4, Name = "Ноутбук Developer Pro 16\"", Category = "Комп'ютери", Price = 45990.00m, Quantity = 5, InStock = true },
                new() { Id = 5, Name = "Бездротова миша оптична", Category = "Периферія", Price = 899.00m, Quantity = 30, InStock = true },
                new() { Id = 6, Name = "USB-C HUB 8-in-1", Category = "Аксесуари", Price = 1250.00m, Quantity = 15, InStock = true },
                new() { Id = 7, Name = "SSD NVMe M.2 1TB", Category = "Електроніка", Price = 3450.00m, Quantity = 0, InStock = false }
            };

            _allProducts.ListChanged += (s, e) => UpdateSummary();
            _bindingSource.DataSource = _allProducts;
            _dataGridView.DataSource = _bindingSource;
        }

        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            int nextId = _allProducts.Any() ? _allProducts.Max(p => p.Id) + 1 : 1;
            var newItem = new ProductItem
            {
                Id = nextId,
                Name = $"Новий товар #{nextId}",
                Category = "Електроніка",
                Price = 100.00m,
                Quantity = 1,
                InStock = true
            };

            _allProducts.Add(newItem);
            _statusInfoLabel.Text = $"Додано запис ID: {nextId}";
            _dataGridView.FirstDisplayedScrollingRowIndex = _dataGridView.RowCount - 1;
        }

        private void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (_bindingSource.Current is ProductItem selectedItem)
            {
                var dialogResult = MessageBox.Show(
                    $"Ви дійсно бажаєте видалити товар \"{selectedItem.Name}\" (ID: {selectedItem.Id})?",
                    "Підтвердження видалення",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (dialogResult == DialogResult.Yes)
                {
                    _allProducts.Remove(selectedItem);
                    _statusInfoLabel.Text = $"Видалено товар ID: {selectedItem.Id}";
                    UpdateSummary();
                }
            }
            else
            {
                MessageBox.Show("Оберіть рядок для видалення.", "Повідомлення", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ApplyFilter()
        {
            string keyword = _txtSearch.Text.Trim().ToLower();
            if (string.IsNullOrWhiteSpace(keyword))
            {
                ResetFilter();
                return;
            }

            var filtered = _allProducts.Where(p => p.Name.ToLower().Contains(keyword) || p.Category.ToLower().Contains(keyword)).ToList();
            _bindingSource.DataSource = new BindingList<ProductItem>(filtered);
            _statusInfoLabel.Text = $"Знайдено записів: {filtered.Count} за фільтром \"{keyword}\"";
            UpdateSummary();
        }

        private void ResetFilter()
        {
            _txtSearch.Clear();
            _bindingSource.DataSource = _allProducts;
            _statusInfoLabel.Text = "Фільтр скинуто (відображено всі записи)";
            UpdateSummary();
        }

        private void DataGridView_CellValidating(object? sender, DataGridViewCellValidatingEventArgs e)
        {
            string columnName = _dataGridView.Columns[e.ColumnIndex].DataPropertyName;
            string rawValue = e.FormattedValue?.ToString()?.Trim() ?? string.Empty;

            if (columnName == nameof(ProductItem.Name))
            {
                if (string.IsNullOrEmpty(rawValue))
                {
                    e.Cancel = true;
                    _dataGridView.Rows[e.RowIndex].ErrorText = "Назва товару не може бути порожньою!";
                    MessageBox.Show("Назва товару є обов'язковим полем і не може бути порожньою.", "Помилка валідації", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    _dataGridView.Rows[e.RowIndex].ErrorText = string.Empty;
                }
            }
            else if (columnName == nameof(ProductItem.Price))
            {
                if (!decimal.TryParse(rawValue, NumberStyles.Any, CultureInfo.CurrentCulture, out decimal price) &&
                    !decimal.TryParse(rawValue, NumberStyles.Any, CultureInfo.InvariantCulture, out price) || price < 0)
                {
                    e.Cancel = true;
                    _dataGridView.Rows[e.RowIndex].ErrorText = "Ціна має бути додатнім дійсним числом!";
                    MessageBox.Show("Ціна товару повинна бути невід'ємним числовим значенням.", "Помилка валідації", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    _dataGridView.Rows[e.RowIndex].ErrorText = string.Empty;
                }
            }
            else if (columnName == nameof(ProductItem.Quantity))
            {
                if (!int.TryParse(rawValue, out int qty) || qty < 0)
                {
                    e.Cancel = true;
                    _dataGridView.Rows[e.RowIndex].ErrorText = "Кількість має бути невід'ємним цілим числом!";
                    MessageBox.Show("Кількість товару повинна бути цілим числом >= 0.", "Помилка валідації", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    _dataGridView.Rows[e.RowIndex].ErrorText = string.Empty;
                }
            }
        }

        private void DataGridView_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && _dataGridView.Columns[e.ColumnIndex] is DataGridViewButtonColumn)
            {
                if (_dataGridView.Rows[e.RowIndex].DataBoundItem is ProductItem item)
                {
                    if (item.Quantity > 0)
                    {
                        item.Quantity--;
                        if (item.Quantity == 0)
                        {
                            item.InStock = false;
                        }
                        _dataGridView.Refresh();
                        UpdateSummary();
                        _statusInfoLabel.Text = $"Списано 1 од. \"{item.Name}\". Залишок: {item.Quantity}";
                    }
                    else
                    {
                        MessageBox.Show($"Товар \"{item.Name}\" уже відсутній на складі!", "Операція неможлива", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void UpdateSummary()
        {
            var currentList = _bindingSource.List.OfType<ProductItem>().ToList();
            int totalCount = currentList.Count;
            decimal totalSum = currentList.Sum(p => p.TotalValue);

            _statusCountLabel.Text = $"Всього записів: {totalCount}";
            _statusSumLabel.Text = $"Загальна вартість: {totalSum:N2} грн";
        }

        private void ExportToCsv()
        {
            using var sfd = new SaveFileDialog
            {
                Filter = "CSV файли (*.csv)|*.csv|Усі файли (*.*)|*.*",
                FileName = "products_export.csv",
                Title = "Збереження даних у форматі CSV"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var sb = new StringBuilder();
                    sb.AppendLine("Id;Name;Category;Price;Quantity;InStock;TotalValue");
                    foreach (var item in _allProducts)
                    {
                        sb.AppendLine($"{item.Id};\"{item.Name.Replace("\"", "\"\"")}\";{item.Category};{item.Price.ToString(CultureInfo.InvariantCulture)};{item.Quantity};{item.InStock};{item.TotalValue.ToString(CultureInfo.InvariantCulture)}");
                    }
                    File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                    _statusInfoLabel.Text = $"Експортовано в CSV: {Path.GetFileName(sfd.FileName)}";
                    MessageBox.Show("Дані успішно експортовано у CSV!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка експорту у CSV: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ExportToJson()
        {
            using var sfd = new SaveFileDialog
            {
                Filter = "JSON файли (*.json)|*.json|Усі файли (*.*)|*.*",
                FileName = "products_export.json",
                Title = "Збереження даних у форматі JSON"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var options = new JsonSerializerOptions { WriteIndented = true };
                    string json = JsonSerializer.Serialize(_allProducts.ToList(), options);
                    File.WriteAllText(sfd.FileName, json, Encoding.UTF8);
                    _statusInfoLabel.Text = $"Експортовано в JSON: {Path.GetFileName(sfd.FileName)}";
                    MessageBox.Show("Дані успішно експортовано у JSON!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка експорту у JSON: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ImportFromCsv()
        {
            using var ofd = new OpenFileDialog
            {
                Filter = "CSV файли (*.csv)|*.csv|Усі файли (*.*)|*.*",
                Title = "Імпорт даних з CSV файлу"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var lines = File.ReadAllLines(ofd.FileName, Encoding.UTF8);
                    if (lines.Length <= 1)
                    {
                        MessageBox.Show("Файл порожній або містить тільки заголовок.", "Попередження", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    _allProducts.Clear();
                    for (int i = 1; i < lines.Length; i++)
                    {
                        var line = lines[i];
                        if (string.IsNullOrWhiteSpace(line)) continue;
                        var parts = line.Split(';');
                        if (parts.Length >= 6)
                        {
                            int id = int.Parse(parts[0]);
                            string name = parts[1].Trim('"');
                            string cat = parts[2];
                            decimal price = decimal.Parse(parts[3], CultureInfo.InvariantCulture);
                            int qty = int.Parse(parts[4]);
                            bool inStock = bool.Parse(parts[5]);

                            _allProducts.Add(new ProductItem
                            {
                                Id = id,
                                Name = name,
                                Category = cat,
                                Price = price,
                                Quantity = qty,
                                InStock = inStock
                            });
                        }
                    }

                    ResetFilter();
                    _statusInfoLabel.Text = $"Імпортовано записів: {_allProducts.Count} з CSV";
                    MessageBox.Show($"Успішно завантажено {_allProducts.Count} записів!", "Імпорт завершено", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка читання CSV: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ImportFromJson()
        {
            using var ofd = new OpenFileDialog
            {
                Filter = "JSON файли (*.json)|*.json|Усі файли (*.*)|*.*",
                Title = "Імпорт даних з JSON файлу"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string json = File.ReadAllText(ofd.FileName, Encoding.UTF8);
                    var list = JsonSerializer.Deserialize<System.Collections.Generic.List<ProductItem>>(json);
                    if (list != null)
                    {
                        _allProducts.Clear();
                        foreach (var item in list)
                        {
                            _allProducts.Add(item);
                        }
                        ResetFilter();
                        _statusInfoLabel.Text = $"Імпортовано записів: {_allProducts.Count} з JSON";
                        MessageBox.Show($"Успішно завантажено {_allProducts.Count} записів!", "Імпорт завершено", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка читання JSON: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
