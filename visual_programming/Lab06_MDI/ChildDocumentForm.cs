// ============================================================================
// Практична робота 6. Багатовіконні інтерфейси (MDI) у Windows Forms
// Студент: ТАРАС Вадим, група аІк43
// Дочірня форма документа (ChildDocumentForm.cs)
// ============================================================================

using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Lab06_MDI;

/// <summary>
/// Дочірня MDI-форма текстового документа.
/// Забезпечує редагування тексту, відстеження модифікацій, завантаження та збереження файлів.
/// </summary>
public partial class ChildDocumentForm : Form
{
    private string _documentTitle = "Новий документ";
    private string? _filePath = null;
    private bool _isModified = false;
    private bool _suppressModifiedEvent = false;

    /// <summary>
    /// Подія сповіщення про зміну вмісту або позиції курсора для оновлення рядка стану.
    /// </summary>
    public event EventHandler? DocumentStateChanged;

    /// <summary>
    /// Конструктор новоствореного документа.
    /// </summary>
    /// <param name="docNumber">Порядковий номер нового документа.</param>
    public ChildDocumentForm(int docNumber)
    {
        InitializeComponent();
        _documentTitle = $"Документ {docNumber}";
        _filePath = null;
        UpdateWindowTitle();
    }

    /// <summary>
    /// Конструктор для завантаження наявного файлу.
    /// </summary>
    /// <param name="filePath">Абсолютний шлях до файлу.</param>
    public ChildDocumentForm(string filePath)
    {
        InitializeComponent();
        _filePath = filePath;
        _documentTitle = Path.GetFileName(filePath);
        LoadFileContent(filePath);
        UpdateWindowTitle();
    }

    /// <summary>
    /// Шлях до збереженого файлу (null для нового документа).
    /// </summary>
    public string? FilePath => _filePath;

    /// <summary>
    /// Прапорець наявності незбережених змін у документі.
    /// </summary>
    public bool IsModified => _isModified;

    /// <summary>
    /// Базова назва документа.
    /// </summary>
    public string DocumentTitle => _documentTitle;

    /// <summary>
    /// Властивість доступу до текстового редактора RichTextBox.
    /// </summary>
    public RichTextBox Editor => rtbEditor;

    /// <summary>
    /// Загальна кількість символів у документі.
    /// </summary>
    public int CharacterCount => rtbEditor.TextLength;

    /// <summary>
    /// Поточний номер рядка курсора (1-indexed).
    /// </summary>
    public int CurrentLine
    {
        get
        {
            int index = rtbEditor.SelectionStart;
            return rtbEditor.GetLineFromCharIndex(index) + 1;
        }
    }

    /// <summary>
    /// Поточний номер стовпчика курсора (1-indexed).
    /// </summary>
    public int CurrentColumn
    {
        get
        {
            int index = rtbEditor.SelectionStart;
            int line = rtbEditor.GetLineFromCharIndex(index);
            int firstCharIndex = rtbEditor.GetFirstCharIndexFromLine(line);
            return (index - firstCharIndex) + 1;
        }
    }

    /// <summary>
    /// Призначення контекстного меню для текстового поля.
    /// </summary>
    public void SetContextMenu(ContextMenuStrip contextMenu)
    {
        rtbEditor.ContextMenuStrip = contextMenu;
    }

    /// <summary>
    /// Оновлення заголовка вікна з індикацією змін '*'.
    /// </summary>
    private void UpdateWindowTitle()
    {
        Text = _isModified ? $"{_documentTitle} *" : _documentTitle;
    }

    /// <summary>
    /// Завантаження вмісту файлу у редактор.
    /// </summary>
    private void LoadFileContent(string path)
    {
        try
        {
            _suppressModifiedEvent = true;
            string ext = Path.GetExtension(path).ToLowerInvariant();
            if (ext == ".rtf")
            {
                rtbEditor.LoadFile(path, RichTextBoxStreamType.RichText);
            }
            else
            {
                rtbEditor.LoadFile(path, RichTextBoxStreamType.PlainText);
            }
            _filePath = path;
            _documentTitle = Path.GetFileName(path);
            _isModified = false;
        }
        finally
        {
            _suppressModifiedEvent = false;
            UpdateWindowTitle();
        }
    }

    /// <summary>
    /// Збереження вмісту документа у файл.
    /// </summary>
    public void Save(string path)
    {
        string ext = Path.GetExtension(path).ToLowerInvariant();
        if (ext == ".rtf")
        {
            rtbEditor.SaveFile(path, RichTextBoxStreamType.RichText);
        }
        else
        {
            rtbEditor.SaveFile(path, RichTextBoxStreamType.PlainText);
        }

        _filePath = path;
        _documentTitle = Path.GetFileName(path);
        _isModified = false;
        UpdateWindowTitle();
        DocumentStateChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Обробка зміни тексту.
    /// </summary>
    private void RtbEditor_TextChanged(object? sender, EventArgs e)
    {
        if (_suppressModifiedEvent) return;

        if (!_isModified)
        {
            _isModified = true;
            UpdateWindowTitle();
        }
        DocumentStateChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Обробка зміни виділення та переміщення курсора.
    /// </summary>
    private void RtbEditor_SelectionChanged(object? sender, EventArgs e)
    {
        DocumentStateChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Обробка закриття дочірньої форми: перевірка наявності незбережених змін.
    /// </summary>
    private void ChildDocumentForm_FormClosing(object? sender, FormClosingEventArgs e)
    {
        if (_isModified)
        {
            DialogResult result = MessageBox.Show(
                $"Зберегти зміни у документі \"{_documentTitle}\" перед закриттям?",
                "Незбережені зміни",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                // Запит збереження через батьківську форму
                if (MdiParent is ParentForm parent)
                {
                    bool saved = parent.SaveChildDocument(this);
                    if (!saved)
                    {
                        // Якщо користувач скасував діалог збереження
                        e.Cancel = true;
                        return;
                    }
                }
            }
            else if (result == DialogResult.Cancel)
            {
                e.Cancel = true;
                return;
            }
        }
    }

    // --- Методи редагування тексту ---

    public void UndoAction() => rtbEditor.Undo();
    public void RedoAction() => rtbEditor.Redo();
    public void CutAction() => rtbEditor.Cut();
    public void CopyAction() => rtbEditor.Copy();
    public void PasteAction() => rtbEditor.Paste();
    public void SelectAllAction() => rtbEditor.SelectAll();

    public void ApplyFont(Font font)
    {
        if (rtbEditor.SelectionLength > 0)
            rtbEditor.SelectionFont = font;
        else
            rtbEditor.Font = font;
    }

    public void ApplyTextColor(Color color)
    {
        if (rtbEditor.SelectionLength > 0)
            rtbEditor.SelectionColor = color;
        else
            rtbEditor.ForeColor = color;
    }

    public void ApplyBackColor(Color color)
    {
        if (rtbEditor.SelectionLength > 0)
            rtbEditor.SelectionBackColor = color;
        else
            rtbEditor.BackColor = color;
    }
}
