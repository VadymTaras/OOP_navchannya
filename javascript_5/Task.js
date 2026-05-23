/**
 * Клас, що представляє окрему задачу в системі.
 */
class Task {
    /**
     * @param {Object} data - Дані для створення задачі.
     * @param {string} data.id - Унікальний ідентифікатор.
     * @param {string} data.title - Назва задачі.
     * @param {string} data.description - Опис задачі.
     * @param {string} data.status - Статус ('pending', 'completed').
     * @param {string} data.priority - Пріоритет ('low', 'medium', 'high').
     * @param {string|Date} data.dueDate - Дедлайн задачі.
     * @param {string[]} [data.tags=[]] - Масив тегів.
     */
    constructor({ id, title, description, status, priority, dueDate, tags = [] }) {
        this.id = id;
        this.title = title;
        this.description = description;
        this.status = status;
        this.priority = priority;
        this.dueDate = new Date(dueDate);
        this.tags = tags;
    }

    /**
     * Позначає задачу як виконану.
     */
    markComplete() {
        this.status = "completed";
    }

    /**
     * Позначає задачу як невиконану.
     */
    markIncomplete() {
        this.status = "pending";
    }

    /**
     * Додає новий тег до задачі, якщо його ще немає.
     * @param {string} tag - Назва тегу.
     */
    addTag(tag) {
        if (!this.tags.includes(tag)) {
            this.tags.push(tag);
        }
    }

    /**
     * Видаляє тег із задачі.
     * @param {string} tag - Назва тегу для видалення.
     */
    removeTag(tag) {
        this.tags = this.tags.filter((t) => t !== tag);
    }

    /**
     * Геттер, який перевіряє, чи протермінована задача.
     * @returns {boolean} True, якщо дедлайн минув і задача не виконана.
     */
    get isOverdue() {
        const today = new Date();
        return this.status !== "completed" && this.dueDate < today;
    }
}

export default Task;