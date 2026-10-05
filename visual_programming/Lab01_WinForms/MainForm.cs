using System;
using System.Globalization;
using System.Windows.Forms;

namespace VisualAppLab01
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            // Валідація та парсинг значень аргументів X та Y
            if (!double.TryParse(txtInputX.Text.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double x))
            {
                MessageBox.Show("Будь ласка, введіть коректне числове значення для аргументу X!", "Помилка введення", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtInputX.Focus();
                return;
            }

            if (!double.TryParse(txtInputY.Text.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double y))
            {
                MessageBox.Show("Будь ласка, введіть коректне числове значення для аргументу Y!", "Помилка введення", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtInputY.Focus();
                return;
            }

            double result = 0;
            string selectedFunction = "";

            try
            {
                if (rbFunction1.Checked)
                {
                    selectedFunction = "F1 = Sqrt(X^2 + Y^2) / (X - Y)";
                    if (Math.Abs(x - y) < 1e-9)
                    {
                        MessageBox.Show("Помилка ділення на нуль: аргументи X та Y не можуть бути однаковими!", "Математична помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    result = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2)) / (x - y);
                }
                else if (rbFunction2.Checked)
                {
                    selectedFunction = "F2 = Sin(X) * Cos(Y) + Ln(|X|)";
                    if (Math.Abs(x) < 1e-9)
                    {
                        MessageBox.Show("Помилка: логарифм від нуля не визначений (|X| > 0)!", "Математична помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    result = Math.Sin(x) * Math.Cos(y) + Math.Log(Math.Abs(x));
                }
                else if (rbFunction3.Checked)
                {
                    selectedFunction = "F3 = Exp(X - Y) + Tan(Y)";
                    result = Math.Exp(x - y) + Math.Tan(y);
                }
                else
                {
                    MessageBox.Show("Оберіть одну з функцій для обчислення!", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                lblResultValue.Text = $"Значення {selectedFunction.Split('=')[0].Trim()} = {result:F6}";
                lblStatus.Text = "Статус: Обчислення успішно завершено";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Виникла непередбачувана помилка під час обчислень: {ex.Message}", "Критична помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtInputX.Clear();
            txtInputY.Clear();
            rbFunction1.Checked = true;
            lblResultValue.Text = "Результат ще не обчислено";
            lblStatus.Text = "Статус: Очікування введення даних";
            txtInputX.Focus();
        }
    }
}
