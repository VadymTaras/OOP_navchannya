/**
 * Плагін аналітики даних
 */
export const analyticsPlugin = {
    id: "AnalyticsPlugin",
    privateApi: null,

    install(api) {
        // Зберігаємо надане API у внутрішньому замиканні об'єкта
        this.privateApi = api;
        this.privateApi.log("Плагін успішно інстальовано.");

        // Підписка на системну подію (тестування витоків пам'яті)
        this.privateApi.onEvent("click", () => {
            // Клік фіксується внутрішньо
        });
    },

    processData(data) {
        this.privateApi.log("Обробка масиву даних. Кількість елементів: " + String(data.length));
        return data.map(x => x * 2);
    },

    uninstall() {
        this.privateApi.log("Хук деінсталяції виконано.");
        this.privateApi = null;
    }
};

/**
 * Плагін генерації звітів
 */
export const reportPlugin = {
    id: "ReportPlugin",
    privateApi: null,

    install(api) {
        this.privateApi = api;
        this.privateApi.log("Плагін успішно інстальовано. Версія ядра додатка: " + this.privateApi.getAppVersion());
    },

    generate(title) {
        this.privateApi.log("Створення звіту з назвою: " + title);
        return "ЗВІТ: " + title.toUpperCase();
    },

    uninstall() {
        this.privateApi.log("Плагін успішно видалено з системи.");
        this.privateApi = null;
    }
};