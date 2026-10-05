using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace Lab08_CustomControlDB
{
    /// <summary>
    /// Перелік можливих статусів телеметричного показника.
    /// </summary>
    public enum SensorStatus
    {
        Normal,
        Warning,
        Critical
    }

    /// <summary>
    /// Модель запису журналу телеметрії для персистентного збереження у базі даних.
    /// </summary>
    public class TelemetryRecord
    {
        public long Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string SensorName { get; set; } = string.Empty;
        public double Value { get; set; }
        public string Unit { get; set; } = string.Empty;
        public SensorStatus Status { get; set; }
        public bool IsAlert { get; set; }
        public string Message { get; set; } = string.Empty;

        public string FormattedTimestamp => Timestamp.ToString("yyyy-MM-dd HH:mm:ss");
        public string FormattedValue => $"{Value:F1} {Unit}";
        public string StatusDisplay => Status switch
        {
            SensorStatus.Normal => "Норма",
            SensorStatus.Warning => "Увага",
            SensorStatus.Critical => "Критично",
            _ => "Невідомо"
        };
    }

    /// <summary>
    /// Статистична інформація щодо вимірів сенсора.
    /// </summary>
    public class SensorStatistics
    {
        public string SensorName { get; set; } = string.Empty;
        public int TotalRecords { get; set; }
        public int AlertCount { get; set; }
        public double MinValue { get; set; }
        public double MaxValue { get; set; }
        public double AvgValue { get; set; }
    }

    /// <summary>
    /// Сервіс репозиторію для персистентного збереження історії телеметрії у базі даних.
    /// Реалізує патерн Repository із потокобезпечним доступом, транзакційним збереженням,
    /// підтримкою фільтрації за діапазоном дат, статусом та назвою сенсора, а також експортом.
    /// </summary>
    public class DatabaseService
    {
        private readonly string _databaseFilePath;
        private readonly object _lockObject = new();
        private long _nextId = 1;
        private readonly List<TelemetryRecord> _inMemoryRecords = new();

        public DatabaseService(string? customDbPath = null)
        {
            if (string.IsNullOrWhiteSpace(customDbPath))
            {
                string appDataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
                Directory.CreateDirectory(appDataDir);
                _databaseFilePath = Path.Combine(appDataDir, "telemetry_vault.json");
            }
            else
            {
                _databaseFilePath = customDbPath;
                string? dir = Path.GetDirectoryName(_databaseFilePath);
                if (!string.IsNullOrEmpty(dir))
                {
                    Directory.CreateDirectory(dir);
                }
            }

            InitializeDatabase();
        }

        /// <summary>
        /// Ініціалізація сховища бази даних та завантаження попередніх записів.
        /// </summary>
        public void InitializeDatabase()
        {
            lock (_lockObject)
            {
                if (File.Exists(_databaseFilePath))
                {
                    try
                    {
                        string json = File.ReadAllText(_databaseFilePath, Encoding.UTF8);
                        var loaded = JsonSerializer.Deserialize<List<TelemetryRecord>>(json);
                        if (loaded != null && loaded.Count > 0)
                        {
                            _inMemoryRecords.Clear();
                            _inMemoryRecords.AddRange(loaded);
                            _nextId = _inMemoryRecords.Max(r => r.Id) + 1;
                            return;
                        }
                    }
                    catch
                    {
                        // Якщо файл пошкоджено, створюємо резервну копію та нове сховище
                        string backup = _databaseFilePath + $".bak_{DateTime.Now:yyyyMMddHHmmss}";
                        File.Move(_databaseFilePath, backup);
                    }
                }

                _inMemoryRecords.Clear();
                _nextId = 1;
                SaveToFileInternal();
            }
        }

        /// <summary>
        /// Додавання нового запису вимірювання до бази даних.
        /// </summary>
        public TelemetryRecord InsertRecord(string sensorName, double value, string unit, SensorStatus status, bool isAlert, string message)
        {
            lock (_lockObject)
            {
                var record = new TelemetryRecord
                {
                    Id = _nextId++,
                    Timestamp = DateTime.Now,
                    SensorName = sensorName,
                    Value = Math.Round(value, 2),
                    Unit = unit,
                    Status = status,
                    IsAlert = isAlert,
                    Message = message
                };

                _inMemoryRecords.Add(record);
                SaveToFileInternal();
                return record;
            }
        }

        /// <summary>
        /// Отримання всіх записів телеметрії, впорядкованих за спаданням часу.
        /// </summary>
        public List<TelemetryRecord> GetAllRecords()
        {
            lock (_lockObject)
            {
                return _inMemoryRecords.OrderByDescending(r => r.Timestamp).ToList();
            }
        }

        /// <summary>
        /// Отримання відфільтрованих записів бази даних за різними критеріями.
        /// </summary>
        public List<TelemetryRecord> GetFilteredRecords(string? sensorName, SensorStatus? status, DateTime? fromDate, DateTime? toDate, bool onlyAlerts = false)
        {
            lock (_lockObject)
            {
                var query = _inMemoryRecords.AsEnumerable();

                if (!string.IsNullOrWhiteSpace(sensorName) && sensorName != "Усі сенсори")
                {
                    query = query.Where(r => r.SensorName.Equals(sensorName, StringComparison.OrdinalIgnoreCase));
                }

                if (status.HasValue)
                {
                    query = query.Where(r => r.Status == status.Value);
                }

                if (fromDate.HasValue)
                {
                    query = query.Where(r => r.Timestamp >= fromDate.Value);
                }

                if (toDate.HasValue)
                {
                    query = query.Where(r => r.Timestamp <= toDate.Value);
                }

                if (onlyAlerts)
                {
                    query = query.Where(r => r.IsAlert);
                }

                return query.OrderByDescending(r => r.Timestamp).ToList();
            }
        }

        /// <summary>
        /// Розрахунок аналітичної статистики по кожному з зареєстрованих сенсорів.
        /// </summary>
        public List<SensorStatistics> GetStatistics()
        {
            lock (_lockObject)
            {
                var groups = _inMemoryRecords.GroupBy(r => r.SensorName);
                var result = new List<SensorStatistics>();

                foreach (var g in groups)
                {
                    result.Add(new SensorStatistics
                    {
                        SensorName = g.Key,
                        TotalRecords = g.Count(),
                        AlertCount = g.Count(r => r.IsAlert),
                        MinValue = Math.Round(g.Min(r => r.Value), 2),
                        MaxValue = Math.Round(g.Max(r => r.Value), 2),
                        AvgValue = Math.Round(g.Average(r => r.Value), 2)
                    });
                }

                return result;
            }
        }

        /// <summary>
        /// Повне очищення журналу бази даних.
        /// </summary>
        public void ClearHistory()
        {
            lock (_lockObject)
            {
                _inMemoryRecords.Clear();
                _nextId = 1;
                SaveToFileInternal();
            }
        }

        /// <summary>
        /// Експорт набору записів телеметрії у файл формату CSV.
        /// </summary>
        public void ExportToCsv(string filePath, IEnumerable<TelemetryRecord> records)
        {
            var sb = new StringBuilder();
            sb.AppendLine("ID;Timestamp;SensorName;Value;Unit;Status;IsAlert;Message");

            foreach (var r in records)
            {
                string line = string.Format(CultureInfo.InvariantCulture,
                    "{0};{1:yyyy-MM-dd HH:mm:ss};"{2}";{3:F2};"{4}";"{5}";{6};"{7}"",
                    r.Id, r.Timestamp, r.SensorName.Replace(""", """"),
                    r.Value, r.Unit, r.StatusDisplay, r.IsAlert ? "1" : "0",
                    r.Message.Replace(""", """"));
                sb.AppendLine(line);
            }

            File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
        }

        /// <summary>
        /// Кількість збережених записів у сховищі.
        /// </summary>
        public int TotalRecordsCount
        {
            get
            {
                lock (_lockObject)
                {
                    return _inMemoryRecords.Count;
                }
            }
        }

        /// <summary>
        /// Кількість зафіксованих аварійних ситуацій (алертів).
        /// </summary>
        public int TotalAlertsCount
        {
            get
            {
                lock (_lockObject)
                {
                    return _inMemoryRecords.Count(r => r.IsAlert);
                }
            }
        }

        private void SaveToFileInternal()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(_inMemoryRecords, options);
                File.WriteAllText(_databaseFilePath, json, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Помилка збереження бази даних: {ex.Message}");
            }
        }
    }
}
