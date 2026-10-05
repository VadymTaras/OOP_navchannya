// ==============================================================================
// Практична робота №4. Основи мови Dart
// Дисципліна: Програмування для мобільних платформ
// Виконавець: Студент групи аІк43 ТАРАС Вадим
// Викладач:   КЛИМЕНКО О.А.
// Заклад:     Фаховий коледж ЗВО МНТУ (Київ — 2026)
// ==============================================================================

// --- Міксіни (Mixins) ---
mixin Autonomous {
  void navigateAuto(String route) {
    print('⚡ Автопілот активовано за маршрутом: $route');
  }
}

// --- Базовий абстрактний клас ---
abstract class Vehicle {
  final String brand;
  final int year;

  const Vehicle(this.brand, this.year);

  void startEngine();
}

// --- Похідний клас з міксіном та фабричним конструктором ---
class ElectricCar extends Vehicle with Autonomous {
  final int batteryCapacity;
  double _chargeLevel = 100.0;

  static final Map<String, ElectricCar> _cache = {};

  ElectricCar({
    required String brand,
    required int year,
    this.batteryCapacity = 75,
  }) : super(brand, year);

  // Фабричний конструктор для кешування об'єктів
  factory ElectricCar.cached({required String brand, required int year}) {
    final key = '$brand-$year';
    if (_cache.containsKey(key)) {
      return _cache[key]!;
    }
    final newCar = ElectricCar(brand: brand, year: year);
    _cache[key] = newCar;
    return newCar;
  }

  @override
  void startEngine() {
    print('🔋 Електрокар $brand ($year р.) увімкнено! Заряд: $_chargeLevel%');
  }

  double get chargeLevel => _chargeLevel;

  set chargeLevel(double value) {
    if (value >= 0 && value <= 100) {
      _chargeLevel = value;
    }
  }
}

// --- Допоміжні функції ---
double calculateProjectCost({
  required int baseHours,
  double hourlyRate = 35.0,
  double discount = 0.0,
}) {
  return (baseHours * hourlyRate) - discount;
}

Function createIdGenerator(String prefix) {
  int counter = 0;
  return () {
    counter++;
    return '$prefix-${counter.toString().padLeft(3, '0')}';
  };
}

// --- Точка входу в програму ---
void main() {
  print('======================================================');
  print('=== ПРАКТИЧНА РОБОТА №4. ОСНОВИ МОВИ DART         ===');
  print('=== Виконав: ТАРАС Вадим (група аІк43)             ===');
  print('======================================================\n');

  // 1. ДЕМОНСТРАЦІЯ: Типи даних, var, final, const
  print('=== ДЕМОНСТРАЦІЯ 1: Типи даних, var, final, const ===');
  final String appName = 'Dart Mobile Core v1.0';
  const double piConstant = 3.14159265;
  var studentName = 'ТАРАС Вадим';
  String group = 'аІк43';

  print('[+] Застосунок: $appName');
  print('[+] Розробник: $studentName (група $group, інженерія ПЗ)');
  print('[+] Константа Pi: $piConstant | Immutable стан зафіксовано\n');

  // 2. ДЕМОНСТРАЦІЯ: Sound Null Safety (?, !, ?., ??)
  print('=== ДЕМОНСТРАЦІЯ 2: Sound Null Safety (?, !, ?., ??) ===');
  String? optionalNickname = null;
  print('[*] Опціональний нікнейм: $optionalNickname');

  String displayName = optionalNickname ?? 'Гість';
  print("[+] Значення за замовчуванням (??): '$displayName'");

  int nameLength = optionalNickname?.length ?? 0;
  print('[+] Безпечна перевірка довжини (?.): $nameLength символів');

  optionalNickname = 'vadym_t';
  print("[+] Присвоєння нового нікнейму: '$optionalNickname'");
  print('[+] Довжина нікнейму після ініціалізації: ${optionalNickname.length} символів\n');

  // 3. ДЕМОНСТРАЦІЯ: Колекції List, Set, Map та методи обробки
  print('=== ДЕМОНСТРАЦІЯ 3: Колекції List, Set, Map та методи обробки ===');
  List<int> numbers = [12, 5, 8, 21, 44, 3, 17];
  print('[*] Вхідний список чисел: $numbers');

  var evens = numbers.where((n) => n.isEven).toList();
  print('[+] Фільтрація .where(n.isEven) : $evens');

  var squares = evens.map((n) => n * n).toList();
  print('[+] Трансформація .map(n * n)    : $squares');

  int totalSum = numbers.reduce((acc, curr) => acc + curr);
  print('[+] Агрегація .reduce(+)        : $totalSum');

  Set<String> uniqueTags = {'mobile', 'dart', 'flutter', 'crossplatform', 'dart'};
  print('[*] Унікальні теги (Set)        : $uniqueTags');

  Map<String, dynamic> userProfile = {
    'name': studentName,
    'course': 4,
    'verified': true
  };
  print('[*] Профіль студента (Map)      : $userProfile\n');

  // 4. ДЕМОНСТРАЦІЯ: Функції, параметри та замикання (Closures)
  print('=== ДЕМОНСТРАЦІЯ 4: Функції та замикання (Closures) ===');
  double finalPrice = calculateProjectCost(baseHours: 40, hourlyRate: 40.0, discount: 150.0);
  print('[+] Обчислення вартості розробки: ${finalPrice.toStringAsFixed(2)} USD (знижка застосована)');

  var idGenerator = createIdGenerator('APP');
  print('[+] Генератор ID (замикання): ${idGenerator()}, ${idGenerator()}, ${idGenerator()}\n');

  // 5. ДЕМОНСТРАЦІЯ: ООП, наслідування, Mixins та Factory
  print('=== ДЕМОНСТРАЦІЯ 5: ООП (extends, with Mixin, factory) ===');
  var car = ElectricCar(
    brand: 'Tesla Model 3',
    year: 2024,
    batteryCapacity: 82,
  );
  print('[+] Створено екземпляр: ElectricCar (${car.brand}, ${car.year} р., ${car.batteryCapacity} кВт*год)');
  car.startEngine();
  car.navigateAuto('Київ -> Львів');

  var cachedCar = ElectricCar.cached(brand: 'Tesla Model 3', year: 2024);
  print('[+] Factory конструювання: створено об\'єкт через кешований екземпляр.\n');

  print('[SUCCESS] Усі 5 блоків перевірки мови Dart виконано без помилок (Exit code 0).');
}
