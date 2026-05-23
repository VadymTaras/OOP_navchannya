import { createPluginManager } from "./plugin-core.js";
import { analyticsPlugin, reportPlugin } from "./plugins.js";

const manager = createPluginManager();

const logList = document.getElementById("log-list");
const execCounter = document.getElementById("exec-count");
const activePluginsList = document.getElementById("active-plugins");

// Слухач подій для виводу логів з ізольованих замикань в UI
window.addEventListener("plugin-log", (e) => {
    const li = document.createElement("li");
    li.textContent = e.detail.logMessage;
    logList.appendChild(li);
    execCounter.textContent = e.detail.count;
});

function updatePluginsUI() {
    const active = manager.getActivePlugins();
    if (active.length === 0) {
        activePluginsList.innerHTML = "<li>Немає активних плагінів</li>";
        return;
    }
    activePluginsList.innerHTML = "";
    active.forEach(id => {
        const li = document.createElement("li");
        li.textContent = id;
        activePluginsList.appendChild(li);
    });
}

// Обробники подій для кнопок інтерфейсу
document.getElementById("btn-reg-analytics").addEventListener("click", () => {
    manager.register(analyticsPlugin);
    updatePluginsUI();
});

document.getElementById("btn-unreg-analytics").addEventListener("click", () => {
    manager.unregister("AnalyticsPlugin");
    updatePluginsUI();
});

document.getElementById("btn-exec-analytics").addEventListener("click", () => {
    manager.execute("AnalyticsPlugin", "processData", [10, 20, 30]);
});

document.getElementById("btn-reg-report").addEventListener("click", () => {
    manager.register(reportPlugin);
    updatePluginsUI();
});

document.getElementById("btn-unreg-report").addEventListener("click", () => {
    manager.unregister("ReportPlugin");
    updatePluginsUI();
});

document.getElementById("btn-exec-report").addEventListener("click", () => {
    manager.execute("ReportPlugin", "generate", "Лабораторна з замикань");
});

// Початкове оновлення UI
updatePluginsUI();