import TaskManager from "./TaskManager.js";

const manager = new TaskManager();

console.log("=== Створення нових задач ===");
const task1 = manager.createTask({
    id: "task-1",
    title: "Лабораторна з QA",
    description: "Написати тести",
    status: "pending",
    priority: "high",
    dueDate: "2026-05-20",
    tags: ["університет"]
});

manager.createTask({
    id: "task-2",
    title: "Закрити дедлайни JS",
    description: "Реалізувати систему CRUD",
    status: "completed",
    priority: "medium",
    dueDate: "2026-05-28",
    tags: ["програмування"]
});
console.log("Успішно створено задач: " + String(manager.tasks.length));


console.log("\n===  Пошук задачі за ID ===");
const found = manager.getTaskById("task-1");
if (found) {
    console.log("Знайдено задачу з ID 'task-1': " + found.title);
} else {
    console.log("Задачу не знайдено.");
}


console.log("\n=== Оновлення полів та перемикання статусу ===");
manager.updateTask("task-1", { priority: "low" });
console.log("Новий пріоритет task-1: " + task1.priority);

manager.toggleTaskStatus("task-1");
console.log("Статус task-1 після toggle: " + task1.status);


console.log("\n=== Пакетне видалення за статусом ===");
console.log("Кількість задач до видалення: " + String(manager.tasks.length));
manager.deleteTasksByStatus("completed");
console.log("Кількість задач після видалення виконаних: " + String(manager.tasks.length));


console.log("\n=== Очищення всього репозиторію ===");
manager.clearAllTasks();
console.log("Репозиторій повністю очищено. Задач залишилось: " + String(manager.tasks.length));