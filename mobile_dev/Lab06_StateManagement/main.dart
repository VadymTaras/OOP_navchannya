import 'package:flutter/material.dart';

// ============================================================================
// ПРАКТИЧНА РОБОТА №6: КЕРУВАННЯ СТАНОМ (STATE MANAGEMENT) У FLUTTER
// Дисципліна: Програмування для мобільних платформ
// Виконавець: студент групи аІк43 Тарас Вадим
// Керівник: викладач Клименко О.А.
// ============================================================================

void main() {
  // Точка входу в застосунок
  runApp(const SmartShopApp());
}

// ----------------------------------------------------------------------------
// 1. МОДЕЛІ ДАНИХ (DATA MODELS)
// ----------------------------------------------------------------------------

/// Модель товару в каталозі
class Product {
  final String id;
  final String name;
  final String category;
  final double price;
  final IconData icon;
  final String description;

  const Product({
    required this.id,
    required this.name,
    required this.category,
    required this.price,
    required this.icon,
    required this.description,
  });
}

/// Модель позиції в кошику (товар + кількість)
class CartItem {
  final Product product;
  int quantity;

  CartItem({
    required this.product,
    this.quantity = 1,
  });

  double get totalPrice => product.price * quantity;
}

// ----------------------------------------------------------------------------
// 2. КЕРУВАННЯ СТАНОМ (STATE MANAGEMENT ARCHITECTURE)
// Використання патерну ChangeNotifier без важких зовнішніх бібліотек
// ----------------------------------------------------------------------------

/// Глобальний менеджер теми застосунку (App State)
class ThemeStateManager extends ChangeNotifier {
  ThemeMode _themeMode = ThemeMode.system;

  ThemeMode get themeMode => _themeMode;

  bool get isDarkMode => _themeMode == ThemeMode.dark;

  void toggleTheme() {
    if (_themeMode == ThemeMode.dark) {
      _themeMode = ThemeMode.light;
    } else {
      _themeMode = ThemeMode.dark;
    }
    // Сповіщаємо всіх підписників про зміну стану
    notifyListeners();
  }

  void setThemeMode(ThemeMode mode) {
    _themeMode = mode;
    notifyListeners();
  }
}

/// Глобальний менеджер стану кошика та замовлення (App State)
class CartStateManager extends ChangeNotifier {
  final Map<String, CartItem> _items = {};
  String? _appliedPromoCode;
  double _discountPercent = 0.0;

  // Незмінна колекція для безпечного читання ззовні
  List<CartItem> get items => _items.values.toList();

  int get totalItemCount {
    int count = 0;
    for (var item in _items.values) {
      count += item.quantity;
    }
    return count;
  }

  double get subtotalAmount {
    double total = 0.0;
    for (var item in _items.values) {
      total += item.totalPrice;
    }
    return total;
  }

  double get discountAmount => subtotalAmount * (_discountPercent / 100.0);

  double get finalAmount => subtotalAmount - discountAmount;

  String? get appliedPromoCode => _appliedPromoCode;
  double get discountPercent => _discountPercent;

  /// Додавання товару до кошика
  void addProduct(Product product) {
    if (_items.containsKey(product.id)) {
      _items[product.id]!.quantity += 1;
    } else {
      _items[product.id] = CartItem(product: product, quantity: 1);
    }
    notifyListeners();
  }

  /// Зменшення кількості товару або видалення
  void removeSingleItem(String productId) {
    if (!_items.containsKey(productId)) return;

    if (_items[productId]!.quantity > 1) {
      _items[productId]!.quantity -= 1;
    } else {
      _items.remove(productId);
    }
    notifyListeners();
  }

  /// Повне видалення позиції
  void removeItemCompletely(String productId) {
    if (_items.containsKey(productId)) {
      _items.remove(productId);
      notifyListeners();
    }
  }

  /// Очищення кошика
  void clearCart() {
    _items.clear();
    _appliedPromoCode = null;
    _discountPercent = 0.0;
    notifyListeners();
  }

