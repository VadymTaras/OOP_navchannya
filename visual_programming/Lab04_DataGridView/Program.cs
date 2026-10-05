using System;
using System.Windows.Forms;

namespace Lab04_DataGridView
{
    internal static class Program
    {
        /// <summary>
        /// Головна точка входу для додатку.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }
}
