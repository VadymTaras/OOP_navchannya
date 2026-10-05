import 'dart:async';
import 'dart:math';
import 'package:flutter/material.dart';

/// Головна точка входу в програму.
/// Демонструє запуск Material 3 застосунку для моніторингу телеметрії.
void main() {
  runApp(const TelemetryMonitorApp());
}

/// Кореневий віджет програми.
class TelemetryMonitorApp extends StatelessWidget {
  const TelemetryMonitorApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Real-Time Hardware & Telemetry Monitor',
      debugShowCheckedModeBanner: false,
      theme: ThemeData(
        useMaterial3: true,
        colorScheme: ColorScheme.fromSeed(
          seedColor: Colors.teal,
          brightness: Brightness.dark,
        ),
        cardTheme: CardTheme(
          elevation: 2,
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(12),
          ),
        ),
      ),
      home: const TelemetryDashboardScreen(),
    );
  }
}

/// Модель первинних діагностичних даних системи.
/// Завантажується асинхронно через [Future] під час старту.
class SystemDiagnostics {
  final String processorModel;
  final int totalCores;
  final double totalRamGb;
  final String firmwareVersion;
  final DateTime bootTime;

  const SystemDiagnostics({
    required this.processorModel,
    required this.totalCores,
    required this.totalRamGb,
    required this.firmwareVersion,
    required this.bootTime,
  });
}

/// Модель періодичних телеметричних показників.
/// Надходить реактивним потоком через [Stream] щосекунди.
class TelemetryData {
  final double cpuLoad; // у відсотках (0..100)
  final double cpuTemperature; // у градусах Цельсія
  final double ramUsedGb; // зайнята пам'ять у ГБ
  final double networkThroughputMbps; // пропускна здатність у Мбіт/с
  final DateTime timestamp;

  const TelemetryData({
    required this.cpuLoad,
    required this.cpuTemperature,
    required this.ramUsedGb,
    required this.networkThroughputMbps,
    required this.timestamp,
  });
}

/// Сервіс генерації реактивного потоку подій телеметрії.
/// Використовує [StreamController.broadcast] для підтримки кількох підписників.
class TelemetryService {
  final Random _random = Random();
  late StreamController<TelemetryData> _controller;
  Timer? _timer;
  bool _isPaused = false;
  int _tickCount = 0;

  TelemetryService() {
    // Створюємо широкомовний (broadcast) потік для гнучкого підключення слухачів
    _controller = StreamController<TelemetryData>.broadcast(
      onListen: _startEmitting,
      onCancel: _stopEmitting,
    );
  }

  /// Потік телеметричних даних для підписки віджетів
  Stream<TelemetryData> get telemetryStream => _controller.stream;

  bool get isPaused => _isPaused;
  int get tickCount => _tickCount;

  void _startEmitting() {
    _timer ??= Timer.periodic(const Duration(seconds: 1), (timer) {
      if (_isPaused) return;

      _tickCount++;
      // Формуємо динамічні метрики з реалістичними псевдовипадковими коливаннями
      final cpuLoad = 20.0 + _random.nextDouble() * 65.0; // 20% .. 85%
      final cpuTemp = 42.0 + (cpuLoad * 0.45) + (_random.nextDouble() * 5.0); // 42°C .. 85°C
      final ramUsed = 4.2 + _random.nextDouble() * 6.5; // 4.2 .. 10.7 ГБ
      final netSpeed = 12.0 + _random.nextDouble() * 140.0; // 12 .. 152 Мбіт/с

      final data = TelemetryData(
        cpuLoad: cpuLoad,
        cpuTemperature: cpuTemp,
        ramUsedGb: ramUsed,
        networkThroughputMbps: netSpeed,
        timestamp: DateTime.now(),
      );

      // Надсилаємо нову подію в потік
      if (!_controller.isClosed) {
        _controller.add(data);
      }
    });
  }

  void _stopEmitting() {
    _timer?.cancel();
    _timer = null;
  }

  /// Призупинення генерації подій
  void togglePause() {
    _isPaused = !_isPaused;
  }

  /// Скидання лічильника та генератора
  void reset() {
    _tickCount = 0;
    _isPaused = false;
  }

  /// Симуляція аварійної помилки в потоці даних (Stream.error)
  /// Демонструє реакцію StreamBuilder на стан snapshot.hasError
  void simulateEmergencyFault() {
    if (!_controller.isClosed) {
      _controller.addError(
        'АВАРІЙНИЙ ЗБІЙ ШИНИ I2C: Перевищено критичний поріг температури термодатчика сенсорного блоку #3!',
      );
    }
  }

