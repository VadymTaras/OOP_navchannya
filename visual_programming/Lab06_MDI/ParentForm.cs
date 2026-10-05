// ============================================================================
// Практична робота 6. Багатовіконні інтерфейси (MDI) у Windows Forms
// Студент: ТАРАС Вадим, група аІк43
// Головна MDI форма-контейнер (ParentForm.cs)
// ============================================================================

using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Lab06_MDI;

/// <summary>
/// Головна форма додатку (MDI Container).
/// Реалізує координацію дочірніх вікон, керування меню MenuStrip,
/// панеллю ToolStrip, рядком стану StatusStrip та системними діалогами.
/// </summary>
public partial class ParentForm : Form
{
    private int _documentCounter = 0;

    public ParentForm()
    {
        InitializeComponent();
        UpdateUiState();
        UpdateClock();
    }

    /// <summary>
    /// Отримання посилання на поточний активний дочірній документ.
    /// </summary>
    private ChildDocumentForm? ActiveChildDocument => ActiveMdiChild as ChildDocumentForm;

    /// <summary>
    /// Оновлення стану доступності пунктів меню та кнопок панелі інструментів
    /// залежно від наявності активного дочірнього документа.
    /// </summary>
    private void UpdateUiState()
    {
        bool hasActiveDoc = ActiveChildDocument != null;
        int docCount = MdiChildren.Length;

        // Пункти меню Файл
        menuFileSave.Enabled = hasActiveDoc;
        menuFileSaveAs.Enabled = hasActiveDoc;
        menuFileClose.Enabled = hasActiveDoc;
        menuFileCloseAll.Enabled = docCount > 0;

        // Пункти меню Правка
        menuEditUndo.Enabled = hasActiveDoc;
        menuEditRedo.Enabled = hasActiveDoc;
        menuEditCut.Enabled = hasActiveDoc;
        menuEditCopy.Enabled = hasActiveDoc;
        menuEditPaste.Enabled = hasActiveDoc;
        menuEditSelectAll.Enabled = hasActiveDoc;

        // Пункти меню Вигляд
        menuViewFont.Enabled = hasActiveDoc;
        menuViewColor.Enabled = hasActiveDoc;
        menuViewBackColor.Enabled = hasActiveDoc;

        // Пункти меню Вікно
        menuWindowCascade.Enabled = docCount > 0;
        menuWindowTileHoriz.Enabled = docCount > 0;
        menuWindowTileVert.Enabled = docCount > 0;
        menuWindowArrangeIcons.Enabled = docCount > 0;
        menuWindowCloseAll.Enabled = docCount > 0;

        // Кнопки панелі швидких дій ToolStrip
        btnSave.Enabled = hasActiveDoc;
        btnCut.Enabled = hasActiveDoc;
        btnCopy.Enabled = hasActiveDoc;
        btnPaste.Enabled = hasActiveDoc;
        btnCascade.Enabled = docCount > 0;
        btnTileH.Enabled = docCount > 0;
        btnTileV.Enabled = docCount > 0;
        btnFont.Enabled = hasActiveDoc;

        // Оновлення рядка стану
        lblDocCount.Text = $"Відкрито вікон: {docCount}";
        if (hasActiveDoc && ActiveChildDocument != null)
        {
            lblCursorPos.Text = $"Рядок: {ActiveChildDocument.CurrentLine}, Стовпчик: {ActiveChildDocument.CurrentColumn}";
            lblCharCount.Text = $"Символів: {ActiveChildDocument.CharacterCount}";
            lblStatus.Text = $"Активний документ: {ActiveChildDocument.DocumentTitle}";
        }
        else
        {
            lblCursorPos.Text = "Рядок: -, Стовпчик: -";
            lblCharCount.Text = "Символів: 0";
            lblStatus.Text = "Немає відкритих документів";
        }
    }

    /// <summary>
    /// Створення нового документа з унікальним номером.
    /// </summary>
    private void CreateNewDocument()
    {
        _documentCounter++;
        var child = new ChildDocumentForm(_documentCounter);
        child.MdiParent = this;
        child.SetContextMenu(contextMenuText);
        child.DocumentStateChanged += Child_DocumentStateChanged;
        child.Show();
        UpdateUiState();
    }

