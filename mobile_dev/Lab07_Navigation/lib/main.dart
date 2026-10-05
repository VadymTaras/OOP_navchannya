// ============================================================================
// ПРАКТИЧНА РОБОТА №7
// Дисципліна: Програмування для мобільних платформ
// Тема: Роутинг і навігація у Flutter (Навігація між екранами, передача
//       аргументів, отримання результатів та захист переходів)
//
// Виконавець: студент групи аІк43 ТАРАС Вадим
// Керівник: викладач КЛИМЕНКО О.А.
// ============================================================================

import 'package:flutter/material.dart';

void main() {
  runApp(const SmartHomeLabApp());
}

// ============================================================================
// 1. КОНСТАНТИ ІМЕНОВАНИХ МАРШРУТІВ (NAMED ROUTES CONSTANTS)
// ============================================================================
/// Централізований клас констант іменованих маршрутів.
/// Запобігає помилкам ручного введення рядків ("magic strings").
class AppRoutes {
  static const String home = '/';
  static const String deviceDetails = '/device-details';
  static const String deviceEdit = '/device-edit';
  static const String about = '/about';
  static const String telemetryHistory = '/telemetry-history';
}

// ============================================================================
// 2. МОДЕЛІ ДАНИХ (DATA MODELS)
// ============================================================================

/// Категорія обладнання у розумній лабораторії / будинку
enum DeviceCategory {
  sensor,     // Датчики телеметрії
  controller, // Контролери автоматизації
  climate,    // Кліматичне обладнання
  lighting,   // Розумне освітлення
  security,   // Системи моніторингу та безпеки
}

/// Розширення для зручного отримання локалізованих назв та піктограм категорій
extension DeviceCategoryExtension on DeviceCategory {
  String get title {
    switch (this) {
      case DeviceCategory.sensor:
        return 'Датчики телеметрії';
      case DeviceCategory.controller:
        return 'Мікроконтролери & PLC';
      case DeviceCategory.climate:
        return 'Клімат-контроль';
      case DeviceCategory.lighting:
        return 'Освітлення';
      case DeviceCategory.security:
        return 'Системи безпеки';
    }
  }

  IconData get icon {
    switch (this) {
      case DeviceCategory.sensor:
        return Icons.sensors;
      case DeviceCategory.controller:
        return Icons.developer_board;
      case DeviceCategory.climate:
        return Icons.thermostat;
      case DeviceCategory.lighting:
        return Icons.lightbulb_outline;
      case DeviceCategory.security:
        return Icons.shield_outlined;
    }
  }

  Color get color {
    switch (this) {
      case DeviceCategory.sensor:
        return Colors.teal;
      case DeviceCategory.controller:
        return Colors.deepPurple;
      case DeviceCategory.climate:
        return Colors.orange;
      case DeviceCategory.lighting:
        return Colors.amber.shade800;
      case DeviceCategory.security:
        return Colors.indigo;
    }
  }
}

/// Сутність приладу/вузла лабораторної мережі
class DeviceItem {
  final String id;
  final String name;
  final String location;
  final DeviceCategory category;
  final bool isOnline;
  final double currentMetric; // Наприклад: температура, споживання, напруга
  final String metricUnit;
  final String ipAddress;
  final String firmwareVersion;
  final String description;

  const DeviceItem({
    required this.id,
    required this.name,
    required this.location,
    required this.category,
    required this.isOnline,
    required this.currentMetric,
    required this.metricUnit,
    required this.ipAddress,
    required this.firmwareVersion,
    required this.description,
  });

  /// Створення оновленої копії незмінного об'єкта (Immutability Pattern)
  DeviceItem copyWith({
    String? id,
    String? name,
    String? location,
    DeviceCategory? category,
    bool? isOnline,
    double? currentMetric,
    String? metricUnit,
    String? ipAddress,
    String? firmwareVersion,
    String? description,
  }) {
    return DeviceItem(
      id: id ?? this.id,
      name: name ?? this.name,
      location: location ?? this.location,
      category: category ?? this.category,
      isOnline: isOnline ?? this.isOnline,
      currentMetric: currentMetric ?? this.currentMetric,
      metricUnit: metricUnit ?? this.metricUnit,
      ipAddress: ipAddress ?? this.ipAddress,
      firmwareVersion: firmwareVersion ?? this.firmwareVersion,
      description: description ?? this.description,
    );
  }
}

// ============================================================================
// 3. ГОЛОВНИЙ ВІДЖЕТ ДОДАТКУ ТА ГЕНЕРАТОР МАРШРУТІВ
// ============================================================================

