using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace Lab05_GDI;

/// <summary>
/// Елемент керування для візуалізації графіків математичних функцій та геометричних примітивів засобами GDI+.
/// Підтримує подвійну буферизацію (DoubleBuffered) для усунення мерехтіння, перетворення координат,
/// відображення сітки, осей, екстремумів, інтерактивний приціл та експорт зображення.
/// </summary>
public class PlotterCanvas : Control
{
    private Func<double, double>? _mathFunc;
    private string _functionName = "sin(x)";
    private double _xMin = -10.0;
    private double _xMax = 10.0;
    private double _step = 0.05;

    private Color _penColor = Color.FromArgb(220, 53, 69);
    private float _penWidth = 2.0f;
    private DashStyle _penDashStyle = DashStyle.Solid;

    private bool _antiAlias = true;
    private bool _showGrid = true;
    private bool _showAxes = true;
    private bool _showExtrema = true;
    private bool _fillArea = true;
    private bool _showGeometricPrimitives = false;

    // Стан курсора миші для інтерактивності
    private Point _mousePos = Point.Empty;
    private bool _isMouseInside = false;

    // Відступи для побудови області графіка
    private const int MarginLeft = 65;
    private const int MarginRight = 35;
    private const int MarginTop = 35;
    private const int MarginBottom = 45;

    /// <summary>
    /// Подія оновлення математичних координат під курсором (X, Y, f(X), кількість точок).
    /// </summary>
    public event Action<double, double, double?, int>? MouseCoordinatesChanged;

    public PlotterCanvas()
    {
        // Налаштування прапорців оптимізації відмальовування Windows Forms (Double Buffering)
        SetStyle(ControlStyles.AllPaintingInWmPaint |
                 ControlStyles.UserPaint |
                 ControlStyles.OptimizedDoubleBuffer |
                 ControlStyles.ResizeRedraw, true);

        UpdateStyles();
        BackColor = Color.White;
    }

    #region Властивості конфігурації

    public Func<double, double>? MathFunc
    {
        get => _mathFunc;
        set { _mathFunc = value; Invalidate(); }
    }

    public string FunctionName
    {
        get => _functionName;
        set { _functionName = value; Invalidate(); }
    }

    public double XMin
    {
        get => _xMin;
        set { _xMin = value; Invalidate(); }
    }

    public double XMax
    {
        get => _xMax;
        set { _xMax = value; Invalidate(); }
    }

    public double Step
    {
        get => _step;
        set { _step = Math.Max(0.001, value); Invalidate(); }
    }

    public Color PenColor
    {
        get => _penColor;
        set { _penColor = value; Invalidate(); }
    }

    public float PenWidth
    {
        get => _penWidth;
        set { _penWidth = Math.Max(1.0f, value); Invalidate(); }
    }

    public DashStyle PenDashStyle
    {
        get => _penDashStyle;
        set { _penDashStyle = value; Invalidate(); }
    }

    public bool AntiAlias
    {
        get => _antiAlias;
        set { _antiAlias = value; Invalidate(); }
    }

    public bool ShowGrid
    {
        get => _showGrid;
        set { _showGrid = value; Invalidate(); }
    }

    public bool ShowAxes
    {
        get => _showAxes;
        set { _showAxes = value; Invalidate(); }
    }

    public bool ShowExtrema
    {
        get => _showExtrema;
        set { _showExtrema = value; Invalidate(); }
    }

    public bool FillArea
    {
        get => _fillArea;
        set { _fillArea = value; Invalidate(); }
    }

    public bool ShowGeometricPrimitives
    {
        get => _showGeometricPrimitives;
        set { _showGeometricPrimitives = value; Invalidate(); }
    }

    #endregion

    #region Обробка подій миші

    protected override void OnMouseEnter(EventArgs e)
    {
        base.OnMouseEnter(e);
        _isMouseInside = true;
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        _isMouseInside = false;
        Invalidate();
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        _mousePos = e.Location;
        _isMouseInside = true;

        Rectangle plotArea = GetPlotArea(ClientRectangle);
        if (plotArea.Width > 0 && plotArea.Height > 0)
        {
            // Обчислення математичних меж функції для розрахунку світових координат
            GetCalculatedYBounds(out double yMin, out double yMax);
            double worldX = ScreenToWorldX(e.X, plotArea);
            double worldY = ScreenToWorldY(e.Y, plotArea, yMin, yMax);

            double? funcY = null;
            if (_mathFunc != null && worldX >= _xMin && worldX <= _xMax)
            {
                try
                {
                    double val = _mathFunc(worldX);
                    if (!double.IsNaN(val) && !double.IsInfinity(val))
                    {
                        funcY = val;
                    }
                }
                catch
                {
                    funcY = null;
                }
            }

            int pointsCount = (int)Math.Max(2, Math.Ceiling((_xMax - _xMin) / _step) + 1);
            MouseCoordinatesChanged?.Invoke(worldX, worldY, funcY, pointsCount);
        }

        Invalidate();
    }