  /// Відновлення після помилки (надсилання валідного стану)
  void recoverFromFault() {
    if (!_controller.isClosed) {
      _controller.add(
        TelemetryData(
          cpuLoad: 25.0,
          cpuTemperature: 45.0,
          ramUsedGb: 4.5,
          networkThroughputMbps: 20.0,
          timestamp: DateTime.now(),
        ),
      );
    }
  }

  void dispose() {
    _stopEmitting();
    _controller.close();
  }
}

/// Головний екран моніторингу телеметрії та стану обладнання.
class TelemetryDashboardScreen extends StatefulWidget {
  const TelemetryDashboardScreen({super.key});

  @override
  State<TelemetryDashboardScreen> createState() =>
      _TelemetryDashboardScreenState();
}

class _TelemetryDashboardScreenState extends State<TelemetryDashboardScreen> {
  late TelemetryService _telemetryService;
  late Future<SystemDiagnostics> _initialDiagnosticsFuture;
  final List<TelemetryData> _telemetryHistory = [];
  static const int _maxHistoryLength = 10;

  @override
  void initState() {
    super.initState();
    _telemetryService = TelemetryService();
    // Ініціалізуємо асинхронне завантаження первинних характеристик
    _initialDiagnosticsFuture = _fetchHardwareDiagnostics();
  }

  @override
  void dispose() {
    _telemetryService.dispose();
    super.dispose();
  }

  /// Імітація асинхронного звернення до апаратного шару через Future.delayed.
  /// Демонструє конструкцію async/await у Dart.
  Future<SystemDiagnostics> _fetchHardwareDiagnostics() async {
    // Імітуємо затримку апаратного опитування сенсорів (2.5 секунди)
    await Future.delayed(const Duration(milliseconds: 2500));

    // Повертаємо сформовані діагностичні дані
    return SystemDiagnostics(
      processorModel: 'ARM Cortex-A78 (Octa-Core @ 2.84 GHz)',
      totalCores: 8,
      totalRamGb: 16.0,
      firmwareVersion: 'UEFI/Kernel v6.8.4-telemetry-rt',
      bootTime: DateTime.now().subtract(const Duration(hours: 4, minutes: 18)),
    );
  }