    /// <summary>
    /// Відкриття файлу з диска.
    /// </summary>
    private void OpenDocument()
    {
        using var openDlg = new OpenFileDialog
        {
            Title = "Відкрити текстовий або RTF-документ",
            Filter = "Усі підтримувані формати (*.txt;*.rtf)|*.txt;*.rtf|Текстові файли (*.txt)|*.txt|Rich Text Format (*.rtf)|*.rtf|Усі файли (*.*)|*.*",
            FilterIndex = 1
        };

        if (openDlg.ShowDialog(this) == DialogResult.OK)
        {
            // Перевірка, чи не відкрито вже цей файл у дочірньому вікні
            foreach (Form form in MdiChildren)
            {
                if (form is ChildDocumentForm existingChild &&
                    string.Equals(existingChild.FilePath, openDlg.FileName, StringComparison.OrdinalIgnoreCase))
                {
                    existingChild.Activate();
                    lblStatus.Text = $"Документ уже відкрито: {Path.GetFileName(openDlg.FileName)}";
                    return;
                }
            }

            var child = new ChildDocumentForm(openDlg.FileName);
            child.MdiParent = this;
            child.SetContextMenu(contextMenuText);
            child.DocumentStateChanged += Child_DocumentStateChanged;
            child.Show();
            UpdateUiState();
            lblStatus.Text = $"Файл успішно відкрито: {Path.GetFileName(openDlg.FileName)}";
        }
    }