class SmartHomeLabApp extends StatelessWidget {
  const SmartHomeLabApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Smart Home & Lab Navigator',
      debugShowCheckedModeBanner: false,
      theme: ThemeData(
        useMaterial3: true,
        colorScheme: ColorScheme.fromSeed(
          seedColor: const Color(0xFF0D47A1),
          brightness: Brightness.light,
        ),
        appBarTheme: const AppBarTheme(
          centerTitle: true,
          elevation: 2,
        ),
        cardTheme: CardTheme(
          elevation: 2,
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(14),
          ),
        ),
      ),
      darkTheme: ThemeData(
        useMaterial3: true,
        colorScheme: ColorScheme.fromSeed(
          seedColor: const Color(0xFF1976D2),
          brightness: Brightness.dark,
        ),
        appBarTheme: const AppBarTheme(
          centerTitle: true,
          elevation: 2,
        ),
      ),
      themeMode: ThemeMode.system,

      // Початковий маршрут
      initialRoute: AppRoutes.home,

      // 1. Статична таблиця маршрутів для екранів без динамічних обов'язкових параметрів
      routes: {
        AppRoutes.home: (context) => const MainNavigationShell(),
        AppRoutes.about: (context) => const AboutScreen(),
        AppRoutes.telemetryHistory: (context) => const TelemetryHistoryScreen(),
      },

      // 2. Динамічний генератор маршрутів (onGenerateRoute)
      // Забезпечує сувору перевірку типів аргументів та безпечний перехід
      onGenerateRoute: (RouteSettings settings) {
        switch (settings.name) {
          case AppRoutes.deviceDetails:
            // Перевірка наявності та типу переданого аргументу
            if (settings.arguments is DeviceItem) {
              final device = settings.arguments as DeviceItem;
              return MaterialPageRoute<DeviceItem>(
                builder: (context) => DeviceDetailScreen(initialDevice: device),
                settings: settings,
              );
            }
            return _errorRoute('Помилка навігації: аргумент DeviceItem не знайдено');

          case AppRoutes.deviceEdit:
            // Перевірка аргументу для екрана редагування
            if (settings.arguments is DeviceItem) {
              final device = settings.arguments as DeviceItem;
              return MaterialPageRoute<DeviceItem>(
                builder: (context) => DeviceEditScreen(device: device),
                settings: settings,
                fullscreenDialog: true, // Відкриття як модального діалогу
              );
            }
            return _errorRoute('Помилка навігації: відсутній прилад для редагування');

          default:
            return null; // Перехід до onUnknownRoute
        }
      },

      // 3. Обробник невідомих маршрутів (404 Not Found)
      onUnknownRoute: (RouteSettings settings) {
        return MaterialPageRoute(
          builder: (context) => UnknownRouteScreen(routeName: settings.name ?? 'Unknown'),
        );
      },
    );
  }

  /// Допоміжний метод генерації маршруту з повідомленням про помилку
  static Route<dynamic> _errorRoute(String message) {
    return MaterialPageRoute(
      builder: (context) => Scaffold(
        appBar: AppBar(title: const Text('Помилка маршрутизації')),
        body: Center(
          child: Padding(
            padding: const EdgeInsets.all(24.0),
            child: Column(
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                const Icon(Icons.error_outline, size: 64, color: Colors.redAccent),
                const SizedBox(height: 16),
                Text(
                  message,
                  textAlign: TextAlign.center,
                  style: const TextStyle(fontSize: 16),
                ),
                const SizedBox(height: 24),
                ElevatedButton.icon(
                  onPressed: () => Navigator.pop(context),
                  icon: const Icon(Icons.arrow_back),
                  label: const Text('Повернутися назад'),
                ),
              ],
            ),
          ),
        ),
      ),
    );
  }
}

// ============================================================================
// 4. ГОЛОВНИЙ НАВІГАЦІЙНИЙ КАРКАС (BOTTOM NAVIGATION & DRAWER)
// ============================================================================

class MainNavigationShell extends StatefulWidget {
  const MainNavigationShell({super.key});

  @override
  State<MainNavigationShell> createState() => _MainNavigationShellState();
}

class _MainNavigationShellState extends State<MainNavigationShell> {
  int _currentIndex = 0;

  // Централізований список приладів у лабораторії
  late List<DeviceItem> _devices;

  @override
  void initState() {
    super.initState();
    _devices = _getInitialDeviceList();
  }

  /// Ініціалізація демонстраційного переліку IoT-обладнання
  List<DeviceItem> _getInitialDeviceList() {
    return [
      const DeviceItem(
        id: 'DEV-001',
        name: 'ESP32 Метеостанція Вузлова',
        location: 'Лабораторія 402, Сектор А',
        category: DeviceCategory.sensor,
        isOnline: true,
        currentMetric: 22.4,
        metricUnit: '°C',
        ipAddress: '192.168.1.120',
        firmwareVersion: 'v2.4.1-build88',
        description: 'Датчик температури, відносної вологості (BME280) та освітленості.',
      ),
      const DeviceItem(
        id: 'DEV-002',
        name: 'STM32 PLC Контролер вентиляції',
        location: 'Серверна кімната',
        category: DeviceCategory.climate,
        isOnline: true,
        currentMetric: 65.0,
        metricUnit: '% потужність',
        ipAddress: '192.168.1.125',
        firmwareVersion: 'v1.1.0-rtos',
        description: 'Керування припливно-витяжною вентиляцією та фільтрацією повітря.',
      ),
      const DeviceItem(
        id: 'DEV-003',
        name: 'Raspberry Pi 4 AI Gateway',
        location: 'Головна стійка керування',
        category: DeviceCategory.controller,
        isOnline: true,
        currentMetric: 48.2,
        metricUnit: '°C CPU',
        ipAddress: '192.168.1.100',
        firmwareVersion: 'Ubuntu 24.04 LTS',
        description: 'Шлюз збору телеметрії MQTT, обробка локальних нейромережевих моделей.',
      ),
      const DeviceItem(
        id: 'DEV-004',
        name: 'Розумне LED освітлення робочих місць',
        location: 'Лабораторія 402, Сектор Б',
        category: DeviceCategory.lighting,
        isOnline: false,
        currentMetric: 0.0,
        metricUnit: 'Lm',
        ipAddress: '192.168.1.133',
        firmwareVersion: 'v3.0.4-zigbee',
        description: 'Адресне регулювання яскравості та колірної температури CRI > 95.',
      ),
      const DeviceItem(
        id: 'DEV-005',
        name: 'Модуль контролю доступу (RFID + Cam)',
        location: 'Вхідні двері лаб. 402',
        category: DeviceCategory.security,
        isOnline: true,
        currentMetric: 142.0,
        metricUnit: 'подій/добу',
        ipAddress: '192.168.1.150',
        firmwareVersion: 'v2.8.0-secure',
        description: 'Контролер електронного замка, зчитувач карток Mifare та камера верифікації.',
      ),
    ];
  }

