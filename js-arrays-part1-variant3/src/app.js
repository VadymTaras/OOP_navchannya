import TaskManager from "./TaskManager.js";

const manager = new TaskManager();

// Наповнення початковими демонстраційними даними
manager.addTask({ title: "Лабораторна №5", description: "Здати роботу з масивів JS", priority: "high", status: "in-progress", category: "Навчання", deadline: "2026-06-15" });
manager.addTask({ title: "Проєкт у CLion", description: "Написати систему класів ES6", priority: "medium", status: "todo", category: "Програмування", deadline: "2026-05-20" }); // Прострочена
manager.addTask({ title: "Купити продукти", description: "Фрукти, овочі, молоко", priority: "low", status: "done", category: "Побут", deadline: "2026-06-01" });

/**
 * Функція відображення списку завдань на сторінці інтерфейсу.
 * @param {Array} tasksList - Масив задач для рендерингу.
 */
function renderTasks(tasksList) {
    const container = document.getElementById("tasks-container");
    container.innerHTML = "";

    if (tasksList.length === 0) {
        container.innerHTML = "<div class='no-data'>Немає завдань, що відповідають критеріям</div>";
        return;
    }

    tasksList.forEach((task) => {
        const card = document.createElement("div");
        card.className = "task-card " + (task.isOverdue ? "task-overdue" : "");

        const overdueBadge = task.isOverdue ? "<span class='badge-overdue'>Прострочено!</span>" : "";
        const dateFormatted = task.deadline.toISOString().split("T")[0];

        card.innerHTML =
            "<div class='task-header'>" +
            "<h4>" + task.title + "</h4>" +
            "<div>" + overdueBadge + "<span class='priority-tag priority-" + task.priority + "'>" + task.priority + "</span></div>" +
            "</div>" +
            "<p class='task-desc'>" + task.description + "</p>" +
            "<div class='task-footer'>" +
            "<span>Категорія: <b>" + task.category + "</b></span>" +
            "<span>Статус: <b>" + task.status + "</b></span>" +
            "<span>Дедлайн: " + dateFormatted + "</span>" +
            "</div>";

        container.appendChild(card);
    });
}

/**
 * Оновлення інформаційних блоків аналітики та статистики.
 */
function updateStatsUI() {
    const stats = manager.getStats();
    document.getElementById("stat-total").innerText = stats.total;
    document.getElementById("stat-done").innerText = stats.done;
    document.getElementById("stat-overdue").innerText = stats.overdue;
    document.getElementById("stat-rate").innerText = stats.rate;
}

/**
 * Головна функція синхронізації та зчитування поточних фільтрів та сортування.
 */
function applyFiltersAndSort() {
    const status = document.getElementById("filter-status").value;
    const priority = document.getElementById("filter-priority").value;
    const category = document.getElementById("filter-category").value;
    const sortBy = document.getElementById("sort-by").value;

    let filtered = manager.filterTasks({ status, priority, category });
    let sorted = manager.sortTasks(sortBy, filtered);

    renderTasks(sorted);
}

// Обробник події створення нового завдання
document.getElementById("task-form").addEventListener("submit", (e) => {
    e.preventDefault();

    const title = document.getElementById("title").value;
    const description = document.getElementById("description").value;
    const priority = document.getElementById("priority").value;
    const status = document.getElementById("status").value;
    const category = document.getElementById("category").value;
    const deadline = document.getElementById("deadline").value;

    if (!title.trim() || !deadline) return;

    manager.addTask({ title, description, priority, status, category, deadline });

    e.target.reset();
    updateStatsUI();
    applyFiltersAndSort();
});

// Події зміни фільтрів та параметрів сортування
document.getElementById("filter-status").addEventListener("change", applyFiltersAndSort);
document.getElementById("filter-priority").addEventListener("change", applyFiltersAndSort);
document.getElementById("filter-category").addEventListener("input", applyFiltersAndSort);
document.getElementById("sort-by").addEventListener("change", applyFiltersAndSort);

// Подія пошуку в реальному часі (input)
document.getElementById("search-input").addEventListener("input", (e) => {
    const query = e.target.value;
    if (query.trim() === "") {
        applyFiltersAndSort();
    } else {
        const searchResults = manager.searchTasks(query);
        renderTasks(searchResults);
    }
});

// Первинна ініціалізація додатка при завантаженні сторінки
updateStatsUI();
applyFiltersAndSort();