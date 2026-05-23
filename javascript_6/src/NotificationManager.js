import Notification from "./Notification.js";

/**
 * Клас менеджера для контролю черги та позицій сповіщень.
 */
class NotificationManager {
    /**
     * @param {Object} config - Загальні налаштування менеджера.
     * @param {number} [config.maxNotifications=3] - Ліміт сповіщень на екрані.
     * @param {string} [config.stacking='bottom'] - Порядок стеку: top або bottom.
     */
    constructor(config = {}) {
        this.maxNotifications = config.maxNotifications || 3;
        this.stacking = config.stacking || "bottom";
        this.containers = {};
        this.activeNotifications = [];
        this.queue = [];
    }

    /**
     * Створює контейнер для певної позиції, якщо його немає.
     * @param {string} position - Позиція сповіщення.
     * @returns {HTMLElement}
     */
    getContainer(position) {
        if (this.containers[position]) {
            return this.containers[position];
        }

        const container = document.createElement("div");
        container.className = "notification-container position-" + position;
        document.body.appendChild(container);
        this.containers[position] = container;
        return container;
    }

    /**
     * Додає нове сповіщення або ставить його в чергу.
     * @param {Object} options - Опції сповіщення.
     * @param {string} [position='top-right'] - Позиція на екрані.
     */
    createNotification(options, position = "top-right") {
        const notificationData = {
            id: "notif-" + String(Date.now()) + "-" + String(Math.floor(Math.random() * 1000)),
            position: position,
            options: options
        };

        if (this.activeNotifications.length >= this.maxNotifications) {
            this.queue.push(notificationData);
        } else {
            this.displayNotification(notificationData);
        }
    }

    /**
     * Відображає сповіщення у відповідному контейнері.
     * @param {Object} data - Дані про сповіщення з id та позицією.
     */
    displayNotification(data) {
        const container = this.getContainer(data.position);

        const notification = new Notification({
            ...data.options,
            id: data.id,
            onClose: (id) => this.handleNotificationClose(id)
        });

        const instanceData = { id: data.id, instance: notification, position: data.position };

        if (this.stacking === "top") {
            container.insertBefore(notification.element, container.firstChild);
            this.activeNotifications.unshift(instanceData);
        } else {
            container.appendChild(notification.element);
            this.activeNotifications.push(instanceData);
        }
    }

    /**
     * Обробка події видалення сповіщення та запуск наступного з черги.
     * @param {string} id - Ідентифікатор видаленого сповіщення.
     */
    handleNotificationClose(id) {
        this.activeNotifications = this.activeNotifications.filter((n) => n.id !== id);

        if (this.queue.length > 0 && this.activeNotifications.length < this.maxNotifications) {
            const nextNotification = this.queue.shift();
            this.displayNotification(nextNotification);
        }
    }
}

export default NotificationManager;