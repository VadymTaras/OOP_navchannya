/**
 * Функція генерації випадкової помилки (20% шанс).
 * @returns {boolean} True, якщо сталась помилка.
 */
function hasRandomError() {
    return Math.random() < 0.20;
}

/**
 * Етап 1: Перевірка наявності товарів на складі.
 * @param {string} orderId - Ідентифікатор замовлення.
 * @returns {Promise} затримка 1000 мс.
 */
export function checkAvailability(orderId) {
    return new Promise((resolve, reject) => {
        if (!orderId || typeof orderId !== "string") {
            reject(new Error("Помилка валідації: Некоректний ID замовлення на етапі перевірки."));
            return;
        }

        setTimeout(() => {
            if (hasRandomError()) {
                reject(new Error("Товарів немає в наявності на складі для замовлення " + orderId));
            } else {
                // Передаємо суму замовлення на наступний етап (наприклад, 450 грн)
                resolve({ orderId: orderId, amount: 450, status: "В наявності" });
            }
        }, 1000);
    });
}

/**
 * Етап 2: Резервування товарів.
 * @param {string} orderId - Ідентифікатор замовлення.
 * @returns {Promise} затримка 1000 мс.
 */
export function reserveItems(orderId) {
    return new Promise((resolve, reject) => {
        if (!orderId) {
            reject(new Error("Помилка валідації: Відсутній ID для резервування."));
            return;
        }

        setTimeout(() => {
            if (hasRandomError()) {
                reject(new Error("Не вдалося зарезервувати товари (помилка бази даних) для замовлення " + orderId));
            } else {
                resolve({ orderId: orderId, status: "Зарезервовано" });
            }
        }, 1000);
    });
}

/**
 * Етап 3: Обробка та проведення оплати.
 * @param {string} orderId - Ідентифікатор замовлення.
 * @param {number} amount - Сума до сплати.
 * @returns {Promise} затримка 1500 мс.
 */
export function processPayment(orderId, amount) {
    return new Promise((resolve, reject) => {
        if (!orderId || !amount || amount <= 0) {
            reject(new Error("Помилка валідації: Некоректна сума або ID на етапі оплати."));
            return;
        }

        setTimeout(() => {
            if (hasRandomError()) {
                reject(new Error("Банк відхилив транзакцію оплати для замовлення " + orderId));
            } else {
                resolve({ orderId: orderId, amount: amount, status: "Оплачено" });
            }
        }, 1500);
    });
}

/**
 * Етап 4: Планування та передача в службу доставки.
 * @param {string} orderId - Ідентифікатор замовлення.
 * @returns {Promise} затримка 1000 мс.
 */
export function scheduleDelivery(orderId) {
    return new Promise((resolve, reject) => {
        if (!orderId) {
            reject(new Error("Помилка валідації: Відсутній ID для планування доставки."));
            return;
        }

        setTimeout(() => {
            if (hasRandomError()) {
                reject(new Error("Кур'єрська служба перевантажена, доставку не сплановано для замовлення " + orderId));
            } else {
                resolve({ orderId: orderId, status: "Передано кур'єру" });
            }
        }, 1000);
    });
}