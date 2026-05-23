const BASE_URL = "https://api.spoonacular.com";
const API_KEY = "ваш_реальний_токен_spoonacular";

window.RecipeAppAPI = {
    /**
     * Локальний переклад пошукового запиту
     */
    translateQueryToEnglish: function(query) {
        const trimmed = query.trim().toLowerCase();
        if (/^[A-Za-z0-9\s,.-]*$/.test(trimmed)) {
            return trimmed;
        }
        const dict = window.RecipeAppDictionary.localDictionary;
        const translated = dict[trimmed] || trimmed;
        console.log("Локальний переклад: '" + query + "' -> '" + translated + "'");
        return translated;
    },

    /**
     * Перевірка статусів відповідей сервера
     */
    validateResponse: function(response) {
        if (response.status === 402) {
            throw new Error("Вичерпано денний ліміт запитів до Spoonacular API (150 req/day).");
        }
        if (!response.ok) {
            throw new Error("Помилка сервера Spoonacular: Статус " + response.status);
        }
    },

    /**
     * Комплексний пошук рецептів з фільтрацією та кешуванням
     */
    searchRecipes: async function(query, diet = "", maxTime = "", maxCalories = "") {
        const cacheKey = "search_" + query + "_" + diet + "_" + maxTime + "_" + maxCalories;
        const cachedData = localStorage.getItem(cacheKey);

        if (cachedData) {
            return JSON.parse(cachedData);
        }

        if (!API_KEY || API_KEY === "YOUR_API_KEY") {
            throw new Error("Критична помилка: Вкажіть ваш робочий токен у змінну API_KEY всередині файлу src/api.js!");
        }

        let url = BASE_URL + "/recipes/complexSearch?apiKey=" + API_KEY + "&query=" + encodeURIComponent(query) + "&number=12";

        if (diet) url += "&diet=" + encodeURIComponent(diet);
        if (maxTime) url += "&maxReadyTime=" + Number(maxTime);
        if (maxCalories) url += "&maxCalories=" + Number(maxCalories);

        const response = await fetch(url);
        this.validateResponse(response);
        const data = await response.json();

        localStorage.setItem(cacheKey, JSON.stringify(data.results));
        return data.results;
    },

    /**
     * Паралельне отримання деталей та БЖУ за допомогою Promise.all
     */
    getRecipeFullDetails: async function(id) {
        const cacheKey = "detail_" + id;
        const cachedData = localStorage.getItem(cacheKey);

        if (cachedData) {
            return JSON.parse(cachedData);
        }

        const infoUrl = BASE_URL + "/recipes/" + id + "/information?apiKey=" + API_KEY;
        const nutritionUrl = BASE_URL + "/recipes/" + id + "/nutritionWidget.json?apiKey=" + API_KEY;

        const [infoRes, nutritionRes] = await Promise.all([
            fetch(infoUrl),
            fetch(nutritionUrl)
        ]);

        this.validateResponse(infoRes);
        this.validateResponse(nutritionRes);

        const infoData = await infoRes.json();
        const nutritionData = await nutritionRes.json();

        const fullDetails = {
            id: infoData.id,
            title: infoData.title,
            image: infoData.image,
            readyInMinutes: infoData.readyInMinutes,
            servings: infoData.servings,
            extendedIngredients: infoData.extendedIngredients || [],
            instructions: infoData.instructions || "Інструкції відсутні.",
            nutrition: {
                calories: nutritionData.calories,
                carbs: nutritionData.carbs,
                fat: nutritionData.fat,
                protein: nutritionData.protein
            }
        };

        localStorage.setItem(cacheKey, JSON.stringify(fullDetails));
        return fullDetails;
    }
};