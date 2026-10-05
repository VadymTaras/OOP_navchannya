namespace Lab02_Controls
{
    partial class MainForm
    {
        /// <summary>
        /// Обов'язкова змінна конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Звільнити всі використовувані ресурси.
        /// </summary>
        /// <param name="disposing">true якщо керовані ресурси мають бути видалені; інакше false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматично створений конструктором форм Windows

        private void InitializeComponent()
        {
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileNew = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileSave = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEditReset = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
            
            this.lblPlatform = new System.Windows.Forms.Label();
            this.cmbPlatform = new System.Windows.Forms.ComboBox();
            
            this.grpWarranty = new System.Windows.Forms.GroupBox();
            this.rbWarranty12 = new System.Windows.Forms.RadioButton();
            this.rbWarranty24 = new System.Windows.Forms.RadioButton();
            this.rbWarranty36 = new System.Windows.Forms.RadioButton();
            
            this.grpOptions = new System.Windows.Forms.GroupBox();
            this.chkOS = new System.Windows.Forms.CheckBox();
            this.chkSSD = new System.Windows.Forms.CheckBox();
            this.chkCooling = new System.Windows.Forms.CheckBox();
            this.chkCableMgmt = new System.Windows.Forms.CheckBox();
            this.chkTesting = new System.Windows.Forms.CheckBox();
            
            this.grpCustomComponents = new System.Windows.Forms.GroupBox();
            this.lstComponents = new System.Windows.Forms.ListBox();
            this.txtNewComponent = new System.Windows.Forms.TextBox();
            this.btnAddComponent = new System.Windows.Forms.Button();
            this.btnRemoveComponent = new System.Windows.Forms.Button();
            
            this.btnCalculate = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnSaveReport = new System.Windows.Forms.Button();
            
            this.grpSummary = new System.Windows.Forms.GroupBox();
            this.txtSpecification = new System.Windows.Forms.RichTextBox();
            
            this.statusStripMain = new System.Windows.Forms.StatusStrip();
            this.statusLabelInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusLabelPrice = new System.Windows.Forms.ToolStripStatusLabel();
            
            this.saveFileDialogReport = new System.Windows.Forms.SaveFileDialog();
            
            this.menuStripMain.SuspendLayout();
            this.grpWarranty.SuspendLayout();
            this.grpOptions.SuspendLayout();
            this.grpCustomComponents.SuspendLayout();
            this.grpSummary.SuspendLayout();
            this.statusStripMain.SuspendLayout();
            this.SuspendLayout();
            
            // 
            // menuStripMain
            // 
            this.menuStripMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuEdit,
            this.menuHelp});
            this.menuStripMain.Location = new System.Drawing.Point(0, 0);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Size = new System.Drawing.Size(920, 24);
            this.menuStripMain.TabIndex = 0;
            this.menuStripMain.Text = "menuStripMain";
            
            // 
            // menuFile
            // 
            this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFileNew,
            this.menuFileSave,
            this.toolStripSeparator1,
            this.menuFileExit});
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(48, 20);
            this.menuFile.Text = "&Файл";
            
            // 
            // menuFileNew
            // 
            this.menuFileNew.Name = "menuFileNew";
            this.menuFileNew.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.menuFileNew.Size = new System.Drawing.Size(207, 22);
            this.menuFileNew.Text = "&Нове замовлення";
            this.menuFileNew.Click += new System.EventHandler(this.menuFileNew_Click);
            
            // 
            // menuFileSave
            // 
            this.menuFileSave.Name = "menuFileSave";
            this.menuFileSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.menuFileSave.Size = new System.Drawing.Size(207, 22);
            this.menuFileSave.Text = "&Зберегти чек...";
            this.menuFileSave.Click += new System.EventHandler(this.menuFileSave_Click);
            
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(204, 6);
            
            // 
            // menuFileExit
            // 
            this.menuFileExit.Name = "menuFileExit";
            this.menuFileExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.menuFileExit.Size = new System.Drawing.Size(207, 22);
            this.menuFileExit.Text = "Ви&хід";
            this.menuFileExit.Click += new System.EventHandler(this.menuFileExit_Click);
            
            // 
            // menuEdit
            // 
            this.menuEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuEditReset});
            this.menuEdit.Name = "menuEdit";
            this.menuEdit.Size = new System.Drawing.Size(59, 20);
            this.menuEdit.Text = "&Правка";
            
            // 
            // menuEditReset
            // 
            this.menuEditReset.Name = "menuEditReset";
            this.menuEditReset.Size = new System.Drawing.Size(180, 22);
            this.menuEditReset.Text = "&Скинути вибір";
            this.menuEditReset.Click += new System.EventHandler(this.menuEditReset_Click);
            
            // 
            // menuHelp
            // 
            this.menuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuHelpAbout});
            this.menuHelp.Name = "menuHelp";
            this.menuHelp.Size = new System.Drawing.Size(61, 20);
            this.menuHelp.Text = "&Довідка";
            
            // 
            // menuHelpAbout
            // 
            this.menuHelpAbout.Name = "menuHelpAbout";
            this.menuHelpAbout.Size = new System.Drawing.Size(154, 22);
            this.menuHelpAbout.Text = "&Про програму";
            this.menuHelpAbout.Click += new System.EventHandler(this.menuHelpAbout_Click);
            
            // 
            // lblPlatform
            // 
            this.lblPlatform.AutoSize = true;
            this.lblPlatform.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblPlatform.Location = new System.Drawing.Point(20, 38);
            this.lblPlatform.Name = "lblPlatform";
            this.lblPlatform.Size = new System.Drawing.Size(225, 17);
            this.lblPlatform.TabIndex = 1;
            this.lblPlatform.Text = "Базова платформа комп'ютера:";
            
            // 
            // cmbPlatform
            // 
            this.cmbPlatform.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPlatform.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbPlatform.FormattingEnabled = true;
            this.cmbPlatform.Location = new System.Drawing.Point(23, 60);
            this.cmbPlatform.Name = "cmbPlatform";
            this.cmbPlatform.Size = new System.Drawing.Size(430, 23);
            this.cmbPlatform.TabIndex = 2;
            this.cmbPlatform.SelectedIndexChanged += new System.EventHandler(this.cmbPlatform_SelectedIndexChanged);
            
            // 
            // grpWarranty
            // 
            this.grpWarranty.Controls.Add(this.rbWarranty36);
            this.grpWarranty.Controls.Add(this.rbWarranty24);
            this.grpWarranty.Controls.Add(this.rbWarranty12);
            this.grpWarranty.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpWarranty.Location = new System.Drawing.Point(23, 100);
            this.grpWarranty.Name = "grpWarranty";
            this.grpWarranty.Size = new System.Drawing.Size(430, 110);
            this.grpWarranty.TabIndex = 3;
            this.grpWarranty.TabStop = false;
            this.grpWarranty.Text = "Термін гарантійного обслуговування (RadioButton)";
            
            // 
            // rbWarranty12
            // 
            this.rbWarranty12.AutoSize = true;
            this.rbWarranty12.Checked = true;
            this.rbWarranty12.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.rbWarranty12.Location = new System.Drawing.Point(16, 25);
            this.rbWarranty12.Name = "rbWarranty12";
            this.rbWarranty12.Size = new System.Drawing.Size(225, 19);
            this.rbWarranty12.TabIndex = 0;
            this.rbWarranty12.TabStop = true;
            this.rbWarranty12.Text = "Базова гарантія (12 міс.) — Включено";
            this.rbWarranty12.UseVisualStyleBackColor = true;
            this.rbWarranty12.CheckedChanged += new System.EventHandler(this.Warranty_CheckedChanged);
            
            // 
            // rbWarranty24
            // 
            this.rbWarranty24.AutoSize = true;
            this.rbWarranty24.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.rbWarranty24.Location = new System.Drawing.Point(16, 50);
            this.rbWarranty24.Name = "rbWarranty24";
            this.rbWarranty24.Size = new System.Drawing.Size(250, 19);
            this.rbWarranty24.TabIndex = 1;
            this.rbWarranty24.Text = "Розширена гарантія (24 міс.) — +2 500 грн";
            this.rbWarranty24.UseVisualStyleBackColor = true;
            this.rbWarranty24.CheckedChanged += new System.EventHandler(this.Warranty_CheckedChanged);
            
            // 
            // rbWarranty36
            // 
            this.rbWarranty36.AutoSize = true;
            this.rbWarranty36.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.rbWarranty36.Location = new System.Drawing.Point(16, 75);
            this.rbWarranty36.Name = "rbWarranty36";
            this.rbWarranty36.Size = new System.Drawing.Size(270, 19);
            this.rbWarranty36.TabIndex = 2;
            this.rbWarranty36.Text = "Преміум сервіс On-Site (36 міс.) — +5 500 грн";
            this.rbWarranty36.UseVisualStyleBackColor = true;
            this.rbWarranty36.CheckedChanged += new System.EventHandler(this.Warranty_CheckedChanged);
            
            // 
            // grpOptions
            // 
            this.grpOptions.Controls.Add(this.chkTesting);
            this.grpOptions.Controls.Add(this.chkCableMgmt);
            this.grpOptions.Controls.Add(this.chkCooling);
            this.grpOptions.Controls.Add(this.chkSSD);
            this.grpOptions.Controls.Add(this.chkOS);
            this.grpOptions.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpOptions.Location = new System.Drawing.Point(23, 220);
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.Size = new System.Drawing.Size(430, 160);
            this.grpOptions.TabIndex = 4;
            this.grpOptions.TabStop = false;
            this.grpOptions.Text = "Додаткові пакети та опції (CheckBox)";
            
            // 
            // chkOS
            // 
            this.chkOS.AutoSize = true;
            this.chkOS.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkOS.Location = new System.Drawing.Point(16, 26);
            this.chkOS.Name = "chkOS";
            this.chkOS.Size = new System.Drawing.Size(280, 19);
            this.chkOS.TabIndex = 0;
            this.chkOS.Text = "Ліцензійна ОС Windows 11 Pro (+4 200 грн)";
            this.chkOS.UseVisualStyleBackColor = true;
            this.chkOS.CheckedChanged += new System.EventHandler(this.Option_CheckedChanged);
            
            // 
            // chkSSD
            // 
            this.chkSSD.AutoSize = true;
            this.chkSSD.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkSSD.Location = new System.Drawing.Point(16, 51);
            this.chkSSD.Name = "chkSSD";
            this.chkSSD.Size = new System.Drawing.Size(275, 19);
            this.chkSSD.TabIndex = 1;
            this.chkSSD.Text = "Швидкісний NVMe SSD 2TB (+3 800 грн)";
            this.chkSSD.UseVisualStyleBackColor = true;
            this.chkSSD.CheckedChanged += new System.EventHandler(this.Option_CheckedChanged);
            
            // 
            // chkCooling
            // 
            this.chkCooling.AutoSize = true;
            this.chkCooling.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkCooling.Location = new System.Drawing.Point(16, 76);
            this.chkCooling.Name = "chkCooling";
            this.chkCooling.Size = new System.Drawing.Size(286, 19);
            this.chkCooling.TabIndex = 2;
            this.chkCooling.Text = "Рідинна система охолодження 360mm (+4 500 грн)";
            this.chkCooling.UseVisualStyleBackColor = true;
            this.chkCooling.CheckedChanged += new System.EventHandler(this.Option_CheckedChanged);
            
            // 
            // chkCableMgmt
            // 
            this.chkCableMgmt.AutoSize = true;
            this.chkCableMgmt.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkCableMgmt.Location = new System.Drawing.Point(16, 101);
            this.chkCableMgmt.Name = "chkCableMgmt";
            this.chkCableMgmt.Size = new System.Drawing.Size(288, 19);
            this.chkCableMgmt.TabIndex = 3;
            this.chkCableMgmt.Text = "Кабель-менеджмент та шумоізоляція (+1 200 грн)";
            this.chkCableMgmt.UseVisualStyleBackColor = true;
            this.chkCableMgmt.CheckedChanged += new System.EventHandler(this.Option_CheckedChanged);
            
            // 
            // chkTesting
            // 
            this.chkTesting.AutoSize = true;
            this.chkTesting.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkTesting.Location = new System.Drawing.Point(16, 126);
            this.chkTesting.Name = "chkTesting";
            this.chkTesting.Size = new System.Drawing.Size(262, 19);
            this.chkTesting.TabIndex = 4;
            this.chkTesting.Text = "Стрес-тестування 24 години (+800 грн)";
            this.chkTesting.UseVisualStyleBackColor = true;
            this.chkTesting.CheckedChanged += new System.EventHandler(this.Option_CheckedChanged);
            
            // 
            // grpCustomComponents
            // 
            this.grpCustomComponents.Controls.Add(this.btnRemoveComponent);
            this.grpCustomComponents.Controls.Add(this.btnAddComponent);
            this.grpCustomComponents.Controls.Add(this.txtNewComponent);
            this.grpCustomComponents.Controls.Add(this.lstComponents);
            this.grpCustomComponents.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpCustomComponents.Location = new System.Drawing.Point(23, 390);
            this.grpCustomComponents.Name = "grpCustomComponents";
            this.grpCustomComponents.Size = new System.Drawing.Size(430, 200);
            this.grpCustomComponents.TabIndex = 5;
            this.grpCustomComponents.TabStop = false;
            this.grpCustomComponents.Text = "Комплектуючі та модулі системи (ListBox)";
            
            // 
            // lstComponents
            // 
            this.lstComponents.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lstComponents.FormattingEnabled = true;
            this.lstComponents.ItemHeight = 15;
            this.lstComponents.Location = new System.Drawing.Point(16, 25);
            this.lstComponents.Name = "lstComponents";
            this.lstComponents.Size = new System.Drawing.Size(395, 109);
            this.lstComponents.TabIndex = 0;
            // 
            // txtNewComponent
            // 
            this.txtNewComponent.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtNewComponent.Location = new System.Drawing.Point(16, 145);
            this.txtNewComponent.Name = "txtNewComponent";
            this.txtNewComponent.Size = new System.Drawing.Size(220, 23);
            this.txtNewComponent.TabIndex = 1;
            // 
            // btnAddComponent
            // 
            this.btnAddComponent.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.btnAddComponent.Location = new System.Drawing.Point(242, 144);
            this.btnAddComponent.Name = "btnAddComponent";
            this.btnAddComponent.Size = new System.Drawing.Size(80, 25);
            this.btnAddComponent.TabIndex = 2;
            this.btnAddComponent.Text = "Додати";
            this.btnAddComponent.UseVisualStyleBackColor = true;
            this.btnAddComponent.Click += new System.EventHandler(this.btnAddComponent_Click);
            
            // 
            // btnRemoveComponent
            // 
            this.btnRemoveComponent.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.btnRemoveComponent.Location = new System.Drawing.Point(328, 144);
            this.btnRemoveComponent.Name = "btnRemoveComponent";
            this.btnRemoveComponent.Size = new System.Drawing.Size(83, 25);
            this.btnRemoveComponent.TabIndex = 3;
            this.btnRemoveComponent.Text = "Видалити";
            this.btnRemoveComponent.UseVisualStyleBackColor = true;
            this.btnRemoveComponent.Click += new System.EventHandler(this.btnRemoveComponent_Click);
            
            // 
            // grpSummary
            // 
            this.grpSummary.Controls.Add(this.txtSpecification);
            this.grpSummary.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpSummary.Location = new System.Drawing.Point(470, 38);
            this.grpSummary.Name = "grpSummary";
            this.grpSummary.Size = new System.Drawing.Size(430, 480);
            this.grpSummary.TabIndex = 6;
            this.grpSummary.TabStop = false;
            this.grpSummary.Text = "Специфікація замовлення (RichTextBox)";
            
            // 
            // txtSpecification
            // 
            this.txtSpecification.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.txtSpecification.Font = new System.Drawing.Font("Consolas", 9.5F);
            this.txtSpecification.Location = new System.Drawing.Point(15, 25);
            this.txtSpecification.Name = "txtSpecification";
            this.txtSpecification.ReadOnly = true;
            this.txtSpecification.Size = new System.Drawing.Size(400, 440);
            this.txtSpecification.TabIndex = 0;
            this.txtSpecification.Text = "";
            
            // 
            // btnCalculate
            // 
            this.btnCalculate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.btnCalculate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCalculate.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnCalculate.ForeColor = System.Drawing.Color.White;
            this.btnCalculate.Location = new System.Drawing.Point(470, 530);
            this.btnCalculate.Name = "btnCalculate";
            this.btnCalculate.Size = new System.Drawing.Size(150, 40);
            this.btnCalculate.TabIndex = 7;
            this.btnCalculate.Text = "Розрахувати чек";
            this.btnCalculate.UseVisualStyleBackColor = false;
            this.btnCalculate.Click += new System.EventHandler(this.btnCalculate_Click);
            
            // 
            // btnSaveReport
            // 
            this.btnSaveReport.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnSaveReport.Location = new System.Drawing.Point(630, 530);
            this.btnSaveReport.Name = "btnSaveReport";
            this.btnSaveReport.Size = new System.Drawing.Size(140, 40);
            this.btnSaveReport.TabIndex = 8;
            this.btnSaveReport.Text = "Зберегти чек...";
            this.btnSaveReport.UseVisualStyleBackColor = true;
            this.btnSaveReport.Click += new System.EventHandler(this.btnSaveReport_Click);
            
            // 
            // btnReset
            // 
            this.btnReset.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnReset.Location = new System.Drawing.Point(780, 530);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(120, 40);
            this.btnReset.TabIndex = 9;
            this.btnReset.Text = "Скинути";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            
            // 
            // statusStripMain
            // 
            this.statusStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabelInfo,
            this.statusLabelPrice});
            this.statusStripMain.Location = new System.Drawing.Point(0, 605);
            this.statusStripMain.Name = "statusStripMain";
            this.statusStripMain.Size = new System.Drawing.Size(920, 24);
            this.statusStripMain.TabIndex = 10;
            this.statusStripMain.Text = "statusStripMain";
            
            // 
            // statusLabelInfo
            // 
            this.statusLabelInfo.Name = "statusLabelInfo";
            this.statusLabelInfo.Size = new System.Drawing.Size(250, 19);
            this.statusLabelInfo.Text = "Статус: Оберіть параметри робочої станції";
            
            // 
            // statusLabelPrice
            // 
            this.statusLabelPrice.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.statusLabelPrice.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(0)))));
            this.statusLabelPrice.Name = "statusLabelPrice";
            this.statusLabelPrice.Size = new System.Drawing.Size(175, 19);
            this.statusLabelPrice.Text = "Орієнтовна сума: 0.00 грн";
            
            // 
            // saveFileDialogReport
            // 
            this.saveFileDialogReport.DefaultExt = "txt";
            this.saveFileDialogReport.Filter = "Текстові файли (*.txt)|*.txt|Усі файли (*.*)|*.*";
            this.saveFileDialogReport.Title = "Збереження специфікації замовлення";
            
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(920, 629);
            this.Controls.Add(this.statusStripMain);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnSaveReport);
            this.Controls.Add(this.btnCalculate);
            this.Controls.Add(this.grpSummary);
            this.Controls.Add(this.grpCustomComponents);
            this.Controls.Add(this.grpOptions);
            this.Controls.Add(this.grpWarranty);
            this.Controls.Add(this.cmbPlatform);
            this.Controls.Add(this.lblPlatform);
            this.Controls.Add(this.menuStripMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStripMain;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Конфігуратор робочих станцій розробника — Практична робота № 2 (Тарас Вадим, аІк43)";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            this.grpWarranty.ResumeLayout(false);
            this.grpWarranty.PerformLayout();
            this.grpOptions.ResumeLayout(false);
            this.grpOptions.PerformLayout();
            this.grpCustomComponents.ResumeLayout(false);
            this.grpCustomComponents.PerformLayout();
            this.grpSummary.ResumeLayout(false);
            this.statusStripMain.ResumeLayout(false);
            this.statusStripMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStripMain;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem menuFileNew;
        private System.Windows.Forms.ToolStripMenuItem menuFileSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuFileExit;
        private System.Windows.Forms.ToolStripMenuItem menuEdit;
        private System.Windows.Forms.ToolStripMenuItem menuEditReset;
        private System.Windows.Forms.ToolStripMenuItem menuHelp;
        private System.Windows.Forms.ToolStripMenuItem menuHelpAbout;
        
        private System.Windows.Forms.Label lblPlatform;
        private System.Windows.Forms.ComboBox cmbPlatform;
        
        private System.Windows.Forms.GroupBox grpWarranty;
        private System.Windows.Forms.RadioButton rbWarranty12;
        private System.Windows.Forms.RadioButton rbWarranty24;
        private System.Windows.Forms.RadioButton rbWarranty36;
        
        private System.Windows.Forms.GroupBox grpOptions;
        private System.Windows.Forms.CheckBox chkOS;
        private System.Windows.Forms.CheckBox chkSSD;
        private System.Windows.Forms.CheckBox chkCooling;
        private System.Windows.Forms.CheckBox chkCableMgmt;
        private System.Windows.Forms.CheckBox chkTesting;
        
        private System.Windows.Forms.GroupBox grpCustomComponents;
        private System.Windows.Forms.ListBox lstComponents;
        private System.Windows.Forms.TextBox txtNewComponent;
        private System.Windows.Forms.Button btnAddComponent;
        private System.Windows.Forms.Button btnRemoveComponent;
        
        private System.Windows.Forms.GroupBox grpSummary;
        private System.Windows.Forms.RichTextBox txtSpecification;
        
        private System.Windows.Forms.Button btnCalculate;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnSaveReport;
        
        private System.Windows.Forms.StatusStrip statusStripMain;
        private System.Windows.Forms.ToolStripStatusLabel statusLabelInfo;
        private System.Windows.Forms.ToolStripStatusLabel statusLabelPrice;
        
        private System.Windows.Forms.SaveFileDialog saveFileDialogReport;
    }
}
