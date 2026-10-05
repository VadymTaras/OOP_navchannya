namespace Lab05_GDI;

/// <summary>
/// Головна точка входу для додатку.
/// Практична робота 5. Графічні можливості GDI+ у Windows Forms.
/// Студент: ТАРАС Вадим, група аІк43.
/// </summary>
internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());
    }
}