  /// Застосування промокоду
  bool applyPromoCode(String code) {
    final cleanCode = code.trim().toUpperCase();
    if (cleanCode == 'FLUTTER10') {
      _appliedPromoCode = cleanCode;
      _discountPercent = 10.0;
      notifyListeners();
      return true;
    } else if (cleanCode == 'STUDENT20') {
      _appliedPromoCode = cleanCode;
      _discountPercent = 20.0;
      notifyListeners();
      return true;
    }
    return false;
  }

  /// Отримати кількість конкретного товару в кошику
  int getItemQuantity(String productId) {
    return _items[productId]?.quantity ?? 0;
  }
}

// ----------------------------------------------------------------------------
// 3. SERVICE LOCATOR / DEPENDENCY HOLDER
// Простий та надійний синглтон для доступу до сервісів стану
// ----------------------------------------------------------------------------

class AppStateContainer {
  static final AppStateContainer instance = AppStateContainer._internal();
  AppStateContainer._internal();

  final ThemeStateManager themeManager = ThemeStateManager();
  final CartStateManager cartManager = CartStateManager();

  // Демо-каталог товарів (сенсори, мікроконтролери, IoT обладнання)
  final List<Product> catalog = const [
    Product(
      id: 'iot_esp32',
      name: 'ESP32 NodeMCU Wi-Fi + BLE',
      category: 'Мікроконтролери',
      price: 240.0,
      icon: Icons.developer_board,
      description: 'Двоядерний модуль 240MHz з підтримкою Wi-Fi та Bluetooth 4.2 BLE.',
    ),
    Product(
      id: 'sensor_dht22',
      name: 'Датчик температури DHT22',
      category: 'Сенсори',
      price: 135.0,
      icon: Icons.thermostat,
      description: 'Прецизійний цифровий датчик вологості та температури з каліброваним виходом.',
    ),
    Product(
      id: 'display_oled',
      name: 'OLED Дисплей 0.96" I2C 128x64',
      category: 'Індикація',
      price: 180.0,
      icon: Icons.tv,
      description: 'Монохромний висококонтрастний екран з інтерфейсом I2C для телеметрії.',
    ),
    Product(
      id: 'relay_4ch',
      name: 'Модуль реле 4-канальний 5V',
      category: 'Комутація',
      price: 195.0,
      icon: Icons.electrical_services,
      description: 'Опторозв\'язаний блок силових реле для керування навантаженням до 250V 10A.',
    ),
    Product(
      id: 'sensor_bme280',
      name: 'Барометр BME280 SPI/I2C',
      category: 'Сенсори',
      price: 220.0,
      icon: Icons.speed,
      description: 'Цифровий датчик атмосферного тиску, вологості та температури Bosch.',
    ),
    Product(
      id: 'step_down_lm2596',
      name: 'DC-DC Перетворювач LM2596',
      category: 'Живлення',
      price: 85.0,
      icon: Icons.battery_charging_full,
      description: 'Імпульсний понижуючий стабілізатор напруги з регулюванням 1.25V–35V.',
    ),
  ];
}

// ----------------------------------------------------------------------------
// 4. ГОЛОВНИЙ ВІДЖЕТ ЗАСТОСУНКУ (ROOT WIDGET)
// ----------------------------------------------------------------------------

class SmartShopApp extends StatelessWidget {
  const SmartShopApp({super.key});

  @override
  Widget build(BuildContext context) {
    final themeManager = AppStateContainer.instance.themeManager;

    // Реактивне слухання зміни теми через ListenableBuilder
    return ListenableBuilder(
      listenable: themeManager,
      builder: (context, child) {
        return MaterialApp(
          title: 'Flutter State Management Hub',
          debugShowCheckedModeBanner: false,
          themeMode: themeManager.themeMode,
          theme: ThemeData(
            useMaterial3: true,
            colorScheme: ColorScheme.fromSeed(
              seedColor: Colors.indigo,
              brightness: Brightness.light,
            ),
            appBarTheme: const AppBarTheme(
              centerTitle: true,
              elevation: 2,
            ),
          ),
          darkTheme: ThemeData(
            useMaterial3: true,
            colorScheme: ColorScheme.fromSeed(
              seedColor: Colors.indigo,
              brightness: Brightness.dark,
            ),
            appBarTheme: const AppBarTheme(
              centerTitle: true,
              elevation: 2,
            ),
          ),
          home: const CatalogScreen(),
        );
      },
    );
  }
}