  /// Обробник оновлення приладу після повернення з екрана редагування/деталей
  void _updateDevice(DeviceItem updatedDevice) {
    setState(() {
      final index = _devices.indexWhere((d) => d.id == updatedDevice.id);
      if (index != -1) {
        _devices[index] = updatedDevice;
      }
    });
  }

  /// Перехід до екрана деталей з асинхронним очікуванням результату
  Future<void> _navigateToDetails(DeviceItem device) async {
    // Navigator.pushNamed повертає Future<T?>, де T - тип повернутого результату
    final result = await Navigator.pushNamed<dynamic>(
      context,
      AppRoutes.deviceDetails,
      arguments: device,
    );

    // Якщо екран деталей повернув оновлений прилад
    if (result is DeviceItem && mounted) {
      _updateDevice(result);
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text('Конфігурацію приладу "${result.name}" синхронізовано!'),
          backgroundColor: Colors.teal.shade700,
          behavior: SnackBarBehavior.floating,
          duration: const Duration(seconds: 3),
        ),
      );
    }
  }

  @override
  Widget build(BuildContext context) {
    // Перелік сторінок для вкладок BottomNavigationBar
    final pages = [
      DevicesListPage(
        devices: _devices,
        onDeviceTap: _navigateToDetails,
      ),
      MonitoringDashboardPage(devices: _devices),
      const SettingsProfilePage(),
    ];

    return Scaffold(
      appBar: AppBar(
        title: Text(_getAppBarTitle(_currentIndex)),
        actions: [
          IconButton(
            tooltip: 'Історія телеметрії',
            icon: const Icon(Icons.history),
            onPressed: () {
              Navigator.pushNamed(context, AppRoutes.telemetryHistory);
            },
          ),
          IconButton(
            tooltip: 'Про програму',
            icon: const Icon(Icons.info_outline),
            onPressed: () {
              Navigator.pushNamed(context, AppRoutes.about);
            },
          ),
        ],
      ),

      // Бічне навігаційне меню (Navigation Drawer)
      drawer: AppDrawer(
        activeRoute: AppRoutes.home,
        onNavigate: (route) {
          Navigator.pop(context); // Закриваємо Drawer перед переходом
          if (route != AppRoutes.home) {
            Navigator.pushNamed(context, route);
          }
        },
      ),

      // Тіло екрана з підтримкою збереження стану сторінок
      body: IndexedStack(
        index: _currentIndex,
        children: pages,
      ),

      // Нижня панель навігації
      bottomNavigationBar: NavigationBar(
        selectedIndex: _currentIndex,
        onDestinationSelected: (index) {
          setState(() {
            _currentIndex = index;
          });
        },
        destinations: const [
          NavigationDestination(
            icon: Icon(Icons.devices_outlined),
            selectedIcon: Icon(Icons.devices),
            label: 'Обладнання',
          ),
          NavigationDestination(
            icon: Icon(Icons.analytics_outlined),
            selectedIcon: Icon(Icons.analytics),
            label: 'Моніторинг',
          ),
          NavigationDestination(
            icon: Icon(Icons.settings_outlined),
            selectedIcon: Icon(Icons.settings),
            label: 'Налаштування',
          ),
        ],
      ),
    );
  }

  String _getAppBarTitle(int index) {
    switch (index) {
      case 0:
        return 'Лабораторні прилади';
      case 1:
        return 'Телеметрія та моніторинг';
      case 2:
        return 'Профіль інженера';
      default:
        return 'Smart Lab Navigator';
    }
  }
}

// ============================================================================
// 5. БІЧНЕ МЕНЮ НАВІГАЦІЇ (NAVIGATION DRAWER)
// ============================================================================

class AppDrawer extends StatelessWidget {
  final String activeRoute;
  final ValueChanged<String> onNavigate;

  const AppDrawer({
    super.key,
    required this.activeRoute,
    required this.onNavigate,
  });

  @override
  Widget build(BuildContext context) {
    return Drawer(
      child: Column(
        children: [
          UserAccountsDrawerHeader(
            decoration: const BoxDecoration(
              gradient: LinearGradient(
                colors: [Color(0xFF0D47A1), Color(0xFF1976D2)],
                begin: Alignment.topLeft,
                end: Alignment.bottomRight,
              ),
            ),
            accountName: const Text(
              'ТАРАС Вадим',
              style: TextStyle(fontWeight: FontWeight.bold, fontSize: 16),
            ),
            accountEmail: const Text('Студент гр. аІк43 | QA & IoT Інженерія'),
            currentAccountPicture: const CircleAvatar(
              backgroundColor: Colors.white,
              child: Icon(
                Icons.memory,
                size: 44,
                color: Color(0xFF0D47A1),
              ),
            ),
            otherAccountsPictures: [
              IconButton(
                icon: const Icon(Icons.terminal, color: Colors.white),
                onPressed: () {},
              )
            ],
          ),
          ListTile(
            leading: const Icon(Icons.home),
            title: const Text('Головна панель приладів'),
            selected: activeRoute == AppRoutes.home,
            onTap: () => onNavigate(AppRoutes.home),
          ),
          ListTile(
            leading: const Icon(Icons.history),
            title: const Text('Журнал телеметрії'),
            selected: activeRoute == AppRoutes.telemetryHistory,
            onTap: () => onNavigate(AppRoutes.telemetryHistory),
          ),
          const Divider(),
          ListTile(
            leading: const Icon(Icons.info),
            title: const Text('Про систему & ДСТУ звіт'),
            selected: activeRoute == AppRoutes.about,
            onTap: () => onNavigate(AppRoutes.about),
          ),
          const Spacer(),
          Padding(
            padding: const EdgeInsets.all(16.0),
            child: Text(
              'Flutter Navigation 1.0 & 2.0\nПрактична робота №7',
              textAlign: TextAlign.center,
              style: TextStyle(color: Colors.grey.shade600, fontSize: 12),
            ),
          ),
        ],
      ),
    );
  }
}

