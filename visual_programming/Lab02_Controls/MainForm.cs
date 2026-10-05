using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Lab02_Controls
{
    public partial class MainForm : Form
    {
        // Словник цін базових апаратних платформ
        private readonly Dictionary<string, decimal> platformPrices = new Dictionary<string, decimal>
        {
            { "Intel Core i7-14700K / 32GB DDR5 / RTX 4070 Ti (Base Dev)", 68500m },
            { "Intel Core i9-14900K / 64GB DDR5 / RTX 4090 (Heavy Compute)", 115000m },
            { "AMD Ryzen 9 7950X / 64GB DDR5 / Radeon RX 7900 XTX (Engineering)", 98000m },
            { "AMD Ryzen 7 7800X3D / 32GB DDR5 / RTX 4080 Super (Balanced Studio)", 82500m }
        };

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Ініціалізація випадаючого списку ComboBox
            cmbPlatform.Items.Clear();
            foreach (var platform in platformPrices.Keys)
            {
                cmbPlatform.Items.Add(platform);
            }
            cmbPlatform.SelectedIndex = 0;

            // Заповнення списку компонентів за замовчуванням у ListBox
            PopulateDefaultComponents();

            // Попередній розрахунок ціни
            UpdateLiveCalculation();
        }

        /// <summary>
        /// Початкове заповнення списку компонентів.
        /// </summary>
        private void PopulateDefaultComponents()
        {
            lstComponents.Items.Clear();
            lstComponents.Items.Add("Модуль Wi-Fi 6E + Bluetooth 5.3");
            lstComponents.Items.Add("Блок живлення 1000W 80+ Gold Modular");
            lstComponents.Items.Add("Корпус Fractal Design Define 7 (Sound Dampened)");
            lstComponents.Items.Add("Встановлення Docker Desktop та WSL2 Ubuntu");
            lstComponents.Items.Add("Налаштування середовища розробки Visual Studio 2022");
        }

        /// <summary>
        /// Обробник події зміни обраної платформи у ComboBox.
        /// </summary>
        private void cmbPlatform_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateLiveCalculation();
            statusLabelInfo.Text = $"Обрано платформу: {cmbPlatform.SelectedItem}";
        }

        /// <summary>
        /// Обробник перемикання терміну гарантії (RadioButton).
        /// </summary>
        private void Warranty_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is RadioButton rb && rb.Checked)
            {
                UpdateLiveCalculation();
                statusLabelInfo.Text = $"Змінено гарантійне покриття: {rb.Text}";
            }
        }

        /// <summary>
        /// Обробник перемикання прапорців додаткових послуг (CheckBox).
        /// </summary>
        private void Option_CheckedChanged(object sender, EventArgs e)
        {
            UpdateLiveCalculation();
            statusLabelInfo.Text = "Оновлено перелік додаткових опцій";
        }

        /// <summary>
        /// Живий перерахунок поточної орієнтовної вартості в рядку статусу.
        /// </summary>
        private decimal CalculateTotalCost()
        {
            decimal total = 0m;

            // 1. Вартість базової платформи (ComboBox)
            if (cmbPlatform.SelectedItem != null && platformPrices.TryGetValue(cmbPlatform.SelectedItem.ToString(), out decimal basePrice))
            {
                total += basePrice;
            }

            // 2. Вартість гарантії (RadioButton)
            if (rbWarranty24.Checked) total += 2500m;
            else if (rbWarranty36.Checked) total += 5500m;

            // 3. Вартість додаткових опцій (CheckBox)
            if (chkOS.Checked) total += 4200m;
            if (chkSSD.Checked) total += 3800m;
            if (chkCooling.Checked) total += 4500m;
            if (chkCableMgmt.Checked) total += 1200m;
            if (chkTesting.Checked) total += 800m;

            return total;
        }

        private void UpdateLiveCalculation()
        {
            decimal total = CalculateTotalCost();
            statusLabelPrice.Text = $"Орієнтовна сума: {total:N2} грн";
        }

        /// <summary>
        /// Додавання нового компонента або вимоги до ListBox.
        /// </summary>
        private void btnAddComponent_Click(object sender, EventArgs e)
        {
            string newComp = txtNewComponent.Text.Trim();
            if (string.IsNullOrEmpty(newComp))
            {
                MessageBox.Show("Введіть найменування компонента або пакету для додавання!", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNewComponent.Focus();
                return;
            }

            if (lstComponents.Items.Contains(newComp))
            {
                MessageBox.Show("Такий компонент вже додано до списку!", "Попередження", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            lstComponents.Items.Add(newComp);
            txtNewComponent.Clear();
            txtNewComponent.Focus();
            statusLabelInfo.Text = $"Додано компонент: {newComp}";
        }

        /// <summary>
        /// Видалення обраного компонента з ListBox.
        /// </summary>
        private void btnRemoveComponent_Click(object sender, EventArgs e)
        {
            if (lstComponents.SelectedIndex == -1)
            {
                MessageBox.Show("Оберіть елемент у списку для видалення!", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string removed = lstComponents.SelectedItem.ToString();
            lstComponents.Items.RemoveAt(lstComponents.SelectedIndex);
            statusLabelInfo.Text = $"Видалено компонент: {removed}";
        }

        /// <summary>
        /// Формування повної детальної специфікації замовлення в RichTextBox.
        /// </summary>
        private void btnCalculate_Click(object sender, EventArgs e)
        {
            if (cmbPlatform.SelectedItem == null)
            {
                MessageBox.Show("Будь ласка, оберіть базову платформу!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            decimal basePrice = platformPrices[cmbPlatform.SelectedItem.ToString()];
            decimal warrantyPrice = rbWarranty12.Checked ? 0m : (rbWarranty24.Checked ? 2500m : 5500m);
            string warrantyName = rbWarranty12.Checked ? "12 місяців (Стандарт)" : (rbWarranty24.Checked ? "24 місяці (Розширена)" : "36 місяців (Преміум On-Site)");

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("==================================================");
            sb.AppendLine("         СПЕЦИФІКАЦІЯ ЗАМОВЛЕННЯ СТАНЦІЇ          ");
            sb.AppendLine("       Конфігуратор ПК розробника ПЗ              ");
            sb.AppendLine("==================================================");
            sb.AppendLine($"Дата та час формування: {DateTime.Now:dd.MM.yyyy HH:mm:ss}");
            sb.AppendLine($"Студент-розробник: ТАРАС Вадим (група аІк43)");
            sb.AppendLine($"Дисципліна: Інструментальні засоби візуального програмування");
            sb.AppendLine("--------------------------------------------------");
            sb.AppendLine("1. БАЗОВА АПАРАТНА ПЛАТФОРМА (ComboBox):");
            sb.AppendLine($"   - {cmbPlatform.SelectedItem}");
            sb.AppendLine($"     Вартість бази: {basePrice:N2} грн");
            sb.AppendLine();
            sb.AppendLine("2. ПАКЕТ ГАРАНТІЙНОГО ОБСЛУГОВУВАННЯ (RadioButton):");
            sb.AppendLine($"   - {warrantyName}");
            sb.AppendLine($"     Вартість гарантії: {warrantyPrice:N2} грн");
            sb.AppendLine();
            sb.AppendLine("3. ДОДАТКОВІ ПАКЕТИ ТА ОПЦІЇ (CheckBox):");

            decimal optionsTotal = 0m;
            if (chkOS.Checked) { sb.AppendLine("   [✓] Ліцензійна ОС Windows 11 Pro (+4 200.00 грн)"); optionsTotal += 4200m; }
            if (chkSSD.Checked) { sb.AppendLine("   [✓] Швидкісний NVMe SSD 2TB (+3 800.00 грн)"); optionsTotal += 3800m; }
            if (chkCooling.Checked) { sb.AppendLine("   [✓] Рідинна система охолодження 360mm (+4 500.00 грн)"); optionsTotal += 4500m; }
            if (chkCableMgmt.Checked) { sb.AppendLine("   [✓] Професійний кабель-менеджмент (+1 200.00 грн)"); optionsTotal += 1200m; }
            if (chkTesting.Checked) { sb.AppendLine("   [✓] Стрес-тестування 24 години (+800.00 грн)"); optionsTotal += 800m; }

            if (optionsTotal == 0m)
            {
                sb.AppendLine("   (Додаткові опції не вибрано)");
            }
            sb.AppendLine($"     Сума за додаткові опції: {optionsTotal:N2} грн");
            sb.AppendLine();
            sb.AppendLine("4. СКЛАД КОМПЛЕКТУЮЧИХ ТА ПАКЕТІВ ПЗ (ListBox):");
            if (lstComponents.Items.Count == 0)
            {
                sb.AppendLine("   (Список компонентів порожній)");
            }
            else
            {
                for (int i = 0; i < lstComponents.Items.Count; i++)
                {
                    sb.AppendLine($"   {i + 1}. {lstComponents.Items[i]}");
                }
            }
            sb.AppendLine();
            sb.AppendLine("==================================================");
            decimal totalCost = basePrice + warrantyPrice + optionsTotal;
            sb.AppendLine($"ЗАГАЛЬНА ВАРТІСТЬ ЗАМОВЛЕННЯ: {totalCost:N2} грн");
            sb.AppendLine("==================================================");
            sb.AppendLine("Статус розрахунку: УСПІШНО ВЕРИФІКОВАНО");

            txtSpecification.Text = sb.ToString();
            statusLabelInfo.Text = "Статус: Специфікацію замовлення сформовано";
            UpdateLiveCalculation();
        }

        /// <summary>
        /// Збереження специфікації у текстовий файл.
        /// </summary>
        private void btnSaveReport_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSpecification.Text))
            {
                MessageBox.Show("Спочатку сформуйте специфікацію кнопкою «Розрахувати чек»!", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            saveFileDialogReport.FileName = $"Специфікація_замовлення_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
            if (saveFileDialogReport.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    File.WriteAllText(saveFileDialogReport.FileName, txtSpecification.Text, Encoding.UTF8);
                    MessageBox.Show("Специфікацію замовлення успішно збережено у файл!", "Збереження виконано", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    statusLabelInfo.Text = $"Збережено у: {Path.GetFileName(saveFileDialogReport.FileName)}";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка збереження файлу: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Скидання форми до початкових значень.
        /// </summary>
        private void btnReset_Click(object sender, EventArgs e)
        {
            cmbPlatform.SelectedIndex = 0;
            rbWarranty12.Checked = true;
            chkOS.Checked = false;
            chkSSD.Checked = false;
            chkCooling.Checked = false;
            chkCableMgmt.Checked = false;
            chkTesting.Checked = false;
            txtNewComponent.Clear();
            PopulateDefaultComponents();
            txtSpecification.Clear();
            UpdateLiveCalculation();
            statusLabelInfo.Text = "Статус: Всі параметри скинуто до початкових значень";
        }

        // Обробники меню (MenuStrip)
        private void menuFileNew_Click(object sender, EventArgs e) => btnReset_Click(sender, e);
        private void menuFileSave_Click(object sender, EventArgs e) => btnSaveReport_Click(sender, e);
        private void menuFileExit_Click(object sender, EventArgs e) => Application.Exit();
        private void menuEditReset_Click(object sender, EventArgs e) => btnReset_Click(sender, e);

        private void menuHelpAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Практична робота № 2 з дисципліни:\n" +
                "«Інструментальні засоби візуального програмування»\n\n" +
                "Тема: Базові елементи керування Windows Forms (ListBox, ComboBox, CheckBox, RadioButton, MenuStrip)\n\n" +
                "Виконавець: студент групи аІк43 ТАРАС Вадим\n" +
                "Викладач: КОСТІКОВ О.А.\n" +
                "Рік: 2026",
                "Про програму",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }
}
