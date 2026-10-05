import 'package:flutter/material.dart';

void main() {
  runApp(const HardwareTaskManagerApp());
}

/// Головний кореневий віджет програми (StatelessWidget)
class HardwareTaskManagerApp extends StatelessWidget {
  const HardwareTaskManagerApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Диспетчер інженерних завдань',
      debugShowCheckedModeBanner: false,
      theme: ThemeData(
        useMaterial3: true,
        brightness: Brightness.dark,
        colorSchemeSeed: Colors.teal,
        scaffoldBackgroundColor: const Color(0xFF121418),
        cardTheme: const CardTheme(
          color: Color(0xFF1E222B),
          elevation: 2,
          margin: EdgeInsets.symmetric(horizontal: 14, vertical: 6),
        ),
        appBarTheme: const AppBarTheme(
          backgroundColor: Color(0xFF181C24),
          foregroundColor: Colors.white,
          elevation: 0,
        ),
      ),
      home: const TaskManagerHomePage(
        title: 'Інженерний диспетчер завдань',
        studentInfo: 'Студент: Вадим ТАРАС (аІк43)',
      ),
    );
  }
}

/// Пріоритет інженерного завдання
enum TaskPriority {
  low('Низький', Colors.blueGrey),
  medium('Звичайний', Colors.amber),
  high('Високий', Colors.orange),
  critical('Критичний', Colors.redAccent);

  final String label;
  final Color color;
  const TaskPriority(this.label, this.color);
}

/// Категорія обладнання/робіт
enum TaskCategory {
  all('Усі'),
  diagnostics('Діагностика'),
  soldering('Пайка та залізо'),
  firmware('BIOS / Прошивка'),
  printing3D('3D-друк та оснащення');

  final String label;
  const TaskCategory(this.label);
}

/// Модель сутності інженерного завдання
class TaskItem {
  final String id;
  String title;
  String description;
  TaskCategory category;
  TaskPriority priority;
  bool isCompleted;
  final DateTime createdAt;

  TaskItem({
    required this.id,
    required this.title,
    required this.description,
    required this.category,
    required this.priority,
    this.isCompleted = false,
    DateTime? createdAt,
  }) : createdAt = createdAt ?? DateTime.now();
}

/// Головний екран із динамічним списком та станом (StatefulWidget)
class TaskManagerHomePage extends StatefulWidget {
  final String title;
  final String studentInfo;

  const TaskManagerHomePage({
    super.key,
    required this.title,
    required this.studentInfo,
  });

  @override
  State<TaskManagerHomePage> createState() => _TaskManagerHomePageState();
}

/// Стан головного екрана (реалізація життєвого циклу State)
class _TaskManagerHomePageState extends State<TaskManagerHomePage> {
  // Колекція інженерних завдань
  final List<TaskItem> _tasks = [];

  // Поточний фільтр категорій
  TaskCategory _selectedCategory = TaskCategory.all;

  // Пошуковий запит
  String _searchQuery = '';
  final TextEditingController _searchController = TextEditingController();

  @override
  void initState() {
    super.initState();
    // Ініціалізація початкового набору завдань у initState
    _seedInitialTasks();
  }

  @override
  void dispose() {
    // Звільнення ресурсів контролера введення у dispose
    _searchController.dispose();
    super.dispose();
  }

  void _seedInitialTasks() {
    _tasks.addAll([
      TaskItem(
        id: '1',
        title: 'Відновлення дампа BIOS ASUS ROG Ally',
        description: 'Очистити ME регіон та прошити чіп Winbond через програматор CH341A 1.8V.',
        category: TaskCategory.firmware,
        priority: TaskPriority.critical,
        isCompleted: false,
      ),
      TaskItem(
        id: '2',
        title: 'Калібрування резонансів Klipper (Input Shaper)',
        description: 'Підключити акселерометр ADXL345 до голівки принтера та зняти графіки осі X/Y.',
        category: TaskCategory.printing3D,
        priority: TaskPriority.high,
        isCompleted: true,
      ),
      TaskItem(
        id: '3',
        title: 'Діагностика шини LVDS телевізора KIVI 32HR50GU',
        description: 'Перевірити диференціальні пари та узгодження таймінгів матриці AU32 на шасі MSD6488.',
        category: TaskCategory.diagnostics,
        priority: TaskPriority.medium,
        isCompleted: false,
      ),
      TaskItem(
        id: '4',
        title: 'Заміна роз’єму живлення Type-C на платі TOX3',
        description: 'Демонтаж пошкодженого порту термоповітряною станцією при 360°C та впаювання нового.',
        category: TaskCategory.soldering,
        priority: TaskPriority.low,
        isCompleted: false,
      ),
    ]);
  }