// ============================================================================
// 6. ВКЛАДКА 1: СПИСОК ПРИЛАДІВ (DEVICES LIST PAGE)
// ============================================================================

class DevicesListPage extends StatelessWidget {
  final List<DeviceItem> devices;
  final ValueChanged<DeviceItem> onDeviceTap;

  const DevicesListPage({
    super.key,
    required this.devices,
    required this.onDeviceTap,
  });

  @override
  Widget build(BuildContext context) {
    return ListView.builder(
      padding: const EdgeInsets.all(12),
      itemCount: devices.length,
      itemBuilder: (context, index) {
        final device = devices[index];
        return Card(
          margin: const EdgeInsets.only(bottom: 12),
          child: InkWell(
            borderRadius: BorderRadius.circular(14),
            onTap: () => onDeviceTap(device),
            child: Padding(
              padding: const EdgeInsets.all(14.0),
              child: Row(
                children: [
                  Container(
                    width: 52,
                    height: 52,
                    decoration: BoxDecoration(
                      color: device.category.color.withOpacity(0.15),
                      borderRadius: BorderRadius.circular(12),
                    ),
                    child: Icon(
                      device.category.icon,
                      color: device.category.color,
                      size: 28,
                    ),
                  ),
                  const SizedBox(width: 14),
                  Expanded(
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Row(
                          children: [
                            Expanded(
                              child: Text(
                                device.name,
                                style: const TextStyle(
                                  fontWeight: FontWeight.bold,
                                  fontSize: 15,
                                ),
                                maxLines: 1,
                                overflow: TextOverflow.ellipsis,
                              ),
                            ),
                            Container(
                              padding: const EdgeInsets.symmetric(horizontal: 8, vertical: 3),
                              decoration: BoxDecoration(
                                color: device.isOnline ? Colors.green.shade100 : Colors.red.shade100,
                                borderRadius: BorderRadius.circular(8),
                              ),
                              child: Row(
                                mainAxisSize: MainAxisSize.min,
                                children: [
                                  CircleAvatar(
                                    radius: 4,
                                    backgroundColor: device.isOnline ? Colors.green : Colors.red,
                                  ),
                                  const SizedBox(width: 4),
                                  Text(
                                    device.isOnline ? 'Online' : 'Offline',
                                    style: TextStyle(
                                      fontSize: 11,
                                      fontWeight: FontWeight.w600,
                                      color: device.isOnline ? Colors.green.shade900 : Colors.red.shade900,
                                    ),
                                  ),
                                ],
                              ),
                            ),
                          ],
                        ),
                        const SizedBox(height: 4),
                        Text(
                          device.location,
                          style: TextStyle(color: Colors.grey.shade600, fontSize: 13),
                        ),
                        const SizedBox(height: 6),
                        Row(
                          children: [
                            Icon(Icons.speed, size: 16, color: Colors.blueGrey.shade600),
                            const SizedBox(width: 4),
                            Text(
                              'Показник: ${device.currentMetric} ${device.metricUnit}',
                              style: TextStyle(
                                fontSize: 12,
                                fontWeight: FontWeight.w500,
                                color: Colors.blueGrey.shade800,
                              ),
                            ),
                            const Spacer(),
                            const Icon(Icons.arrow_forward_ios, size: 14, color: Colors.grey),
                          ],
                        ),
                      ],
                    ),
                  ),
                ],
              ),
            ),
          ),
        );
      },
    );
  }
}

// ============================================================================
// 7. ЕКРАН ДЕТАЛЕЙ ПРИЛАДУ (DEVICE DETAIL SCREEN - ARGUMENTS & AWAIT RESULT)
// ============================================================================

class DeviceDetailScreen extends StatefulWidget {
  final DeviceItem initialDevice;

  const DeviceDetailScreen({
    super.key,
    required this.initialDevice,
  });

  @override
  State<DeviceDetailScreen> createState() => _DeviceDetailScreenState();
}

class _DeviceDetailScreenState extends State<DeviceDetailScreen> {
  late DeviceItem _device;
  bool _hasModified = false;

  @override
  void initState() {
    super.initState();
    _device = widget.initialDevice;
  }