  /// Перезапуск ініціалізаційного Future
  void _reloadDiagnostics() {
    setState(() {
      _initialDiagnosticsFuture = _fetchHardwareDiagnostics();
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Row(
          children: [
            Icon(Icons.monitor_heart_outlined, color: Colors.tealAccent),
            SizedBox(width: 10),
            Text('Hardware & Telemetry Monitor'),
          ],
        ),
        actions: [
          IconButton(
            tooltip: 'Повторне сканування апаратного вузла',
            icon: const Icon(Icons.refresh),
            onPressed: _reloadDiagnostics,
          ),
        ],
      ),
      body: SingleChildScrollView(
        padding: const EdgeInsets.all(16.0),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: [
            // СЕКЦІЯ 1: FutureBuilder (Асинхронна ініціалізація обладнання)
            _buildFutureDiagnosticsSection(),
            const SizedBox(height: 20),

            // СЕКЦІЯ 2: Панель кнопок керування реактивним потоком Stream
            _buildStreamControlsSection(),
            const SizedBox(height: 20),

            // СЕКЦІЯ 3: StreamBuilder (Живе відображення показників у реальному часі)
            _buildStreamTelemetrySection(),
            const SizedBox(height: 20),

            // СЕКЦІЯ 4: Історія останніх подій потоку
            _buildHistoryLogSection(),
          ],
        ),
      ),
    );
  }

  /// Віджет Секції 1: Асинхронний моніторинг конфігурації через FutureBuilder.
  Widget _buildFutureDiagnosticsSection() {
    return Card(
      child: Padding(
        padding: const EdgeInsets.all(16.0),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            const Row(
              children: [
                Icon(Icons.memory, color: Colors.cyanAccent),
                SizedBox(width: 8),
                Text(
                  'Апаратна конфігурація (FutureBuilder)',
                  style: TextStyle(fontSize: 16, fontWeight: FontWeight.bold),
                ),
              ],
            ),
            const Divider(height: 20),
            FutureBuilder<SystemDiagnostics>(
              future: _initialDiagnosticsFuture,
              builder: (context, snapshot) {
                // 1. Стан очікування виконання асинхронної операції
                if (snapshot.connectionState == ConnectionState.waiting) {
                  return const Padding(
                    padding: EdgeInsets.symmetric(vertical: 20.0),
                    child: Center(
                      child: Column(
                        children: [
                          CircularProgressIndicator(strokeWidth: 3),
                          SizedBox(height: 12),
                          Text(
                            'Опитування апаратних шин та калібрування сенсорів...',
                            style: TextStyle(color: Colors.grey),
                          ),
                        ],
                      ),
                    ),
                  );
                }

                // 2. Обробка аварійного стану або помилки Future
                if (snapshot.hasError) {
                  return Container(
                    padding: const EdgeInsets.all(12),
                    decoration: BoxDecoration(
                      color: Colors.red.withOpacity(0.15),
                      borderRadius: BorderRadius.circular(8),
                      border: Border.all(color: Colors.redAccent),
                    ),
                    child: Row(
                      children: [
                        const Icon(Icons.error_outline, color: Colors.redAccent),
                        const SizedBox(width: 10),
                        Expanded(
                          child: Text(
                            'Помилка опитування системи: ${snapshot.error}',
                            style: const TextStyle(color: Colors.redAccent),
                          ),
                        ),
                        TextButton(
                          onPressed: _reloadDiagnostics,
                          child: const Text('Повторити'),
                        ),
                      ],
                    ),
                  );
                }

                // 3. Успішне отримання результату (snapshot.hasData)
                if (snapshot.hasData) {
                  final data = snapshot.data!;
                  return Column(
                    children: [
                      _buildInfoRow(
                        Icons.developer_board,
                        'Процесор:',
                        data.processorModel,
                      ),
                      const SizedBox(height: 8),
                      _buildInfoRow(
                        Icons.speed,
                        'Кількість ядер:',
                        '${data.totalCores} ядер (SMP)',
                      ),
                      const SizedBox(height: 8),
                      _buildInfoRow(
                        Icons.straighten,
                        'Оперативна пам\'ять:',
                        '${data.totalRamGb.toStringAsFixed(1)} ГБ LPDDR5',
                      ),
                      const SizedBox(height: 8),
                      _buildInfoRow(
                        Icons.terminal,
                        'Версія прошивки:',
                        data.firmwareVersion,
                      ),
                    ],
                  );
                }

                return const Text('Немає доступних діагностичних даних.');
              },
            ),
          ],
        ),
      ),
    );
  }

  /// Допоміжний рядок виводу параметрів конфігурації
  Widget _buildInfoRow(IconData icon, String label, String value) {
    return Row(
      children: [
        Icon(icon, size: 18, color: Colors.tealAccent),
        const SizedBox(width: 8),
        Text(label, style: const TextStyle(color: Colors.white70)),
        const SizedBox(width: 6),
        Expanded(
          child: Text(
            value,
            textAlign: TextAlign.end,
            style: const TextStyle(fontWeight: FontWeight.w600),
          ),
        ),
      ],
    );
  }

  /// Віджет Секції 2: Панель інтерактивного керування реактивним потоком Stream.
  Widget _buildStreamControlsSection() {
    return Card(
      color: Colors.blueGrey.shade900.withOpacity(0.5),
      child: Padding(
        padding: const EdgeInsets.all(12.0),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            const Text(
              'Керування реактивним потоком (StreamController):',
              style: TextStyle(fontWeight: FontWeight.bold, fontSize: 14),
            ),
            const SizedBox(height: 10),
            Wrap(
              spacing: 8,
              runSpacing: 8,
              children: [
                ElevatedButton.icon(
                  onPressed: () {
                    setState(() {
                      _telemetryService.togglePause();
                    });
                  },
                  icon: Icon(
                    _telemetryService.isPaused
                        ? Icons.play_arrow
                        : Icons.pause,
                  ),
                  label: Text(_telemetryService.isPaused ? 'Відновити' : 'Пауза'),
                  style: ElevatedButton.styleFrom(
                    backgroundColor: _telemetryService.isPaused
                        ? Colors.green.shade700
                        : Colors.amber.shade800,
                  ),
                ),
                ElevatedButton.icon(
                  onPressed: () {
                    setState(() {
                      _telemetryService.reset();
                      _telemetryHistory.clear();
                    });
                  },
                  icon: const Icon(Icons.restart_alt),
                  label: const Text('Скинути лічильник'),
                ),
                ElevatedButton.icon(
                  onPressed: () {
                    _telemetryService.simulateEmergencyFault();
                  },
                  icon: const Icon(Icons.bolt, color: Colors.white),
                  label: const Text('Симуляція аварії'),
                  style: ElevatedButton.styleFrom(
                    backgroundColor: Colors.red.shade800,
                  ),
                ),
                ElevatedButton.icon(
                  onPressed: () {
                    _telemetryService.recoverFromFault();
                  },
                  icon: const Icon(Icons.healing, color: Colors.white),
                  label: const Text('Відновити після збою'),
                  style: ElevatedButton.styleFrom(
                    backgroundColor: Colors.teal.shade700,
                  ),
                ),
              ],
            ),
          ],
        ),
      ),
    );
  }

  /// Віджет Секції 3: Реактивний слухач потоку через StreamBuilder.
  Widget _buildStreamTelemetrySection() {
    return StreamBuilder<TelemetryData>(
      stream: _telemetryService.telemetryStream,
      builder: (context, snapshot) {
        // Обробка стану наявності помилки у потоці (Stream.error)
        if (snapshot.hasError) {
          return Card(
            color: Colors.red.shade900.withOpacity(0.3),
            shape: RoundedRectangleBorder(
              borderRadius: BorderRadius.circular(12),
              side: const BorderSide(color: Colors.redAccent, width: 1.5),
            ),
            child: Padding(
              padding: const EdgeInsets.all(16.0),
              child: Column(
                children: [
                  const Icon(Icons.warning_amber_rounded,
                      color: Colors.redAccent, size: 48),
                  const SizedBox(height: 8),
                  const Text(
                    'КРИТИЧНА ПОМИЛКА В ПОТОЦІ ТЕЛЕМЕТРІЇ',
                    style: TextStyle(
                      color: Colors.redAccent,
                      fontWeight: FontWeight.bold,
                      fontSize: 16,
                    ),
                  ),
                  const SizedBox(height: 8),
                  Text(
                    '${snapshot.error}',
                    textAlign: TextAlign.center,
                    style: const TextStyle(color: Colors.white),
                  ),
                  const SizedBox(height: 12),
                  ElevatedButton.icon(
                    onPressed: () => _telemetryService.recoverFromFault(),
                    icon: const Icon(Icons.refresh),
                    label: const Text('Очистити аварію та продовжити прийом'),
                  ),
                ],
              ),
            ),
          );
        }

        // Обробка початкового стану підключення (очікування першого пакету)
        if (snapshot.connectionState == ConnectionState.waiting) {
          return const Card(
            child: Padding(
              padding: EdgeInsets.all(24.0),
              child: Center(
                child: Column(
                  children: [
                    CircularProgressIndicator(),
                    SizedBox(height: 12),
                    Text(
                      'Очікування первинного пакету від StreamController...',
                      style: TextStyle(color: Colors.grey),
                    ),
                  ],
                ),
              ),
            ),
          );
        }

        // Якщо дані успішно надійшли, зберігаємо їх у локальну історію
        if (snapshot.hasData) {
          final data = snapshot.data!;
          // Уникаємо дублювання однакових міток
          if (_telemetryHistory.isEmpty ||
              _telemetryHistory.last.timestamp != data.timestamp) {
            WidgetsBinding.instance.addPostFrameCallback((_) {
              if (mounted) {
                setState(() {
                  _telemetryHistory.add(data);
                  if (_telemetryHistory.length > _maxHistoryLength) {
                    _telemetryHistory.removeAt(0);
                  }
                });
              }
            });
          }

          return Column(
            children: [
              Row(
                children: [
                  Expanded(
                    child: _buildMetricTile(
                      title: 'CPU Навантаження',
                      value: '${data.cpuLoad.toStringAsFixed(1)}%',
                      progress: data.cpuLoad / 100.0,
                      icon: Icons.speed,
                      color: data.cpuLoad > 75.0 ? Colors.redAccent : Colors.tealAccent,
                    ),
                  ),
                  const SizedBox(width: 12),
                  Expanded(
                    child: _buildMetricTile(
                      title: 'Температура ЦП',
                      value: '${data.cpuTemperature.toStringAsFixed(1)} °C',
                      progress: (data.cpuTemperature - 30.0) / 70.0,
                      icon: Icons.thermostat,
                      color: data.cpuTemperature > 75.0
                          ? Colors.deepOrangeAccent
                          : Colors.orangeAccent,
                    ),
                  ),
                ],
              ),
              const SizedBox(height: 12),
              Row(
                children: [
                  Expanded(
                    child: _buildMetricTile(
                      title: 'Оперативна пам\'ять',
                      value: '${data.ramUsedGb.toStringAsFixed(2)} / 16.0 ГБ',
                      progress: data.ramUsedGb / 16.0,
                      icon: Icons.memory,
                      color: Colors.lightBlueAccent,
                    ),
                  ),
                  const SizedBox(width: 12),
                  Expanded(
                    child: _buildMetricTile(
                      title: 'Мережевий трафік',
                      value: '${data.networkThroughputMbps.toStringAsFixed(1)} Мбіт/с',
                      progress: data.networkThroughputMbps / 150.0,
                      icon: Icons.network_check,
                      color: Colors.purpleAccent,
                    ),
                  ),
                ],
              ),
            ],
          );
        }

        return const SizedBox.shrink();
      },
    );
  }

  /// Візуальна плашка для окремої метрики з прогрес-баром
  Widget _buildMetricTile({
    required String title,
    required String value,
    required double progress,
    required IconData icon,
    required Color color,
  }) {
    final clampedProgress = progress.clamp(0.0, 1.0);
    return Card(
      child: Padding(
        padding: const EdgeInsets.all(14.0),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              children: [
                Icon(icon, color: color, size: 20),
                const SizedBox(width: 8),
                Expanded(
                  child: Text(
                    title,
                    style: const TextStyle(fontSize: 13, color: Colors.white70),
                    overflow: TextOverflow.ellipsis,
                  ),
                ),
              ],
            ),
            const SizedBox(height: 10),
            Text(
              value,
              style: TextStyle(
                fontSize: 18,
                fontWeight: FontWeight.bold,
                color: color,
              ),
            ),
            const SizedBox(height: 10),
            ClipRRect(
              borderRadius: BorderRadius.circular(4),
              child: LinearProgressIndicator(
                value: clampedProgress,
                backgroundColor: Colors.white10,
                valueColor: AlwaysStoppedAnimation<Color>(color),
                minHeight: 6,
              ),
            ),
          ],
        ),
      ),
    );
  }

  /// Віджет Секції 4: Журнал останніх подій телеметрії
  Widget _buildHistoryLogSection() {
    return Card(
      child: Padding(
        padding: const EdgeInsets.all(16.0),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: [
                const Row(
                  children: [
                    Icon(Icons.history, color: Colors.tealAccent),
                    SizedBox(width: 8),
                    Text(
                      'Журнал останніх подій потоку (Buffer)',
                      style: TextStyle(fontWeight: FontWeight.bold),
                    ),
                  ],
                ),
                Text(
                  '${_telemetryHistory.length} записів',
                  style: const TextStyle(color: Colors.grey, fontSize: 12),
                ),
              ],
            ),
            const Divider(height: 20),
            if (_telemetryHistory.isEmpty)
              const Padding(
                padding: EdgeInsets.symmetric(vertical: 12.0),
                child: Center(
                  child: Text(
                    'Поки немає зареєстрованих подій',
                    style: TextStyle(color: Colors.grey),
                  ),
                ),
              )
            else
              ListView.separated(
                shrinkWrap: true,
                physics: const NeverScrollableScrollPhysics(),
                itemCount: _telemetryHistory.reversed.length,
                separatorBuilder: (_, __) => const Divider(height: 1),
                itemBuilder: (context, index) {
                  final item = _telemetryHistory.reversed.toList()[index];
                  final timeStr =
                      "${item.timestamp.hour.toString().padLeft(2, '0')}:${item.timestamp.minute.toString().padLeft(2, '0')}:${item.timestamp.second.toString().padLeft(2, '0')}";
                  return Padding(
                    padding: const EdgeInsets.symmetric(vertical: 6.0),
                    child: Row(
                      mainAxisAlignment: MainAxisAlignment.spaceBetween,
                      children: [
                        Text(
                          timeStr,
                          style: const TextStyle(
                            fontFamily: 'monospace',
                            color: Colors.tealAccent,
                            fontSize: 12,
                          ),
                        ),
                        Text(
                          'CPU: ${item.cpuLoad.toStringAsFixed(1)}%',
                          style: const TextStyle(fontSize: 12),
                        ),
                        Text(
                          'T: ${item.cpuTemperature.toStringAsFixed(1)}°C',
                          style: TextStyle(
                            fontSize: 12,
                            color: item.cpuTemperature > 75.0
                                ? Colors.redAccent
                                : Colors.white70,
                          ),
                        ),
                        Text(
                          'Net: ${item.networkThroughputMbps.toStringAsFixed(0)}M',
                          style: const TextStyle(fontSize: 12, color: Colors.grey),
                        ),
                      ],
                    ),
                  );
                },
              ),
          ],
        ),
      ),
    );
  }
}
