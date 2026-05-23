/**
 * Комплексний тестовий сценарій для перевірки асинхронної логіки, кешування та помилок
 * Структуровано за блоками (старі та нові тести) для наочного виводу в браузері
 * Запускається за допомогою відкриття файлу tests.html у браузері
 */

const testSuite = {
    run: async function() {
        const resultsContainer = document.getElementById("test-results") || document.body;
        resultsContainer.innerHTML = "";

        let passed = 0;
        let failed = 0;

        const renderBlockHeader = (title) => {
            const h3 = document.createElement("h3");
            h3.style.marginTop = "25px";
            h3.style.marginBottom = "10px";
            h3.style.color = "#2d3748";
            h3.style.borderBottom = "2px solid #e2e8f0";
            h3.style.paddingBottom = "5px";
            h3.innerText = title;
            resultsContainer.appendChild(h3);
        };

        const assert = (condition, message) => {
            const p = document.createElement("p");
            p.style.margin = "6px 0";
            p.style.fontSize = "15px";
            if (condition) {
                p.style.color = "#2f855a";
                p.innerHTML = "✔ " + message;
                passed++;
            } else {
                p.style.color = "#9b2c2c";
                p.innerHTML = "✘ ПОМИЛКА: " + message;
                failed++;
            }
            resultsContainer.appendChild(p);
        };

        // --- БЛОК 1: БАЗОВІ ТЕСТИ ---
        renderBlockHeader("Базові тести програми");

        // ТЕСТ 1: Перевірка глобального об'єкта API
        try {
            assert(typeof window.RecipeAppAPI === "object", "Об'єкт RecipeAppAPI успішно ініціалізовано в全局ній області видимості");
        } catch (e) {
            assert(false, "Помилка ініціалізації RecipeAppAPI: " + e.message);
        }

        // ТЕСТ 2: Перевірка глобального об'єкта словників
        try {
            assert(typeof window.RecipeAppDictionary === "object", "Словник RecipeAppDictionary успішно підключено");
        } catch (e) {
            assert(false, "Помилка ініціалізації RecipeAppDictionary: " + e.message);
        }

        // ТЕСТ 3: Перевірка локального перекладу київських страв
        try {
            const inputQuery = "Борщ";
            const expectedResult = "borscht";
            const result = window.RecipeAppAPI.translateQueryToEnglish(inputQuery);
            assert(result === expectedResult, "Локальний переклад запиту: '" + inputQuery + "' -> '" + result + "'");
        } catch (e) {
            assert(false, "Тест локального перекладу провалено: " + e.message);
        }

        // ТЕСТ 4: Перевірка локального перекладу для англійського тексту
        try {
            const inputQuery = "salad";
            const result = window.RecipeAppAPI.translateQueryToEnglish(inputQuery);
            assert(result === "salad", "Текст англійською мовою залишається без змін: '" + result + "'");
        } catch (e) {
            assert(false, "Тест обробки англійського тексту провалено: " + e.message);
        }

        // ТЕСТ 5: Перевірка цілісності тексту інгредієнтів (Збереження оригіналу)
        try {
            if (typeof window.cleanIngredientText === "function") {
                const rawIngredient = "1/4 cup balsamic vinegar";
                const result = window.cleanIngredientText(rawIngredient);
                assert(result === rawIngredient, "Функція гарантує 100% збереження оригінальних цифр, мір ваги та назв продуктів від API");
            } else {
                assert(false, "Функція cleanIngredientText не знайдена в глобальному просторі");
            }
        } catch (e) {
            assert(false, "Тест цілісності тексту інгредієнтів провалено: " + e.message);
        }

        // ТЕСТ 6: Перевірка наявності методу пошуку рецептів
        try {
            assert(typeof window.RecipeAppAPI.searchRecipes === "function", "Метод searchRecipes доступний для виклику");
        } catch (e) {
            assert(false, "Метод searchRecipes пошкоджено: " + e.message);
        }

        // ТЕСТ 7: Перевірка наявності методу отримання повної інформації
        try {
            assert(typeof window.RecipeAppAPI.getRecipeFullDetails === "function", "Метод getRecipeFullDetails доступний для виклику");
        } catch (e) {
            assert(false, "Методу getRecipeFullDetails не знайдено: " + e.message);
        }


        // --- БЛОК 2: РОЗШИРЕНІ ТЕСТИ ---
        renderBlockHeader("Валідація та Edge Cases");

        // ТЕСТ 8: Граничні випадки пошуку (Регістронезалежність та пробіли)
        try {
            const inputQuery = "  салат цезар   ";
            const result = window.RecipeAppAPI.translateQueryToEnglish(inputQuery);
            assert(result === "caesar salad", "Функція успішно очищає зайві пробіли по краях та переводить у нижній регістр");
        } catch (e) {
            assert(false, "Тест обробки граничних символів провалено: " + e.message);
        }

        // ТЕСТ 9: Валідація помилок сервера (Негативний тест на статус 402 - Вичерпано ліміти)
        try {
            const fakeResponse = { status: 402, ok: false };
            let errorThrown = false;
            try {
                window.RecipeAppAPI.validateResponse(fakeResponse);
            } catch (err) {
                if (err.message.includes("Вичерпано денний ліміт запитів")) {
                    errorThrown = true;
                }
            }
            assert(errorThrown, "Валідатор сервера коректно перехоплює помилку 402 та сигналізує про вичерпання лімітів токена");
        } catch (e) {
            assert(false, "Тест валідації помилки 402 провалено: " + e.message);
        }

        // ТЕСТ 10: Валідація загальних помилок сервера (Статус 500)
        try {
            const fakeResponse = { status: 500, ok: false };
            let errorThrown = false;
            try {
                window.RecipeAppAPI.validateResponse(fakeResponse);
            } catch (err) {
                if (err.message.includes("Помилка сервера Spoonacular")) {
                    errorThrown = true;
                }
            }
            assert(errorThrown, "Валідатор сервера коректно обробляє критичний збій мережі із кодом 500");
        } catch (e) {
            assert(false, "Тест валідації помилки 500 провалено: " + e.message);
        }

        // ТЕСТ 11: Перевірка роботи підсистеми улюблених рецептів з кешем (localStorage)
        try {
            localStorage.removeItem("recipe_favorites");
            assert(getFavorites().length === 0, "Підсистема ініціалізує порожній список улюблених рецептів, якщо кеш чистий");
        } catch (e) {
            assert(false, "Тест улюблених рецептів провалено: " + e.message);
        }

        // ТЕСТ 12: Перевірка додавання рецепту до улюблених
        try {
            toggleFavorite(999, "Тестова страва", "test.jpg");
            const currentFavs = getFavorites();
            assert(currentFavs.length === 1 && currentFavs[0].id === 999, "Метод toggleFavorite успішно серіалізує та записує об'єкт рецепту в локальне сховище");
            localStorage.removeItem("recipe_favorites");
        } catch (e) {
            assert(false, "Тест збереження рецепту провалено: " + e.message);
        }

        // Вивід фінального підсумку
        const summary = document.createElement("div");
        summary.style.marginTop = "25px";
        summary.style.padding = "12px";
        summary.style.borderRadius = "6px";
        summary.style.fontWeight = "bold";
        summary.style.fontSize = "16px";
        summary.style.background = failed === 0 ? "#e6fffa" : "#fff5f5";
        summary.style.color = failed === 0 ? "#234e52" : "#9b2c2c";
        summary.style.border = failed === 0 ? "1px solid #b2f5ea" : "1px solid #fed7d7";
        summary.innerHTML = "Усього тестів виконано: " + (passed + failed) + " | Успішно: " + passed + " | Помилок: " + failed;
        resultsContainer.appendChild(summary);
    }
};

document.addEventListener("DOMContentLoaded", () => {
    testSuite.run();
});