  /// Перехід до редагування з отриманням оновленого приладу через Navigator.pop
  Future<void> _openEditScreen() async {
    // Виклик екрана редагування через іменований маршрут
    final updatedDevice = await Navigator.pushNamed<dynamic>(
      context,
      AppRoutes.deviceEdit,
      arguments: _device,
    );

    // Перевірка повернутого результату
    if (updatedDevice is DeviceItem && mounted) {
      setState(() {
        _device = updatedDevice;
        _hasModified = true;
      });

      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(
          content: Text('Параметри приладу успішно збережено!'),
          backgroundColor: Colors.indigo,
          behavior: SnackBarBehavior.floating,
        ),
      );
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(_device.name),
        leading: IconButton(
          icon: const Icon(Icons.arrow_back),
          onPressed: () {
            // Повернення зміненого приладу назад у головний стек
            Navigator.pop(context, _hasModified ? _device : null);
          },
        ),
        actions: [
          IconButton(
            tooltip: 'Редагувати параметри',
            icon: const Icon(Icons.edit),
            onPressed: _openEditScreen,
          ),
        ],
      ),
      body: SingleChildScrollView(
        padding: const EdgeInsets.all(16.0),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: [
            // Картка статусу та головної інформації
            Card(
              child: Padding(
                padding: const EdgeInsets.all(18.0),
                child: Column(
                  children: [
                    CircleAvatar(
                      radius: 36,
                      backgroundColor: _device.category.color.withOpacity(0.15),
                      child: Icon(
                        _device.category.icon,
                        color: _device.category.color,
                        size: 40,
                      ),
                    ),
                    const SizedBox(height: 12),
                    Text(
                      _device.name,
                      style: const TextStyle(fontSize: 19, fontWeight: FontWeight.bold),
                      textAlign: TextAlign.center,
                    ),
                    const SizedBox(height: 4),
                    Text(
                      _device.category.title,
                      style: TextStyle(color: _device.category.color, fontWeight: FontWeight.w600),
                    ),
                    const SizedBox(height: 12),
                    Chip(
                      avatar: CircleAvatar(
                        backgroundColor: _device.isOnline ? Colors.green : Colors.red,
                        radius: 5,
                      ),
                      label: Text(
                        _device.isOnline ? 'Підключено до мережі (Online)' : 'Зв\'язок відсутній (Offline)',
                      ),
                    ),
                  ],
                ),
              ),
            ),
            const SizedBox(height: 16),

            // Детальні технічні характеристики
            Card(
              child: Padding(
                padding: const EdgeInsets.all(16.0),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    const Text(
                      'Технічна специфікація та адресація',
                      style: TextStyle(fontSize: 16, fontWeight: FontWeight.bold),
                    ),
                    const Divider(height: 24),
                    _buildInfoRow('Ідентифікатор вузла:', _device.id),
                    _buildInfoRow('Локація розміщення:', _device.location),
                    _buildInfoRow('IP-адреса у підмережі:', _device.ipAddress),
                    _buildInfoRow('Версія прошивки (FW):', _device.firmwareVersion),
                    _buildInfoRow(
                      'Поточна телеметрія:',
                      '${_device.currentMetric} ${_device.metricUnit}',
                    ),
                  ],
                ),
              ),
            ),
            const SizedBox(height: 16),

            // Опис та інженерні примітки
            Card(
              child: Padding(
                padding: const EdgeInsets.all(16.0),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    const Text(
                      'Опис функціоналу вузла',
                      style: TextStyle(fontSize: 16, fontWeight: FontWeight.bold),
                    ),
                    const SizedBox(height: 8),
                    Text(
                      _device.description,
                      style: const TextStyle(fontSize: 14, height: 1.4),
                    ),
                  ],
                ),
              ),
            ),
            const SizedBox(height: 24),

            // Кнопка швидкого редагування
            FilledButton.icon(
              onPressed: _openEditScreen,
              icon: const Icon(Icons.edit_note),
              label: const Text('Змінити параметри приладу'),
              style: FilledButton.styleFrom(
                padding: const EdgeInsets.symmetric(vertical: 14),
              ),
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildInfoRow(String label, String value) {
    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 6.0),
      child: Row(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          SizedBox(
            width: 170,
            child: Text(
              label,
              style: TextStyle(color: Colors.grey.shade700, fontWeight: FontWeight.w500),
            ),
          ),
          Expanded(
            child: Text(
              value,
              style: const TextStyle(fontWeight: FontWeight.w600),
            ),
          ),
        ],
      ),
    );
  }
}

// ============================================================================
// 8. ЕКРАН РЕДАГУВАННЯ З PopScope (ЗАХИСТ НЕЗБЕРЕЖЕНИХ ЗМІН ТА Navigator.pop)
// ============================================================================

class DeviceEditScreen extends StatefulWidget {
  final DeviceItem device;

  const DeviceEditScreen({
    super.key,
    required this.device,
  });

  @override
  State<DeviceEditScreen> createState() => _DeviceEditScreenState();
}

class _DeviceEditScreenState extends State<DeviceEditScreen> {
  final _formKey = GlobalKey<FormState>();

  late TextEditingController _nameController;
  late TextEditingController _locationController;
  late TextEditingController _ipController;
  late TextEditingController _metricController;
  late TextEditingController _descriptionController;

  late DeviceCategory _selectedCategory;
  late bool _isOnline;
  bool _hasUnsavedChanges = false;

  @override
  void initState() {
    super.initState();
    _nameController = TextEditingController(text: widget.device.name);
    _locationController = TextEditingController(text: widget.device.location);
    _ipController = TextEditingController(text: widget.device.ipAddress);
    _metricController = TextEditingController(text: widget.device.currentMetric.toString());
    _descriptionController = TextEditingController(text: widget.device.description);

    _selectedCategory = widget.device.category;
    _isOnline = widget.device.isOnline;

    // Відстежуємо будь-яку зміну в полях вводу
    for (final c in [_nameController, _locationController, _ipController, _metricController, _descriptionController]) {
      c.addListener(_markChanged);
    }
  }

  void _markChanged() {
    if (!_hasUnsavedChanges) {
      setState(() {
        _hasUnsavedChanges = true;
      });
    }
  }

  @override
  void dispose() {
    _nameController.dispose();
    _locationController.dispose();
    _ipController.dispose();
    _metricController.dispose();
    _descriptionController.dispose();
    super.dispose();
  }