// ----------------------------------------------------------------------------
// 5. ЕКРАН КАТАЛОГУ (CATALOG SCREEN)
// Демонстрація комбінації:
// - Ephemeral State (пошуковий рядок, вибрана категорія в межах екрана)
// - App State (глобальний лічильник кошика та зміна теми)
// ----------------------------------------------------------------------------

class CatalogScreen extends StatefulWidget {
  const CatalogScreen({super.key});

  @override
  State<CatalogScreen> createState() => _CatalogScreenState();
}

class _CatalogScreenState extends State<CatalogScreen> {
  // ЕФЕМЕРНИЙ СТАН (Ephemeral State): існує виключно всередині цього екрана
  String _searchQuery = '';
  String _selectedCategory = 'Всі';

  @override
  Widget build(BuildContext context) {
    final app = AppStateContainer.instance;
    final allProducts = app.catalog;

    // Фільтрація каталогу за локальним станом
    final categories = ['Всі', ...{...allProducts.map((p) => p.category)}];
    final filteredProducts = allProducts.where((p) {
      final matchesSearch = p.name.toLowerCase().contains(_searchQuery.toLowerCase()) ||
          p.description.toLowerCase().contains(_searchQuery.toLowerCase());
      final matchesCat = _selectedCategory == 'Всі' || p.category == _selectedCategory;
      return matchesSearch && matchesCat;
    }).toList();

    return Scaffold(
      appBar: AppBar(
        title: const Text('IoT Маркет & Телеметрія'),
        actions: [
          // Кнопка перемикання теми
          ListenableBuilder(
            listenable: app.themeManager,
            builder: (context, _) {
              final isDark = app.themeManager.isDarkMode;
              return IconButton(
                tooltip: isDark ? 'Увімкнути світлу тему' : 'Увімкнути темну тему',
                icon: Icon(isDark ? Icons.light_mode : Icons.dark_mode),
                onPressed: () => app.themeManager.toggleTheme(),
              );
            },
          ),
          // Кнопка переходу до кошика з динамічним бейджем кількості товарів
          ListenableBuilder(
            listenable: app.cartManager,
            builder: (context, _) {
              final count = app.cartManager.totalItemCount;
              return Stack(
                alignment: Alignment.center,
                children: [
                  IconButton(
                    tooltip: 'Перейти до кошика',
                    icon: const Icon(Icons.shopping_cart_outlined),
                    onPressed: () {
                      Navigator.push(
                        context,
                        MaterialPageRoute(builder: (context) => const CartScreen()),
                      );
                    },
                  ),
                  if (count > 0)
                    Positioned(
                      top: 6,
                      right: 6,
                      child: Container(
                        padding: const EdgeInsets.all(4),
                        decoration: BoxDecoration(
                          color: Theme.of(context).colorScheme.error,
                          shape: BoxShape.circle,
                        ),
                        constraints: const BoxConstraints(minWidth: 18, minHeight: 18),
                        child: Text(
                          '$count',
                          style: const TextStyle(
                            color: Colors.white,
                            fontSize: 10,
                            fontWeight: FontWeight.bold,
                          ),
                          textAlign: TextAlign.center,
                        ),
                      ),
                    ),
                ],
              );
            },
          ),
          IconButton(
            tooltip: 'Інформація про стан (Архітектура)',
            icon: const Icon(Icons.info_outline),
            onPressed: () => _showArchitectureInfoModal(context),
          ),
        ],
      ),
      body: Column(
        children: [
          // Блок пошуку (Ephemeral State)
          Padding(
            padding: const EdgeInsets.fromLTRB(16, 12, 16, 8),
            child: TextField(
              decoration: InputDecoration(
                prefixIcon: const Icon(Icons.search),
                hintText: 'Пошук компонентів, датчиків...',
                filled: true,
                border: OutlineInputBorder(
                  borderRadius: BorderRadius.circular(12),
                  borderSide: BorderSide.none,
                ),
                contentPadding: const EdgeInsets.symmetric(horizontal: 16, vertical: 12),
              ),
              onChanged: (val) {
                setState(() {
                  _searchQuery = val;
                });
              },
            ),
          ),

          // Фільтр категорій (Ephemeral State)
          SingleChildScrollView(
            scrollDirection: Axis.horizontal,
            padding: const EdgeInsets.symmetric(horizontal: 12, vertical: 4),
            child: Row(
              children: categories.map((cat) {
                final isSelected = _selectedCategory == cat;
                return Padding(
                  padding: const EdgeInsets.symmetric(horizontal: 4),
                  child: FilterChip(
                    label: Text(cat),
                    selected: isSelected,
                    onSelected: (selected) {
                      setState(() {
                        _selectedCategory = cat;
                      });
                    },
                  ),
                );
              }).toList(),
            ),
          ),
          const Divider(height: 16),

          // Список товарів
          Expanded(
            child: filteredProducts.isEmpty
                ? const Center(
                    child: Text(
                      'Товарів за вашим запитом не знайдено',
                      style: TextStyle(fontSize: 16, color: Colors.grey),
                    ),
                  )
                : ListView.builder(
                    itemCount: filteredProducts.length,
                    padding: const EdgeInsets.symmetric(horizontal: 12, vertical: 8),
                    itemBuilder: (context, index) {
                      final product = filteredProducts[index];
                      return _ProductCard(product: product);
                    },
                  ),
          ),
        ],
      ),
    );
  }

