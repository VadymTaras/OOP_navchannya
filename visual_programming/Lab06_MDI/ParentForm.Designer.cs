namespace Lab06_MDI;

partial class ParentForm
{
    private System.ComponentModel.IContainer components = null;

    // Меню та смуги
    private System.Windows.Forms.MenuStrip menuStrip;
    private System.Windows.Forms.ToolStrip toolStrip;
    private System.Windows.Forms.StatusStrip statusStrip;
    private System.Windows.Forms.ContextMenuStrip contextMenuText;

    // Пункти головного меню: Файл
    private System.Windows.Forms.ToolStripMenuItem menuFile;
    private System.Windows.Forms.ToolStripMenuItem menuFileNew;
    private System.Windows.Forms.ToolStripMenuItem menuFileOpen;
    private System.Windows.Forms.ToolStripMenuItem menuFileSave;
    private System.Windows.Forms.ToolStripMenuItem menuFileSaveAs;
    private System.Windows.Forms.ToolStripSeparator separatorFile1;
    private System.Windows.Forms.ToolStripMenuItem menuFileClose;
    private System.Windows.Forms.ToolStripMenuItem menuFileCloseAll;
    private System.Windows.Forms.ToolStripSeparator separatorFile2;
    private System.Windows.Forms.ToolStripMenuItem menuFileExit;

    // Пункти головного меню: Правка
    private System.Windows.Forms.ToolStripMenuItem menuEdit;
    private System.Windows.Forms.ToolStripMenuItem menuEditUndo;
    private System.Windows.Forms.ToolStripMenuItem menuEditRedo;
    private System.Windows.Forms.ToolStripSeparator separatorEdit1;
    private System.Windows.Forms.ToolStripMenuItem menuEditCut;
    private System.Windows.Forms.ToolStripMenuItem menuEditCopy;
    private System.Windows.Forms.ToolStripMenuItem menuEditPaste;
    private System.Windows.Forms.ToolStripSeparator separatorEdit2;
    private System.Windows.Forms.ToolStripMenuItem menuEditSelectAll;

    // Пункти головного меню: Вигляд
    private System.Windows.Forms.ToolStripMenuItem menuView;
    private System.Windows.Forms.ToolStripMenuItem menuViewFont;
    private System.Windows.Forms.ToolStripMenuItem menuViewColor;
    private System.Windows.Forms.ToolStripMenuItem menuViewBackColor;
    private System.Windows.Forms.ToolStripSeparator separatorView1;
    private System.Windows.Forms.ToolStripMenuItem menuViewToolbar;
    private System.Windows.Forms.ToolStripMenuItem menuViewStatusbar;

    // Пункти головного меню: Вікно
    private System.Windows.Forms.ToolStripMenuItem menuWindow;
    private System.Windows.Forms.ToolStripMenuItem menuWindowCascade;
    private System.Windows.Forms.ToolStripMenuItem menuWindowTileHoriz;
    private System.Windows.Forms.ToolStripMenuItem menuWindowTileVert;
    private System.Windows.Forms.ToolStripMenuItem menuWindowArrangeIcons;
    private System.Windows.Forms.ToolStripSeparator separatorWindow1;
    private System.Windows.Forms.ToolStripMenuItem menuWindowCloseAll;

    // Пункти головного меню: Довідка
    private System.Windows.Forms.ToolStripMenuItem menuHelp;
    private System.Windows.Forms.ToolStripMenuItem menuHelpAbout;

    // Кнопки ToolStrip
    private System.Windows.Forms.ToolStripButton btnNew;
    private System.Windows.Forms.ToolStripButton btnOpen;
    private System.Windows.Forms.ToolStripButton btnSave;
    private System.Windows.Forms.ToolStripSeparator toolSep1;
    private System.Windows.Forms.ToolStripButton btnCut;
    private System.Windows.Forms.ToolStripButton btnCopy;
    private System.Windows.Forms.ToolStripButton btnPaste;
    private System.Windows.Forms.ToolStripSeparator toolSep2;
    private System.Windows.Forms.ToolStripButton btnCascade;
    private System.Windows.Forms.ToolStripButton btnTileH;
    private System.Windows.Forms.ToolStripButton btnTileV;
    private System.Windows.Forms.ToolStripSeparator toolSep3;
    private System.Windows.Forms.ToolStripButton btnFont;
    private System.Windows.Forms.ToolStripButton btnAbout;