  /// Відображення діалогового вікна при спробі вийти з незбереженими даними
  Future<bool> _showDiscardDialog() async {
    final shouldDiscard = await showDialog<bool>(
      context: context,
      builder: (context) => AlertDialog(
        title: const Row(
          children: [
            Icon(Icons.warning_amber_rounded, color: Colors.orange),
            SizedBox(width: 8),
            Text('Незбережені зміни'),
          ],
        ),
        content: const Text(
          'Ви внесли зміни до параметрів приладу. Якщо вийти зараз, усі незбережені модифікації буде втрачено. Бажаєте скасувати зміни?',
        ),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(context, false), // Залишитися на формі
            child: const Text('Залишитися'),
          ),
          FilledButton(
            onPressed: () => Navigator.pop(context, true), // Дозволити вихід
            style: FilledButton.styleFrom(backgroundColor: Colors.red),
            child: const Text('Скасувати зміни'),
          ),
        ],
      ),
    );
    return shouldDiscard ?? false;
  }

  /// Збереження форми та повернення нового об'єкта через Navigator.pop
  void _saveForm() {
    if (_formKey.currentState!.validate()) {
      final updatedDevice = widget.device.copyWith(
        name: _nameController.text.trim(),
        location: _locationController.text.trim(),
        ipAddress: _ipController.text.trim(),
        currentMetric: double.tryParse(_metricController.text.trim()) ?? widget.device.currentMetric,
        description: _descriptionController.text.trim(),
        category: _selectedCategory,
        isOnline: _isOnline,
      );

      // Повернення типізованого результату назад до екрана деталей
      Navigator.pop(context, updatedDevice);
    }
  }

  @override
  Widget build(BuildContext context) {
    // PopScope (новий API Flutter 3.12+) запобігає випадковій втраті даних
    // як при натисканні системної кнопки "Назад", так і при жесті свайпу
    return PopScope(
      canPop: !_hasUnsavedChanges,
      onPopInvokedWithResult: (didPop, result) async {
        if (didPop) return;
        final shouldLeave = await _showDiscardDialog();
        if (shouldLeave && context.mounted) {
          Navigator.pop(context);
        }
      },
      child: Scaffold(
        appBar: AppBar(
          title: const Text('Редагування вузла'),
          actions: [
            IconButton(
              tooltip: 'Зберегти зміни',
              icon: const Icon(Icons.check),
              onPressed: _saveForm,
            ),
          ],
        ),
        body: Form(
          key: _formKey,
          child: ListView(
            padding: const EdgeInsets.all(16.0),
            children: [
              // Ідентифікатор вузла (тільки для читання)
              TextFormField(
                initialValue: widget.device.id,
                decoration: const InputDecoration(
                  labelText: 'Ідентифікатор вузла (ReadOnly)',
                  prefixIcon: Icon(Icons.qr_code),
                  border: OutlineInputBorder(),
                  filled: true,
                ),
                enabled: false,
              ),
              const SizedBox(height: 16),

              // Назва приладу
              TextFormField(
                controller: _nameController,
                decoration: const InputDecoration(
                  labelText: 'Назва приладу *',
                  prefixIcon: Icon(Icons.label_outline),
                  border: OutlineInputBorder(),
                ),
                validator: (val) {
                  if (val == null || val.trim().isEmpty) {
                    return 'Вкажіть назву обладнання';
                  }
                  if (val.trim().length < 3) {
                    return 'Назва має містити щонайменше 3 символи';
                  }
                  return null;
                },
              ),
              const SizedBox(height: 16),

              // Локація
              TextFormField(
                controller: _locationController,
                decoration: const InputDecoration(
                  labelText: 'Локація розміщення *',
                  prefixIcon: Icon(Icons.place_outlined),
                  border: OutlineInputBorder(),
                ),
                validator: (val) {
                  if (val == null || val.trim().isEmpty) {
                    return 'Вкажіть локацію приладу';
                  }
                  return null;
                },
              ),
              const SizedBox(height: 16),

              // Мережева IP-адреса
              TextFormField(
                controller: _ipController,
                decoration: const InputDecoration(
                  labelText: 'Мережева IP-адреса *',
                  prefixIcon: Icon(Icons.wifi),
                  border: OutlineInputBorder(),
                ),
                validator: (val) {
                  if (val == null || val.trim().isEmpty) {
                    return 'Вкажіть IPv4 адресу';
                  }
                  final ipRegex = RegExp(r'^((25[0-5]|(2[0-4]|1\d|[1-9]|)\d)\.?\b){4}$');
                  if (!ipRegex.hasMatch(val.trim())) {
                    return 'Введіть коректну IPv4 адресу (напр. 192.168.1.10)';
                  }
                  return null;
                },
              ),
              const SizedBox(height: 16),

              // Показник телеметрії
              TextFormField(
                controller: _metricController,
                keyboardType: const TextInputType.numberWithOptions(decimal: true),
                decoration: InputDecoration(
                  labelText: 'Поточне значення (${widget.device.metricUnit}) *',
                  prefixIcon: const Icon(Icons.speed),
                  border: const OutlineInputBorder(),
                ),
                validator: (val) {
                  if (val == null || val.trim().isEmpty) {
                    return 'Введіть числове значення телеметрії';
                  }
                  if (double.tryParse(val.trim()) == null) {
                    return 'Введіть дійсне число';
                  }
                  return null;
                },
              ),
              const SizedBox(height: 16),

              // Вибір категорії
              DropdownButtonFormField<DeviceCategory>(
                value: _selectedCategory,
                decoration: const InputDecoration(
                  labelText: 'Категорія обладнання',
                  prefixIcon: Icon(Icons.category_outlined),
                  border: OutlineInputBorder(),
                ),
                items: DeviceCategory.values.map((cat) {
                  return DropdownMenuItem(
                    value: cat,
                    child: Row(
                      children: [
                        Icon(cat.icon, size: 20, color: cat.color),
                        const SizedBox(width: 8),
                        Text(cat.title),
                      ],
                    ),
                  );
                }).toList(),
                onChanged: (val) {
                  if (val != null) {
                    setState(() {
                      _selectedCategory = val;
                      _hasUnsavedChanges = true;
                    });
                  }
                },
              ),
              const SizedBox(height: 16),

              // Статус мережі Online/Offline
              SwitchListTile(
                title: const Text('Мережева активність вузла'),
                subtitle: Text(_isOnline ? 'Вузол увімкнений та опитується' : 'Вузол ізольований або вимкнений'),
                value: _isOnline,
                activeColor: Colors.green,
                onChanged: (val) {
                  setState(() {
                    _isOnline = val;
                    _hasUnsavedChanges = true;
                  });
                },
              ),
              const SizedBox(height: 16),

              // Опис та інженерні коментарі
              TextFormField(
                controller: _descriptionController,
                maxLines: 3,
                decoration: const InputDecoration(
                  labelText: 'Інженерні примітки та опис',
                  prefixIcon: Icon(Icons.notes),
                  border: OutlineInputBorder(),
                ),
              ),
              const SizedBox(height: 24),

              // Кнопка збереження форми
              FilledButton.icon(
                onPressed: _saveForm,
                icon: const Icon(Icons.save),
                label: const Text('Зберегти та повернутися'),
                style: FilledButton.styleFrom(
                  padding: const EdgeInsets.symmetric(vertical: 14),
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}

// ============================================================================
// 9. ВКЛАДКА 2: ДАШБОРД МОНІТОРИНГУ (MONITORING DASHBOARD)
// ============================================================================

class MonitoringDashboardPage extends StatelessWidget {
  final List<DeviceItem> devices;

  const MonitoringDashboardPage({
    super.key,
    required this.devices,
  });

  @override
  Widget build(BuildContext context) {
    final onlineCount = devices.where((d) => d.isOnline).length;
    final totalCount = devices.length;

    return ListView(
      padding: const EdgeInsets.all(16),
      children: [
        // Загальна зведена картка
        Card(
          color: Colors.blue.shade900,
          child: Padding(
            padding: const EdgeInsets.all(20),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                const Text(
                  'Стан IoT інфраструктури',
                  style: TextStyle(color: Colors.white70, fontSize: 14),
                ),
                const SizedBox(height: 8),
                Text(
                  '$onlineCount / $totalCount вузлів онлайн',
                  style: const TextStyle(
                    color: Colors.white,
                    fontSize: 22,
                    fontWeight: FontWeight.bold,
                  ),
                ),
                const SizedBox(height: 12),
                LinearProgressIndicator(
                  value: totalCount > 0 ? onlineCount / totalCount : 0,
                  backgroundColor: Colors.white24,
                  valueColor: const AlwaysStoppedAnimation<Color>(Colors.greenAccent),
                ),
              ],
            ),
          ),
        ),
        const SizedBox(height: 16),

        const Text(
          'Оперативні показники телеметрії',
          style: TextStyle(fontSize: 16, fontWeight: FontWeight.bold),
        ),
        const SizedBox(height: 12),

        ...devices.map((device) {
          return Card(
            margin: const EdgeInsets.only(bottom: 8),
            child: ListTile(
              leading: Icon(
                device.category.icon,
                color: device.category.color,
              ),
              title: Text(device.name),
              subtitle: Text(device.location),
              trailing: Text(
                '${device.currentMetric} ${device.metricUnit}',
                style: const TextStyle(
                  fontWeight: FontWeight.bold,
                  fontSize: 14,
                ),
              ),
            ),
          );
        }),
      ],
    );
  }
}

// ============================================================================
// 10. ВКЛАДКА 3: ПРОФІЛЬ ТА НАЛАШТУВАННЯ (SETTINGS PROFILE PAGE)
// ============================================================================

class SettingsProfilePage extends StatelessWidget {
  const SettingsProfilePage({super.key});

  @override
  Widget build(BuildContext context) {
    return ListView(
      padding: const EdgeInsets.all(16),
      children: [
        const Card(
          child: Padding(
            padding: const EdgeInsets.all(16),
            child: Row(
              children: [
                CircleAvatar(
                  radius: 32,
                  backgroundColor: Color(0xFF0D47A1),
                  child: Icon(Icons.person, size: 36, color: Colors.white),
                ),
                SizedBox(width: 16),
                Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(
                      'ТАРАС Вадим',
                      style: TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
                    ),
                    Text('Група аІк43 | Спеціальність 121'),
                    Text('ФК ЗВО МНТУ, 2026 рік'),
                  ],
                ),
              ],
            ),
          ),
        ),
        const SizedBox(height: 16),
        ListTile(
          leading: const Icon(Icons.router),
          title: const Text('Шлюз телеметрії MQTT'),
          subtitle: const Text('192.168.1.100:1883 (Active)'),
          trailing: const Icon(Icons.check_circle, color: Colors.green),
          onTap: () {},
        ),
        ListTile(
          leading: const Icon(Icons.security),
          title: const Text('Сертифікат TLS/SSL'),
          subtitle: const Text('lab.taras.local (Дійсний до 2027)'),
          trailing: const Icon(Icons.chevron_right),
          onTap: () {},
        ),
        ListTile(
          leading: const Icon(Icons.info_outline),
          title: const Text('Довідка та маршрутизація'),
          subtitle: const Text('Перегляд інформації про проект'),
          trailing: const Icon(Icons.chevron_right),
          onTap: () {
            Navigator.pushNamed(context, AppRoutes.about);
          },
        ),
      ],
    );
  }
}

// ============================================================================
// 11. ЕКРАН ІСТОРІЇ ТЕЛЕМЕТРІЇ (TELEMETRY HISTORY SCREEN)
// ============================================================================

class TelemetryHistoryScreen extends StatelessWidget {
  const TelemetryHistoryScreen({super.key});

  @override
  Widget build(BuildContext context) {
    final logs = [
      {'time': '20:45:12', 'node': 'DEV-001', 'event': 'Оновлення температури: 22.4°C', 'type': 'OK'},
      {'time': '20:42:00', 'node': 'DEV-002', 'event': 'Швидкість вентилятора змінена на 65%', 'type': 'INFO'},
      {'time': '20:38:15', 'node': 'DEV-005', 'event': 'Вхід: Студент Тарас Вадим (UID: 0x4A12B)', 'type': 'SEC'},
      {'time': '20:30:22', 'node': 'DEV-004', 'event': 'Втрата пінгу з LED контролером (Zigbee timeout)', 'type': 'WARN'},
    ];

    return Scaffold(
      appBar: AppBar(
        title: const Text('Журнал телеметрії'),
      ),
      body: ListView.separated(
        padding: const EdgeInsets.all(16),
        itemCount: logs.length,
        separatorBuilder: (_, __) => const Divider(),
        itemBuilder: (context, index) {
          final log = logs[index];
          return ListTile(
            leading: CircleAvatar(
              backgroundColor: log['type'] == 'WARN' ? Colors.amber.shade100 : Colors.blue.shade100,
              child: Icon(
                log['type'] == 'WARN' ? Icons.warning_amber : Icons.check,
                color: log['type'] == 'WARN' ? Colors.amber.shade900 : Colors.blue.shade900,
              ),
            ),
            title: Text('${log['node']} — ${log['event']}'),
            subtitle: Text('Час фіксації: ${log['time']}'),
          );
        },
      ),
    );
  }
}

// ============================================================================
// 12. ЕКРАН «ПРО ПРОГРАМУ» (ABOUT SCREEN)
// ============================================================================

class AboutScreen extends StatelessWidget {
  const AboutScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Про систему'),
      ),
      body: Padding(
        padding: const EdgeInsets.all(20.0),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.center,
          children: [
            const CircleAvatar(
              radius: 40,
              backgroundColor: Color(0xFF0D47A1),
              child: Icon(Icons.alt_route, size: 48, color: Colors.white),
            ),
            const SizedBox(height: 16),
            const Text(
              'Smart Home & Lab Navigator',
              style: TextStyle(fontSize: 20, fontWeight: FontWeight.bold),
            ),
            const SizedBox(height: 6),
            const Text(
              'Практична робота №7 з курсу "Програмування для мобільних платформ"',
              textAlign: TextAlign.center,
              style: TextStyle(color: Colors.grey),
            ),
            const Divider(height: 32),
            const Align(
              alignment: Alignment.centerLeft,
              child: Text(
                'Ключові архітектурні рішення навігації:',
                style: TextStyle(fontWeight: FontWeight.bold, fontSize: 16),
              ),
            ),
            const SizedBox(height: 10),
            const ListTile(
              dense: true,
              leading: Icon(Icons.looks_one, color: Colors.blue),
              title: Text('Іменовані маршрути (Named Routes & AppRoutes)'),
              subtitle: Text('Централізована реєстрація шляхів та усунення "магічних" рядків'),
            ),
            const ListTile(
              dense: true,
              leading: Icon(Icons.looks_two, color: Colors.blue),
              title: Text('Динамічна генерація (onGenerateRoute)'),
              subtitle: Text('Строга типізація передачі аргументів та відлов помилок навігації'),
            ),
            const ListTile(
              dense: true,
              leading: Icon(Icons.looks_3, color: Colors.blue),
              title: Text('Асинхронний результат через Navigator.pop'),
              subtitle: Text('Повернення зміненого об\'єкта та миттєве оновлення батьківського стану'),
            ),
            const ListTile(
              dense: true,
              leading: Icon(Icons.looks_4, color: Colors.blue),
              title: Text('Контроль виходу (PopScope / WillPopScope)'),
              subtitle: Text('Запобігання втрати незбережених користувацьких даних при жестах Back'),
            ),
            const Spacer(),
            ElevatedButton.icon(
              onPressed: () => Navigator.pop(context),
              icon: const Icon(Icons.arrow_back),
              label: const Text('Повернутися до навігатора'),
            ),
          ],
        ),
      ),
    );
  }
}

// ============================================================================
// 13. ЕКРАН 404 (UNKNOWN ROUTE SCREEN)
// ============================================================================

class UnknownRouteScreen extends StatelessWidget {
  final String routeName;

  const UnknownRouteScreen({
    super.key,
    required this.routeName,
  });

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('404 — Сторінку не знайдено'),
      ),
      body: Center(
        child: Padding(
          padding: const EdgeInsets.all(24.0),
          child: Column(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              const Icon(Icons.wrong_location, size: 80, color: Colors.redAccent),
              const SizedBox(height: 16),
              Text(
                'Маршрут "$routeName" не зареєстровано в системі.',
                textAlign: TextAlign.center,
                style: const TextStyle(fontSize: 16),
              ),
              const SizedBox(height: 24),
              FilledButton.icon(
                onPressed: () => Navigator.pushNamedAndRemoveUntil(
                  context,
                  AppRoutes.home,
                  (route) => false,
                ),
                icon: const Icon(Icons.home),
                label: const Text('На головний екран'),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
