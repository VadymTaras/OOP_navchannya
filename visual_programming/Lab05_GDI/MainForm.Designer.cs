namespace Lab05_GDI;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.statusStrip = new System.Windows.Forms.StatusStrip();
        this.lblMouseCoords = new System.Windows.Forms.ToolStripStatusLabel();
        this.lblFunctionValue = new System.Windows.Forms.ToolStripStatusLabel();
        this.lblPointsCount = new System.Windows.Forms.ToolStripStatusLabel();
        this.lblStatusInfo = new System.Windows.Forms.ToolStripStatusLabel();
        this.panelSidebar = new System.Windows.Forms.Panel();
        this.gbExport = new System.Windows.Forms.GroupBox();
        this.btnExportImage = new System.Windows.Forms.Button();
        this.btnResetView = new System.Windows.Forms.Button();
        this.gbPrimitives = new System.Windows.Forms.GroupBox();
        this.chkGeometricPrimitives = new System.Windows.Forms.CheckBox();
        this.chkFillArea = new System.Windows.Forms.CheckBox();
        this.chkShowExtrema = new System.Windows.Forms.CheckBox();
        this.chkShowAxes = new System.Windows.Forms.CheckBox();
        this.chkShowGrid = new System.Windows.Forms.CheckBox();
        this.chkAntiAlias = new System.Windows.Forms.CheckBox();
        this.gbPenStyle = new System.Windows.Forms.GroupBox();
        this.lblPenColorPreview = new System.Windows.Forms.Label();
        this.btnSelectPenColor = new System.Windows.Forms.Button();
        this.lblPenWidth = new System.Windows.Forms.Label();
        this.nudPenWidth = new System.Windows.Forms.NumericUpDown();
        this.lblDashStyle = new System.Windows.Forms.Label();
        this.cmbDashStyle = new System.Windows.Forms.ComboBox();
        this.gbFunction = new System.Windows.Forms.GroupBox();
        this.lblFunction = new System.Windows.Forms.Label();
        this.cmbFunction = new System.Windows.Forms.ComboBox();
        this.lblXMin = new System.Windows.Forms.Label();
        this.nudXMin = new System.Windows.Forms.NumericUpDown();
        this.lblXMax = new System.Windows.Forms.Label();
        this.nudXMax = new System.Windows.Forms.NumericUpDown();
        this.lblStep = new System.Windows.Forms.Label();
        this.nudStep = new System.Windows.Forms.NumericUpDown();
        this.plotCanvas = new Lab05_GDI.PlotterCanvas();
        this.statusStrip.SuspendLayout();
        this.panelSidebar.SuspendLayout();
        this.gbExport.SuspendLayout();
        this.gbPrimitives.SuspendLayout();
        this.gbPenStyle.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.nudPenWidth)).BeginInit();
        this.gbFunction.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.nudXMin)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.nudXMax)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.nudStep)).BeginInit();
        this.SuspendLayout();
        // 
        // statusStrip
        // 
        this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
        this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblMouseCoords,
            this.lblFunctionValue,
            this.lblPointsCount,
            this.lblStatusInfo});
        this.statusStrip.Location = new System.Drawing.Point(0, 715);
        this.statusStrip.Name = "statusStrip";
        this.statusStrip.Size = new System.Drawing.Size(1182, 26);
        this.statusStrip.TabIndex = 0;
        // 
        // lblMouseCoords
        // 
        this.lblMouseCoords.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
        this.lblMouseCoords.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.lblMouseCoords.Name = "lblMouseCoords";
        this.lblMouseCoords.Size = new System.Drawing.Size(180, 20);
        this.lblMouseCoords.Text = "Координати: X = 0.00, Y = 0.00";
        // 
        // lblFunctionValue
        // 
        this.lblFunctionValue.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
        this.lblFunctionValue.Name = "lblFunctionValue";
        this.lblFunctionValue.Size = new System.Drawing.Size(120, 20);
        this.lblFunctionValue.Text = "f(X) = 0.0000";
        // 
        // lblPointsCount
        // 
        this.lblPointsCount.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
        this.lblPointsCount.Name = "lblPointsCount";
        this.lblPointsCount.Size = new System.Drawing.Size(110, 20);
        this.lblPointsCount.Text = "Точок: 0";
        // 
        // lblStatusInfo
        // 
        this.lblStatusInfo.Name = "lblStatusInfo";
        this.lblStatusInfo.Size = new System.Drawing.Size(185, 20);
        this.lblStatusInfo.Text = "Готово до малювання GDI+";
        // 
        // panelSidebar
        // 
        this.panelSidebar.AutoScroll = true;
        this.panelSidebar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
        this.panelSidebar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.panelSidebar.Controls.Add(this.gbExport);
        this.panelSidebar.Controls.Add(this.gbPrimitives);
        this.panelSidebar.Controls.Add(this.gbPenStyle);
        this.panelSidebar.Controls.Add(this.gbFunction);
        this.panelSidebar.Dock = System.Windows.Forms.DockStyle.Left;
        this.panelSidebar.Location = new System.Drawing.Point(0, 0);
        this.panelSidebar.Name = "panelSidebar";
        this.panelSidebar.Padding = new System.Windows.Forms.Padding(12);
        this.panelSidebar.Size = new System.Drawing.Size(340, 715);
        this.panelSidebar.TabIndex = 1;
        // 
        // gbExport
        // 
        this.gbExport.Controls.Add(this.btnExportImage);
        this.gbExport.Controls.Add(this.btnResetView);
        this.gbExport.Dock = System.Windows.Forms.DockStyle.Top;
        this.gbExport.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.gbExport.Location = new System.Drawing.Point(12, 572);
        this.gbExport.Name = "gbExport";
        this.gbExport.Size = new System.Drawing.Size(314, 125);
        this.gbExport.TabIndex = 3;
        this.gbExport.TabStop = false;
        this.gbExport.Text = "4. Експорт та масштабування";
        // 
        // btnExportImage
        // 
        this.btnExportImage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(110)))), ((int)(((byte)(253)))));
        this.btnExportImage.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnExportImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.btnExportImage.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.btnExportImage.ForeColor = System.Drawing.Color.White;
        this.btnExportImage.Location = new System.Drawing.Point(12, 73);
        this.btnExportImage.Name = "btnExportImage";
        this.btnExportImage.Size = new System.Drawing.Size(288, 38);
        this.btnExportImage.TabIndex = 1;
        this.btnExportImage.Text = "Експортувати у файл (PNG/BMP)...";
        this.btnExportImage.UseVisualStyleBackColor = false;
        this.btnExportImage.Click += new System.EventHandler(this.btnExportImage_Click);
        // 
        // btnResetView
        // 
        this.btnResetView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
        this.btnResetView.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnResetView.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.btnResetView.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.btnResetView.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
        this.btnResetView.Location = new System.Drawing.Point(12, 28);
        this.btnResetView.Name = "btnResetView";
        this.btnResetView.Size = new System.Drawing.Size(288, 35);
        this.btnResetView.TabIndex = 0;
        this.btnResetView.Text = "Скинути діапазон до типового";
        this.btnResetView.UseVisualStyleBackColor = false;
        this.btnResetView.Click += new System.EventHandler(this.btnResetView_Click);
        // 
        // gbPrimitives
        // 
        this.gbPrimitives.Controls.Add(this.chkGeometricPrimitives);
        this.gbPrimitives.Controls.Add(this.chkFillArea);
        this.gbPrimitives.Controls.Add(this.chkShowExtrema);
        this.gbPrimitives.Controls.Add(this.chkShowAxes);
        this.gbPrimitives.Controls.Add(this.chkShowGrid);
        this.gbPrimitives.Controls.Add(this.chkAntiAlias);
        this.gbPrimitives.Dock = System.Windows.Forms.DockStyle.Top;
        this.gbPrimitives.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.gbPrimitives.Location = new System.Drawing.Point(12, 382);
        this.gbPrimitives.Name = "gbPrimitives";
        this.gbPrimitives.Size = new System.Drawing.Size(314, 190);
        this.gbPrimitives.TabIndex = 2;
        this.gbPrimitives.TabStop = false;
        this.gbPrimitives.Text = "3. Примітиви та візуальні опції";
        // 
        // chkGeometricPrimitives
        // 
        this.chkGeometricPrimitives.AutoSize = true;
        this.chkGeometricPrimitives.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.chkGeometricPrimitives.Location = new System.Drawing.Point(15, 155);
        this.chkGeometricPrimitives.Name = "chkGeometricPrimitives";
        this.chkGeometricPrimitives.Size = new System.Drawing.Size(251, 24);
        this.chkGeometricPrimitives.TabIndex = 5;
        this.chkGeometricPrimitives.Text = "Геометричні фігури (GDI+ демо)";
        this.chkGeometricPrimitives.UseVisualStyleBackColor = true;
        this.chkGeometricPrimitives.CheckedChanged += new System.EventHandler(this.OnCanvasParametersChanged);
        // 
        // chkFillArea
        // 
        this.chkFillArea.AutoSize = true;
        this.chkFillArea.Checked = true;
        this.chkFillArea.CheckState = System.Windows.Forms.CheckState.Checked;
        this.chkFillArea.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.chkFillArea.Location = new System.Drawing.Point(15, 129);
        this.chkFillArea.Name = "chkFillArea";
        this.chkFillArea.Size = new System.Drawing.Size(232, 24);
        this.chkFillArea.TabIndex = 4;
        this.chkFillArea.Text = "Градієнтна заливка під кривою";
        this.chkFillArea.UseVisualStyleBackColor = true;
        this.chkFillArea.CheckedChanged += new System.EventHandler(this.OnCanvasParametersChanged);
        // 
        // chkShowExtrema
        // 
        this.chkShowExtrema.AutoSize = true;
        this.chkShowExtrema.Checked = true;
        this.chkShowExtrema.CheckState = System.Windows.Forms.CheckState.Checked;
        this.chkShowExtrema.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.chkShowExtrema.Location = new System.Drawing.Point(15, 103);
        this.chkShowExtrema.Name = "chkShowExtrema";
        this.chkShowExtrema.Size = new System.Drawing.Size(242, 24);
        this.chkShowExtrema.TabIndex = 3;
        this.chkShowExtrema.Text = "Позначки екстремумів (Min/Max)";
        this.chkShowExtrema.UseVisualStyleBackColor = true;
        this.chkShowExtrema.CheckedChanged += new System.EventHandler(this.OnCanvasParametersChanged);
        // 
        // chkShowAxes
        // 
        this.chkShowAxes.AutoSize = true;
        this.chkShowAxes.Checked = true;
        this.chkShowAxes.CheckState = System.Windows.Forms.CheckState.Checked;
        this.chkShowAxes.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.chkShowAxes.Location = new System.Drawing.Point(15, 77);
        this.chkShowAxes.Name = "chkShowAxes";
        this.chkShowAxes.Size = new System.Drawing.Size(223, 24);
        this.chkShowAxes.TabIndex = 2;
        this.chkShowAxes.Text = "Осі координат (OX, OY стрілки)";
        this.chkShowAxes.UseVisualStyleBackColor = true;
        this.chkShowAxes.CheckedChanged += new System.EventHandler(this.OnCanvasParametersChanged);
        // 
        // chkShowGrid
        // 
        this.chkShowGrid.AutoSize = true;
        this.chkShowGrid.Checked = true;
        this.chkShowGrid.CheckState = System.Windows.Forms.CheckState.Checked;
        this.chkShowGrid.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.chkShowGrid.Location = new System.Drawing.Point(15, 51);
        this.chkShowGrid.Name = "chkShowGrid";
        this.chkShowGrid.Size = new System.Drawing.Size(217, 24);
        this.chkShowGrid.TabIndex = 1;
        this.chkShowGrid.Text = "Координатна сітка (Grid lines)";
        this.chkShowGrid.UseVisualStyleBackColor = true;
        this.chkShowGrid.CheckedChanged += new System.EventHandler(this.OnCanvasParametersChanged);
        // 
        // chkAntiAlias
        // 
        this.chkAntiAlias.AutoSize = true;
        this.chkAntiAlias.Checked = true;
        this.chkAntiAlias.CheckState = System.Windows.Forms.CheckState.Checked;
        this.chkAntiAlias.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.chkAntiAlias.Location = new System.Drawing.Point(15, 25);
        this.chkAntiAlias.Name = "chkAntiAlias";
        this.chkAntiAlias.Size = new System.Drawing.Size(206, 24);
        this.chkAntiAlias.TabIndex = 0;
        this.chkAntiAlias.Text = "Згладжування (AntiAliasing)";
        this.chkAntiAlias.UseVisualStyleBackColor = true;
        this.chkAntiAlias.CheckedChanged += new System.EventHandler(this.OnCanvasParametersChanged);
        // 
        // gbPenStyle
        // 
        this.gbPenStyle.Controls.Add(this.lblPenColorPreview);
        this.gbPenStyle.Controls.Add(this.btnSelectPenColor);
        this.gbPenStyle.Controls.Add(this.lblPenWidth);
        this.gbPenStyle.Controls.Add(this.nudPenWidth);
        this.gbPenStyle.Controls.Add(this.lblDashStyle);
        this.gbPenStyle.Controls.Add(this.cmbDashStyle);
        this.gbPenStyle.Dock = System.Windows.Forms.DockStyle.Top;
        this.gbPenStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.gbPenStyle.Location = new System.Drawing.Point(12, 218);
        this.gbPenStyle.Name = "gbPenStyle";
        this.gbPenStyle.Size = new System.Drawing.Size(314, 164);
        this.gbPenStyle.TabIndex = 1;
        this.gbPenStyle.TabStop = false;
        this.gbPenStyle.Text = "2. Налаштування пера (Pen)";
        // 
        // lblPenColorPreview
        // 
        this.lblPenColorPreview.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
        this.lblPenColorPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.lblPenColorPreview.Location = new System.Drawing.Point(15, 30);
        this.lblPenColorPreview.Name = "lblPenColorPreview";
        this.lblPenColorPreview.Size = new System.Drawing.Size(36, 28);
        this.lblPenColorPreview.TabIndex = 0;
        // 
        // btnSelectPenColor
        // 
        this.btnSelectPenColor.BackColor = System.Drawing.Color.White;
        this.btnSelectPenColor.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnSelectPenColor.FlatStyle = System.Windows.Forms.FlatStyle.System;
        this.btnSelectPenColor.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.btnSelectPenColor.Location = new System.Drawing.Point(58, 29);
        this.btnSelectPenColor.Name = "btnSelectPenColor";
        this.btnSelectPenColor.Size = new System.Drawing.Size(242, 30);
        this.btnSelectPenColor.TabIndex = 1;
        this.btnSelectPenColor.Text = "Обрати колір пера...";
        this.btnSelectPenColor.UseVisualStyleBackColor = false;
        this.btnSelectPenColor.Click += new System.EventHandler(this.btnSelectPenColor_Click);
        // 
        // lblPenWidth
        // 
        this.lblPenWidth.AutoSize = true;
        this.lblPenWidth.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.lblPenWidth.Location = new System.Drawing.Point(15, 71);
        this.lblPenWidth.Name = "lblPenWidth";
        this.lblPenWidth.Size = new System.Drawing.Size(130, 20);
        this.lblPenWidth.TabIndex = 2;
        this.lblPenWidth.Text = "Товщина лінії (px):";
        // 
        // nudPenWidth
        // 
        this.nudPenWidth.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.nudPenWidth.Location = new System.Drawing.Point(165, 68);
        this.nudPenWidth.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
        this.nudPenWidth.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        this.nudPenWidth.Name = "nudPenWidth";
        this.nudPenWidth.Size = new System.Drawing.Size(135, 27);
        this.nudPenWidth.TabIndex = 3;
        this.nudPenWidth.Value = new decimal(new int[] { 2, 0, 0, 0 });
        this.nudPenWidth.ValueChanged += new System.EventHandler(this.OnCanvasParametersChanged);
        // 
        // lblDashStyle
        // 
        this.lblDashStyle.AutoSize = true;
        this.lblDashStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.lblDashStyle.Location = new System.Drawing.Point(15, 114);
        this.lblDashStyle.Name = "lblDashStyle";
        this.lblDashStyle.Size = new System.Drawing.Size(90, 20);
        this.lblDashStyle.TabIndex = 4;
        this.lblDashStyle.Text = "Стиль лінії:";
        // 
        // cmbDashStyle
        // 
        this.cmbDashStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cmbDashStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.cmbDashStyle.FormattingEnabled = true;
        this.cmbDashStyle.Items.AddRange(new object[] {
            "Solid (Суцільна)",
            "Dash (Штрихова)",
            "Dot (Пунктирна)",
            "DashDot (Штрих-пунктир)"});
        this.cmbDashStyle.Location = new System.Drawing.Point(120, 111);
        this.cmbDashStyle.Name = "cmbDashStyle";
        this.cmbDashStyle.Size = new System.Drawing.Size(180, 28);
        this.cmbDashStyle.TabIndex = 5;
        this.cmbDashStyle.SelectedIndexChanged += new System.EventHandler(this.OnCanvasParametersChanged);
        // 
        // gbFunction
        // 
        this.gbFunction.Controls.Add(this.lblFunction);
        this.gbFunction.Controls.Add(this.cmbFunction);
        this.gbFunction.Controls.Add(this.lblXMin);
        this.gbFunction.Controls.Add(this.nudXMin);
        this.gbFunction.Controls.Add(this.lblXMax);
        this.gbFunction.Controls.Add(this.nudXMax);
        this.gbFunction.Controls.Add(this.lblStep);
        this.gbFunction.Controls.Add(this.nudStep);
        this.gbFunction.Dock = System.Windows.Forms.DockStyle.Top;
        this.gbFunction.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.gbFunction.Location = new System.Drawing.Point(12, 12);
        this.gbFunction.Name = "gbFunction";
        this.gbFunction.Size = new System.Drawing.Size(314, 206);
        this.gbFunction.TabIndex = 0;
        this.gbFunction.TabStop = false;
        this.gbFunction.Text = "1. Математична функція";
        // 
        // lblFunction
        // 
        this.lblFunction.AutoSize = true;
        this.lblFunction.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.lblFunction.Location = new System.Drawing.Point(15, 27);
        this.lblFunction.Name = "lblFunction";
        this.lblFunction.Size = new System.Drawing.Size(68, 20);
        this.lblFunction.TabIndex = 0;
        this.lblFunction.Text = "Функція:";
        // 
        // cmbFunction
        // 
        this.cmbFunction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cmbFunction.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.cmbFunction.FormattingEnabled = true;
        this.cmbFunction.Location = new System.Drawing.Point(15, 51);
        this.cmbFunction.Name = "cmbFunction";
        this.cmbFunction.Size = new System.Drawing.Size(285, 28);
        this.cmbFunction.TabIndex = 1;
        this.cmbFunction.SelectedIndexChanged += new System.EventHandler(this.OnCanvasParametersChanged);
        // 
        // lblXMin
        // 
        this.lblXMin.AutoSize = true;
        this.lblXMin.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.lblXMin.Location = new System.Drawing.Point(15, 93);
        this.lblXMin.Name = "lblXMin";
        this.lblXMin.Size = new System.Drawing.Size(46, 20);
        this.lblXMin.TabIndex = 2;
        this.lblXMin.Text = "X min:";
        // 
        // nudXMin
        // 
        this.nudXMin.DecimalPlaces = 1;
        this.nudXMin.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.nudXMin.Increment = new decimal(new int[] { 5, 0, 0, 65536 });
        this.nudXMin.Location = new System.Drawing.Point(67, 90);
        this.nudXMin.Maximum = new decimal(new int[] { 0, 0, 0, 0 });
        this.nudXMin.Minimum = new decimal(new int[] { 100, 0, 0, -2147483648 });
        this.nudXMin.Name = "nudXMin";
        this.nudXMin.Size = new System.Drawing.Size(80, 27);
        this.nudXMin.TabIndex = 3;
        this.nudXMin.Value = new decimal(new int[] { 10, 0, 0, -2147483648 });
        this.nudXMin.ValueChanged += new System.EventHandler(this.OnCanvasParametersChanged);
        // 
        // lblXMax
        // 
        this.lblXMax.AutoSize = true;
        this.lblXMax.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.lblXMax.Location = new System.Drawing.Point(165, 93);
        this.lblXMax.Name = "lblXMax";
        this.lblXMax.Size = new System.Drawing.Size(49, 20);
        this.lblXMax.TabIndex = 4;
        this.lblXMax.Text = "X max:";
        // 
        // nudXMax
        // 
        this.nudXMax.DecimalPlaces = 1;
        this.nudXMax.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.nudXMax.Increment = new decimal(new int[] { 5, 0, 0, 65536 });
        this.nudXMax.Location = new System.Drawing.Point(220, 90);
        this.nudXMax.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
        this.nudXMax.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        this.nudXMax.Name = "nudXMax";
        this.nudXMax.Size = new System.Drawing.Size(80, 27);
        this.nudXMax.TabIndex = 5;
        this.nudXMax.Value = new decimal(new int[] { 10, 0, 0, 0 });
        this.nudXMax.ValueChanged += new System.EventHandler(this.OnCanvasParametersChanged);
        // 
        // lblStep
        // 
        this.lblStep.AutoSize = true;
        this.lblStep.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.lblStep.Location = new System.Drawing.Point(15, 142);
        this.lblStep.Name = "lblStep";
        this.lblStep.Size = new System.Drawing.Size(121, 20);
        this.lblStep.TabIndex = 6;
        this.lblStep.Text = "Крок табуляції h:";
        // 
        // nudStep
        // 
        this.nudStep.DecimalPlaces = 3;
        this.nudStep.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.nudStep.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
        this.nudStep.Location = new System.Drawing.Point(165, 140);
        this.nudStep.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
        this.nudStep.Minimum = new decimal(new int[] { 1, 0, 0, 196608 });
        this.nudStep.Name = "nudStep";
        this.nudStep.Size = new System.Drawing.Size(135, 27);
        this.nudStep.TabIndex = 7;
        this.nudStep.Value = new decimal(new int[] { 5, 0, 0, 131072 });
        this.nudStep.ValueChanged += new System.EventHandler(this.OnCanvasParametersChanged);
        // 
        // plotCanvas
        // 
        this.plotCanvas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
        this.plotCanvas.Cursor = System.Windows.Forms.Cursors.Cross;
        this.plotCanvas.Dock = System.Windows.Forms.DockStyle.Fill;
        this.plotCanvas.Location = new System.Drawing.Point(340, 0);
        this.plotCanvas.Name = "plotCanvas";
        this.plotCanvas.Size = new System.Drawing.Size(842, 715);
        this.plotCanvas.TabIndex = 2;
        // 
        // MainForm
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(1182, 741);
        this.Controls.Add(this.plotCanvas);
        this.Controls.Add(this.panelSidebar);
        this.Controls.Add(this.statusStrip);
        this.MinimumSize = new System.Drawing.Size(950, 650);
        this.Name = "MainForm";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "Практична робота 5: Scientific Function & Geometric Graphics Plotter (GDI+) — Тарас Вадим, аІк43";
        this.statusStrip.ResumeLayout(false);
        this.statusStrip.PerformLayout();
        this.panelSidebar.ResumeLayout(false);
        this.gbExport.ResumeLayout(false);
        this.gbPrimitives.ResumeLayout(false);
        this.gbPrimitives.PerformLayout();
        this.gbPenStyle.ResumeLayout(false);
        this.gbPenStyle.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.nudPenWidth)).EndInit();
        this.gbFunction.ResumeLayout(false);
        this.gbFunction.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.nudXMin)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.nudXMax)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.nudStep)).EndInit();
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private System.Windows.Forms.StatusStrip statusStrip;
    private System.Windows.Forms.ToolStripStatusLabel lblMouseCoords;
    private System.Windows.Forms.ToolStripStatusLabel lblFunctionValue;
    private System.Windows.Forms.ToolStripStatusLabel lblPointsCount;
    private System.Windows.Forms.ToolStripStatusLabel lblStatusInfo;
    private System.Windows.Forms.Panel panelSidebar;
    private System.Windows.Forms.GroupBox gbFunction;
    private System.Windows.Forms.Label lblFunction;
    private System.Windows.Forms.ComboBox cmbFunction;
    private System.Windows.Forms.Label lblXMin;
    private System.Windows.Forms.NumericUpDown nudXMin;
    private System.Windows.Forms.Label lblXMax;
    private System.Windows.Forms.NumericUpDown nudXMax;
    private System.Windows.Forms.Label lblStep;
    private System.Windows.Forms.NumericUpDown nudStep;
    private System.Windows.Forms.GroupBox gbPenStyle;
    private System.Windows.Forms.Label lblPenColorPreview;
    private System.Windows.Forms.Button btnSelectPenColor;
    private System.Windows.Forms.Label lblPenWidth;
    private System.Windows.Forms.NumericUpDown nudPenWidth;
    private System.Windows.Forms.Label lblDashStyle;
    private System.Windows.Forms.ComboBox cmbDashStyle;
    private System.Windows.Forms.GroupBox gbPrimitives;
    private System.Windows.Forms.CheckBox chkAntiAlias;
    private System.Windows.Forms.CheckBox chkShowGrid;
    private System.Windows.Forms.CheckBox chkShowAxes;
    private System.Windows.Forms.CheckBox chkShowExtrema;
    private System.Windows.Forms.CheckBox chkFillArea;
    private System.Windows.Forms.CheckBox chkGeometricPrimitives;
    private System.Windows.Forms.GroupBox gbExport;
    private System.Windows.Forms.Button btnExportImage;
    private System.Windows.Forms.Button btnResetView;
    private Lab05_GDI.PlotterCanvas plotCanvas;
}