    #endregion

    #region Перетворення координат (World <-> Screen)

    private Rectangle GetPlotArea(Rectangle bounds)
    {
        int x = bounds.Left + MarginLeft;
        int y = bounds.Top + MarginTop;
        int width = Math.Max(10, bounds.Width - MarginLeft - MarginRight);
        int height = Math.Max(10, bounds.Height - MarginTop - MarginBottom);
        return new Rectangle(x, y, width, height);
    }

    private PointF WorldToScreen(double worldX, double worldY, Rectangle plotArea, double yMin, double yMax)
    {
        double xRange = _xMax - _xMin;
        if (Math.Abs(xRange) < 1e-9) xRange = 1.0;

        double yRange = yMax - yMin;
        if (Math.Abs(yRange) < 1e-9) yRange = 1.0;

        float sx = plotArea.Left + (float)((worldX - _xMin) / xRange * plotArea.Width);
        float sy = plotArea.Bottom - (float)((worldY - yMin) / yRange * plotArea.Height);

        return new PointF(sx, sy);
    }

    private double ScreenToWorldX(float screenX, Rectangle plotArea)
    {
        double xRange = _xMax - _xMin;
        return _xMin + (screenX - plotArea.Left) / plotArea.Width * xRange;
    }

    private double ScreenToWorldY(float screenY, Rectangle plotArea, double yMin, double yMax)
    {
        double yRange = yMax - yMin;
        return yMin + (plotArea.Bottom - screenY) / plotArea.Height * yRange;
    }

    private void GetCalculatedYBounds(out double yMin, out double yMax)
    {
        yMin = -1.0;
        yMax = 1.0;

        if (_mathFunc == null || _xMax <= _xMin) return;

        double calculatedMin = double.MaxValue;
        double calculatedMax = double.MinValue;
        bool foundValid = false;

        for (double x = _xMin; x <= _xMax + 1e-9; x += _step)
        {
            try
            {
                double y = _mathFunc(x);
                if (!double.IsNaN(y) && !double.IsInfinity(y))
                {
                    if (y < calculatedMin) calculatedMin = y;
                    if (y > calculatedMax) calculatedMax = y;
                    foundValid = true;
                }
            }
            catch
            {
                // Пропуск точок розриву
            }
        }

        if (foundValid && calculatedMax > calculatedMin)
        {
            double padding = (calculatedMax - calculatedMin) * 0.15;
            if (padding < 0.1) padding = 0.5;
            yMin = calculatedMin - padding;
            yMax = calculatedMax + padding;
        }
        else
        {
            yMin = -2.0;
            yMax = 2.0;
        }

        // Гарантуємо, що вісь 0 потрапляє в діапазон для наочності
        if (yMin > 0) yMin = -0.5;
        if (yMax < 0) yMax = 0.5;
    }

    #endregion