  void _showArchitectureInfoModal(BuildContext context) {
    showModalBottomSheet(
      context: context,
      shape: const RoundedRectangleBorder(
        borderRadius: BorderRadius.vertical(top: Radius.circular(20)),
      ),
      builder: (context) => Padding(
        padding: const EdgeInsets.all(20),
        child: Column(
          mainAxisSize: MainAxisSize.min,
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              children: [
                Icon(Icons.layers, color: Theme.of(context).colorScheme.primary),
                const SizedBox(width: 8),
                const Text(
                  'Архітектура керування станом',
                  style: TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
                ),
              ],
            ),
            const SizedBox(height: 12),
            const Text(
              '• Ephemeral (Local) State: стан пошуку та фільтрації категорій керується локально через StatefulWidget (setState) та знищується при зміні екрана.\n\n'
              '• App (Global) State: стан кошика (CartStateManager) та обрана тема (ThemeStateManager) живуть на глобальному рівні через патерн ChangeNotifier + ListenableBuilder.\n\n'
              '• Оптимізація рендеру: завдяки ListenableBuilder перебудовуються лише зацікавлені піддерева віджетів (бейджик, кнопки), уникаючи зайвих рендерів усього списку.',
              style: TextStyle(fontSize: 14, height: 1.4),
            ),
            const SizedBox(height: 16),
            Align(
              alignment: Alignment.centerRight,
              child: ElevatedButton(
                onPressed: () => Navigator.pop(context),
                child: const Text('Зрозуміло'),
              ),
            ),
          ],
        ),
      ),
    );
  }
}

// ----------------------------------------------------------------------------
// 6. ВІДЖЕТ КАРТКИ ТОВАРУ (PRODUCT CARD)
// Слухає CartStateManager тільки для свого конкретного товару
// ----------------------------------------------------------------------------

class _ProductCard extends StatelessWidget {
  final Product product;

  const _ProductCard({required this.product});

