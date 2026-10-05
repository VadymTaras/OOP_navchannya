namespace Lab08_CustomControlDB
{
    partial class SensorCardControl
    {
        /// <summary> 
        /// Обов'язкова змінна конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null!;

        /// <summary> 
        /// Звільнення використовуваних ресурсів.
        /// </summary>
        /// <param name="disposing">true якщо керовані ресурси мають бути звільнені; інакше false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Обов'язковий метод для підтримки конструктора.
        /// </summary>
        private void InitializeComponent()
        {
            pnlContainer = new Panel();
            lblStatusBadge = new Label();
            lblThresholds = new Label();
            progressBar = new ProgressBar();
            lblUnit = new Label();
            lblValue = new Label();
            lblSensorName = new Label();
            pnlHeaderColor = new Panel();
            pnlContainer.SuspendLayout();
            SuspendLayout();
            // 
            // pnlContainer
            // 
            pnlContainer.BackColor = Color.FromArgb(250, 251, 253);
            pnlContainer.BorderStyle = BorderStyle.FixedSingle;
            pnlContainer.Controls.Add(lblStatusBadge);
            pnlContainer.Controls.Add(lblThresholds);
            pnlContainer.Controls.Add(progressBar);
            pnlContainer.Controls.Add(lblUnit);
            pnlContainer.Controls.Add(lblValue);
            pnlContainer.Controls.Add(lblSensorName);
            pnlContainer.Controls.Add(pnlHeaderColor);
            pnlContainer.Dock = DockStyle.Fill;
            pnlContainer.Location = new Point(0, 0);
            pnlContainer.Name = "pnlContainer";
            pnlContainer.Size = new Size(260, 160);
            pnlContainer.TabIndex = 0;
            // 
            // lblStatusBadge
            // 
            lblStatusBadge.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblStatusBadge.BackColor = Color.FromArgb(46, 125, 50);
            lblStatusBadge.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblStatusBadge.ForeColor = Color.White;
            lblStatusBadge.Location = new Point(165, 12);
            lblStatusBadge.Name = "lblStatusBadge";
            lblStatusBadge.Size = new Size(82, 22);
            lblStatusBadge.TabIndex = 6;
            lblStatusBadge.Text = "НОРМА";
            lblStatusBadge.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblThresholds
            // 
            lblThresholds.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblThresholds.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular);
            lblThresholds.ForeColor = Color.FromArgb(100, 116, 139);
            lblThresholds.Location = new Point(12, 132);
            lblThresholds.Name = "lblThresholds";
            lblThresholds.Size = new Size(234, 18);
            lblThresholds.TabIndex = 5;
            lblThresholds.Text = "Поріг: [0.0 - 80.0 °C]";
            lblThresholds.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // progressBar
            // 
            progressBar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            progressBar.Location = new Point(12, 116);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(234, 10);
            progressBar.Style = ProgressBarStyle.Continuous;
            progressBar.TabIndex = 4;
            // 
            // lblUnit
            // 
            lblUnit.AutoSize = true;
            lblUnit.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblUnit.ForeColor = Color.FromArgb(71, 85, 105);
            lblUnit.Location = new Point(140, 68);
            lblUnit.Name = "lblUnit";
            lblUnit.Size = new Size(33, 21);
            lblUnit.TabIndex = 3;
            lblUnit.Text = "°C";
            // 
            // lblValue
            // 
            lblValue.AutoSize = true;
            lblValue.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblValue.ForeColor = Color.FromArgb(15, 23, 42);
            lblValue.Location = new Point(10, 48);
            lblValue.Name = "lblValue";
            lblValue.Size = new Size(84, 45);
            lblValue.TabIndex = 2;
            lblValue.Text = "00.0";
            // 
            // lblSensorName
            // 
            lblSensorName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblSensorName.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            lblSensorName.ForeColor = Color.FromArgb(30, 41, 59);
            lblSensorName.Location = new Point(10, 12);
            lblSensorName.Name = "lblSensorName";
            lblSensorName.Size = new Size(150, 22);
            lblSensorName.TabIndex = 1;
            lblSensorName.Text = "Сенсор";
            lblSensorName.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pnlHeaderColor
            // 
            pnlHeaderColor.BackColor = Color.FromArgb(37, 99, 235);
            pnlHeaderColor.Dock = DockStyle.Top;
            pnlHeaderColor.Location = new Point(0, 0);
            pnlHeaderColor.Name = "pnlHeaderColor";
            pnlHeaderColor.Size = new Size(258, 4);
            pnlHeaderColor.TabIndex = 0;
            // 
            // SensorCardControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(pnlContainer);
            Name = "SensorCardControl";
            Size = new Size(260, 160);
            pnlContainer.ResumeLayout(false);
            pnlContainer.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlContainer;
        private Panel pnlHeaderColor;
        private Label lblSensorName;
        private Label lblValue;
        private Label lblUnit;
        private ProgressBar progressBar;
        private Label lblThresholds;
        private Label lblStatusBadge;
    }
}
