import Task from "./Task.js";

/**
 * Клас для управління списком задач та генерації статистики.
 */
class TaskManager {
    constructor() {
        this.tasks = [];
    }

    /**
     * Створює та додає нову задачу.
     * @param {Object} data - Параметри задачі.
     * @returns {Task} Створений об'єкт задачі.
     */
    createTask(data) {
        const newTask = new Task(data);
        this.tasks.push(newTask);
        return newTask;
    }

    /**
     * Знаходить задачу за її унікальним ідентифікатором.
     * @param {string} id - Ідентифікатор задачі.
     * @returns {Task|null} Знайдена задача або null.
     */
    getTaskById(id) {
        const task = this.tasks.find((t) => t.id === id);
        return task || null;
    }

    /**
     * Оновлює параметри існуючої задачі за її ID.
     * @param {string} id - Ідентифікатор задачі.
     * @param {Object} data - Нові дані для оновлення.
     * @returns {Task|null} Оновлена задача або null.
     */
    updateTask(id, data) {
        const task = this.getTaskById(id);
        if (!task) return null;

        const { title, description, status, priority, dueDate, tags } = data;

        if (title !== undefined) task.title = title;
        if (description !== undefined) task.description = description;
        if (status !== undefined) task.status = status;
        if (priority !== undefined) task.priority = priority;
        if (dueDate !== undefined) task.dueDate = new Date(dueDate);
        if (tags !== undefined) task.tags = tags;

        return task;
    }

    /**
     * Перемикає статус задачі між 'pending' та 'completed'.
     * @param {string} id - Ідентифікатор задачі.
     * @returns {Task|null} Оновлена задача або null.
     */
    toggleTaskStatus(id) {
        const task = this.getTaskById(id);
        if (!task) return null;

        if (task.status === "completed") {
            task.markIncomplete();
        } else {
            task.markComplete();
        }
        return task;
    }

    /**
     * Видаляє задачу за її ID.
     * @param {string} id - Идентифікатор задачі.
     * @returns {boolean} True, якщо видалення успішне.
     */
    deleteTask(id) {
        const initialLength = this.tasks.length;
        this.tasks = this.tasks.filter((t) => t.id !== id);
        return this.tasks.length < initialLength;
    }

    /**
     * Видаляє всі задачі із заданим статусом.
     * @param {string} status - Статус задач для видалення.
     */
    deleteTasksByStatus(status) {
        this.tasks = this.tasks.filter((t) => t.status !== status);
    }

    /**
     * Повністю очищує список задач.
     */
    clearAllTasks() {
        this.tasks = [];
    }

    /**
     * Повертає задачі за вказаним статусом.
     * @param {string} status - Статус для фільтрації.
     * @returns {Task[]}
     */
    getTasksByStatus(status) {
        return this.tasks.filter((t) => t.status === status);
    }

    /**
     * Повертає задачі за вказаним пріоритетом.
     * @param {string} priority - Пріоритет для фільтрації.
     * @returns {Task[]}
     */
    getTasksByPriority(priority) {
        return this.tasks.filter((t) => t.priority === priority);
    }

    /**
     * Повертає всі протерміновані задачі.
     * @returns {Task[]}
     */
    getOverdueTasks() {
        return this.tasks.filter((t) => t.isOverdue);
    }

    /**
     * Повертає задачі, які містять певний тег.
     * @param {string} tag - Назва тегу.
     * @returns {Task[]}
     */
    getTasksByTag(tag) {
        return this.tasks.filter((t) => t.tags.includes(tag));
    }

    /**
     * Шукає задачі за збігом у назві або описі.
     * @param {string} query - Пошуковий запит.
     * @returns {Task[]}
     */
    searchTasks(query) {
        const lowerQuery = query.toLowerCase();
        return this.tasks.filter(
            (t) =>
                t.title.toLowerCase().includes(lowerQuery) ||
                t.description.toLowerCase().includes(lowerQuery)
        );
    }

    /**
     * Розраховує загальну статистику по задачах.
     * @returns {Object} Об'єкт зі статистичними даними.
     */
    getStats() {
        const total = this.tasks.length;
        const completed = this.getTasksByStatus("completed").length;
        const pending = this.getTasksByStatus("pending").length;
        const overdue = this.getOverdueTasks().length;

        return { total, completed, pending, overdue };
    }

    /**
     * Розраховує відсоток виконання задач.
     * @returns {string} Відсоток виконаних задач у форматі рядка.
     */
    getCompletionRate() {
        const total = this.tasks.length;
        if (total === 0) return "0%";
        const completed = this.getTasksByStatus("completed").length;
        const rate = (completed / total) * 100;
        return String(Number.parseInt(rate)) + "%";
    }

    /**
     * Групує задачі по датах дедлайнів (у форматі YYYY-MM-DD).
     * @returns {Object} Об'єкт, де ключі — дати, а значення — масиви задач.
     */
    getTasksByDueDate() {
        return this.tasks.reduce((groups, task) => {
            const dateKey = task.dueDate.toISOString().split("T")[0];
            if (!groups[dateKey]) {
                groups[dateKey] = [];
            }
            groups[dateKey].push(task);
            return groups;
        }, {});
    }
}

export default TaskManager;