    // Елементи StatusStrip
    private System.Windows.Forms.ToolStripStatusLabel lblStatus;
    private System.Windows.Forms.ToolStripStatusLabel lblCursorPos;
    private System.Windows.Forms.ToolStripStatusLabel lblCharCount;
    private System.Windows.Forms.ToolStripStatusLabel lblDocCount;
    private System.Windows.Forms.ToolStripStatusLabel lblClock;

    // Пункти контекстного меню
    private System.Windows.Forms.ToolStripMenuItem ctxUndo;
    private System.Windows.Forms.ToolStripSeparator ctxSep1;
    private System.Windows.Forms.ToolStripMenuItem ctxCut;
    private System.Windows.Forms.ToolStripMenuItem ctxCopy;
    private System.Windows.Forms.ToolStripMenuItem ctxPaste;
    private System.Windows.Forms.ToolStripSeparator ctxSep2;
    private System.Windows.Forms.ToolStripMenuItem ctxSelectAll;

    // Таймер годинника
    private System.Windows.Forms.Timer statusTimer;

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
        this.components = new System.ComponentModel.Container();

        this.menuStrip = new System.Windows.Forms.MenuStrip();
        this.toolStrip = new System.Windows.Forms.ToolStrip();
        this.statusStrip = new System.Windows.Forms.StatusStrip();
        this.contextMenuText = new System.Windows.Forms.ContextMenuStrip(this.components);

        // Файл
        this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
        this.menuFileNew = new System.Windows.Forms.ToolStripMenuItem();
        this.menuFileOpen = new System.Windows.Forms.ToolStripMenuItem();
        this.menuFileSave = new System.Windows.Forms.ToolStripMenuItem();
        this.menuFileSaveAs = new System.Windows.Forms.ToolStripMenuItem();
        this.separatorFile1 = new System.Windows.Forms.ToolStripSeparator();
        this.menuFileClose = new System.Windows.Forms.ToolStripMenuItem();
        this.menuFileCloseAll = new System.Windows.Forms.ToolStripMenuItem();
        this.separatorFile2 = new System.Windows.Forms.ToolStripSeparator();
        this.menuFileExit = new System.Windows.Forms.ToolStripMenuItem();

        // Правка
        this.menuEdit = new System.Windows.Forms.ToolStripMenuItem();
        this.menuEditUndo = new System.Windows.Forms.ToolStripMenuItem();
        this.menuEditRedo = new System.Windows.Forms.ToolStripMenuItem();
        this.separatorEdit1 = new System.Windows.Forms.ToolStripSeparator();
        this.menuEditCut = new System.Windows.Forms.ToolStripMenuItem();
        this.menuEditCopy = new System.Windows.Forms.ToolStripMenuItem();
        this.menuEditPaste = new System.Windows.Forms.ToolStripMenuItem();
        this.separatorEdit2 = new System.Windows.Forms.ToolStripSeparator();
        this.menuEditSelectAll = new System.Windows.Forms.ToolStripMenuItem();

        // Вигляд
        this.menuView = new System.Windows.Forms.ToolStripMenuItem();
        this.menuViewFont = new System.Windows.Forms.ToolStripMenuItem();
        this.menuViewColor = new System.Windows.Forms.ToolStripMenuItem();
        this.menuViewBackColor = new System.Windows.Forms.ToolStripMenuItem();
        this.separatorView1 = new System.Windows.Forms.ToolStripSeparator();
        this.menuViewToolbar = new System.Windows.Forms.ToolStripMenuItem();
        this.menuViewStatusbar = new System.Windows.Forms.ToolStripMenuItem();

