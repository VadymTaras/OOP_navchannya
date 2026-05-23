import { pipeAsync, tapLog, branch, catchError } from "./fp-core.js";
import { extractAndValidate, transformPrices, filterPremiumItems, keepAllItems, aggregateAnalytics } from "./etl-processors.js";

// Початковий набір сирих демонстраційних даних із помилками для перевірки крайніх випадків
const rawInputData = [
    { id: "p1", title: "Ноутбук CLion", price: 1000, category: "Електроніка" },
    { id: "p2", title: "Мишка", price: 50, category: "Електроніка" },
    { id: "p3", title: "Крісло", price: 400, category: "Меблі" },
    { id: null, title: "Зламаний товар", price: 0 }, // Некоректні дані
    { id: "p4", title: "Клавіатура", price: 120, category: "Електроніка" }
];

/**
 * Композиція асинхронного ETL Пайплайну на основі чистих функцій
 */
const runETLPipeline = (data, isPremiumMode) => {
    const discount = 0.10; // 10%
    const tax = 0.20;      // 20% ПДВ

    const pipeline = pipeAsync(
        tapLog("Старт процесу Extraction & Validation"),
        catchError(extractAndValidate),

        tapLog("Запуск Transformation (Карроване обчислення цін з урахуванням знижок та ПДВ)"),
        catchError(transformPrices(discount, tax)),

        tapLog("Перевірка умовного розгалуження (Branching)"),
        branch(
            () => isPremiumMode,
            catchError(filterPremiumItems),
            catchError(keepAllItems)
        ),

        tapLog("Запуск фінальної агрегації даних (Aggregation/Loading)"),
        catchError(aggregateAnalytics),

        tapLog("Пайплайн успішно завершено")
    );

    return pipeline(data);
};

// Обробка виводу логів в інтерфейс програми
const logList = document.getElementById("log-list");
const resultBlock = document.getElementById("result-output");

window.addEventListener("pipeline-log", (e) => {
    const li = document.createElement("li");
    li.textContent = e.detail.logMessage;
    logList.appendChild(li);
});

// Обробники подій натискання на кнопки інтерфейсу
document.getElementById("btn-run-standard").addEventListener("click", async () => {
    logList.innerHTML = "";
    resultBlock.textContent = "Обробка...";

    const result = await runETLPipeline(rawInputData, false);
    resultBlock.textContent = JSON.stringify(result, null, 2);
});

document.getElementById("btn-run-premium").addEventListener("click", async () => {
    logList.innerHTML = "";
    resultBlock.textContent = "Обробка...";

    const result = await runETLPipeline(rawInputData, true);
    resultBlock.textContent = JSON.stringify(result, null, 2);
});