    /// <summary>
    /// Збереження активного або зазначеного документа.
    /// Повертає true, якщо документ успішно збережено, і false, якщо користувач скасував діалог.
    /// </summary>
    public bool SaveChildDocument(ChildDocumentForm child)
    {
        if (child.FilePath != null)
        {
            try
            {
                child.Save(child.FilePath);
                lblStatus.Text = $"Документ збережено: {Path.GetFileName(child.FilePath)}";
                UpdateUiState();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"Помилка збереження файлу:\n{ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        else
        {
            return SaveChildDocumentAs(child);
        }
    }

    /// <summary>
    /// Збереження документа за новим ім'ям через SaveFileDialog.
    /// </summary>
    public bool SaveChildDocumentAs(ChildDocumentForm child)
    {
        using var saveDlg = new SaveFileDialog
        {
            Title = "Зберегти документ як...",
            Filter = "Текстові файли (*.txt)|*.txt|Rich Text Format (*.rtf)|*.rtf|Усі файли (*.*)|*.*",
            FileName = child.DocumentTitle.EndsWith(".txt") || child.DocumentTitle.EndsWith(".rtf")
                ? child.DocumentTitle
                : $"{child.DocumentTitle}.txt"
        };

        if (saveDlg.ShowDialog(this) == DialogResult.OK)
        {
            try
            {
                child.Save(saveDlg.FileName);
                lblStatus.Text = $"Файл збережено як: {Path.GetFileName(saveDlg.FileName)}";
                UpdateUiState();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"Помилка збереження:\n{ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        return false;
    }

    // --- Обробники меню «Файл» ---

    private void MenuFileNew_Click(object? sender, EventArgs e) => CreateNewDocument();

    private void MenuFileOpen_Click(object? sender, EventArgs e) => OpenDocument();

    private void MenuFileSave_Click(object? sender, EventArgs e)
    {
        if (ActiveChildDocument != null)
            SaveChildDocument(ActiveChildDocument);
    }

    private void MenuFileSaveAs_Click(object? sender, EventArgs e)
    {
        if (ActiveChildDocument != null)
            SaveChildDocumentAs(ActiveChildDocument);
    }

    private void MenuFileClose_Click(object? sender, EventArgs e)
    {
        ActiveChildDocument?.Close();
        UpdateUiState();
    }

    private void MenuFileCloseAll_Click(object? sender, EventArgs e)
    {
        // Закриваємо всі дочірні форми у зворотньому порядку
        for (int i = MdiChildren.Length - 1; i >= 0; i--)
        {
            MdiChildren[i].Close();
        }
        UpdateUiState();
    }

    private void MenuFileExit_Click(object? sender, EventArgs e)
    {
        Close();
    }

    // --- Обробники меню «Правка» ---

    private void MenuEditUndo_Click(object? sender, EventArgs e) => ActiveChildDocument?.UndoAction();

    private void MenuEditRedo_Click(object? sender, EventArgs e) => ActiveChildDocument?.RedoAction();

    private void MenuEditCut_Click(object? sender, EventArgs e) => ActiveChildDocument?.CutAction();

    private void MenuEditCopy_Click(object? sender, EventArgs e) => ActiveChildDocument?.CopyAction();

    private void MenuEditPaste_Click(object? sender, EventArgs e) => ActiveChildDocument?.PasteAction();

    private void MenuEditSelectAll_Click(object? sender, EventArgs e) => ActiveChildDocument?.SelectAllAction();

    // --- Обробники меню «Вигляд» ---

    private void MenuViewFont_Click(object? sender, EventArgs e)
    {
        if (ActiveChildDocument == null) return;

        using var fontDlg = new FontDialog
        {
            Font = ActiveChildDocument.Editor.SelectionFont ?? ActiveChildDocument.Editor.Font,
            ShowColor = true,
            Color = ActiveChildDocument.Editor.SelectionColor
        };

        if (fontDlg.ShowDialog(this) == DialogResult.OK)
        {
            ActiveChildDocument.ApplyFont(fontDlg.Font);
            ActiveChildDocument.ApplyTextColor(fontDlg.Color);
            lblStatus.Text = $"Застосовано шрифт: {fontDlg.Font.Name}, {fontDlg.Font.Size}pt";
        }
    }

    private void MenuViewColor_Click(object? sender, EventArgs e)
    {
        if (ActiveChildDocument == null) return;

        using var colorDlg = new ColorDialog
        {
            Color = ActiveChildDocument.Editor.SelectionColor
        };

        if (colorDlg.ShowDialog(this) == DialogResult.OK)
        {
            ActiveChildDocument.ApplyTextColor(colorDlg.Color);
            lblStatus.Text = $"Застосовано колір шрифту: RGB({colorDlg.Color.R}, {colorDlg.Color.G}, {colorDlg.Color.B})";
        }
    }

    private void MenuViewBackColor_Click(object? sender, EventArgs e)
    {
        if (ActiveChildDocument == null) return;

        using var colorDlg = new ColorDialog
        {
            Color = ActiveChildDocument.Editor.SelectionBackColor
        };

        if (colorDlg.ShowDialog(this) == DialogResult.OK)
        {
            ActiveChildDocument.ApplyBackColor(colorDlg.Color);
            lblStatus.Text = $"Застосовано колір фону: RGB({colorDlg.Color.R}, {colorDlg.Color.G}, {colorDlg.Color.B})";
        }
    }

    private void MenuViewToolbar_Click(object? sender, EventArgs e)
    {
        toolStrip.Visible = menuViewToolbar.Checked;
    }

    private void MenuViewStatusbar_Click(object? sender, EventArgs e)
    {
        statusStrip.Visible = menuViewStatusbar.Checked;
    }

    // --- Обробники меню «Вікно» (MdiLayout) ---

    private void MenuWindowCascade_Click(object? sender, EventArgs e)
    {
        LayoutMdi(MdiLayout.Cascade);
        lblStatus.Text = "Дочірні вікна впорядковано каскадом";
    }

    private void MenuWindowTileHoriz_Click(object? sender, EventArgs e)
    {
        LayoutMdi(MdiLayout.TileHorizontal);
        lblStatus.Text = "Дочірні вікна розташовано горизонтально";
    }

    private void MenuWindowTileVert_Click(object? sender, EventArgs e)
    {
        LayoutMdi(MdiLayout.TileVertical);
        lblStatus.Text = "Дочірні вікна розташовано вертикально";
    }

    private void MenuWindowArrangeIcons_Click(object? sender, EventArgs e)
    {
        LayoutMdi(MdiLayout.ArrangeIcons);
        lblStatus.Text = "Значки мінімізованих вікон упорядковано";
    }

    // --- Обробники меню «Довідка» ---

    private void MenuHelpAbout_Click(object? sender, EventArgs e)
    {
        using var about = new AboutForm();
        about.ShowDialog(this);
    }

    // --- Системні події MDI-контейнера ---

    private void ParentForm_MdiChildActivate(object? sender, EventArgs e)
    {
        UpdateUiState();
    }

    private void Child_DocumentStateChanged(object? sender, EventArgs e)
    {
        if (sender == ActiveChildDocument)
        {
            UpdateUiState();
        }
    }

    private void StatusTimer_Tick(object? sender, EventArgs e)
    {
        UpdateClock();
    }

    private void UpdateClock()
    {
        lblClock.Text = DateTime.Now.ToString("HH:mm:ss");
    }

    private void ParentForm_FormClosing(object? sender, FormClosingEventArgs e)
    {
        // Перед виходом з головного контейнера перевіряємо, чи є модифіковані вікна
        foreach (Form child in MdiChildren)
        {
            if (child is ChildDocumentForm doc && doc.IsModified)
            {
                // Активуємо форму для привернення уваги користувача
                doc.Activate();
                DialogResult res = MessageBox.Show(
                    this,
                    $"Документ \"{doc.DocumentTitle}\" містить незбережені зміни.\nЗберегти перед виходом з програми?",
                    "Незбережені дані",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning);

                if (res == DialogResult.Yes)
                {
                    bool saved = SaveChildDocument(doc);
                    if (!saved)
                    {
                        e.Cancel = true;
                        return;
                    }
                }
                else if (res == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
            }
        }
    }
}
