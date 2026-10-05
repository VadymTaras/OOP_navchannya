// ============================================================================
// Практична робота 6. Багатовіконні інтерфейси (MDI) у Windows Forms
// Студент: ТАРАС Вадим, група аІк43
// Точка входу в програму (Program.cs)
// ============================================================================

namespace Lab06_MDI;

internal static class Program
{
    /// <summary>
    ///  Головна точка входу для додатку.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // Ініціалізація налаштувань середовища Windows Forms
        ApplicationConfiguration.Initialize();

        // Запуск головної батьківської MDI-форми
        Application.Run(new ParentForm());
    }
}
