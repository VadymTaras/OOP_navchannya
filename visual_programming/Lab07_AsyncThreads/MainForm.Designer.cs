// ============================================================================
// Практична робота 7. Асинхронне програмування та багатопотоковість у Windows Forms
// Дисципліна: Інструментальні засоби візуального програмування (Костіков О.А.)
// Студент: ТАРАС Вадим, група аІк43
// Декларація компонентів форми (MainForm.Designer.cs)
// ============================================================================

namespace Lab07_AsyncThreads;

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
        this.grpExecutionMode = new System.Windows.Forms.GroupBox();
        this.radAsyncAwait = new System.Windows.Forms.RadioButton();
        this.radBackgroundWorker = new System.Windows.Forms.RadioButton();
        this.radDirectInvoke = new System.Windows.Forms.RadioButton();
        this.radSyncFreeze = new System.Windows.Forms.RadioButton();
        this.grpTaskParameters = new System.Windows.Forms.GroupBox();
        this.lblTotalItems = new System.Windows.Forms.Label();
        this.numTotalItems = new System.Windows.Forms.NumericUpDown();
        this.lblDelayMs = new System.Windows.Forms.Label();
        this.numDelayMs = new System.Windows.Forms.NumericUpDown();
        this.chkSimulateHeavyWork = new System.Windows.Forms.CheckBox();
        this.grpControls = new System.Windows.Forms.GroupBox();
        this.btnStart = new System.Windows.Forms.Button();
        this.btnPause = new System.Windows.Forms.Button();
        this.btnCancel = new System.Windows.Forms.Button();
        this.btnClearLog = new System.Windows.Forms.Button();
        this.btnTestUi = new System.Windows.Forms.Button();
        this.lblUiClickCount = new System.Windows.Forms.Label();
        this.grpProgress = new System.Windows.Forms.GroupBox();
        this.progressBar = new System.Windows.Forms.ProgressBar();
        this.lblPercentage = new System.Windows.Forms.Label();
        this.lblProcessedCount = new System.Windows.Forms.Label();
        this.lblElapsedTime = new System.Windows.Forms.Label();
        this.lblEta = new System.Windows.Forms.Label();
        this.lblCurrentThreadInfo = new System.Windows.Forms.Label();
        this.lblStatusMessage = new System.Windows.Forms.Label();
        this.grpLog = new System.Windows.Forms.GroupBox();
        this.lstLog = new System.Windows.Forms.ListBox();
        this.statusStrip = new System.Windows.Forms.StatusStrip();
        this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
        this.statusThreadInfo = new System.Windows.Forms.ToolStripStatusLabel();
        this.statusTime = new System.Windows.Forms.ToolStripStatusLabel();
        this.grpExecutionMode.SuspendLayout();
        this.grpTaskParameters.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.numTotalItems)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.numDelayMs)).BeginInit();
        this.grpControls.SuspendLayout();
        this.grpProgress.SuspendLayout();
        this.grpLog.SuspendLayout();
        this.statusStrip.SuspendLayout();
        this.SuspendLayout();

        // 
        // grpExecutionMode
        // 
        this.grpExecutionMode.Controls.Add(this.radAsyncAwait);
        this.grpExecutionMode.Controls.Add(this.radBackgroundWorker);
        this.grpExecutionMode.Controls.Add(this.radDirectInvoke);
        this.grpExecutionMode.Controls.Add(this.radSyncFreeze);
        this.grpExecutionMode.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.grpExecutionMode.Location = new System.Drawing.Point(12, 12);
        this.grpExecutionMode.Name = "grpExecutionMode";
        this.grpExecutionMode.Size = new System.Drawing.Size(460, 160);
        this.grpExecutionMode.TabIndex = 0;
        this.grpExecutionMode.TabStop = false;
        this.grpExecutionMode.Text = "1. Вибір технології фонового виконання";

        // 
        // radAsyncAwait
        // 
        this.radAsyncAwait.AutoSize = true;
        this.radAsyncAwait.Checked = true;
        this.radAsyncAwait.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.radAsyncAwait.Location = new System.Drawing.Point(15, 25);
        this.radAsyncAwait.Name = "radAsyncAwait";
        this.radAsyncAwait.Size = new System.Drawing.Size(420, 24);
        this.radAsyncAwait.TabIndex = 0;
        this.radAsyncAwait.TabStop = true;
        this.radAsyncAwait.Text = "Сучасний підхід: async / await + Task.Run + Progress<T>";
        this.radAsyncAwait.UseVisualStyleBackColor = true;

        // 
        // radBackgroundWorker
        // 
        this.radBackgroundWorker.AutoSize = true;
        this.radBackgroundWorker.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.radBackgroundWorker.Location = new System.Drawing.Point(15, 55);
        this.radBackgroundWorker.Name = "radBackgroundWorker";
        this.radBackgroundWorker.Size = new System.Drawing.Size(425, 24);
        this.radBackgroundWorker.TabIndex = 1;
        this.radBackgroundWorker.Text = "Класичний підхід: компонент BackgroundWorker (DoWork)";
        this.radBackgroundWorker.UseVisualStyleBackColor = true;

        // 
        // radDirectInvoke
        // 
        this.radDirectInvoke.AutoSize = true;
        this.radDirectInvoke.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.radDirectInvoke.Location = new System.Drawing.Point(15, 85);
        this.radDirectInvoke.Name = "radDirectInvoke";
        this.radDirectInvoke.Size = new System.Drawing.Size(420, 24);
        this.radDirectInvoke.TabIndex = 2;
        this.radDirectInvoke.Text = "Міжпотоковий виклик: Task.Run + InvokeRequired / Invoke";
        this.radDirectInvoke.UseVisualStyleBackColor = true;

        // 
        // radSyncFreeze
        // 
        this.radSyncFreeze.AutoSize = true;
        this.radSyncFreeze.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.radSyncFreeze.ForeColor = System.Drawing.Color.DarkRed;
        this.radSyncFreeze.Location = new System.Drawing.Point(15, 115);
        this.radSyncFreeze.Name = "radSyncFreeze";
        this.radSyncFreeze.Size = new System.Drawing.Size(420, 24);
        this.radSyncFreeze.TabIndex = 3;
        this.radSyncFreeze.Text = "Синхронний блокуючий виклик (Thread.Sleep у UI — фріз)";
        this.radSyncFreeze.UseVisualStyleBackColor = true;

        // 
        // grpTaskParameters
        // 
        this.grpTaskParameters.Controls.Add(this.lblTotalItems);
        this.grpTaskParameters.Controls.Add(this.numTotalItems);
        this.grpTaskParameters.Controls.Add(this.lblDelayMs);
        this.grpTaskParameters.Controls.Add(this.numDelayMs);
        this.grpTaskParameters.Controls.Add(this.chkSimulateHeavyWork);
        this.grpTaskParameters.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.grpTaskParameters.Location = new System.Drawing.Point(485, 12);
        this.grpTaskParameters.Name = "grpTaskParameters";
        this.grpTaskParameters.Size = new System.Drawing.Size(485, 160);
        this.grpTaskParameters.TabIndex = 1;
        this.grpTaskParameters.TabStop = false;
        this.grpTaskParameters.Text = "2. Параметри тестової задачі";

        // 
        // lblTotalItems
        // 
        this.lblTotalItems.AutoSize = true;
        this.lblTotalItems.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.lblTotalItems.Location = new System.Drawing.Point(15, 30);
        this.lblTotalItems.Name = "lblTotalItems";
        this.lblTotalItems.Size = new System.Drawing.Size(206, 20);
        this.lblTotalItems.TabIndex = 0;
        this.lblTotalItems.Text = "Кількість ітерацій (записів):";

        // 
        // numTotalItems
        // 
        this.numTotalItems.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.numTotalItems.Location = new System.Drawing.Point(240, 28);
        this.numTotalItems.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
        this.numTotalItems.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
        this.numTotalItems.Name = "numTotalItems";
        this.numTotalItems.Size = new System.Drawing.Size(120, 27);
        this.numTotalItems.TabIndex = 1;
        this.numTotalItems.Value = new decimal(new int[] { 100, 0, 0, 0 });

        // 
        // lblDelayMs
        // 
        this.lblDelayMs.AutoSize = true;
        this.lblDelayMs.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.lblDelayMs.Location = new System.Drawing.Point(15, 70);
        this.lblDelayMs.Name = "lblDelayMs";
        this.lblDelayMs.Size = new System.Drawing.Size(197, 20);
        this.lblDelayMs.TabIndex = 2;
        this.lblDelayMs.Text = "Затримка кроку (мс/ітем):";

        // 
        // numDelayMs
        // 
        this.numDelayMs.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.numDelayMs.Location = new System.Drawing.Point(240, 68);
        this.numDelayMs.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
        this.numDelayMs.Minimum = new decimal(new int[] { 5, 0, 0, 0 });
        this.numDelayMs.Name = "numDelayMs";
        this.numDelayMs.Size = new System.Drawing.Size(120, 27);
        this.numDelayMs.TabIndex = 3;
        this.numDelayMs.Value = new decimal(new int[] { 40, 0, 0, 0 });

        // 
        // chkSimulateHeavyWork
        // 
        this.chkSimulateHeavyWork.AutoSize = true;
        this.chkSimulateHeavyWork.Checked = true;
        this.chkSimulateHeavyWork.CheckState = System.Windows.Forms.CheckState.Checked;
        this.chkSimulateHeavyWork.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.chkSimulateHeavyWork.Location = new System.Drawing.Point(18, 115);
        this.chkSimulateHeavyWork.Name = "chkSimulateHeavyWork";
        this.chkSimulateHeavyWork.Size = new System.Drawing.Size(370, 24);
        this.chkSimulateHeavyWork.TabIndex = 4;
        this.chkSimulateHeavyWork.Text = "Імітувати CPU-обчислення (матричні множення)";
        this.chkSimulateHeavyWork.UseVisualStyleBackColor = true;

        // 
        // grpControls
        // 
        this.grpControls.Controls.Add(this.btnStart);
        this.grpControls.Controls.Add(this.btnPause);
        this.grpControls.Controls.Add(this.btnCancel);
        this.grpControls.Controls.Add(this.btnClearLog);
        this.grpControls.Controls.Add(this.btnTestUi);
        this.grpControls.Controls.Add(this.lblUiClickCount);
        this.grpControls.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.grpControls.Location = new System.Drawing.Point(12, 178);
        this.grpControls.Name = "grpControls";
        this.grpControls.Size = new System.Drawing.Size(958, 90);
        this.grpControls.TabIndex = 2;
        this.grpControls.TabStop = false;
        this.grpControls.Text = "3. Панель керування та перевірки чуйності інтерфейсу (UI Responsiveness)";

        // 
        // btnStart
        // 
        this.btnStart.BackColor = System.Drawing.Color.FromArgb(40, 167, 69);
        this.btnStart.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.btnStart.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.btnStart.ForeColor = System.Drawing.Color.White;
        this.btnStart.Location = new System.Drawing.Point(15, 28);
        this.btnStart.Name = "btnStart";
        this.btnStart.Size = new System.Drawing.Size(125, 42);
        this.btnStart.TabIndex = 0;
        this.btnStart.Text = "▶ Старт";
        this.btnStart.UseVisualStyleBackColor = false;

        // 
        // btnPause
        // 
        this.btnPause.BackColor = System.Drawing.Color.FromArgb(255, 193, 7);
        this.btnPause.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnPause.Enabled = false;
        this.btnPause.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.btnPause.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.btnPause.ForeColor = System.Drawing.Color.Black;
        this.btnPause.Location = new System.Drawing.Point(150, 28);
        this.btnPause.Name = "btnPause";
        this.btnPause.Size = new System.Drawing.Size(125, 42);
        this.btnPause.TabIndex = 1;
        this.btnPause.Text = "⏸ Пауза";
        this.btnPause.UseVisualStyleBackColor = false;

        // 
        // btnCancel
        // 
        this.btnCancel.BackColor = System.Drawing.Color.FromArgb(220, 53, 69);
        this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnCancel.Enabled = false;
        this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.btnCancel.ForeColor = System.Drawing.Color.White;
        this.btnCancel.Location = new System.Drawing.Point(285, 28);
        this.btnCancel.Name = "btnCancel";
        this.btnCancel.Size = new System.Drawing.Size(125, 42);
        this.btnCancel.TabIndex = 2;
        this.btnCancel.Text = "⏹ Скасувати";
        this.btnCancel.UseVisualStyleBackColor = false;

        // 
        // btnClearLog
        // 
        this.btnClearLog.BackColor = System.Drawing.Color.FromArgb(108, 117, 125);
        this.btnClearLog.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnClearLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.btnClearLog.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.btnClearLog.ForeColor = System.Drawing.Color.White;
        this.btnClearLog.Location = new System.Drawing.Point(420, 28);
        this.btnClearLog.Name = "btnClearLog";
        this.btnClearLog.Size = new System.Drawing.Size(130, 42);
        this.btnClearLog.TabIndex = 3;
        this.btnClearLog.Text = "🗑 Очистити";
        this.btnClearLog.UseVisualStyleBackColor = false;

        // 
        // btnTestUi
        // 
        this.btnTestUi.BackColor = System.Drawing.Color.FromArgb(23, 162, 184);
        this.btnTestUi.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnTestUi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.btnTestUi.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.btnTestUi.ForeColor = System.Drawing.Color.White;
        this.btnTestUi.Location = new System.Drawing.Point(580, 28);
        this.btnTestUi.Name = "btnTestUi";
        this.btnTestUi.Size = new System.Drawing.Size(165, 42);
        this.btnTestUi.TabIndex = 4;
        this.btnTestUi.Text = "⚡ Клік-тест UI!";
        this.btnTestUi.UseVisualStyleBackColor = false;

        // 
        // lblUiClickCount
        // 
        this.lblUiClickCount.AutoSize = true;
        this.lblUiClickCount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.lblUiClickCount.Location = new System.Drawing.Point(755, 39);
        this.lblUiClickCount.Name = "lblUiClickCount";
        this.lblUiClickCount.Size = new System.Drawing.Size(175, 20);
        this.lblUiClickCount.TabIndex = 5;
        this.lblUiClickCount.Text = "Кліків: 0 (UI доступний)";

        // 
        // grpProgress
        // 
        this.grpProgress.Controls.Add(this.progressBar);
        this.grpProgress.Controls.Add(this.lblPercentage);
        this.grpProgress.Controls.Add(this.lblProcessedCount);
        this.grpProgress.Controls.Add(this.lblElapsedTime);
        this.grpProgress.Controls.Add(this.lblEta);
        this.grpProgress.Controls.Add(this.lblCurrentThreadInfo);
        this.grpProgress.Controls.Add(this.lblStatusMessage);
        this.grpProgress.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.grpProgress.Location = new System.Drawing.Point(12, 274);
        this.grpProgress.Name = "grpProgress";
        this.grpProgress.Size = new System.Drawing.Size(958, 140);
        this.grpProgress.TabIndex = 3;
        this.grpProgress.TabStop = false;
        this.grpProgress.Text = "4. Індикація прогресу та метрики виконання";

        // 
        // progressBar
        // 
        this.progressBar.Location = new System.Drawing.Point(15, 28);
        this.progressBar.Name = "progressBar";
        this.progressBar.Size = new System.Drawing.Size(760, 30);
        this.progressBar.TabIndex = 0;

        // 
        // lblPercentage
        // 
        this.lblPercentage.AutoSize = true;
        this.lblPercentage.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.lblPercentage.ForeColor = System.Drawing.Color.FromArgb(0, 102, 204);
        this.lblPercentage.Location = new System.Drawing.Point(790, 28);
        this.lblPercentage.Name = "lblPercentage";
        this.lblPercentage.Size = new System.Drawing.Size(51, 28);
        this.lblPercentage.TabIndex = 1;
        this.lblPercentage.Text = "0 %";

        // 
        // lblProcessedCount
        // 
        this.lblProcessedCount.AutoSize = true;
        this.lblProcessedCount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.lblProcessedCount.Location = new System.Drawing.Point(15, 72);
        this.lblProcessedCount.Name = "lblProcessedCount";
        this.lblProcessedCount.Size = new System.Drawing.Size(163, 20);
        this.lblProcessedCount.TabIndex = 2;
        this.lblProcessedCount.Text = "Оброблено: 0 з 0 ітемів";

        // 
        // lblElapsedTime
        // 
        this.lblElapsedTime.AutoSize = true;
        this.lblElapsedTime.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.lblElapsedTime.Location = new System.Drawing.Point(260, 72);
        this.lblElapsedTime.Name = "lblElapsedTime";
        this.lblElapsedTime.Size = new System.Drawing.Size(155, 20);
        this.lblElapsedTime.TabIndex = 3;
        this.lblElapsedTime.Text = "Час: 00:00:00.000";

        // 
        // lblEta
        // 
        this.lblEta.AutoSize = true;
        this.lblEta.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.lblEta.Location = new System.Drawing.Point(500, 72);
        this.lblEta.Name = "lblEta";
        this.lblEta.Size = new System.Drawing.Size(175, 20);
        this.lblEta.TabIndex = 4;
        this.lblEta.Text = "Оцінка завершення: --";

        // 
        // lblCurrentThreadInfo
        // 
        this.lblCurrentThreadInfo.AutoSize = true;
        this.lblCurrentThreadInfo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.lblCurrentThreadInfo.Location = new System.Drawing.Point(15, 105);
        this.lblCurrentThreadInfo.Name = "lblCurrentThreadInfo";
        this.lblCurrentThreadInfo.Size = new System.Drawing.Size(262, 20);
        this.lblCurrentThreadInfo.TabIndex = 5;
        this.lblCurrentThreadInfo.Text = "Потоки: UI = Потік #1 | Робочий = --";

        // 
        // lblStatusMessage
        // 
        this.lblStatusMessage.AutoSize = true;
        this.lblStatusMessage.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.lblStatusMessage.ForeColor = System.Drawing.Color.DarkSlateGray;
        this.lblStatusMessage.Location = new System.Drawing.Point(500, 105);
        this.lblStatusMessage.Name = "lblStatusMessage";
        this.lblStatusMessage.Size = new System.Drawing.Size(207, 20);
        this.lblStatusMessage.TabIndex = 6;
        this.lblStatusMessage.Text = "Статус: Очікування команди";

        // 
        // grpLog
        // 
        this.grpLog.Controls.Add(this.lstLog);
        this.grpLog.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.grpLog.Location = new System.Drawing.Point(12, 420);
        this.grpLog.Name = "grpLog";
        this.grpLog.Size = new System.Drawing.Size(958, 240);
        this.grpLog.TabIndex = 4;
        this.grpLog.TabStop = false;
        this.grpLog.Text = "5. Журнал операцій у реальному часі (час, ManagedThreadId, опис)";

        // 
        // lstLog
        // 
        this.lstLog.Dock = System.Windows.Forms.DockStyle.Fill;
        this.lstLog.Font = new System.Drawing.Font("Cascadia Mono", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.lstLog.FormattingEnabled = true;
        this.lstLog.ItemHeight = 20;
        this.lstLog.Location = new System.Drawing.Point(3, 23);
        this.lstLog.Name = "lstLog";
        this.lstLog.Size = new System.Drawing.Size(952, 214);
        this.lstLog.TabIndex = 0;

        // 
        // statusStrip
        // 
        this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
        this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
        this.statusLabel,
        this.statusThreadInfo,
        this.statusTime});
        this.statusStrip.Location = new System.Drawing.Point(0, 670);
        this.statusStrip.Name = "statusStrip";
        this.statusStrip.Size = new System.Drawing.Size(982, 26);
        this.statusStrip.TabIndex = 5;
        this.statusStrip.Text = "statusStrip";

        // 
        // statusLabel
        // 
        this.statusLabel.Name = "statusLabel";
        this.statusLabel.Size = new System.Drawing.Size(650, 20);
        this.statusLabel.Spring = true;
        this.statusLabel.Text = "Готовий до тестування багатопотоковості";
        this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

        // 
        // statusThreadInfo
        // 
        this.statusThreadInfo.Name = "statusThreadInfo";
        this.statusThreadInfo.Size = new System.Drawing.Size(167, 20);
        this.statusThreadInfo.Text = "UI ManagedThreadId: 1";

        // 
        // statusTime
        // 
        this.statusTime.Name = "statusTime";
        this.statusTime.Size = new System.Drawing.Size(150, 20);
        this.statusTime.Text = "Практична робота 7";

        // 
        // MainForm
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.BackColor = System.Drawing.Color.FromArgb(246, 248, 252);
        this.ClientSize = new System.Drawing.Size(982, 696);
        this.Controls.Add(this.statusStrip);
        this.Controls.Add(this.grpLog);
        this.Controls.Add(this.grpProgress);
        this.Controls.Add(this.grpControls);
        this.Controls.Add(this.grpTaskParameters);
        this.Controls.Add(this.grpExecutionMode);
        this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;
        this.Name = "MainForm";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "Async & Multithreaded Processing Hub — Практична робота 7 (Тарас Вадим, аІк43)";
        this.grpExecutionMode.ResumeLayout(false);
        this.grpExecutionMode.PerformLayout();
        this.grpTaskParameters.ResumeLayout(false);
        this.grpTaskParameters.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.numTotalItems)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.numDelayMs)).EndInit();
        this.grpControls.ResumeLayout(false);
        this.grpControls.PerformLayout();
        this.grpProgress.ResumeLayout(false);
        this.grpProgress.PerformLayout();
        this.grpLog.ResumeLayout(false);
        this.statusStrip.ResumeLayout(false);
        this.statusStrip.PerformLayout();
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private System.Windows.Forms.GroupBox grpExecutionMode;
    private System.Windows.Forms.RadioButton radAsyncAwait;
    private System.Windows.Forms.RadioButton radBackgroundWorker;
    private System.Windows.Forms.RadioButton radDirectInvoke;
    private System.Windows.Forms.RadioButton radSyncFreeze;
    private System.Windows.Forms.GroupBox grpTaskParameters;
    private System.Windows.Forms.Label lblTotalItems;
    private System.Windows.Forms.NumericUpDown numTotalItems;
    private System.Windows.Forms.Label lblDelayMs;
    private System.Windows.Forms.NumericUpDown numDelayMs;
    private System.Windows.Forms.CheckBox chkSimulateHeavyWork;
    private System.Windows.Forms.GroupBox grpControls;
    private System.Windows.Forms.Button btnStart;
    private System.Windows.Forms.Button btnPause;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Button btnClearLog;
    private System.Windows.Forms.Button btnTestUi;
    private System.Windows.Forms.Label lblUiClickCount;
    private System.Windows.Forms.GroupBox grpProgress;
    private System.Windows.Forms.ProgressBar progressBar;
    private System.Windows.Forms.Label lblPercentage;
    private System.Windows.Forms.Label lblProcessedCount;
    private System.Windows.Forms.Label lblElapsedTime;
    private System.Windows.Forms.Label lblEta;
    private System.Windows.Forms.Label lblCurrentThreadInfo;
    private System.Windows.Forms.Label lblStatusMessage;
    private System.Windows.Forms.GroupBox grpLog;
    private System.Windows.Forms.ListBox lstLog;
    private System.Windows.Forms.StatusStrip statusStrip;
    private System.Windows.Forms.ToolStripStatusLabel statusLabel;
    private System.Windows.Forms.ToolStripStatusLabel statusThreadInfo;
    private System.Windows.Forms.ToolStripStatusLabel statusTime;
}
