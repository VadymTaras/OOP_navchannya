using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Lab05_GDI;

/// <summary>
/// Головна форма додатку «Scientific Function & Geometric Graphics Plotter».
/// Практична робота 5 з дисципліни «Інструментальні засоби візуального програмування».
/// Розробник: студент групи аІк43 ТАРАС Вадим.
/// Керівник: викладач КОСТІКОВ О.А.
/// </summary>
public partial class MainForm : Form
{
    private record FunctionItem(string Name, string Formula, Func<double, double> Evaluator);

    private readonly List<FunctionItem> _functions = new()
    {
        new("sin(x)", "f(x) = sin(x)", Math.Sin),
        new("cos(x)", "f(x) = cos(x)", Math.Cos),
        new("x² - 4 (Парабола)", "f(x) = x² - 4", x => (x * x) - 4.0),
        new("x³ - 3x (Кубічний поліном)", "f(x) = x³ - 3x", x => Math.Pow(x, 3) - (3.0 * x)),
        new("e^(-0.1x) * sin(2x) (Демпфовані коливання)", "f(x) = e^(-0.1x) · sin(2x)", x => Math.Exp(-0.1 * x) * Math.Sin(2.0 * x)),
        new("sinc(x) = sin(x)/x (Кардинальний синус)", "f(x) = sin(x) / x", x => Math.Abs(x) < 1e-9 ? 1.0 : Math.Sin(x) / x),
        new("2·cos(x) + sin(3x) (Суперпозиція хвиль)", "f(x) = 2·cos(x) + sin(3x)", x => (2.0 * Math.Cos(x)) + Math.Sin(3.0 * x))
    };

    public MainForm()
    {
        InitializeComponent();
        InitializeFunctionList();
        InitializeCanvasSettings();
    }

    private void InitializeFunctionList()
    {
        cmbFunction.DisplayMember = nameof(FunctionItem.Name);
        foreach (var func in _functions)
        {
            cmbFunction.Items.Add(func);
        }
        cmbFunction.SelectedIndex = 0;
        cmbDashStyle.SelectedIndex = 0;
    }

    private void InitializeCanvasSettings()
    {
        // Підписка на подію інтерактивного руху миші над полотном GDI+
        plotCanvas.MouseCoordinatesChanged += OnCanvasMouseCoordinatesChanged;

        // Початкові параметри
        UpdateCanvasProperties();
    }

    private void OnCanvasMouseCoordinatesChanged(double x, double y, double? funcY, int pointsCount)
    {
        lblMouseCoords.Text = $"Координати: X = {x:F2}, Y = {y:F2}";
        lblFunctionValue.Text = funcY.HasValue ? $"f(X) = {funcY.Value:F4}" : "f(X) = —";
        lblPointsCount.Text = $"Точок: {pointsCount}";
    }

    private void UpdateCanvasProperties()
    {
        if (cmbFunction.SelectedItem is FunctionItem selected)
        {
            plotCanvas.MathFunc = selected.Evaluator;
            plotCanvas.FunctionName = selected.Name;
        }

        // Перевірка коректності діапазону [Xmin, Xmax]
        if (nudXMin.Value >= nudXMax.Value)
        {
            nudXMin.Value = nudXMax.Value - 1;
        }

        plotCanvas.XMin = (double)nudXMin.Value;
        plotCanvas.XMax = (double)nudXMax.Value;
        plotCanvas.Step = (double)nudStep.Value;

        plotCanvas.PenColor = lblPenColorPreview.BackColor;
        plotCanvas.PenWidth = (float)nudPenWidth.Value;

        plotCanvas.PenDashStyle = cmbDashStyle.SelectedIndex switch
        {
            1 => DashStyle.Dash,
            2 => DashStyle.Dot,
            3 => DashStyle.DashDot,
            _ => DashStyle.Solid
        };

        plotCanvas.AntiAlias = chkAntiAlias.Checked;
        plotCanvas.ShowGrid = chkShowGrid.Checked;
        plotCanvas.ShowAxes = chkShowAxes.Checked;
        plotCanvas.ShowExtrema = chkShowExtrema.Checked;
        plotCanvas.FillArea = chkFillArea.Checked;
        plotCanvas.ShowGeometricPrimitives = chkGeometricPrimitives.Checked;

        lblStatusInfo.Text = $"Графік [{plotCanvas.FunctionName}] перемальовано засобами GDI+";
    }

    private void OnCanvasParametersChanged(object? sender, EventArgs e)
    {
        UpdateCanvasProperties();
    }

    private void btnSelectPenColor_Click(object? sender, EventArgs e)
    {
        using var dlg = new ColorDialog
        {
            Color = lblPenColorPreview.BackColor,
            FullOpen = true
        };

        if (dlg.ShowDialog(this) == DialogResult.OK)
        {
            lblPenColorPreview.BackColor = dlg.Color;
            plotCanvas.PenColor = dlg.Color;
            lblStatusInfo.Text = $"Колір пера змінено на: R={dlg.Color.R}, G={dlg.Color.G}, B={dlg.Color.B}";
        }
    }

    private void btnResetView_Click(object? sender, EventArgs e)
    {
        nudXMin.Value = -10.0m;
        nudXMax.Value = 10.0m;
        nudStep.Value = 0.05m;
        nudPenWidth.Value = 2m;
        cmbDashStyle.SelectedIndex = 0;
        chkAntiAlias.Checked = true;
        chkShowGrid.Checked = true;
        chkShowAxes.Checked = true;
        chkShowExtrema.Checked = true;
        chkFillArea.Checked = true;
        chkGeometricPrimitives.Checked = false;

        lblPenColorPreview.BackColor = Color.FromArgb(220, 53, 69);
        UpdateCanvasProperties();
        lblStatusInfo.Text = "Параметри та масштаб скинуто до типових значень";
    }

    private void btnExportImage_Click(object? sender, EventArgs e)
    {
        using var saveDlg = new SaveFileDialog
        {
            Title = "Збереження графіка функції GDI+ у растрове зображення",
            Filter = "PNG Зображення (*.png)|*.png|Bitmap Зображення (*.bmp)|*.bmp|JPEG Зображення (*.jpg)|*.jpg",
            FileName = $"plot_{plotCanvas.FunctionName.Replace("/", "_").Replace(" ", "_")}.png",
            DefaultExt = "png"
        };

        if (saveDlg.ShowDialog(this) == DialogResult.OK)
        {
            try
            {
                // Створення зображення високої чіткості на основі поточних розмірів полотна
                int exportWidth = Math.Max(1200, plotCanvas.Width);
                int exportHeight = Math.Max(800, plotCanvas.Height);

                using var bmp = plotCanvas.ExportToBitmap(exportWidth, exportHeight);

                ImageFormat format = Path.GetExtension(saveDlg.FileName).ToLowerInvariant() switch
                {
                    ".bmp" => ImageFormat.Bmp,
                    ".jpg" or ".jpeg" => ImageFormat.Jpeg,
                    _ => ImageFormat.Png
                };

                bmp.Save(saveDlg.FileName, format);

                MessageBox.Show(
                    $"Графік успішно експортовано у файл:\n{saveDlg.FileName}\n\nРоздільна здатність: {exportWidth}x{exportHeight} px",
                    "Експорт завершено",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                lblStatusInfo.Text = $"Графік успішно збережено: {Path.GetFileName(saveDlg.FileName)}";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Помилка під час збереження графічного файлу:\n{ex.Message}",
                    "Помилка експорту",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
