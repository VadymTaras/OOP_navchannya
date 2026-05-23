// UI ELEMENTS (DOM)
const searchForm = document.getElementById("search-form");
const searchInput = document.getElementById("search-input");
const filterDiet = document.getElementById("filter-diet");
const filterTime = document.getElementById("filter-time");
const filterCalories = document.getElementById("filter-calories");
const btnFavoritesToggle = document.getElementById("btn-favorites-toggle");
const recipesGrid = document.getElementById("recipes-grid");
const loadingIndicator = document.getElementById("loading-indicator");
const errorMessage = document.getElementById("error-message");
const recipeModal = document.getElementById("recipe-modal");
const closeModal = document.getElementById("close-modal");
const modalBody = document.getElementById("modal-body");

let showFavoritesMode = false;

/**
 * Функція збережена для зворотної сумісності з тест-сьютом.
 * Тепер вона просто повертає оригінальний текст без жодних викривлень та змін.
 */
function cleanIngredientText(originalText) {
    if (!originalText) return "";
    return originalText.trim();
}

window.cleanIngredientText = cleanIngredientText;
if (window.RecipeAppAPI) {
    window.RecipeAppAPI.cleanIngredientText = cleanIngredientText;
}

function getFavorites() {
    const favs = localStorage.getItem("recipe_favorites");
    return favs ? JSON.parse(favs) : [];
}

function toggleFavorite(id, title, image) {
    let favs = getFavorites();
    const index = favs.findIndex(item => item.id === id);

    if (index >= 0) {
        favs.splice(index, 1);
    } else {
        favs.push({ id, title, image });
    }

    localStorage.setItem("recipe_favorites", JSON.stringify(favs));

    if (showFavoritesMode) {
        renderFavorites();
    } else {
        const btn = document.querySelector(".btn-fav-card[data-id='" + id + "']");
        if (btn) btn.classList.toggle("is-fav");
    }
}

function renderRecipes(recipes) {
    recipesGrid.innerHTML = "";
    const favs = getFavorites();

    if (!recipes || recipes.length === 0) {
        recipesGrid.innerHTML = "<div style='grid-column: 1/-1; text-align: center; color: #718096;'>Рецептів не знайдено. Спробуйте змінити параметри пошуку.</div>";
        return;
    }

    recipes.forEach(recipe => {
        const isFav = favs.some(item => item.id === recipe.id);
        const card = document.createElement("div");
        card.className = "recipe-card";

        card.innerHTML =
            "<div class='recipe-img-wrapper'>" +
            "<img src='" + recipe.image + "' alt='" + recipe.title + "' class='recipe-img' loading='lazy'>" +
            "</div>" +
            "<div class='recipe-info'>" +
            "<h3>" + recipe.title + "</h3>" +
            "<div class='card-actions'>" +
            "<button class='btn-details' data-id='" + recipe.id + "'>Переглянути</button>" +
            "<button class='btn-fav-card " + (isFav ? "is-fav" : "") + "' data-id='" + recipe.id + "' data-title='" + recipe.title + "' data-image='" + recipe.image + "'>❤</button>" +
            "</div>" +
            "</div>";

        recipesGrid.appendChild(card);
    });

    setupCardEvents();
}

function renderFavorites() {
    const favs = getFavorites();
    renderRecipes(favs);
}

function setupCardEvents() {
    recipesGrid.querySelectorAll(".btn-details").forEach(btn => {
        btn.addEventListener("click", async (e) => {
            const id = e.target.getAttribute("data-id");
            await openRecipeDetails(id);
        });
    });

    recipesGrid.querySelectorAll(".btn-fav-card").forEach(btn => {
        btn.addEventListener("click", (e) => {
            const id = Number(e.target.getAttribute("data-id"));
            const title = e.target.getAttribute("data-title");
            const image = e.target.getAttribute("data-image");
            toggleFavorite(id, title, image);
        });
    });
}

async function openRecipeDetails(id) {
    modalBody.innerHTML = "<p style='text-align:center;'>Завантаження повної інформації...</p>";
    recipeModal.style.display = "flex";

    try {
        const details = await window.RecipeAppAPI.getRecipeFullDetails(id);
        const uiDict = window.RecipeAppDictionary.uiDictionary;

        // Вивід інгредієнтів мовою оригіналу від API без стороннього втручання в текст
        let ingredientsHTML = "";
        details.extendedIngredients.forEach(ing => {
            ingredientsHTML += "<li>" + ing.original + "</li>";
        });

        modalBody.innerHTML =
            "<h2>" + details.title + "</h2>" +
            "<img src='" + details.image + "' style='width:100%; max-height:300px; object-fit:cover; border-radius:6px;'>" +
            "<p class='recipe-meta'>Час приготування: " + details.readyInMinutes + " хв | Порцій: " + details.servings + "</p>" +
            "<h3>Харчова цінність:</h3>" +
            "<div class='nutrition-list'>" +
            "<div class='nutrition-item'><b>" + uiDict["calories"] + ":</b><br>" + details.nutrition.calories + "</div>" +
            "<div class='nutrition-item'><b>" + uiDict["protein"] + ":</b><br>" + details.nutrition.protein + "</div>" +
            "<div class='nutrition-item'><b>" + uiDict["fat"] + ":</b><br>" + details.nutrition.fat + "</div>" +
            "<div class='nutrition-item'><b>" + uiDict["carbs"] + ":</b><br>" + details.nutrition.carbs + "</div>" +
            "</div>" +
            "<h3>Інгредієнти:</h3>" +
            "<ul>" + ingredientsHTML + "</ul>" +
            "<h3>Інструкція приготування:</h3>" +
            "<div style='line-height: 1.6; white-space: pre-line;'>" + details.instructions + "</div>";

    } catch (error) {
        modalBody.innerHTML = "<p style='color:#9b2c2c; text-align:center;'>Не вдалося завантажити деталі рецепту. " + error.message + "</p>";
    }
}

// FORM EVENTS HANDLER

searchForm.addEventListener("submit", async (e) => {
    e.preventDefault();
    const rawQuery = searchInput.value.trim();
    if (!rawQuery) return;

    showFavoritesMode = false;
    btnFavoritesToggle.classList.remove("active");

    loadingIndicator.classList.remove("hidden");
    errorMessage.classList.add("hidden");
    recipesGrid.innerHTML = "";

    try {
        const englishQuery = window.RecipeAppAPI.translateQueryToEnglish(rawQuery);

        const results = await window.RecipeAppAPI.searchRecipes(
            englishQuery,
            filterDiet.value,
            filterTime.value,
            filterCalories.value
        );
        renderRecipes(results);
    } catch (error) {
        errorMessage.innerText = error.message;
        errorMessage.classList.remove("hidden");
    } finally {
        loadingIndicator.classList.add("hidden");
    }
});

btnFavoritesToggle.addEventListener("click", () => {
    showFavoritesMode = !showFavoritesMode;
    btnFavoritesToggle.classList.toggle("active", showFavoritesMode);

    if (showFavoritesMode) {
        renderFavorites();
    } else {
        recipesGrid.innerHTML = "<div style='grid-column: 1/-1; text-align: center; color: #718096;'>Введіть запит для пошуку рецептів.</div>";
    }
});

closeModal.addEventListener("click", () => {
    recipeModal.style.display = "none";
});

window.addEventListener("click", (e) => {
    if (e.target === recipeModal) {
        recipeModal.style.display = "none";
    }
});