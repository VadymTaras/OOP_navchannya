namespace Lab08_CustomControlDB;

/// <summary>
/// Головна точка входу для додатку апаратного моніторингу та телеметрії.
/// Дисципліна: Інструментальні засоби візуального програмування.
/// Виконавець: студент групи аІк43 Тарас Вадим.
/// </summary>
internal static class Program
{
    [STAThread]
    static void Main()
    {
        // Ініціалізація конфігурації Windows Forms (.NET 8.0)
        ApplicationConfiguration.Initialize();
        
        // Запуск головної форми телеметричного моніторингу
        Application.Run(new MainForm());
    }
}
