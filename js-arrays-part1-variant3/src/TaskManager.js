import Task from "./Task.js";

/**
 * Клас управління колекцією завдань за допомогою методів масивів.
 */
class TaskManager {
    constructor() {
        this.tasks = [];
    }

    /**
     * Додає нове завдання до масиву.
     * @param {Object} data - Дані завдання.
     * @returns {Task} Створений об'єкт завдання.
     */
    addTask(data) {
        if (!data || typeof data !== "object") {
            throw new Error("Некоректні дані для створення завдання");
        }
        const task = new Task(data);
        this.tasks.push(task);
        return task;
    }

    /**
     * Повертає відфільтрований масив за критеріями.
     * Обробляє крайній випадок порожніх фільтрів.
     */
    filterTasks({ status, priority, category }) {
        return this.tasks.filter((task) => {
            const matchStatus = !status || task.status === status;
            const matchPriority = !priority || task.priority === priority;
            const matchCategory = !category || task.category.toLowerCase() === category.toLowerCase();
            return matchStatus && matchPriority && matchCategory;
        });
    }

    /**
     * Сортує переданий або внутрішній масив завдань.
     * @param {string} sortBy - Поле сортування (priority, deadline, createdAt).
     * @param {Task[]} [customList] - Альтернативний масив для сортування.
     */
    sortTasks(sortBy, customList = null) {
        const list = customList ? [...customList] : [...this.tasks];

        // Карта ваги пріоритетів для числового порівняння
        const priorityWeight = { high: 3, medium: 2, low: 1 };

        return list.sort((a, b) => {
            if (sortBy === "priority") {
                return priorityWeight[b.priority] - priorityWeight[a.priority]; // Від найвищого до найнижчого
            }
            if (sortBy === "deadline") {
                return a.deadline - b.deadline; // Від найближчого дедлайну
            }
            if (sortBy === "createdAt") {
                return b.createdAt - a.createdAt; // Спочатку новіші
            }
            return 0;
        });
    }

    /**
     * Контекстний пошук за частковим збігом у назві або описі (case-insensitive).
     */
    searchTasks(query) {
        if (!query || typeof query !== "string" || query.trim() === "") return [];
        const lowerQuery = query.toLowerCase().trim();
        return this.tasks.filter(
            (task) =>
                task.title.toLowerCase().includes(lowerQuery) ||
                task.description.toLowerCase().includes(lowerQuery)
        );
    }

    /**
     * Розрахунок кількісної аналітичної статистики та прострочених завдань.
     */
    getStats() {
        const total = this.tasks.length;
        if (total === 0) {
            return { total: 0, todo: 0, inProgress: 0, done: 0, overdue: 0, rate: "0%" };
        }

        const todo = this.tasks.filter((t) => t.status === "todo").length;
        const inProgress = this.tasks.filter((t) => t.status === "in-progress").length;
        const done = this.tasks.filter((t) => t.status === "done").length;
        const overdue = this.tasks.filter((t) => t.isOverdue).length;

        const rate = String(Number.parseInt((done / total) * 100)) + "%";

        return { total, todo, inProgress, done, overdue, rate };
    }

    /**
     * Групує поточний масив задач за унікальними категоріями.
     * @returns {Object} Об'єкт вигляду { "Категорія": [tasks] }
     */
    groupTasksByCategory() {
        return this.tasks.reduce((groups, task) => {
            const cat = task.category;
            if (!groups[cat]) {
                groups[cat] = [];
            }
            groups[cat].push(task);
            return groups;
        }, {});
    }
}

export default TaskManager;