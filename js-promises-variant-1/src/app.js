import { checkAvailability, reserveItems, processPayment, scheduleDelivery } from "./order-processor.js";

const logList = document.getElementById("log-list");
const btnOrder = document.getElementById("btn-start-order");
const statusBadge = document.getElementById("current-status");

/**
 * Допоміжна чиста функція для виводу логів в UI.
 * @param {string} message - Текст логу.
 * @param {string} type - Тип логу (info, success, error).
 */
function logToUI(message, type = "info") {
    const li = document.createElement("li");
    li.textContent = "[" + new Date().toLocaleTimeString() + "] " + message;
    li.className = "log-" + type;
    logList.appendChild(li);
}

// Обробник натискання кнопки старту замовлення
btnOrder.addEventListener("click", () => {
    // Очищення попереднього стану перед новим запуском ланцюжка
    logList.innerHTML = "";
    btnOrder.disabled = true;
    statusBadge.textContent = "Обробка...";
    statusBadge.className = "status-badge status-processing";

    const currentOrderId = "ORD-" + String(Math.floor(100000 + Math.random() * 900000));
    logToUI("Ініціалізовано нове замовлення їжі: " + currentOrderId, "info");

    // ПОСЛІДОВНИЙ ЛАНЦЮЖОК ПРОМІСІВ (Пункт 5 вимог)
    checkAvailability(currentOrderId)
        .then((result) => {
            logToUI("Етап 1 пройдено успішно. Статус: " + result.status, "success");
            logToUI("Запуск етапу 2: Резервування товарів...", "info");
            return reserveItems(result.orderId);
        })
        .then((result) => {
            logToUI("Етап 2 пройдено успішно. Статус: " + result.status, "success");
            logToUI("Запуск етапу 3: Проведення транзакції оплати...", "info");
            // Фіксована сума передається далі
            return processPayment(result.orderId, 450);
        })
        .then((result) => {
            logToUI("Етап 3 пройдено успішно. Статус: " + result.status + " на суму " + String(result.amount) + " грн", "success");
            logToUI("Запуск етапу 4: Передача в службу логістики...", "info");
            return scheduleDelivery(result.orderId);
        })
        .then((result) => {
            logToUI("Етап 4 пройдено успішно. Статус: " + result.status, "success");
            logToUI("Замовлення успішно оброблено та готове до видачі!", "success");
            statusBadge.textContent = "Доставлено";
            statusBadge.className = "status-badge status-success";
        })
        // ЦЕНТРАЛІЗОВАНА ОБРОБКА ПОМИЛОК ЧЕРЕЗ .catch()
        .catch((error) => {
            logToUI("🚨 Переривання ланцюжка обробки: " + error.message, "error");
            statusBadge.textContent = "Скасовано";
            statusBadge.className = "status-badge status-error";
        })
        // ФІНАЛЬНИЙ МЕТОД ДЛЯ ОЧИЩЕННЯ СТАНУ
        .finally(() => {
            logToUI("Асинхронний процес обробки завершено.", "info");
            btnOrder.disabled = false;
        });
});