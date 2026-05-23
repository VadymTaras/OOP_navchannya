/**
 * Кастомна функція каррінга для створення гнучких конфігурацій.
 * @param {Function} fn - Функція, яку треба каррувати.
 * @returns {Function}
 */
export const curry = (fn) => {
    return function curried(...args) {
        if (args.length >= fn.length) {
            return fn.apply(this, args);
        }
        return function(...args2) {
            return curried.apply(this, args.concat(args2));
        };
    };
};

/**
 * Асинхронний Promise-based пайплайн виконання функцій зліва направо.
 * Обробляє крайній випадок порожнього списку функцій.
 * @param {...Function} fns - Масив функцій обробки.
 * @returns {Function} Чиста функція, що приймає вхідні дані.
 */
export const pipeAsync = (...fns) => {
    return (input) => {
        if (fns.length === 0) return Promise.resolve(input);
        return fns.reduce(async (promise, currentFn) => {
            const val = await promise;
            return currentFn(val);
        }, Promise.resolve(input));
    };
};

/**
 * Функція вищого порядку для безпечної обробки помилок усередині пайплайну.
 * @param {Function} fn - Функція, виконання якої може викликати помилку.
 * @returns {Function}
 */
export const catchError = curry((fn, data) => {
    try {
        return fn(data);
    } catch (error) {
        console.error("Помилка виконання кроку:", error.message);
        return { ...data, error: error.message, failed: true };
    }
});

/**
 * Чиста функція для відстеження стану даних (Logging/Debugging) всередині пайплайну.
 * @param {string} message - Текст мітки для логу.
 * @returns {Function}
 */
export const tapLog = curry((message, data) => {
    const logMessage = "[" + new Date().toLocaleTimeString() + "] " + message;

    // Виклик кастомної події для передачі логів в UI без побічного ефекту для даних
    const event = new CustomEvent("pipeline-log", { detail: { logMessage, data: JSON.parse(JSON.stringify(data)) } });
    window.dispatchEvent(event);

    return data;
});

/**
 * Реалізація умовного розгалуження (Branching) в пайплайні.
 * @param {Function} predicate - Умова (повертає true/false).
 * @param {Function} trueFn - Функція, якщо true.
 * @param {Function} falseFn - Функція, якщо false.
 */
export const branch = curry((predicate, trueFn, falseFn, data) => {
    if (data.failed) return data;
    return predicate(data) ? trueFn(data) : falseFn(data);
});