        // Вікно
        this.menuWindow = new System.Windows.Forms.ToolStripMenuItem();
        this.menuWindowCascade = new System.Windows.Forms.ToolStripMenuItem();
        this.menuWindowTileHoriz = new System.Windows.Forms.ToolStripMenuItem();
        this.menuWindowTileVert = new System.Windows.Forms.ToolStripMenuItem();
        this.menuWindowArrangeIcons = new System.Windows.Forms.ToolStripMenuItem();
        this.separatorWindow1 = new System.Windows.Forms.ToolStripSeparator();
        this.menuWindowCloseAll = new System.Windows.Forms.ToolStripMenuItem();

        // Довідка
        this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
        this.menuHelpAbout = new System.Windows.Forms.ToolStripMenuItem();

        // ToolStrip елементи
        this.btnNew = new System.Windows.Forms.ToolStripButton();
        this.btnOpen = new System.Windows.Forms.ToolStripButton();
        this.btnSave = new System.Windows.Forms.ToolStripButton();
        this.toolSep1 = new System.Windows.Forms.ToolStripSeparator();
        this.btnCut = new System.Windows.Forms.ToolStripButton();
        this.btnCopy = new System.Windows.Forms.ToolStripButton();
        this.btnPaste = new System.Windows.Forms.ToolStripButton();
        this.toolSep2 = new System.Windows.Forms.ToolStripSeparator();
        this.btnCascade = new System.Windows.Forms.ToolStripButton();
        this.btnTileH = new System.Windows.Forms.ToolStripButton();
        this.btnTileV = new System.Windows.Forms.ToolStripButton();
        this.toolSep3 = new System.Windows.Forms.ToolStripSeparator();
        this.btnFont = new System.Windows.Forms.ToolStripButton();
        this.btnAbout = new System.Windows.Forms.ToolStripButton();

        // StatusStrip елементи
        this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
        this.lblCursorPos = new System.Windows.Forms.ToolStripStatusLabel();
        this.lblCharCount = new System.Windows.Forms.ToolStripStatusLabel();
        this.lblDocCount = new System.Windows.Forms.ToolStripStatusLabel();
        this.lblClock = new System.Windows.Forms.ToolStripStatusLabel();

        // ContextMenuStrip елементи
        this.ctxUndo = new System.Windows.Forms.ToolStripMenuItem();
        this.ctxSep1 = new System.Windows.Forms.ToolStripSeparator();
        this.ctxCut = new System.Windows.Forms.ToolStripMenuItem();
        this.ctxCopy = new System.Windows.Forms.ToolStripMenuItem();
        this.ctxPaste = new System.Windows.Forms.ToolStripMenuItem();
        this.ctxSep2 = new System.Windows.Forms.ToolStripSeparator();
        this.ctxSelectAll = new System.Windows.Forms.ToolStripMenuItem();

        // Таймер
        this.statusTimer = new System.Windows.Forms.Timer(this.components);

        this.menuStrip.SuspendLayout();
        this.toolStrip.SuspendLayout();
        this.statusStrip.SuspendLayout();
        this.contextMenuText.SuspendLayout();
        this.SuspendLayout();