  @override
  Widget build(BuildContext context) {
    final cartManager = AppStateContainer.instance.cartManager;

    return Card(
      margin: const EdgeInsets.symmetric(vertical: 6, horizontal: 4),
      elevation: 1.5,
      shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(14)),
      child: Padding(
        padding: const EdgeInsets.all(12),
        child: Row(
          crossAxisAlignment: CrossAxisAlignment.center,
          children: [
            // Іконка товару
            Container(
              width: 52,
              height: 52,
              decoration: BoxDecoration(
                color: Theme.of(context).colorScheme.primaryContainer,
                borderRadius: BorderRadius.circular(12),
              ),
              child: Icon(
                product.icon,
                size: 28,
                color: Theme.of(context).colorScheme.onPrimaryContainer,
              ),
            ),
            const SizedBox(width: 14),

            // Назва, категорія та ціна
            Expanded(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(
                    product.name,
                    style: const TextStyle(fontWeight: FontWeight.bold, fontSize: 15),
                  ),
                  const SizedBox(height: 2),
                  Text(
                    product.category,
                    style: TextStyle(fontSize: 12, color: Theme.of(context).hintColor),
                  ),
                  const SizedBox(height: 4),
                  Text(
                    '${product.price.toStringAsFixed(2)} ₴',
                    style: TextStyle(
                      fontWeight: FontWeight.bold,
                      fontSize: 15,
                      color: Theme.of(context).colorScheme.primary,
                    ),
                  ),
                ],
              ),
            ),

            // Реактивна панель додавання / лічильника
            ListenableBuilder(
              listenable: cartManager,
              builder: (context, _) {
                final qty = cartManager.getItemQuantity(product.id);

                if (qty == 0) {
                  return ElevatedButton.icon(
                    onPressed: () => cartManager.addProduct(product),
                    icon: const Icon(Icons.add_shopping_cart, size: 16),
                    label: const Text('Додати'),
                    style: ElevatedButton.styleFrom(
                      padding: const EdgeInsets.symmetric(horizontal: 12, vertical: 8),
                      visualDensity: VisualDensity.compact,
                    ),
                  );
                }

                return Container(
                  decoration: BoxDecoration(
                    color: Theme.of(context).colorScheme.surfaceVariant,
                    borderRadius: BorderRadius.circular(20),
                  ),
                  child: Row(
                    mainAxisSize: MainAxisSize.min,
                    children: [
                      IconButton(
                        icon: const Icon(Icons.remove, size: 18),
                        onPressed: () => cartManager.removeSingleItem(product.id),
                        constraints: const BoxConstraints(minWidth: 32, minHeight: 32),
                        padding: EdgeInsets.zero,
                      ),
                      Text(
                        '$qty',
                        style: const TextStyle(fontWeight: FontWeight.bold, fontSize: 14),
                      ),
                      IconButton(
                        icon: const Icon(Icons.add, size: 18),
                        onPressed: () => cartManager.addProduct(product),
                        constraints: const BoxConstraints(minWidth: 32, minHeight: 32),
                        padding: EdgeInsets.zero,
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

// ----------------------------------------------------------------------------
// 7. ЕКРАН КОШИКА (CART SCREEN)
// Повна синхронізація з App State: підсумки, промокоди, оформлення
// ----------------------------------------------------------------------------

class CartScreen extends StatefulWidget {
  const CartScreen({super.key});

  @override
  State<CartScreen> createState() => _CartScreenState();
}

class _CartScreenState extends State<CartScreen> {
  final TextEditingController _promoController = TextEditingController();

  @override
  void dispose() {
    _promoController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    final cartManager = AppStateContainer.instance.cartManager;

    return Scaffold(
      appBar: AppBar(
        title: const Text('Мій кошик'),
        actions: [
          ListenableBuilder(
            listenable: cartManager,
            builder: (context, _) {
              if (cartManager.items.isEmpty) return const SizedBox.shrink();
              return IconButton(
                tooltip: 'Очистити кошик',
                icon: const Icon(Icons.delete_sweep_outlined),
                onPressed: () => _confirmClearCart(context, cartManager),
              );
            },
          ),
        ],
      ),
      body: ListenableBuilder(
        listenable: cartManager,
        builder: (context, _) {
          final items = cartManager.items;

          if (items.isEmpty) {
            return Center(
              child: Column(
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  Icon(
                    Icons.remove_shopping_cart_outlined,
                    size: 80,
                    color: Theme.of(context).hintColor,
                  ),
                  const SizedBox(height: 16),
                  const Text(
                    'Кошик порожній',
                    style: TextStyle(fontSize: 20, fontWeight: FontWeight.bold),
                  ),
                  const SizedBox(height: 8),
                  const Text(
                    'Додайте потрібні модулі та сенсори з каталогу',
                    style: TextStyle(color: Colors.grey),
                  ),
                  const SizedBox(height: 20),
                  ElevatedButton(
                    onPressed: () => Navigator.pop(context),
                    child: const Text('Перейти до каталогу'),
                  ),
                ],
              ),
            );
          }

          return Column(
            children: [
              // Список замовлених позицій
              Expanded(
                child: ListView.separated(
                  itemCount: items.length,
                  padding: const EdgeInsets.all(12),
                  separatorBuilder: (_, __) => const SizedBox(height: 8),
                  itemBuilder: (context, index) {
                    final item = items[index];
                    return _CartItemTile(item: item, cartManager: cartManager);
                  },
                ),
              ),

              // Блок промокоду та розрахунків
              Container(
                padding: const EdgeInsets.all(16),
                decoration: BoxDecoration(
                  color: Theme.of(context).colorScheme.surface,
                  boxShadow: [
                    BoxShadow(
                      color: Colors.black.withOpacity(0.08),
                      blurRadius: 10,
                      offset: const Offset(0, -3),
                    ),
                  ],
                  borderRadius: const BorderRadius.vertical(top: Radius.circular(20)),
                ),
                child: SafeArea(
                  child: Column(
                    mainAxisSize: MainAxisSize.min,
                    children: [
                      // Поле вводу промокоду
                      Row(
                        children: [
                          Expanded(
                            child: TextField(
                              controller: _promoController,
                              textCapitalization: TextCapitalization.characters,
                              decoration: InputDecoration(
                                hintText: 'Промокод (STUDENT20 / FLUTTER10)',
                                isDense: true,
                                border: OutlineInputBorder(
                                  borderRadius: BorderRadius.circular(10),
                                ),
                                contentPadding: const EdgeInsets.symmetric(
                                  horizontal: 12,
                                  vertical: 10,
                                ),
                              ),
                            ),
                          ),
                          const SizedBox(width: 8),
                          FilledButton.tonal(
                            onPressed: () {
                              final success = cartManager.applyPromoCode(_promoController.text);
                              ScaffoldMessenger.of(context).showSnackBar(
                                SnackBar(
                                  content: Text(
                                    success
                                        ? 'Промокод успішно застосовано (-${cartManager.discountPercent.toInt()}%)!'
                                        : 'Невірний промокод. Спробуйте STUDENT20 або FLUTTER10',
                                  ),
                                  duration: const Duration(seconds: 2),
                                ),
                              );
                            },
                            child: const Text('Застосувати'),
                          ),
                        ],
                      ),
                      const SizedBox(height: 12),

                      // Розрахункові показники
                      Row(
                        mainAxisAlignment: MainAxisAlignment.spaceBetween,
                        children: [
                          const Text('Сума замовлення:', style: TextStyle(color: Colors.grey)),
                          Text('${cartManager.subtotalAmount.toStringAsFixed(2)} ₴'),
                        ],
                      ),
                      if (cartManager.discountPercent > 0) ...[
                        const SizedBox(height: 4),
                        Row(
                          mainAxisAlignment: MainAxisAlignment.spaceBetween,
                          children: [
                            Text(
                              'Знижка (${cartManager.discountPercent.toInt()}% [${cartManager.appliedPromoCode}]):',
                              style: const TextStyle(color: Colors.green),
                            ),
                            Text(
                              '-${cartManager.discountAmount.toStringAsFixed(2)} ₴',
                              style: const TextStyle(
                                color: Colors.green,
                                fontWeight: FontWeight.bold,
                              ),
                            ),
                          ],
                        ),
                      ],
                      const Divider(height: 16),
                      Row(
                        mainAxisAlignment: MainAxisAlignment.spaceBetween,
                        children: [
                          const Text(
                            'До сплати:',
                            style: TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
                          ),
                          Text(
                            '${cartManager.finalAmount.toStringAsFixed(2)} ₴',
                            style: TextStyle(
                              fontSize: 20,
                              fontWeight: FontWeight.bold,
                              color: Theme.of(context).colorScheme.primary,
                            ),
                          ),
                        ],
                      ),
                      const SizedBox(height: 14),

                      // Кнопка оформлення замовлення
                      SizedBox(
                        width: double.infinity,
                        height: 48,
                        child: FilledButton.icon(
                          onPressed: () => _handleCheckout(context, cartManager),
                          icon: const Icon(Icons.check_circle_outline),
                          label: const Text(
                            'Оформити замовлення',
                            style: TextStyle(fontSize: 16, fontWeight: FontWeight.bold),
                          ),
                        ),
                      ),
                    ],
                  ),
                ),
              ),
            ],
          );
        },
      ),
    );
  }

  void _confirmClearCart(BuildContext context, CartStateManager cartManager) {
    showDialog(
      context: context,
      builder: (ctx) => AlertDialog(
        title: const Text('Очистити кошик?'),
        content: const Text('Ви дійсно бажаєте видалити всі позиції з вашого кошика?'),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(ctx),
            child: const Text('Скасувати'),
          ),
          TextButton(
            onPressed: () {
              cartManager.clearCart();
              Navigator.pop(ctx);
            },
            child: const Text('Очистити', style: TextStyle(color: Colors.red)),
          ),
        ],
      ),
    );
  }

  void _handleCheckout(BuildContext context, CartStateManager cartManager) {
    final finalSum = cartManager.finalAmount.toStringAsFixed(2);
    final count = cartManager.totalItemCount;

    cartManager.clearCart();

    showDialog(
      context: context,
      builder: (ctx) => AlertDialog(
        icon: const Icon(Icons.task_alt, color: Colors.green, size: 54),
        title: const Text('Замовлення прийнято!'),
        content: Text(
          'Успішно оформлено $count од. обладнання на суму $finalSum ₴.\n\n'
          'Глобальний стан кошика CartStateManager було синхронно скинуто через notifyListeners().',
          textAlign: TextAlign.center,
        ),
        actions: [
          FilledButton(
            onPressed: () {
              Navigator.pop(ctx); // Закрити діалог
              Navigator.pop(context); // Повернутись до каталогу
            },
            child: const Text('Чудово'),
          ),
        ],
      ),
    );
  }
}

// ----------------------------------------------------------------------------
// 8. ПЛИТКА ПОЗИЦІЇ В КОШИКУ (CART ITEM TILE)
// ----------------------------------------------------------------------------

class _CartItemTile extends StatelessWidget {
  final CartItem item;
  final CartStateManager cartManager;

  const _CartItemTile({
    required this.item,
    required this.cartManager,
  });

  @override
  Widget build(BuildContext context) {
    return Card(
      elevation: 1,
      shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
      child: Padding(
        padding: const EdgeInsets.symmetric(horizontal: 12, vertical: 8),
        child: Row(
          children: [
            Icon(item.product.icon, size: 30, color: Theme.of(context).colorScheme.primary),
            const SizedBox(width: 12),
            Expanded(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(
                    item.product.name,
                    style: const TextStyle(fontWeight: FontWeight.bold, fontSize: 14),
                  ),
                  const SizedBox(height: 2),
                  Text(
                    '${item.product.price.toStringAsFixed(2)} ₴ x ${item.quantity} = ${item.totalPrice.toStringAsFixed(2)} ₴',
                    style: TextStyle(fontSize: 12, color: Theme.of(context).hintColor),
                  ),
                ],
              ),
            ),
            Row(
              mainAxisSize: MainAxisSize.min,
              children: [
                IconButton(
                  icon: const Icon(Icons.remove_circle_outline, size: 22),
                  color: Colors.orange,
                  onPressed: () => cartManager.removeSingleItem(item.product.id),
                ),
                Text(
                  '${item.quantity}',
                  style: const TextStyle(fontWeight: FontWeight.bold, fontSize: 15),
                ),
                IconButton(
                  icon: const Icon(Icons.add_circle_outline, size: 22),
                  color: Theme.of(context).colorScheme.primary,
                  onPressed: () => cartManager.addProduct(item.product),
                ),
                IconButton(
                  icon: const Icon(Icons.delete_outline, size: 22),
                  color: Colors.red,
                  onPressed: () => cartManager.removeItemCompletely(item.product.id),
                ),
              ],
            ),
          ],
        ),
      ),
    );
  }
}
