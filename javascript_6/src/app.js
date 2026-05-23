import NotificationManager from "./NotificationManager.js";

// Максимум 3 нотифікації одночасно на екрані
const manager = new NotificationManager({ maxNotifications: 3, stacking: "bottom" });

document.getElementById("btn-generate").addEventListener("click", () => {
    const title = document.getElementById("notif-title").value;
    const message = document.getElementById("notif-message").value;
    const type = document.getElementById("notif-type").value;
    const position = document.getElementById("notif-position").value;
    const persistent = document.getElementById("notif-persistent").checked;
    const playSound = document.getElementById("notif-sound").checked;
    const hasActions = document.getElementById("notif-actions").checked;

    const options = { title, message, type, persistent, playSound };

    if (hasActions) {
        options.actions = [
            { text: "Скасувати", callback: () => alert("Дію скасовано!") },
            { text: "Деталі", callback: () => alert("Логи розгорнуто.") }
        ];
    }

    manager.createNotification(options, position);
});

document.getElementById("btn-test-queue").addEventListener("click", () => {
    const types = ["success", "error", "warning", "info"];
    const position = document.getElementById("notif-position").value;

    // Генерація 12 нотифікацій одночасно для тестування черги
    for (let i = 1; i <= 12; i = i + 1) {
        const currentType = types[i % 4];
        manager.createNotification({
            title: "Черга: Елемент " + String(i),
            message: "Тестове сповіщення під номером " + String(i),
            type: currentType,
            duration: 3000
        }, position);
    }
});