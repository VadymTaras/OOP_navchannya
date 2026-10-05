using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Lab08_CustomControlDB
{
    /// <summary>
    /// Аргументи події спрацювання тривоги сенсора.
    /// Передає інформацію про поточне значення, встановлені пороги та рівень критичності.
    /// </summary>
    public class AlertEventArgs : EventArgs
    {
        public string SensorName { get; }
        public double CurrentValue { get; }
        public double Threshold { get; }
        public string Unit { get; }
        public SensorStatus Status { get; }
        public DateTime Timestamp { get; }
        public string Message { get; }

        public AlertEventArgs(string sensorName, double currentValue, double threshold, string unit, SensorStatus status, string message)
        {
            SensorName = sensorName;
            CurrentValue = currentValue;
            Threshold = threshold;
            Unit = unit;
            Status = status;
            Timestamp = DateTime.Now;
            Message = message;
        }
    }

    /// <summary>
    /// Спеціалізований багаторазовий візуальний компонент UserControl «Картка сенсора телеметрії».
    /// Демонструє розробку складених користувацьких елементів керування Windows Forms з
    /// атрибутами середовища розробки (Category, Description), власними подіями (AlertTriggered),
    /// візуальною колірною індикацією та методом безпечного оновлення телеметрії.
    /// </summary>
    [DefaultEvent("AlertTriggered")]
    [DefaultProperty("SensorName")]
    [Description("Візуальний компонент панелі моніторингу телеметрії окремого датчика або показника")]
    public partial class SensorCardControl : UserControl
    {
        // Внутрішній стан компонента
        private string _sensorName = "Сенсор";
        private double _value = 0.0;
        private string _unit = "°C";
        private double _minValue = 0.0;
        private double _maxValue = 100.0;
        private double _minThreshold = 10.0;
        private double _maxThreshold = 80.0;
        private SensorStatus _status = SensorStatus.Normal;
        private Color _cardAccentColor = Color.FromArgb(37, 99, 235);
        private bool _isAlertActive = false;

        #region Власні події компонента (Custom Events)

        /// <summary>
        /// Подія виникає, коли показник сенсора виходить за встановлені мінімальні або максимальні пороги безпеки.
        /// </summary>
        [Category("Telemetry Alerts")]
        [Description("Викликається при виході показника сенсора за встановлені пороги або переході в аварійний стан")]
        public event EventHandler<AlertEventArgs>? AlertTriggered;

        /// <summary>
        /// Подія зміни значення сенсора.
        /// </summary>
        [Category("Telemetry Action")]
        [Description("Викликається при кожній зміні числового значення показника сенсора")]
        public event EventHandler? ValueChanged;

        #endregion

        #region Власні властивості з атрибутами дизайнера (Custom Properties)

        [Category("Telemetry Configuration")]
        [Description("Назва сенсора або відстежуваної апаратної метрики")]
        [DefaultValue("Сенсор")]
        public string SensorName
        {
            get => _sensorName;
            set
            {
                _sensorName = value;
                lblSensorName.Text = _sensorName;
            }
        }

        [Category("Telemetry Data")]
        [Description("Поточне числове значення показника сенсора")]
        [DefaultValue(0.0)]
        public double Value
        {
            get => _value;
            set
            {
                UpdateTelemetry(value);
            }
        }

        [Category("Telemetry Configuration")]
        [Description("Одиниця вимірювання фізичної величини (°C, %, Мбіт/с, Бар тощо)")]
        [DefaultValue("°C")]
        public string Unit
        {
            get => _unit;
            set
            {
                _unit = value;
                lblUnit.Text = _unit;
                UpdateThresholdsDisplay();
            }
        }

        [Category("Telemetry Configuration")]
        [Description("Мінімальне значення шкали відображення")]
        [DefaultValue(0.0)]
        public double MinValue
        {
            get => _minValue;
            set
            {
                _minValue = value;
                UpdateProgressBar();
            }
        }

        [Category("Telemetry Configuration")]
        [Description("Максимальне значення шкали відображення")]
        [DefaultValue(100.0)]
        public double MaxValue
        {
            get => _maxValue;
            set
            {
                _maxValue = value;
                UpdateProgressBar();
            }
        }

        [Category("Telemetry Thresholds")]
        [Description("Нижній критичний поріг показника, нижче якого фіксується порушення")]
        [DefaultValue(10.0)]
        public double MinThreshold
        {
            get => _minThreshold;
            set
            {
                _minThreshold = value;
                UpdateThresholdsDisplay();
            }
        }

        [Category("Telemetry Thresholds")]
        [Description("Верхній критичний поріг показника, вище якого фіксується аварія")]
        [DefaultValue(80.0)]
        public double MaxThreshold
        {
            get => _maxThreshold;
            set
            {
                _maxThreshold = value;
                UpdateThresholdsDisplay();
            }
        }

        [Category("Telemetry Data")]
        [Description("Поточний експлуатаційний статус датчика (Normal, Warning, Critical)")]
        [ReadOnly(true)]
        public SensorStatus Status
        {
            get => _status;
            private set
            {
                _status = value;
                UpdateVisualStatus();
            }
        }

        [Category("Telemetry Appearance")]
        [Description("Базовий акцентний колір оформлення картки в нормальному стані")]
        public Color CardAccentColor
        {
            get => _cardAccentColor;
            set
            {
                _cardAccentColor = value;
                pnlHeaderColor.BackColor = _cardAccentColor;
            }
        }

        #endregion

        public SensorCardControl()
        {
            InitializeComponent();
            ApplyInitialStyles();
        }

        private void ApplyInitialStyles()
        {
            // Налаштування початкового оформлення
            pnlContainer.BackColor = Color.FromArgb(250, 251, 253);
            pnlHeaderColor.BackColor = _cardAccentColor;
            lblSensorName.Text = _sensorName;
            lblValue.Text = $"{_value:F1}";
            lblUnit.Text = _unit;
            UpdateThresholdsDisplay();
            UpdateVisualStatus();
        }

        /// <summary>
        /// Головний метод оновлення телеметрії.
        /// Виконує перерахунок статусу, оновлює графічні індикатори та генерує подію AlertTriggered при аварії.
        /// </summary>
        public void UpdateTelemetry(double newValue)
        {
            _value = newValue;
            lblValue.Text = $"{_value:F1}";
            UpdateProgressBar();

            // Аналіз значень відносно порогів
            SensorStatus previousStatus = _status;
            SensorStatus calculatedStatus;
            bool isAlert = false;
            string alertMessage = string.Empty;
            double triggeredThreshold = 0;

            if (_value >= _maxThreshold)
            {
                calculatedStatus = SensorStatus.Critical;
                isAlert = true;
                triggeredThreshold = _maxThreshold;
                alertMessage = $"Перевищено критичний верхній поріг: {_value:F1} {_unit} (поріг: {_maxThreshold:F1} {_unit})";
            }
            else if (_value <= _minThreshold)
            {
                calculatedStatus = SensorStatus.Critical;
                isAlert = true;
                triggeredThreshold = _minThreshold;
                alertMessage = $"Показник нижче критичного мінімуму: {_value:F1} {_unit} (поріг: {_minThreshold:F1} {_unit})";
            }
            else if (_value >= _maxThreshold - (_maxThreshold - _minThreshold) * 0.15)
            {
                // Наближення до верхнього порогу (попереджувальний статус)
                calculatedStatus = SensorStatus.Warning;
            }
            else
            {
                calculatedStatus = SensorStatus.Normal;
            }

            Status = calculatedStatus;

            // Виклик події при переході в стан аварії або триваючій аварії
            if (isAlert)
            {
                _isAlertActive = true;
                OnAlertTriggered(new AlertEventArgs(_sensorName, _value, triggeredThreshold, _unit, calculatedStatus, alertMessage));
            }
            else
            {
                _isAlertActive = false;
            }

            OnValueChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Оновлення індикатора шкали прогресу.
        /// </summary>
        private void UpdateProgressBar()
        {
            if (_maxValue <= _minValue) return;

            double clamped = Math.Max(_minValue, Math.Min(_maxValue, _value));
            int percent = (int)(((clamped - _minValue) / (_maxValue - _minValue)) * 100.0);
            progressBar.Value = Math.Max(0, Math.Min(100, percent));
        }

        /// <summary>
        /// Оновлення тексту відображення граничних порогів.
        /// </summary>
        private void UpdateThresholdsDisplay()
        {
            lblThresholds.Text = $"Поріг: [{_minThreshold:F1} - {_maxThreshold:F1} {_unit}]";
        }

        /// <summary>
        /// Оновлення колірної гами та бейджа картки відповідно до статусу.
        /// </summary>
        private void UpdateVisualStatus()
        {
            switch (_status)
            {
                case SensorStatus.Normal:
                    lblStatusBadge.Text = "НОРМА";
                    lblStatusBadge.BackColor = Color.FromArgb(46, 125, 50); // Зелений
                    lblStatusBadge.ForeColor = Color.White;
                    pnlHeaderColor.BackColor = _cardAccentColor;
                    pnlContainer.BackColor = Color.FromArgb(250, 251, 253);
                    lblValue.ForeColor = Color.FromArgb(15, 23, 42);
                    break;

                case SensorStatus.Warning:
                    lblStatusBadge.Text = "УВАГА";
                    lblStatusBadge.BackColor = Color.FromArgb(245, 158, 11); // Янтарний
                    lblStatusBadge.ForeColor = Color.FromArgb(15, 23, 42);
                    pnlHeaderColor.BackColor = Color.FromArgb(245, 158, 11);
                    pnlContainer.BackColor = Color.FromArgb(254, 252, 232);
                    lblValue.ForeColor = Color.FromArgb(180, 83, 9);
                    break;

                case SensorStatus.Critical:
                    lblStatusBadge.Text = "КРИТИЧНО";
                    lblStatusBadge.BackColor = Color.FromArgb(220, 38, 38); // Червоний
                    lblStatusBadge.ForeColor = Color.White;
                    pnlHeaderColor.BackColor = Color.FromArgb(220, 38, 38);
                    pnlContainer.BackColor = Color.FromArgb(254, 242, 242);
                    lblValue.ForeColor = Color.FromArgb(185, 28, 28);
                    break;
            }
        }

        protected virtual void OnAlertTriggered(AlertEventArgs e)
        {
            AlertTriggered?.Invoke(this, e);
        }

        protected virtual void OnValueChanged(EventArgs e)
        {
            ValueChanged?.Invoke(this, e);
        }
    }
}