        //
        // menuStrip
        //
        this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuEdit,
            this.menuView,
            this.menuWindow,
            this.menuHelp
        });
        this.menuStrip.MdiWindowListItem = this.menuWindow;
        this.menuStrip.Location = new System.Drawing.Point(0, 0);
        this.menuStrip.Name = "menuStrip";
        this.menuStrip.Size = new System.Drawing.Size(1024, 24);
        this.menuStrip.TabIndex = 0;
        this.menuStrip.Text = "Головне меню";

        //
        // Меню «Файл»
        //
        this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFileNew,
            this.menuFileOpen,
            this.menuFileSave,
            this.menuFileSaveAs,
            this.separatorFile1,
            this.menuFileClose,
            this.menuFileCloseAll,
            this.separatorFile2,
            this.menuFileExit
        });
        this.menuFile.Name = "menuFile";
        this.menuFile.Text = "&Файл";

        this.menuFileNew.Name = "menuFileNew";
        this.menuFileNew.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
        this.menuFileNew.Text = "&Створити новий документ";
        this.menuFileNew.Click += new System.EventHandler(this.MenuFileNew_Click);

        this.menuFileOpen.Name = "menuFileOpen";
        this.menuFileOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
        this.menuFileOpen.Text = "&Відкрити...";
        this.menuFileOpen.Click += new System.EventHandler(this.MenuFileOpen_Click);

        this.menuFileSave.Name = "menuFileSave";
        this.menuFileSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
        this.menuFileSave.Text = "&Зберегти";
        this.menuFileSave.Click += new System.EventHandler(this.MenuFileSave_Click);

        this.menuFileSaveAs.Name = "menuFileSaveAs";
        this.menuFileSaveAs.Text = "Зберегти &як...";
        this.menuFileSaveAs.Click += new System.EventHandler(this.MenuFileSaveAs_Click);

        this.separatorFile1.Name = "separatorFile1";

        this.menuFileClose.Name = "menuFileClose";
        this.menuFileClose.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
        this.menuFileClose.Text = "&Закрити документ";
        this.menuFileClose.Click += new System.EventHandler(this.MenuFileClose_Click);

        this.menuFileCloseAll.Name = "menuFileCloseAll";
        this.menuFileCloseAll.Text = "Закрити &всі";
        this.menuFileCloseAll.Click += new System.EventHandler(this.MenuFileCloseAll_Click);

        this.separatorFile2.Name = "separatorFile2";

        this.menuFileExit.Name = "menuFileExit";
        this.menuFileExit.Text = "Ви&хід";
        this.menuFileExit.Click += new System.EventHandler(this.MenuFileExit_Click);

        //
        // Меню «Правка»
        //
        this.menuEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuEditUndo,
            this.menuEditRedo,
            this.separatorEdit1,
            this.menuEditCut,
            this.menuEditCopy,
            this.menuEditPaste,
            this.separatorEdit2,
            this.menuEditSelectAll
        });
        this.menuEdit.Name = "menuEdit";
        this.menuEdit.Text = "&Правка";

        this.menuEditUndo.Name = "menuEditUndo";
        this.menuEditUndo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
        this.menuEditUndo.Text = "&Скасувати (Undo)";
        this.menuEditUndo.Click += new System.EventHandler(this.MenuEditUndo_Click);

        this.menuEditRedo.Name = "menuEditRedo";
        this.menuEditRedo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
        this.menuEditRedo.Text = "&Повторити (Redo)";
        this.menuEditRedo.Click += new System.EventHandler(this.MenuEditRedo_Click);

        this.separatorEdit1.Name = "separatorEdit1";

        this.menuEditCut.Name = "menuEditCut";
        this.menuEditCut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
        this.menuEditCut.Text = "Ви&різати (Cut)";
        this.menuEditCut.Click += new System.EventHandler(this.MenuEditCut_Click);

        this.menuEditCopy.Name = "menuEditCopy";
        this.menuEditCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
        this.menuEditCopy.Text = "&Копіювати (Copy)";
        this.menuEditCopy.Click += new System.EventHandler(this.MenuEditCopy_Click);

        this.menuEditPaste.Name = "menuEditPaste";
        this.menuEditPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
        this.menuEditPaste.Text = "&Вставити (Paste)";
        this.menuEditPaste.Click += new System.EventHandler(this.MenuEditPaste_Click);

        this.separatorEdit2.Name = "separatorEdit2";

        this.menuEditSelectAll.Name = "menuEditSelectAll";
        this.menuEditSelectAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
        this.menuEditSelectAll.Text = "Ви&ділити все";
        this.menuEditSelectAll.Click += new System.EventHandler(this.MenuEditSelectAll_Click);

        //
        // Меню «Вигляд»
        //
        this.menuView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuViewFont,
            this.menuViewColor,
            this.menuViewBackColor,
            this.separatorView1,
            this.menuViewToolbar,
            this.menuViewStatusbar
        });
        this.menuView.Name = "menuView";
        this.menuView.Text = "&Вигляд";

        this.menuViewFont.Name = "menuViewFont";
        this.menuViewFont.Text = "&Шрифт тексту...";
        this.menuViewFont.Click += new System.EventHandler(this.MenuViewFont_Click);

        this.menuViewColor.Name = "menuViewColor";
        this.menuViewColor.Text = "&Колір шрифту...";
        this.menuViewColor.Click += new System.EventHandler(this.MenuViewColor_Click);

        this.menuViewBackColor.Name = "menuViewBackColor";
        this.menuViewBackColor.Text = "Колір &тла документа...";
        this.menuViewBackColor.Click += new System.EventHandler(this.MenuViewBackColor_Click);

        this.separatorView1.Name = "separatorView1";

        this.menuViewToolbar.Checked = true;
        this.menuViewToolbar.CheckOnClick = true;
        this.menuViewToolbar.CheckState = System.Windows.Forms.CheckState.Checked;
        this.menuViewToolbar.Name = "menuViewToolbar";
        this.menuViewToolbar.Text = "&Панель інструментів";
        this.menuViewToolbar.Click += new System.EventHandler(this.MenuViewToolbar_Click);

        this.menuViewStatusbar.Checked = true;
        this.menuViewStatusbar.CheckOnClick = true;
        this.menuViewStatusbar.CheckState = System.Windows.Forms.CheckState.Checked;
        this.menuViewStatusbar.Name = "menuViewStatusbar";
        this.menuViewStatusbar.Text = "&Рядок стану";
        this.menuViewStatusbar.Click += new System.EventHandler(this.MenuViewStatusbar_Click);

        //
        // Меню «Вікно»
        //
        this.menuWindow.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuWindowCascade,
            this.menuWindowTileHoriz,
            this.menuWindowTileVert,
            this.menuWindowArrangeIcons,
            this.separatorWindow1,
            this.menuWindowCloseAll
        });
        this.menuWindow.Name = "menuWindow";
        this.menuWindow.Text = "Ві&кно";

        this.menuWindowCascade.Name = "menuWindowCascade";
        this.menuWindowCascade.Text = "&Каскадом";
        this.menuWindowCascade.Click += new System.EventHandler(this.MenuWindowCascade_Click);

        this.menuWindowTileHoriz.Name = "menuWindowTileHoriz";
        this.menuWindowTileHoriz.Text = "Зверху вниз (&Горизонтально)";
        this.menuWindowTileHoriz.Click += new System.EventHandler(this.MenuWindowTileHoriz_Click);

        this.menuWindowTileVert.Name = "menuWindowTileVert";
        this.menuWindowTileVert.Text = "Поруч (&Вертикально)";
        this.menuWindowTileVert.Click += new System.EventHandler(this.MenuWindowTileVert_Click);

        this.menuWindowArrangeIcons.Name = "menuWindowArrangeIcons";
        this.menuWindowArrangeIcons.Text = "Упорядкувати &значки";
        this.menuWindowArrangeIcons.Click += new System.EventHandler(this.MenuWindowArrangeIcons_Click);

        this.separatorWindow1.Name = "separatorWindow1";

        this.menuWindowCloseAll.Name = "menuWindowCloseAll";
        this.menuWindowCloseAll.Text = "Закрити в&сі вікна";
        this.menuWindowCloseAll.Click += new System.EventHandler(this.MenuFileCloseAll_Click);

        //
        // Меню «Довідка»
        //
        this.menuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuHelpAbout
        });
        this.menuHelp.Name = "menuHelp";
        this.menuHelp.Text = "&Довідка";

        this.menuHelpAbout.Name = "menuHelpAbout";
        this.menuHelpAbout.Text = "&Про програму...";
        this.menuHelpAbout.Click += new System.EventHandler(this.MenuHelpAbout_Click);

        //
        // toolStrip
        //
        this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnNew,
            this.btnOpen,
            this.btnSave,
            this.toolSep1,
            this.btnCut,
            this.btnCopy,
            this.btnPaste,
            this.toolSep2,
            this.btnCascade,
            this.btnTileH,
            this.btnTileV,
            this.toolSep3,
            this.btnFont,
            this.btnAbout
        });
        this.toolStrip.Location = new System.Drawing.Point(0, 24);
        this.toolStrip.Name = "toolStrip";
        this.toolStrip.Size = new System.Drawing.Size(1024, 25);
        this.toolStrip.TabIndex = 1;
        this.toolStrip.Text = "Панель швидких дій";

        this.btnNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        this.btnNew.Name = "btnNew";
        this.btnNew.Text = "📄 Новий";
        this.btnNew.ToolTipText = "Створити новий документ (Ctrl+N)";
        this.btnNew.Click += new System.EventHandler(this.MenuFileNew_Click);

        this.btnOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        this.btnOpen.Name = "btnOpen";
        this.btnOpen.Text = "📂 Відкрити";
        this.btnOpen.ToolTipText = "Відкрити існуючий файл (Ctrl+O)";
        this.btnOpen.Click += new System.EventHandler(this.MenuFileOpen_Click);

        this.btnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        this.btnSave.Name = "btnSave";
        this.btnSave.Text = "💾 Зберегти";
        this.btnSave.ToolTipText = "Зберегти активний документ (Ctrl+S)";
        this.btnSave.Click += new System.EventHandler(this.MenuFileSave_Click);

        this.toolSep1.Name = "toolSep1";

        this.btnCut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        this.btnCut.Name = "btnCut";
        this.btnCut.Text = "✂️ Вирізати";
        this.btnCut.ToolTipText = "Вирізати виділений текст (Ctrl+X)";
        this.btnCut.Click += new System.EventHandler(this.MenuEditCut_Click);

        this.btnCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        this.btnCopy.Name = "btnCopy";
        this.btnCopy.Text = "📋 Копіювати";
        this.btnCopy.ToolTipText = "Копіювати виділений текст (Ctrl+C)";
        this.btnCopy.Click += new System.EventHandler(this.MenuEditCopy_Click);

        this.btnPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        this.btnPaste.Name = "btnPaste";
        this.btnPaste.Text = "📥 Вставити";
        this.btnPaste.ToolTipText = "Вставити з буфера обміну (Ctrl+V)";
        this.btnPaste.Click += new System.EventHandler(this.MenuEditPaste_Click);

        this.toolSep2.Name = "toolSep2";

        this.btnCascade.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        this.btnCascade.Name = "btnCascade";
        this.btnCascade.Text = "🗂️ Каскад";
        this.btnCascade.ToolTipText = "Упорядкувати вікна каскадом";
        this.btnCascade.Click += new System.EventHandler(this.MenuWindowCascade_Click);

        this.btnTileH.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        this.btnTileH.Name = "btnTileH";
        this.btnTileH.Text = " горизонтально";
        this.btnTileH.ToolTipText = "Розташувати вікна горизонтально";
        this.btnTileH.Click += new System.EventHandler(this.MenuWindowTileHoriz_Click);

        this.btnTileV.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        this.btnTileV.Name = "btnTileV";
        this.btnTileV.Text = " вертикально";
        this.btnTileV.ToolTipText = "Розташувати вікна вертикально";
        this.btnTileV.Click += new System.EventHandler(this.MenuWindowTileVert_Click);

        this.toolSep3.Name = "toolSep3";

        this.btnFont.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        this.btnFont.Name = "btnFont";
        this.btnFont.Text = "🔤 Шрифт";
        this.btnFont.ToolTipText = "Налаштування шрифту";
        this.btnFont.Click += new System.EventHandler(this.MenuViewFont_Click);

        this.btnAbout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        this.btnAbout.Name = "btnAbout";
        this.btnAbout.Text = "ℹ️ Про програму";
        this.btnAbout.ToolTipText = "Відомості про програму та автора";
        this.btnAbout.Click += new System.EventHandler(this.MenuHelpAbout_Click);

        //
        // statusStrip
        //
        this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus,
            this.lblCursorPos,
            this.lblCharCount,
            this.lblDocCount,
            this.lblClock
        });
        this.statusStrip.Location = new System.Drawing.Point(0, 597);
        this.statusStrip.Name = "statusStrip";
        this.statusStrip.Size = new System.Drawing.Size(1024, 24);
        this.statusStrip.TabIndex = 2;
        this.statusStrip.Text = "Рядок стану";

        this.lblStatus.Name = "lblStatus";
        this.lblStatus.Size = new System.Drawing.Size(439, 19);
        this.lblStatus.Spring = true;
        this.lblStatus.Text = "Готово до роботи";
        this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

        this.lblCursorPos.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
        this.lblCursorPos.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
        this.lblCursorPos.Name = "lblCursorPos";
        this.lblCursorPos.Size = new System.Drawing.Size(130, 19);
        this.lblCursorPos.Text = "Рядок: 1, Стовпчик: 1";

        this.lblCharCount.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
        this.lblCharCount.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
        this.lblCharCount.Name = "lblCharCount";
        this.lblCharCount.Size = new System.Drawing.Size(95, 19);
        this.lblCharCount.Text = "Символів: 0";

        this.lblDocCount.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
        this.lblDocCount.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
        this.lblDocCount.Name = "lblDocCount";
        this.lblDocCount.Size = new System.Drawing.Size(120, 19);
        this.lblDocCount.Text = "Відкрито вікон: 0";

        this.lblClock.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
        this.lblClock.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
        this.lblClock.Name = "lblClock";
        this.lblClock.Size = new System.Drawing.Size(75, 19);
        this.lblClock.Text = "00:00:00";

        //
        // contextMenuText
        //
        this.contextMenuText.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctxUndo,
            this.ctxSep1,
            this.ctxCut,
            this.ctxCopy,
            this.ctxPaste,
            this.ctxSep2,
            this.ctxSelectAll
        });
        this.contextMenuText.Name = "contextMenuText";
        this.contextMenuText.Size = new System.Drawing.Size(169, 126);

        this.ctxUndo.Name = "ctxUndo";
        this.ctxUndo.Text = "Скасувати";
        this.ctxUndo.Click += new System.EventHandler(this.MenuEditUndo_Click);

        this.ctxSep1.Name = "ctxSep1";

        this.ctxCut.Name = "ctxCut";
        this.ctxCut.Text = "Вирізати";
        this.ctxCut.Click += new System.EventHandler(this.MenuEditCut_Click);

        this.ctxCopy.Name = "ctxCopy";
        this.ctxCopy.Text = "Копіювати";
        this.ctxCopy.Click += new System.EventHandler(this.MenuEditCopy_Click);

        this.ctxPaste.Name = "ctxPaste";
        this.ctxPaste.Text = "Вставити";
        this.ctxPaste.Click += new System.EventHandler(this.MenuEditPaste_Click);

        this.ctxSep2.Name = "ctxSep2";

        this.ctxSelectAll.Name = "ctxSelectAll";
        this.ctxSelectAll.Text = "Виділити все";
        this.ctxSelectAll.Click += new System.EventHandler(this.MenuEditSelectAll_Click);

        //
        // statusTimer
        //
        this.statusTimer.Enabled = true;
        this.statusTimer.Interval = 1000;
        this.statusTimer.Tick += new System.EventHandler(this.StatusTimer_Tick);

        //
        // ParentForm
        //
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(1024, 621);
        this.Controls.Add(this.statusStrip);
        this.Controls.Add(this.toolStrip);
        this.Controls.Add(this.menuStrip);
        this.IsMdiContainer = true;
        this.MainMenuStrip = this.menuStrip;
        this.Name = "ParentForm";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "Advanced MDI Studio — Багатодокументний редактор";
        this.MdiChildActivate += new System.EventHandler(this.ParentForm_MdiChildActivate);
        this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ParentForm_FormClosing);

        this.menuStrip.ResumeLayout(false);
        this.menuStrip.PerformLayout();
        this.toolStrip.ResumeLayout(false);
        this.toolStrip.PerformLayout();
        this.statusStrip.ResumeLayout(false);
        this.statusStrip.PerformLayout();
        this.contextMenuText.ResumeLayout(false);
        this.ResumeLayout(false);
        this.PerformLayout();
    }
}