  // Обчислення відфільтрованого списку
  List<TaskItem> get _filteredTasks {
    return _tasks.where((task) {
      final matchesCategory = _selectedCategory == TaskCategory.all ||
          task.category == _selectedCategory;
      final matchesSearch = _searchQuery.isEmpty ||
          task.title.toLowerCase().contains(_searchQuery.toLowerCase()) ||
          task.description.toLowerCase().contains(_searchQuery.toLowerCase());
      return matchesCategory && matchesSearch;
    }).toList();
  }

  void _toggleTaskStatus(TaskItem task) {
    setState(() {
      task.isCompleted = !task.isCompleted;
    });
    ScaffoldMessenger.of(context).hideCurrentSnackBar();
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(
        content: Text(
          task.isCompleted
              ? 'Завдання "${task.title}" позначено як виконане'
              : 'Завдання "${task.title}" повернуто в роботу',
        ),
        backgroundColor: task.isCompleted ? Colors.teal : Colors.blueGrey,
        duration: const Duration(seconds: 2),
      ),
    );
  }

  void _deleteTask(TaskItem task) {
    final taskIndex = _tasks.indexOf(task);
    setState(() {
      _tasks.remove(task);
    });

    ScaffoldMessenger.of(context).hideCurrentSnackBar();
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(
        content: Text('Видалено: ${task.title}'),
        action: SnackBarAction(
          label: 'СКАСУВАТИ',
          textColor: Colors.amberAccent,
          onPressed: () {
            setState(() {
              _tasks.insert(taskIndex, task);
            });
          },
        ),
        duration: const Duration(seconds: 3),
      ),
    );
  }

  Future<void> _openAddTaskDialog() async {
    final formKey = GlobalKey<FormState>();
    final titleController = TextEditingController();
    final descController = TextEditingController();
    TaskCategory selectedCategory = TaskCategory.diagnostics;
    TaskPriority selectedPriority = TaskPriority.medium;
    bool isUrgent = false;

    await showDialog(
      context: context,
      builder: (BuildContext ctx) {
        return StatefulBuilder(
          builder: (context, setDialogState) {
            return AlertDialog(
              backgroundColor: const Color(0xFF1E222B),
              shape: RoundedRectangleBorder(
                borderRadius: BorderRadius.circular(16),
              ),
              title: const Row(
                children: [
                  Icon(Icons.build_circle, color: Colors.tealAccent),
                  SizedBox(width: 8),
                  Text('Нове завдання', style: TextStyle(color: Colors.white)),
                ],
              ),
              content: SingleChildScrollView(
                child: Form(
                  key: formKey,
                  child: Column(
                    mainAxisSize: MainAxisSize.min,
                    children: [
                      // Поле введення назви завдання з валідацією
                      TextFormField(
                        controller: titleController,
                        style: const TextStyle(color: Colors.white),
                        decoration: const InputDecoration(
                          labelText: 'Назва завдання *',
                          labelStyle: TextStyle(color: Colors.grey),
                          hintText: 'Напр. Прошивка VBIOS RX 5700 XT',
                          prefixIcon: Icon(Icons.title, color: Colors.teal),
                        ),
                        validator: (value) {
                          if (value == null || value.trim().isEmpty) {
                            return 'Будь ласка, введіть назву завдання';
                          }
                          if (value.trim().length < 5) {
                            return 'Назва має містити не менше 5 символів';
                          }
                          return null;
                        },
                      ),
                      const SizedBox(height: 12),
                      // Поле опису
                      TextFormField(
                        controller: descController,
                        style: const TextStyle(color: Colors.white),
                        maxLines: 2,
                        decoration: const InputDecoration(
                          labelText: 'Технічний опис / Примітки',
                          labelStyle: TextStyle(color: Colors.grey),
                          hintText: 'Вкажіть контрольні точки, параметри...',
                          prefixIcon: Icon(Icons.notes, color: Colors.teal),
                        ),
                      ),
                      const SizedBox(height: 14),
                      // Випадаючий список категорій (DropdownButtonFormField)
                      DropdownButtonFormField<TaskCategory>(
                        value: selectedCategory,
                        dropdownColor: const Color(0xFF262C36),
                        style: const TextStyle(color: Colors.white),
                        decoration: const InputDecoration(
                          labelText: 'Категорія робіт',
                          prefixIcon: Icon(Icons.category, color: Colors.teal),
                        ),
                        items: TaskCategory.values
                            .where((c) => c != TaskCategory.all)
                            .map((c) => DropdownMenuItem(
                                  value: c,
                                  child: Text(c.label),
                                ))
                            .toList(),
                        onChanged: (val) {
                          if (val != null) {
                            setDialogState(() => selectedCategory = val);
                          }
                        },
                      ),
                      const SizedBox(height: 14),
                      // Випадаючий список пріоритету
                      DropdownButtonFormField<TaskPriority>(
                        value: selectedPriority,
                        dropdownColor: const Color(0xFF262C36),
                        style: const TextStyle(color: Colors.white),
                        decoration: const InputDecoration(
                          labelText: 'Пріоритет',
                          prefixIcon: Icon(Icons.flag, color: Colors.teal),
                        ),
                        items: TaskPriority.values
                            .map((p) => DropdownMenuItem(
                                  value: p,
                                  child: Row(
                                    children: [
                                      CircleAvatar(
                                        radius: 5,
                                        backgroundColor: p.color,
                                      ),
                                      const SizedBox(width: 8),
                                      Text(p.label),
                                    ],
                                  ),
                                ))
                            .toList(),
                        onChanged: (val) {
                          if (val != null) {
                            setDialogState(() => selectedPriority = val);
                          }
                        },
                      ),
                      const SizedBox(height: 12),
                      // Перемикач SwitchListTile (терміновий дедлайн)
                      SwitchListTile(
                        contentPadding: EdgeInsets.zero,
                        title: const Text('Термінове виконання (Hotfix)',
                            style: TextStyle(fontSize: 14, color: Colors.white70)),
                        value: isUrgent,
                        activeColor: Colors.tealAccent,
                        onChanged: (val) {
                          setDialogState(() {
                            isUrgent = val;
                            if (isUrgent) {
                              selectedPriority = TaskPriority.critical;
                            }
                          });
                        },
                      ),
                    ],
                  ),
                ),
              ),
              actions: [
                TextButton(
                  onPressed: () => Navigator.of(ctx).pop(),
                  child: const Text('СКАСУВАТИ', style: TextStyle(color: Colors.grey)),
                ),
                ElevatedButton.icon(
                  icon: const Icon(Icons.check, size: 18),
                  label: const Text('ДОДАТИ'),
                  style: ElevatedButton.styleFrom(
                    backgroundColor: Colors.teal,
                    foregroundColor: Colors.white,
                  ),
                  onPressed: () {
                    if (formKey.currentState!.validate()) {
                      final newTask = TaskItem(
                        id: DateTime.now().millisecondsSinceEpoch.toString(),
                        title: titleController.text.trim(),
                        description: descController.text.trim().isEmpty
                            ? 'Без додаткового опису'
                            : descController.text.trim(),
                        category: selectedCategory,
                        priority: selectedPriority,
                      );
                      setState(() {
                        _tasks.insert(0, newTask);
                      });
                      Navigator.of(ctx).pop();
                      ScaffoldMessenger.of(context).showSnackBar(
                        const SnackBar(
                          content: Text('Завдання успішно додано до реєстру'),
                          backgroundColor: Colors.teal,
                        ),
                      );
                    }
                  },
                ),
              ],
            );
          },
        );
      },
    );

    titleController.dispose();
    descController.dispose();
  }

  @override
  Widget build(BuildContext context) {
    final completedCount = _tasks.where((t) => t.isCompleted).length;
    final totalCount = _tasks.length;
    final inProgressCount = totalCount - completedCount;

    return Scaffold(
      appBar: AppBar(
        title: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(widget.title, style: const TextStyle(fontSize: 18, fontWeight: FontWeight.bold)),
            Text(widget.studentInfo, style: const TextStyle(fontSize: 12, color: Colors.tealAccent)),
          ],
        ),
        actions: [
          IconButton(
            icon: const Icon(Icons.refresh),
            tooltip: 'Скинути початковий список',
            onPressed: () {
              setState(() {
                _tasks.clear();
                _seedInitialTasks();
                _searchController.clear();
                _searchQuery = '';
                _selectedCategory = TaskCategory.all;
              });
            },
          ),
        ],
      ),
      body: Column(
        children: [
          // 1. Інформаційні плашки аналітики (Container + Row + Expanded)
          Container(
            padding: const EdgeInsets.symmetric(horizontal: 14, vertical: 10),
            color: const Color(0xFF161A22),
            child: Row(
              children: [
                _StatCard(label: 'Усього', count: totalCount, color: Colors.blueGrey),
                const SizedBox(width: 8),
                _StatCard(label: 'В роботі', count: inProgressCount, color: Colors.amber),
                const SizedBox(width: 8),
                _StatCard(label: 'Виконано', count: completedCount, color: Colors.tealAccent),
              ],
            ),
          ),

          // 2. Рядок пошуку (TextField)
          Padding(
            padding: const EdgeInsets.fromLTRB(14, 10, 14, 4),
            child: TextField(
              controller: _searchController,
              style: const TextStyle(color: Colors.white),
              decoration: InputDecoration(
                hintText: 'Пошук за назвою або деталлю...',
                hintStyle: const TextStyle(color: Colors.grey),
                prefixIcon: const Icon(Icons.search, color: Colors.teal),
                suffixIcon: _searchQuery.isNotEmpty
                    ? IconButton(
                        icon: const Icon(Icons.clear, color: Colors.grey),
                        onPressed: () {
                          setState(() {
                            _searchController.clear();
                            _searchQuery = '';
                          });
                        },
                      )
                    : null,
                filled: true,
                fillColor: const Color(0xFF1C2028),
                border: OutlineInputBorder(
                  borderRadius: BorderRadius.circular(12),
                  borderSide: BorderSide.none,
                ),
                contentPadding: const EdgeInsets.symmetric(vertical: 0, horizontal: 16),
              ),
              onChanged: (val) {
                setState(() {
                  _searchQuery = val;
                });
              },
            ),
          ),

          // 3. Фільтр за категоріями (SingleChildScrollView + Row + FilterChip)
          SingleChildScrollView(
            scrollDirection: Axis.horizontal,
            padding: const EdgeInsets.symmetric(horizontal: 14, vertical: 6),
            child: Row(
              children: TaskCategory.values.map((cat) {
                final isSelected = _selectedCategory == cat;
                return Padding(
                  padding: const EdgeInsets.only(right: 6),
                  child: FilterChip(
                    label: Text(cat.label),
                    selected: isSelected,
                    selectedColor: Colors.teal.withOpacity(0.35),
                    checkmarkColor: Colors.tealAccent,
                    labelStyle: TextStyle(
                      color: isSelected ? Colors.tealAccent : Colors.white70,
                      fontWeight: isSelected ? FontWeight.bold : FontWeight.normal,
                      fontSize: 12,
                    ),
                    backgroundColor: const Color(0xFF1F2430),
                    shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(10),
                      side: BorderSide(
                        color: isSelected ? Colors.teal : Colors.transparent,
                      ),
                    ),
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

          const Divider(height: 1, color: Color(0xFF282D38)),

          // 4. Динамічний список віджетів (Expanded + ListView.builder)
          Expanded(
            child: _filteredTasks.isEmpty
                ? Center(
                    child: Column(
                      mainAxisAlignment: MainAxisAlignment.center,
                      children: [
                        Icon(Icons.assignment_turned_in_outlined,
                            size: 64, color: Colors.grey.shade600),
                        const SizedBox(height: 12),
                        Text(
                          'Завдань не знайдено',
                          style: TextStyle(
                              fontSize: 16, color: Colors.grey.shade400, fontWeight: FontWeight.w600),
                        ),
                        const SizedBox(height: 4),
                        const Text(
                          'Спробуйте змінити фільтр або створіть нове',
                          style: TextStyle(fontSize: 12, color: Colors.grey),
                        ),
                      ],
                    ),
                  )
                : ListView.builder(
                    itemCount: _filteredTasks.length,
                    padding: const EdgeInsets.symmetric(vertical: 8),
                    itemBuilder: (context, index) {
                      final task = _filteredTasks[index];
                      return Dismissible(
                        key: Key(task.id),
                        direction: DismissDirection.endToStart,
                        background: Container(
                          alignment: Alignment.centerRight,
                          padding: const EdgeInsets.only(right: 20),
                          color: Colors.red.shade900,
                          child: const Row(
                            mainAxisAlignment: MainAxisAlignment.end,
                            children: [
                              Icon(Icons.delete_forever, color: Colors.white),
                              SizedBox(width: 8),
                              Text('Видалити', style: TextStyle(color: Colors.white)),
                            ],
                          ),
                        ),
                        onDismissed: (direction) => _deleteTask(task),
                        child: Card(
                          shape: RoundedRectangleBorder(
                            borderRadius: BorderRadius.circular(12),
                            side: BorderSide(
                              color: task.isCompleted
                                  ? Colors.teal.withOpacity(0.3)
                                  : Colors.transparent,
                            ),
                          ),
                          child: InkWell(
                            borderRadius: BorderRadius.circular(12),
                            onTap: () => _toggleTaskStatus(task),
                            child: Padding(
                              padding: const EdgeInsets.all(12),
                              child: Row(
                                crossAxisAlignment: CrossAxisAlignment.start,
                                children: [
                                  // Чекбокс виконання
                                  Checkbox(
                                    value: task.isCompleted,
                                    activeColor: Colors.teal,
                                    shape: RoundedRectangleBorder(
                                      borderRadius: BorderRadius.circular(4),
                                    ),
                                    onChanged: (bool? value) => _toggleTaskStatus(task),
                                  ),
                                  const SizedBox(width: 8),
                                  // Основний контент картки
                                  Expanded(
                                    child: Column(
                                      crossAxisAlignment: CrossAxisAlignment.start,
                                      children: [
                                        Row(
                                          children: [
                                            Expanded(
                                              child: Text(
                                                task.title,
                                                style: TextStyle(
                                                  fontSize: 15,
                                                  fontWeight: FontWeight.bold,
                                                  color: task.isCompleted
                                                      ? Colors.grey
                                                      : Colors.white,
                                                  decoration: task.isCompleted
                                                      ? TextDecoration.lineThrough
                                                      : null,
                                                ),
                                              ),
                                            ),
                                            _PriorityBadge(priority: task.priority),
                                          ],
                                        ),
                                        const SizedBox(height: 6),
                                        Text(
                                          task.description,
                                          style: TextStyle(
                                            fontSize: 13,
                                            color: task.isCompleted
                                                ? Colors.grey.shade600
                                                : Colors.white70,
                                            decoration: task.isCompleted
                                                ? TextDecoration.lineThrough
                                                : null,
                                          ),
                                        ),
                                        const SizedBox(height: 8),
                                        Row(
                                          children: [
                                            Icon(Icons.folder_outlined,
                                                size: 14, color: Colors.grey.shade500),
                                            const SizedBox(width: 4),
                                            Text(
                                              task.category.label,
                                              style: TextStyle(
                                                fontSize: 11,
                                                color: Colors.grey.shade400,
                                              ),
                                            ),
                                            const Spacer(),
                                            IconButton(
                                              icon: const Icon(Icons.delete_outline, size: 20),
                                              color: Colors.redAccent.withOpacity(0.8),
                                              padding: EdgeInsets.zero,
                                              constraints: const BoxConstraints(),
                                              tooltip: 'Видалити завдання',
                                              onPressed: () => _deleteTask(task),
                                            ),
                                          ],
                                        ),
                                      ],
                                    ),
                                  ),
                                ],
                              ),
                            ),
                          ),
                        ),
                      );
                    },
                  ),
          ),
        ],
      ),
      floatingActionButton: FloatingActionButton.extended(
        onPressed: _openAddTaskDialog,
        backgroundColor: Colors.teal,
        foregroundColor: Colors.white,
        icon: const Icon(Icons.add),
        label: const Text('НОВЕ ЗАВДАННЯ', style: TextStyle(fontWeight: FontWeight.bold)),
      ),
    );
  }
}

/// Статистична картка віджету аналітики
class _StatCard extends StatelessWidget {
  final String label;
  final int count;
  final Color color;

  const _StatCard({
    required this.label,
    required this.count,
    required this.color,
  });

  @override
  Widget build(BuildContext context) {
    return Expanded(
      child: Container(
        padding: const EdgeInsets.symmetric(vertical: 8, horizontal: 10),
        decoration: BoxDecoration(
          color: const Color(0xFF1E232E),
          borderRadius: BorderRadius.circular(10),
          border: Border.all(color: color.withOpacity(0.3)),
        ),
        child: Column(
          children: [
            Text(
              '$count',
              style: TextStyle(fontSize: 18, fontWeight: FontWeight.bold, color: color),
            ),
            const SizedBox(height: 2),
            Text(
              label,
              style: const TextStyle(fontSize: 11, color: Colors.white60),
            ),
          ],
        ),
      ),
    );
  }
}

/// Бейдж пріоритету
class _PriorityBadge extends StatelessWidget {
  final TaskPriority priority;

  const _PriorityBadge({required this.priority});

  @override
  Widget build(BuildContext context) {
    return Container(
      padding: const EdgeInsets.symmetric(horizontal: 7, vertical: 3),
      decoration: BoxDecoration(
        color: priority.color.withOpacity(0.18),
        borderRadius: BorderRadius.circular(6),
        border: Border.all(color: priority.color.withOpacity(0.5)),
      ),
      child: Text(
        priority.label,
        style: TextStyle(
          color: priority.color,
          fontSize: 10,
          fontWeight: FontWeight.w600,
        ),
      ),
    );
  }
}
