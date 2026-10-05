// ============================================================================
// Практична робота 6. Багатовіконні інтерфейси (MDI) у Windows Forms
// Студент: ТАРАС Вадим, група аІк43
// Модальне вікно відомостей про програму (AboutForm.cs)
// ============================================================================

using System;
using System.Drawing;
using System.Windows.Forms;

namespace Lab06_MDI;

/// <summary>
/// Модальне діалогове вікно "Про програму".
/// Демонструє відображення системної інформації, метаданих проєкту та архітектури MDI.
/// </summary>
public partial class AboutForm : Form
{
    public AboutForm()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        this.SuspendLayout();

        this.Text = "Про програму — Advanced MDI Studio";
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.ShowInTaskbar = false;
        this.StartPosition = FormStartPosition.CenterParent;
        this.ClientSize = new Size(520, 360);
        this.BackColor = Color.White;

        // Панель верхнього заголовка
        Panel headerPanel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 70,
            BackColor = Color.FromArgb(24, 76, 120)
        };

        Label lblHeader = new Label
        {
            Text = "Advanced MDI Studio",
            Font = new Font("Segoe UI", 16F, FontStyle.Bold),
            ForeColor = Color.White,
            Location = new Point(20, 12),
            AutoSize = true
        };

        Label lblSubHeader = new Label
        {
            Text = "Багатодокументний текстовий редактор Windows Forms (.NET 8.0)",
            Font = new Font("Segoe UI", 9.5F, FontStyle.Regular),
            ForeColor = Color.FromArgb(210, 230, 250),
            Location = new Point(22, 42),
            AutoSize = true
        };

        headerPanel.Controls.Add(lblHeader);
        headerPanel.Controls.Add(lblSubHeader);

        // Основне текстове поле з інформацією
        TextBox txtInfo = new TextBox
        {
            Multiline = true,
            ReadOnly = true,
            BorderStyle = BorderStyle.None,
            BackColor = Color.White,
            Font = new Font("Segoe UI", 9.5F),
            Location = new Point(22, 85),
            Size = new Size(475, 215),
            ScrollBars = ScrollBars.Vertical,
            Text = "Дисципліна: Інструментальні засоби візуального програмування\r\n" +
                   "Тема: Практична робота 6. Багатовіконні інтерфейси (MDI) та компоненти меню\r\n\r\n" +
                   "Виконавець: студент групи аІк43 ТАРАС Вадим\r\n" +
                   "Керівник: викладач КОСТІКОВ О.А.\r\n\r\n" +
                   "Ключові архітектурні складові проєкту:\r\n" +
                   " • MDI-контейнер (IsMdiContainer = true) з керуванням MdiChildren\r\n" +
                   " • Головне ієрархічне меню MenuStrip із гарячими клавішами (Ctrl+N, O, S, W)\r\n" +
                   " • Автоматичний список відкритих вікон MdiWindowListItem\r\n" +
                   " • Алгоритми упорядкування вікон: Cascade, TileHorizontal, TileVertical, ArrangeIcons\r\n" +
                   " • Швидка панель інструментів ToolStrip та контекстне меню ContextMenuStrip\r\n" +
                   " • Інформативний рядок стану StatusStrip із відстеженням рядків, колонок, символів та годинником\r\n" +
                   " • Безпечне закриття документів із перевіркою прапорця IsModified\r\n\r\n" +
                   "Версія: 1.0.0 (Release) | Платформа: .NET 8.0 Windows | 2026"
        };

        // Кнопка закриття OK
        Button btnOk = new Button
        {
            Text = "OK",
            DialogResult = DialogResult.OK,
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
            Size = new Size(100, 32),
            Location = new Point(397, 312),
            BackColor = Color.FromArgb(24, 76, 120),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        btnOk.FlatAppearance.BorderSize = 0;

        this.Controls.Add(headerPanel);
        this.Controls.Add(txtInfo);
        this.Controls.Add(btnOk);
        this.AcceptButton = btnOk;

        this.ResumeLayout(false);
    }
}
