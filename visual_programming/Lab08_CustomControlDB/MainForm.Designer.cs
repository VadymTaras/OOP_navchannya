namespace Lab08_CustomControlDB
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            pnlHeader = new Panel();
            lblHeaderSub = new Label();
            lblHeaderTitle = new Label();
            pnlToolBar = new Panel();
            btnSimulateSpike = new Button();
            btnStep = new Button();
            numInterval = new NumericUpDown();
            lblInterval = new Label();
            btnStartSimulation = new Button();
            splitContainer = new SplitContainer();
            flowCardsPanel = new FlowLayoutPanel();
            tabControl = new TabControl();
            tabHistory = new TabPage();
            dgvTelemetry = new DataGridView();
            pnlFilterBar = new Panel();
            btnClearDb = new Button();
            btnExportCsv = new Button();
            btnResetFilter = new Button();
            btnApplyFilter = new Button();
            chkOnlyAlerts = new CheckBox();
            cmbFilterStatus = new ComboBox();
            lblFilterStatus = new Label();
            cmbFilterSensor = new ComboBox();
            lblFilterSensor = new Label();
            tabLog = new TabPage();
            txtAlertLog = new RichTextBox();
            pnlLogActions = new Panel();
            btnClearLog = new Button();
            lblLogCount = new Label();
            statusStrip = new StatusStrip();
            statusSensorsLabel = new ToolStripStatusLabel();
            statusRecordsLabel = new ToolStripStatusLabel();
            statusAlertsLabel = new ToolStripStatusLabel();
            statusSimulationLabel = new ToolStripStatusLabel();
            simulationTimer = new System.Windows.Forms.Timer(components);
            pnlHeader.SuspendLayout();
            pnlToolBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numInterval).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
            splitContainer.Panel1.SuspendLayout();
            splitContainer.Panel2.SuspendLayout();
            splitContainer.SuspendLayout();
            tabControl.SuspendLayout();
            tabHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTelemetry).BeginInit();
            pnlFilterBar.SuspendLayout();
            tabLog.SuspendLayout();
            pnlLogActions.SuspendLayout();
            statusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(15, 23, 42);
            pnlHeader.Controls.Add(lblHeaderSub);
            pnlHeader.Controls.Add(lblHeaderTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1184, 60);
            pnlHeader.TabIndex = 0;
            // 
            // lblHeaderSub
            // 
            lblHeaderSub.AutoSize = true;
            lblHeaderSub.Font = new Font("Segoe UI", 9F);
            lblHeaderSub.ForeColor = Color.FromArgb(148, 163, 184);
            lblHeaderSub.Location = new Point(14, 34);
            lblHeaderSub.Name = "lblHeaderSub";
            lblHeaderSub.Size = new Size(620, 15);
            lblHeaderSub.TabIndex = 1;
            lblHeaderSub.Text = "Практична робота 8 | Спеціальність 121 «Інженерія ПЗ» | Студент: ТАРАС Вадим (аІк43) | Керівник: КОСТІКОВ О.А.";
            // 
            // lblHeaderTitle
            // 
            lblHeaderTitle.AutoSize = true;
            lblHeaderTitle.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblHeaderTitle.ForeColor = Color.White;
            lblHeaderTitle.Location = new Point(12, 9);
            lblHeaderTitle.Name = "lblHeaderTitle";
            lblHeaderTitle.Size = new Size(490, 25);
            lblHeaderTitle.TabIndex = 0;
            lblHeaderTitle.Text = "Hardware Telemetry Dashboard & Custom Controls";
            // 
            // pnlToolBar
            // 
            pnlToolBar.BackColor = Color.FromArgb(241, 245, 249);
            pnlToolBar.Controls.Add(btnSimulateSpike);
            pnlToolBar.Controls.Add(btnStep);
            pnlToolBar.Controls.Add(numInterval);
            pnlToolBar.Controls.Add(lblInterval);
            pnlToolBar.Controls.Add(btnStartSimulation);
            pnlToolBar.Dock = DockStyle.Top;
            pnlToolBar.Location = new Point(0, 60);
            pnlToolBar.Name = "pnlToolBar";
            pnlToolBar.Size = new Size(1184, 46);
            pnlToolBar.TabIndex = 1;
            // 
            // btnSimulateSpike
            // 
            btnSimulateSpike.BackColor = Color.FromArgb(220, 38, 38);
            btnSimulateSpike.FlatStyle = FlatStyle.Flat;
            btnSimulateSpike.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnSimulateSpike.ForeColor = Color.White;
            btnSimulateSpike.Location = new Point(460, 8);
            btnSimulateSpike.Name = "btnSimulateSpike";
            btnSimulateSpike.Size = new Size(160, 30);
            btnSimulateSpike.TabIndex = 4;
            btnSimulateSpike.Text = "⚡ Спровокувати аномалію";
            btnSimulateSpike.UseVisualStyleBackColor = false;
            // 
            // btnStep
            // 
            btnStep.BackColor = Color.FromArgb(226, 232, 240);
            btnStep.FlatStyle = FlatStyle.Flat;
            btnStep.Font = new Font("Segoe UI", 9F);
            btnStep.Location = new Point(180, 8);
            btnStep.Name = "btnStep";
            btnStep.Size = new Size(100, 30);
            btnStep.TabIndex = 3;
            btnStep.Text = "⏭ Один такт";
            btnStep.UseVisualStyleBackColor = false;
            // 
            // numInterval
            // 
            numInterval.Increment = new decimal(new int[] { 250, 0, 0, 0 });
            numInterval.Location = new Point(380, 13);
            numInterval.Maximum = new decimal(new int[] { 5000, 0, 0, 0 });
            numInterval.Minimum = new decimal(new int[] { 200, 0, 0, 0 });
            numInterval.Name = "numInterval";
            numInterval.Size = new Size(65, 23);
            numInterval.TabIndex = 2;
            numInterval.Value = new decimal(new int[] { 1000, 0, 0, 0 });
            // 
            // lblInterval
            // 
            lblInterval.AutoSize = true;
            lblInterval.Location = new Point(292, 16);
            lblInterval.Name = "lblInterval";
            lblInterval.Size = new Size(82, 15);
            lblInterval.TabIndex = 1;
            lblInterval.Text = "Інтервал (мс):";
            // 
            // btnStartSimulation
            // 
            btnStartSimulation.BackColor = Color.FromArgb(22, 163, 74);
            btnStartSimulation.FlatStyle = FlatStyle.Flat;
            btnStartSimulation.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnStartSimulation.ForeColor = Color.White;
            btnStartSimulation.Location = new Point(12, 8);
            btnStartSimulation.Name = "btnStartSimulation";
            btnStartSimulation.Size = new Size(160, 30);
            btnStartSimulation.TabIndex = 0;
            btnStartSimulation.Text = "▶ Запустити збір";
            btnStartSimulation.UseVisualStyleBackColor = false;
            // 
            // splitContainer
            // 
            splitContainer.Dock = DockStyle.Fill;
            splitContainer.Location = new Point(0, 106);
            splitContainer.Name = "splitContainer";
            splitContainer.Orientation = Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            splitContainer.Panel1.Controls.Add(flowCardsPanel);
            splitContainer.Panel1.Padding = new Padding(10);
            // 
            // splitContainer.Panel2
            // 
            splitContainer.Panel2.Controls.Add(tabControl);
            splitContainer.Panel2.Padding = new Padding(10, 0, 10, 10);
            splitContainer.Size = new Size(1184, 580);
            splitContainer.SplitterDistance = 210;
            splitContainer.TabIndex = 2;
            // 
            // flowCardsPanel
            // 
            flowCardsPanel.AutoScroll = true;
            flowCardsPanel.Dock = DockStyle.Fill;
            flowCardsPanel.Location = new Point(10, 10);
            flowCardsPanel.Name = "flowCardsPanel";
            flowCardsPanel.Size = new Size(1164, 190);
            flowCardsPanel.TabIndex = 0;
            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabHistory);
            tabControl.Controls.Add(tabLog);
            tabControl.Dock = DockStyle.Fill;
            tabControl.Location = new Point(10, 0);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(1164, 356);
            tabControl.TabIndex = 0;
            // 
            // tabHistory
            // 
            tabHistory.Controls.Add(dgvTelemetry);
            tabHistory.Controls.Add(pnlFilterBar);
            tabHistory.Location = new Point(4, 24);
            tabHistory.Name = "tabHistory";
            tabHistory.Padding = new Padding(3);
            tabHistory.Size = new Size(1156, 328);
            tabHistory.TabIndex = 0;
            tabHistory.Text = "📊 Журнал бази даних телеметрії";
            tabHistory.UseVisualStyleBackColor = true;
            // 
            // dgvTelemetry
            // 
            dgvTelemetry.AllowUserToAddRows = false;
            dgvTelemetry.AllowUserToDeleteRows = false;
            dgvTelemetry.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTelemetry.BackgroundColor = Color.White;
            dgvTelemetry.BorderStyle = BorderStyle.None;
            dgvTelemetry.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTelemetry.Dock = DockStyle.Fill;
            dgvTelemetry.Location = new Point(3, 47);
            dgvTelemetry.MultiSelect = false;
            dgvTelemetry.Name = "dgvTelemetry";
            dgvTelemetry.ReadOnly = true;
            dgvTelemetry.RowHeadersVisible = false;
            dgvTelemetry.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTelemetry.Size = new Size(1150, 278);
            dgvTelemetry.TabIndex = 1;
            // 
            // pnlFilterBar
            // 
            pnlFilterBar.BackColor = Color.FromArgb(248, 250, 252);
            pnlFilterBar.Controls.Add(btnClearDb);
            pnlFilterBar.Controls.Add(btnExportCsv);
            pnlFilterBar.Controls.Add(btnResetFilter);
            pnlFilterBar.Controls.Add(btnApplyFilter);
            pnlFilterBar.Controls.Add(chkOnlyAlerts);
            pnlFilterBar.Controls.Add(cmbFilterStatus);
            pnlFilterBar.Controls.Add(lblFilterStatus);
            pnlFilterBar.Controls.Add(cmbFilterSensor);
            pnlFilterBar.Controls.Add(lblFilterSensor);
            pnlFilterBar.Dock = DockStyle.Top;
            pnlFilterBar.Location = new Point(3, 3);
            pnlFilterBar.Name = "pnlFilterBar";
            pnlFilterBar.Size = new Size(1150, 44);
            pnlFilterBar.TabIndex = 0;
            // 
            // btnClearDb
            // 
            btnClearDb.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClearDb.BackColor = Color.FromArgb(241, 245, 249);
            btnClearDb.FlatStyle = FlatStyle.Flat;
            btnClearDb.Location = new Point(1020, 7);
            btnClearDb.Name = "btnClearDb";
            btnClearDb.Size = new Size(125, 28);
            btnClearDb.TabIndex = 8;
            btnClearDb.Text = "🗑 Очистити БД";
            btnClearDb.UseVisualStyleBackColor = false;
            // 
            // btnExportCsv
            // 
            btnExportCsv.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnExportCsv.BackColor = Color.FromArgb(241, 245, 249);
            btnExportCsv.FlatStyle = FlatStyle.Flat;
            btnExportCsv.Location = new Point(890, 7);
            btnExportCsv.Name = "btnExportCsv";
            btnExportCsv.Size = new Size(124, 28);
            btnExportCsv.TabIndex = 7;
            btnExportCsv.Text = "💾 Експорт CSV";
            btnExportCsv.UseVisualStyleBackColor = false;
            // 
            // btnResetFilter
            // 
            btnResetFilter.BackColor = Color.FromArgb(241, 245, 249);
            btnResetFilter.FlatStyle = FlatStyle.Flat;
            btnResetFilter.Location = new Point(690, 7);
            btnResetFilter.Name = "btnResetFilter";
            btnResetFilter.Size = new Size(95, 28);
            btnResetFilter.TabIndex = 6;
            btnResetFilter.Text = "↺ Скинути";
            btnResetFilter.UseVisualStyleBackColor = false;
            // 
            // btnApplyFilter
            // 
            btnApplyFilter.BackColor = Color.FromArgb(37, 99, 235);
            btnApplyFilter.FlatStyle = FlatStyle.Flat;
            btnApplyFilter.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnApplyFilter.ForeColor = Color.White;
            btnApplyFilter.Location = new Point(575, 7);
            btnApplyFilter.Name = "btnApplyFilter";
            btnApplyFilter.Size = new Size(110, 28);
            btnApplyFilter.TabIndex = 5;
            btnApplyFilter.Text = "🔍 Фільтрувати";
            btnApplyFilter.UseVisualStyleBackColor = false;
            // 
            // chkOnlyAlerts
            // 
            chkOnlyAlerts.AutoSize = true;
            chkOnlyAlerts.Location = new Point(445, 12);
            chkOnlyAlerts.Name = "chkOnlyAlerts";
            chkOnlyAlerts.Size = new Size(119, 19);
            chkOnlyAlerts.TabIndex = 4;
            chkOnlyAlerts.Text = "Тільки аварії (!)";
            chkOnlyAlerts.UseVisualStyleBackColor = true;
            // 
            // cmbFilterStatus
            // 
            cmbFilterStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFilterStatus.FormattingEnabled = true;
            cmbFilterStatus.Items.AddRange(new object[] { "Усі статуси", "Normal", "Warning", "Critical" });
            cmbFilterStatus.Location = new Point(310, 10);
            cmbFilterStatus.Name = "cmbFilterStatus";
            cmbFilterStatus.Size = new Size(120, 23);
            cmbFilterStatus.TabIndex = 3;
            // 
            // lblFilterStatus
            // 
            lblFilterStatus.AutoSize = true;
            lblFilterStatus.Location = new Point(255, 14);
            lblFilterStatus.Name = "lblFilterStatus";
            lblFilterStatus.Size = new Size(46, 15);
            lblFilterStatus.TabIndex = 2;
            lblFilterStatus.Text = "Статус:";
            // 
            // cmbFilterSensor
            // 
            cmbFilterSensor.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFilterSensor.FormattingEnabled = true;
            cmbFilterSensor.Location = new Point(65, 10);
            cmbFilterSensor.Name = "cmbFilterSensor";
            cmbFilterSensor.Size = new Size(180, 23);
            cmbFilterSensor.TabIndex = 1;
            // 
            // lblFilterSensor
            // 
            lblFilterSensor.AutoSize = true;
            lblFilterSensor.Location = new Point(10, 14);
            lblFilterSensor.Name = "lblFilterSensor";
            lblFilterSensor.Size = new Size(49, 15);
            lblFilterSensor.TabIndex = 0;
            lblFilterSensor.Text = "Сенсор:";
            // 
            // tabLog
            // 
            tabLog.Controls.Add(txtAlertLog);
            tabLog.Controls.Add(pnlLogActions);
            tabLog.Location = new Point(4, 24);
            tabLog.Name = "tabLog";
            tabLog.Padding = new Padding(3);
            tabLog.Size = new Size(1156, 328);
            tabLog.TabIndex = 1;
            tabLog.Text = "⚠️ Журнал подій та аварій (Alerts)";
            tabLog.UseVisualStyleBackColor = true;
            // 
            // txtAlertLog
            // 
            txtAlertLog.BackColor = Color.FromArgb(15, 23, 42);
            txtAlertLog.BorderStyle = BorderStyle.None;
            txtAlertLog.Dock = DockStyle.Fill;
            txtAlertLog.Font = new Font("Consolas", 9.75F, FontStyle.Regular);
            txtAlertLog.ForeColor = Color.FromArgb(248, 250, 252);
            txtAlertLog.Location = new Point(3, 43);
            txtAlertLog.Name = "txtAlertLog";
            txtAlertLog.ReadOnly = true;
            txtAlertLog.Size = new Size(1150, 282);
            txtAlertLog.TabIndex = 1;
            txtAlertLog.Text = "";
            // 
            // pnlLogActions
            // 
            pnlLogActions.BackColor = Color.FromArgb(248, 250, 252);
            pnlLogActions.Controls.Add(btnClearLog);
            pnlLogActions.Controls.Add(lblLogCount);
            pnlLogActions.Dock = DockStyle.Top;
            pnlLogActions.Location = new Point(3, 3);
            pnlLogActions.Name = "pnlLogActions";
            pnlLogActions.Size = new Size(1150, 40);
            pnlLogActions.TabIndex = 0;
            // 
            // btnClearLog
            // 
            btnClearLog.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClearLog.Location = new Point(1030, 6);
            btnClearLog.Name = "btnClearLog";
            btnClearLog.Size = new Size(115, 28);
            btnClearLog.TabIndex = 1;
            btnClearLog.Text = "Очистити лог";
            btnClearLog.UseVisualStyleBackColor = true;
            // 
            // lblLogCount
            // 
            lblLogCount.AutoSize = true;
            lblLogCount.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            lblLogCount.Location = new Point(10, 13);
            lblLogCount.Name = "lblLogCount";
            lblLogCount.Size = new Size(157, 15);
            lblLogCount.TabIndex = 0;
            lblLogCount.Text = "Зафіксовано подій тривог: 0";
            // 
            // statusStrip
            // 
            statusStrip.Items.AddRange(new ToolStripItem[] { statusSensorsLabel, statusRecordsLabel, statusAlertsLabel, statusSimulationLabel });
            statusStrip.Location = new Point(0, 686);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(1184, 24);
            statusStrip.TabIndex = 3;
            // 
            // statusSensorsLabel
            // 
            statusSensorsLabel.Name = "statusSensorsLabel";
            statusSensorsLabel.Size = new Size(128, 19);
            statusSensorsLabel.Text = "Активних сенсорів: 4";
            // 
            // statusRecordsLabel
            // 
            statusRecordsLabel.BorderSides = ToolStripStatusLabelBorderSides.Left;
            statusRecordsLabel.Name = "statusRecordsLabel";
            statusRecordsLabel.Size = new Size(125, 19);
            statusRecordsLabel.Text = "Записів у БД: 0";
            // 
            // statusAlertsLabel
            // 
            statusAlertsLabel.BorderSides = ToolStripStatusLabelBorderSides.Left;
            statusAlertsLabel.ForeColor = Color.FromArgb(220, 38, 38);
            statusAlertsLabel.Name = "statusAlertsLabel";
            statusAlertsLabel.Size = new Size(71, 19);
            statusAlertsLabel.Text = "Аварій: 0";
            // 
            // statusSimulationLabel
            // 
            statusSimulationLabel.BorderSides = ToolStripStatusLabelBorderSides.Left;
            statusSimulationLabel.Name = "statusSimulationLabel";
            statusSimulationLabel.Size = new Size(118, 19);
            statusSimulationLabel.Text = "Статус: Зупинено";
            // 
            // simulationTimer
            // 
            simulationTimer.Interval = 1000;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1184, 710);
            Controls.Add(splitContainer);
            Controls.Add(statusStrip);
            Controls.Add(pnlToolBar);
            Controls.Add(pnlHeader);
            MinimumSize = new Size(950, 600);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Практична робота 8 - Розробка власних компонентів та інтеграція з БД (Тарас Вадим, аІк43)";
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlToolBar.ResumeLayout(false);
            pnlToolBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numInterval).EndInit();
            splitContainer.Panel1.ResumeLayout(false);
            splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
            splitContainer.ResumeLayout(false);
            tabControl.ResumeLayout(false);
            tabHistory.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvTelemetry).EndInit();
            pnlFilterBar.ResumeLayout(false);
            pnlFilterBar.PerformLayout();
            tabLog.ResumeLayout(false);
            pnlLogActions.ResumeLayout(false);
            pnlLogActions.PerformLayout();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel pnlHeader;
        private Label lblHeaderTitle;
        private Label lblHeaderSub;
        private Panel pnlToolBar;
        private Button btnStartSimulation;
        private Label lblInterval;
        private NumericUpDown numInterval;
        private Button btnStep;
        private Button btnSimulateSpike;
        private SplitContainer splitContainer;
        private FlowLayoutPanel flowCardsPanel;
        private TabControl tabControl;
        private TabPage tabHistory;
        private TabPage tabLog;
        private Panel pnlFilterBar;
        private Label lblFilterSensor;
        private ComboBox cmbFilterSensor;
        private Label lblFilterStatus;
        private ComboBox cmbFilterStatus;
        private CheckBox chkOnlyAlerts;
        private Button btnApplyFilter;
        private Button btnResetFilter;
        private Button btnExportCsv;
        private Button btnClearDb;
        private DataGridView dgvTelemetry;
        private RichTextBox txtAlertLog;
        private Panel pnlLogActions;
        private Label lblLogCount;
        private Button btnClearLog;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel statusSensorsLabel;
        private ToolStripStatusLabel statusRecordsLabel;
        private ToolStripStatusLabel statusAlertsLabel;
        private ToolStripStatusLabel statusSimulationLabel;
        private System.Windows.Forms.Timer simulationTimer;
    }
}
