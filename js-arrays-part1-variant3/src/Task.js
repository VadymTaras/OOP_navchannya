/**
 * Клас, що представляє модель окремого завдання.
 */
class Task {
    /**
     * @param {Object} data - Параметри завдання.
     * @param {string} data.id - Унікальний ідентифікатор.
     * @param {string} data.title - Назва.
     * @param {string} data.description - Опис.
     * @param {string} data.priority - Пріоритет (low, medium, high).
     * @param {string} data.status - Статус (todo, in-progress, done).
     * @param {string} data.category - Категорія (напр. Навчання, Робота).
     * @param {string|Date} data.deadline - Дата дедлайну (YYYY-MM-DD).
     */
    constructor({ id, title, description, priority, status, category, deadline }) {
        this.id = id || "task-" + String(Date.now()) + "-" + String(Math.floor(Math.random() * 1000));
        this.title = title || "Без назви";
        this.description = description || "";
        this.priority = ["low", "medium", "high"].includes(priority) ? priority : "medium";
        this.status = ["todo", "in-progress", "done"].includes(status) ? status : "todo";
        this.category = category || "Загальне";
        this.deadline = new Date(deadline);
        this.createdAt = new Date();
    }

    /**
     * Геттер автоматичної перевірки прострочення завдання.
     * @returns {boolean} True, якщо дедлайн минув і завдання не виконано (не в статусі done).
     */
    get isOverdue() {
        if (this.status === "done") return false;
        const today = new Date();
        // Скидаємо години для коректного порівняння лише дат
        today.setHours(0, 0, 0, 0);
        const deadlineCopy = new Date(this.deadline);
        deadlineCopy.setHours(0, 0, 0, 0);
        return deadlineCopy < today;
    }
}

export default Task;