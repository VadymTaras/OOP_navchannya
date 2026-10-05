using System;
using System.Windows.Forms;

namespace Lab03_Dialogs
{
    /// <summary>
    /// Точка входу в застосунок Windows Forms.
    /// Практична робота №3: Діалогові вікна та файли у Windows Forms.
    /// Виконавець: студент групи аІк43 Тарас Вадим.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Головна точка входу для додатку.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Увімкнення візуальних стилів операційної системи
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Налаштування високої роздільної здатності DPI (net8.0-windows)
            ApplicationConfiguration.Initialize();

            // Запуск головної форми додатку
            Application.Run(new MainForm());
        }
    }
}
