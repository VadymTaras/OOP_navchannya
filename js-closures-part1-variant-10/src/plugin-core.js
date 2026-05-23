/**
 * Фабрична функція, що створює менеджер плагінів за допомогою замикань.
 * Усі змінні всередині є приватними та недоступними ззовні.
 */
export function createPluginManager() {
    // Приватні сховища (інкапсульовані в замиканні)
    const plugins = new Map();
    const sharedState = { appVersion: "1.0.0", executionCount: 0 };
    const eventListeners = new Map();

    /**
     * Приватний метод для генерації ізольованого API (Shared API через closure)
     */
    function createPluginAPI(pluginId) {
        return {
            log: (message) => {
                sharedState.executionCount = sharedState.executionCount + 1;
                const logMessage = "[" + pluginId + "] " + message;

                // Виклик події для UI
                const event = new CustomEvent("plugin-log", { detail: { logMessage, count: sharedState.executionCount } });
                window.dispatchEvent(event);
            },
            getAppVersion: () => sharedState.appVersion,
            // Метод для безпечного прослуховування подій
            onEvent: (eventType, callback) => {
                if (!eventListeners.has(pluginId)) {
                    eventListeners.set(pluginId, []);
                }
                window.addEventListener(eventType, callback);
                // Зберігаємо посилання для запобігання витокам пам'яті
                eventListeners.get(pluginId).push({ eventType, callback });
            }
        };
    }

    // Повертаємо публічний інтерфейс менеджера (Module Pattern)
    return {
        /**
         * Реєстрація та інсталяція плагіна (Lifecycle hook: install)
         */
        register: (plugin) => {
            if (!plugin || !plugin.id || typeof plugin.install !== "function") {
                console.error("Некоректна структура плагіна");
                return false;
            }

            if (plugins.has(plugin.id)) {
                return false;
            }

            const api = createPluginAPI(plugin.id);

            // Надання API через Dependency Injection (впровадження залежностей)
            plugin.install(api);
            plugins.set(plugin.id, plugin);
            return true;
        },

        /**
         * Виконання методу конкретного плагіна
         */
        execute: (pluginId, methodName, ...args) => {
            const plugin = plugins.get(pluginId);
            if (!plugin || typeof plugin[methodName] !== "function") {
                return null;
            }
            // Ізольований виклик у власному контексті
            return plugin[methodName](...args);
        },

        /**
         * Повне видалення плагіна та очищення пам'яті (Lifecycle hook: uninstall)
         */
        unregister: (pluginId) => {
            const plugin = plugins.get(pluginId);
            if (!plugin) return false;

            // Виклик хука життєвого циклу плагіна
            if (typeof plugin.uninstall === "function") {
                plugin.uninstall();
            }

            // Усунення витоків пам'яті: видаляємо всі слухачі подій, які створював цей плагін
            if (eventListeners.has(pluginId)) {
                const listeners = eventListeners.get(pluginId);
                listeners.forEach(({ eventType, callback }) => {
                    window.removeEventListener(eventType, callback);
                });
                eventListeners.delete(pluginId);
            }

            plugins.delete(pluginId);
            return true;
        },

        /**
         * Повертає список id зареєстрованих плагінів
         */
        getActivePlugins: () => Array.from(plugins.keys())
    };
}