    #region Відмальовування GDI+ (OnPaint та Render)

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        Render(e.Graphics, ClientRectangle);
    }

    /// <summary>
    /// Головний метод рендерингу графіки за допомогою GDI+.
    /// Може викликатися як для елемента вікна, так і для експорту в растрове зображення Bitmap.
    /// </summary>
    public void Render(Graphics g, Rectangle bounds)
    {
        // 1. Налаштування параметрів якості рендерингу GDI+
        g.SmoothingMode = _antiAlias ? SmoothingMode.AntiAlias : SmoothingMode.None;
        g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
        g.InterpolationMode = InterpolationMode.HighQualityBicubic;

        // Заливка фону полотна
        using (var bgBrush = new SolidBrush(BackColor))
        {
            g.FillRectangle(bgBrush, bounds);
        }

        Rectangle plotArea = GetPlotArea(bounds);
        GetCalculatedYBounds(out double yMin, out double yMax);

        // 2. Рамка області малювання
        using (var borderPen = new Pen(Color.FromArgb(206, 212, 218), 1.0f))
        {
            g.DrawRectangle(borderPen, plotArea);
        }

        // 3. Відмальовування координатної сітки (Grid Lines)
        if (_showGrid)
        {
            DrawGrid(g, plotArea, yMin, yMax);
        }

        // 4. Відмальовування осей координат зі стрілками
        if (_showAxes)
        {
            DrawAxes(g, plotArea, yMin, yMax);
        }

        // 5. Демонстрація геометричних примітивів (якщо увімкнено)
        if (_showGeometricPrimitives)
        {
            DrawGeometricPrimitivesDemo(g, plotArea);
        }

        // 6. Розрахунок точок функції та відмальовування графіка
        if (_mathFunc != null && _xMax > _xMin)
        {
            DrawFunctionPlot(g, plotArea, yMin, yMax);
        }

        // 7. Інтерактивний приціл (Crosshair) під час наведення курсору
        if (_isMouseInside && plotArea.Contains(_mousePos))
        {
            DrawInteractiveCrosshair(g, plotArea, yMin, yMax);
        }

        // 8. Заголовок графіка та легенда у верхньому лівому кутку
        DrawHeaderLegend(g, plotArea);
    }

    private void DrawGrid(Graphics g, Rectangle plotArea, double yMin, double yMax)
    {
        using var gridPen = new Pen(Color.FromArgb(233, 236, 239), 1.0f)
        {
            DashStyle = DashStyle.Dash
        };
        using var textBrush = new SolidBrush(Color.FromArgb(108, 117, 125));
        using var labelFont = new Font("Segoe UI", 8.0f, FontStyle.Regular);

        // Вертикальні лінії сітки (за віссю X)
        double xSpan = _xMax - _xMin;
        double xStepGrid = CalculateOptimalStep(xSpan);

        double firstX = Math.Ceiling(_xMin / xStepGrid) * xStepGrid;
        for (double x = firstX; x <= _xMax; x += xStepGrid)
        {
            PointF pTop = WorldToScreen(x, yMax, plotArea, yMin, yMax);
            PointF pBottom = WorldToScreen(x, yMin, plotArea, yMin, yMax);

            g.DrawLine(gridPen, pTop.X, plotArea.Top, pBottom.X, plotArea.Bottom);

            // Числові підписи осей X
            string label = Math.Abs(x) < 1e-9 ? "0" : x.ToString("0.##");
            SizeF sz = g.MeasureString(label, labelFont);
            g.DrawString(label, labelFont, textBrush, pBottom.X - sz.Width / 2, plotArea.Bottom + 5);
        }

        // Горизонтальні лінії сітки (за віссю Y)
        double ySpan = yMax - yMin;
        double yStepGrid = CalculateOptimalStep(ySpan);

        double firstY = Math.Ceiling(yMin / yStepGrid) * yStepGrid;
        for (double y = firstY; y <= yMax; y += yStepGrid)
        {
            PointF pLeft = WorldToScreen(_xMin, y, plotArea, yMin, yMax);

            g.DrawLine(gridPen, plotArea.Left, pLeft.Y, plotArea.Right, pLeft.Y);

            // Числові підписи осей Y
            string label = Math.Abs(y) < 1e-9 ? "0" : y.ToString("0.##");
            SizeF sz = g.MeasureString(label, labelFont);
            g.DrawString(label, labelFont, textBrush, plotArea.Left - sz.Width - 6, pLeft.Y - sz.Height / 2);
        }
    }

    private void DrawAxes(Graphics g, Rectangle plotArea, double yMin, double yMax)
    {
        using var axisPen = new Pen(Color.FromArgb(52, 58, 64), 1.75f);
        using var arrowCap = new AdjustableArrowCap(4.5f, 7.0f, true);
        axisPen.CustomEndCap = arrowCap;

        using var font = new Font("Segoe UI", 9.0f, FontStyle.Bold);
        using var textBrush = new SolidBrush(Color.FromArgb(33, 37, 41));

        // 1. Вісь X (горизонтальна y = 0)
        PointF pOrigin = WorldToScreen(0, 0, plotArea, yMin, yMax);
        float yAxisScreen = Math.Clamp(pOrigin.Y, (float)plotArea.Top, (float)plotArea.Bottom);

        g.DrawLine(axisPen, plotArea.Left, yAxisScreen, plotArea.Right + 8, yAxisScreen);
        g.DrawString("X", font, textBrush, plotArea.Right + 12, yAxisScreen - 8);

        // 2. Вісь Y (вертикальна x = 0)
        float xAxisScreen = Math.Clamp(pOrigin.X, (float)plotArea.Left, (float)plotArea.Right);

        g.DrawLine(axisPen, xAxisScreen, plotArea.Bottom, xAxisScreen, plotArea.Top - 8);
        g.DrawString("Y", font, textBrush, xAxisScreen - 6, plotArea.Top - 25);
    }

    private void DrawFunctionPlot(Graphics g, Rectangle plotArea, double yMin, double yMax)
    {
        var rawPoints = new List<Tuple<double, double>>();
        var screenPoints = new List<PointF>();

        // Табуляція значень функції
        for (double x = _xMin; x <= _xMax + 1e-9; x += _step)
        {
            try
            {
                double y = _mathFunc!(x);
                if (!double.IsNaN(y) && !double.IsInfinity(y))
                {
                    rawPoints.Add(Tuple.Create(x, y));
                    PointF pt = WorldToScreen(x, y, plotArea, yMin, yMax);
                    screenPoints.Add(pt);
                }
            }
            catch
            {
                // Пропуск точок розриву першого/другого роду
            }
        }

        if (screenPoints.Count < 2) return;

        // 1. Заливка області під графіком (визначений інтеграл) за допомогою LinearGradientBrush
        if (_fillArea && screenPoints.Count >= 2)
        {
            using var fillPath = new GraphicsPath();
            PointF pStartBase = WorldToScreen(rawPoints[0].Item1, 0, plotArea, yMin, yMax);
            PointF pEndBase = WorldToScreen(rawPoints[^1].Item1, 0, plotArea, yMin, yMax);

            fillPath.AddLine(pStartBase, screenPoints[0]);
            for (int i = 0; i < screenPoints.Count - 1; i++)
            {
                fillPath.AddLine(screenPoints[i], screenPoints[i + 1]);
            }
            fillPath.AddLine(screenPoints[^1], pEndBase);
            fillPath.CloseFigure();

            Color topGradColor = Color.FromArgb(85, _penColor.R, _penColor.G, _penColor.B);
            Color bottomGradColor = Color.FromArgb(10, _penColor.R, _penColor.G, _penColor.B);

            using var gradBrush = new LinearGradientBrush(
                new Point(0, plotArea.Top),
                new Point(0, plotArea.Bottom),
                topGradColor,
                bottomGradColor);

            g.FillPath(gradBrush, fillPath);
        }

        // 2. Безпосереднє креслення лінії функції за допомогою Pen
        using (var curvePen = new Pen(_penColor, _penWidth))
        {
            curvePen.DashStyle = _penDashStyle;
            curvePen.LineJoin = LineJoin.Round;
            curvePen.StartCap = LineCap.Round;
            curvePen.EndCap = LineCap.Round;

            // Відсікання графіки за межами робочої області координат
            var oldClip = g.Clip;
            g.SetClip(plotArea);
            g.DrawLines(curvePen, screenPoints.ToArray());
            g.Clip = oldClip;
        }

        // 3. Пошук та візуалізація точок локальних екстремумів
        if (_showExtrema)
        {
            DrawExtremaPoints(g, rawPoints, plotArea, yMin, yMax);
        }
    }

    private void DrawExtremaPoints(Graphics g, List<Tuple<double, double>> points, Rectangle plotArea, double yMin, double yMax)
    {
        using var maxBrush = new SolidBrush(Color.FromArgb(25, 135, 84)); // Зелений для максимуму
        using var minBrush = new SolidBrush(Color.FromArgb(13, 110, 253)); // Синій для мінімуму
        using var strokePen = new Pen(Color.White, 1.5f);
        using var font = new Font("Segoe UI", 7.5f, FontStyle.Bold);
        using var textBrush = new SolidBrush(Color.FromArgb(33, 37, 41));

        for (int i = 1; i < points.Count - 1; i++)
        {
            double prevY = points[i - 1].Item2;
            double curY = points[i].Item2;
            double nextY = points[i + 1].Item2;

            bool isMax = curY > prevY && curY > nextY;
            bool isMin = curY < prevY && curY < nextY;

            if (isMax || isMin)
            {
                double x = points[i].Item1;
                PointF pt = WorldToScreen(x, curY, plotArea, yMin, yMax);

                if (plotArea.Contains((int)pt.X, (int)pt.Y))
                {
                    float radius = 5.0f;
                    RectangleF dotRect = new RectangleF(pt.X - radius, pt.Y - radius, radius * 2, radius * 2);

                    Brush b = isMax ? maxBrush : minBrush;
                    g.FillEllipse(b, dotRect);
                    g.DrawEllipse(strokePen, dotRect);

                    string typeStr = isMax ? "Max" : "Min";
                    string caption = $"{typeStr}({x:F1}; {curY:F2})";
                    float textY = isMax ? pt.Y - 18 : pt.Y + 8;
                    g.DrawString(caption, font, textBrush, pt.X - 25, textY);
                }
            }
        }
    }

    private void DrawInteractiveCrosshair(Graphics g, Rectangle plotArea, double yMin, double yMax)
    {
        using var crossPen = new Pen(Color.FromArgb(140, 108, 117, 125), 1.0f)
        {
            DashStyle = DashStyle.Dot
        };

        // Вертикальна та горизонтальна лінії через позицію миші
        g.DrawLine(crossPen, _mousePos.X, plotArea.Top, _mousePos.X, plotArea.Bottom);
        g.DrawLine(crossPen, plotArea.Left, _mousePos.Y, plotArea.Right, _mousePos.Y);

        // Якщо функція визначена — обчислюємо точку на графіку
        double worldX = ScreenToWorldX(_mousePos.X, plotArea);
        if (_mathFunc != null && worldX >= _xMin && worldX <= _xMax)
        {
            try
            {
                double curY = _mathFunc(worldX);
                if (!double.IsNaN(curY) && !double.IsInfinity(curY))
                {
                    PointF curvePt = WorldToScreen(worldX, curY, plotArea, yMin, yMax);
                    if (plotArea.Contains((int)curvePt.X, (int)curvePt.Y))
                    {
                        using var pointBrush = new SolidBrush(_penColor);
                        using var haloPen = new Pen(Color.FromArgb(160, _penColor), 3.0f);

                        g.DrawEllipse(haloPen, curvePt.X - 7, curvePt.Y - 7, 14, 14);
                        g.FillEllipse(pointBrush, curvePt.X - 4, curvePt.Y - 4, 8, 8);

                        // Інформаційна плашка з координатами
                        string info = $"X = {worldX:F2}\nY = {curY:F3}";
                        using var infoFont = new Font("Segoe UI", 8.0f, FontStyle.Regular);
                        SizeF sz = g.MeasureString(info, infoFont);

                        RectangleF tooltipRect = new RectangleF(curvePt.X + 10, curvePt.Y - 25, sz.Width + 8, sz.Height + 6);
                        using var tipBg = new SolidBrush(Color.FromArgb(235, 255, 255, 255));
                        using var tipBorder = new Pen(Color.FromArgb(180, 180, 180), 1f);

                        g.FillRectangle(tipBg, tooltipRect);
                        g.DrawRectangle(tipBorder, tooltipRect.X, tooltipRect.Y, tooltipRect.Width, tooltipRect.Height);
                        using var tipTextBrush = new SolidBrush(Color.Black);
                        g.DrawString(info, infoFont, tipTextBrush, tooltipRect.X + 4, tooltipRect.Y + 3);
                    }
                }
            }
            catch
            {
                // Ігнорування точок невизначеності
            }
        }
    }

    private void DrawGeometricPrimitivesDemo(Graphics g, Rectangle plotArea)
    {
        // Демонстраційний блок GDI+ примітивів (прямокутник, градієнт, сектор, еліпс, багатокутник)
        int demoWidth = 260;
        int demoHeight = 160;
        int startX = plotArea.Right - demoWidth - 15;
        int startY = plotArea.Top + 15;

        Rectangle demoBox = new Rectangle(startX, startY, demoWidth, demoHeight);

        // 1. Напівпрозора підкладка з тінню
        using (var bgBrush = new SolidBrush(Color.FromArgb(240, 248, 249, 250)))
        using (var boxPen = new Pen(Color.FromArgb(108, 117, 125), 1.5f))
        {
            g.FillRectangle(bgBrush, demoBox);
            g.DrawRectangle(boxPen, demoBox);
        }

        using var fontHeader = new Font("Segoe UI", 8.5f, FontStyle.Bold);
        using var fontSub = new Font("Segoe UI", 7.5f, FontStyle.Regular);
        using var textBrush = new SolidBrush(Color.FromArgb(33, 37, 41));

        g.DrawString("GDI+ Geometric Primitives Demo:", fontHeader, textBrush, startX + 8, startY + 6);

        // 2. Прямокутник із лінійним двоколірним градієнтом (LinearGradientBrush)
        Rectangle gradRect = new Rectangle(startX + 12, startY + 32, 60, 45);
        using (var lgb = new LinearGradientBrush(gradRect, Color.OrangeRed, Color.Gold, 45f))
        using (var gradPen = new Pen(Color.DarkRed, 1.2f))
        {
            g.FillRectangle(lgb, gradRect);
            g.DrawRectangle(gradPen, gradRect);
            g.DrawString("Gradient", fontSub, textBrush, gradRect.Left + 8, gradRect.Bottom + 2);
        }

        // 3. Еліпс/коло із суцільною заливкою та контуром
        Rectangle ellipseRect = new Rectangle(startX + 95, startY + 32, 60, 45);
        using (var ellBrush = new SolidBrush(Color.FromArgb(180, 13, 202, 240)))
        using (var ellPen = new Pen(Color.FromArgb(13, 110, 253), 1.5f))
        {
            g.FillEllipse(ellBrush, ellipseRect);
            g.DrawEllipse(ellPen, ellipseRect);
            g.DrawString("Ellipse", fontSub, textBrush, ellipseRect.Left + 12, ellipseRect.Bottom + 2);
        }

        // 4. Багатокутник (зірка/трикутник) за допомогою FillPolygon та DrawPolygon
        PointF[] starPoints = new PointF[]
        {
            new PointF(startX + 205, startY + 32),
            new PointF(startX + 218, startY + 50),
            new PointF(startX + 238, startY + 52),
            new PointF(startX + 222, startY + 65),
            new PointF(startX + 227, startY + 85),
            new PointF(startX + 205, startY + 73),
            new PointF(startX + 183, startY + 85),
            new PointF(startX + 188, startY + 65),
            new PointF(startX + 172, startY + 52),
            new PointF(startX + 192, startY + 50)
        };

        using (var polyBrush = new SolidBrush(Color.FromArgb(200, 255, 193, 7)))
        using (var polyPen = new Pen(Color.DarkGoldenrod, 1.2f))
        {
            g.FillPolygon(polyBrush, starPoints);
            g.DrawPolygon(polyPen, starPoints);
            g.DrawString("Polygon", fontSub, textBrush, startX + 185, startY + 87);
        }

        // 5. Круговий сектор (Pie)
        Rectangle pieRect = new Rectangle(startX + 12, startY + 98, 50, 50);
        using (var pieBrush = new SolidBrush(Color.FromArgb(170, 111, 66, 193)))
        using (var piePen = new Pen(Color.Indigo, 1.2f))
        {
            g.FillPie(pieBrush, pieRect, 30f, 270f);
            g.DrawPie(piePen, pieRect, 30f, 270f);
            g.DrawString("Pie Slice", fontSub, textBrush, pieRect.Right + 5, pieRect.Top + 16);
        }
    }

    private void DrawHeaderLegend(Graphics g, Rectangle plotArea)
    {
        string title = $"f(x) = {_functionName}";
        using var fontTitle = new Font("Segoe UI", 10.5f, FontStyle.Bold);
        using var textBrush = new SolidBrush(Color.FromArgb(33, 37, 41));
        using var linePen = new Pen(_penColor, 3f) { DashStyle = _penDashStyle };

        float x = plotArea.Left + 15;
        float y = plotArea.Top + 12;

        // Зразок лінії в легенді
        g.DrawLine(linePen, x, y + 8, x + 30, y + 8);
        g.DrawString(title, fontTitle, textBrush, x + 38, y);
    }

    private static double CalculateOptimalStep(double span)
    {
        double roughStep = span / 8.0;
        double magnitude = Math.Pow(10, Math.Floor(Math.Log10(roughStep)));
        double normalized = roughStep / magnitude;

        double step;
        if (normalized < 1.5) step = 1.0;
        else if (normalized < 3.0) step = 2.0;
        else if (normalized < 7.0) step = 5.0;
        else step = 10.0;

        return step * magnitude;
    }

    #endregion

    #region Експорт у зображення

    /// <summary>
    /// Рендерить повний графік у растрове зображення Bitmap заданих розмірів.
    /// </summary>
    public Bitmap ExportToBitmap(int width, int height)
    {
        var bmp = new Bitmap(width, height);
        using (var g = Graphics.FromImage(bmp))
        {
            Render(g, new Rectangle(0, 0, width, height));
        }
        return bmp;
    }

    #endregion
}
