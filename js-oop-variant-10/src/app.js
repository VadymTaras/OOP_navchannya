import { Dog, Cat, Bird } from "./Pet.js";
import { PetStore } from "./PetStore.js";

// Ініціалізація магазину з початковим балансом
const store = new PetStore(5000);

// Базове наповнення каталогу тваринами
store.addPet(new Dog("Рекс", 2, 1200, "https://images.unsplash.com/photo-1589941013453-ec89f33b5e95?w=500", true, "Німецька вівчарка", true, "large"));
store.addPet(new Dog("Біллі", 1, 800, "https://images.unsplash.com/photo-1548199973-03cce0bbc87b?w=500", false, "Коргі", false, "small"));
store.addPet(new Cat("Мурчик", 3, 600, "https://images.unsplash.com/photo-1514888286974-6c03e2ca1dba?w=500", true, "Сірий", true));
store.addPet(new Cat("Соня", 4, 450, "https://images.unsplash.com/photo-1533738363-b7f9aef128ce?w=500", false, "Білий", false));
store.addPet(new Bird("Кеша", 1, 350, "https://images.unsplash.com/photo-1452570053594-1b985d6ea890?w=500", false, true, "small"));

// Отримання посилань на елементи сторінки
const catalogContainer = document.getElementById("catalog-container");
const historyList = document.getElementById("history-list");

const filterSpecies = document.getElementById("filter-species");
const filterBreed = document.getElementById("filter-breed");
const filterAge = document.getElementById("filter-age");

const purchaseModal = document.getElementById("purchase-modal");
const purchaseForm = document.getElementById("purchase-form");
const selectedPetIdInput = document.getElementById("selected-pet-id");
const selectedPetNameSpan = document.getElementById("selected-pet-name");
const cancelPurchaseBtn = document.getElementById("btn-cancel-purchase");

// Функція виведення карток тварин з урахуванням фільтрації
function renderCatalog() {
    const selectedSpecies = filterSpecies.value;
    const breedQuery = filterBreed.value.trim().toLowerCase();
    const maxAge = filterAge.value ? Number(filterAge.value) : null;

    let pets = store.getCompatiblePets(selectedSpecies);

    if (breedQuery) {
        pets = pets.filter(pet => {
            const breed = pet.breed ? pet.breed.toLowerCase() : "";
            const color = pet.color ? pet.color.toLowerCase() : "";
            return breed.includes(breedQuery) || color.includes(breedQuery);
        });
    }

    if (maxAge !== null) {
        pets = pets.filter(pet => pet.age <= maxAge);
    }

    catalogContainer.innerHTML = "";

    if (pets.length === 0) {
        catalogContainer.innerHTML = "<div class='no-data' style='grid-column: 1/-1; text-align: center; color: #718096; padding: 20px;'>Тварин із такими параметрами не знайдено</div>";
        return;
    }

    pets.forEach(pet => {
        const card = document.createElement("div");
        card.className = "pet-card";

        const ukrSpecies = PetStore.formatSpeciesName(pet.species);

        card.innerHTML =
            "<div class='pet-photo-wrapper'>" +
            "<img src='" + pet.photo + "' alt='" + pet.name + "' class='pet-photo-img'>" +
            "</div>" +
            "<h4>" + pet.name + " " + (pet.vaccinated ? "<span class='vac-badge'>✓ Вакцин.</span>" : "<span class='vac-badge not-vac'>Ні</span>") + "</h4>" +
            "<p class='pet-details'>" + pet.getInfo().replace("Вид: " + pet.species, "Вид: " + ukrSpecies) + "</p>" +
            "<div class='pet-price-row'>" +
            "<span class='pet-price'>" + String(pet.price) + " грн</span>" +
            "<div class='card-actions'>" +
            (!pet.vaccinated ? "<button class='btn-vac' data-id='" + pet.id + "'>Вакцинувати</button>" : "") +
            "<button class='btn-buy' data-id='" + pet.id + "' data-name='" + pet.name + "'>Купити</button>" +
            "</div>" +
            "</div>";

        catalogContainer.appendChild(card);
    });

    setupCardEvents();
}

// Оновлення фінансових показників та історії операцій
function updateStatsUI() {
    const stats = store.getStats();
    document.getElementById("stat-count").innerText = stats.inventoryCount;
    document.getElementById("stat-sales").innerText = stats.salesCount;
    document.getElementById("stat-balance").innerText = stats.balance.toFixed(2) + " грн";

    historyList.innerHTML = "";
    if (stats.history.length === 0) {
        historyList.innerHTML = "<li>Історія продажів порожня</li>";
        return;
    }

    stats.history.forEach(sale => {
        const li = document.createElement("li");
        const formattedSpecies = PetStore.formatSpeciesName(sale.species);
        li.innerHTML = "Покупець: <b>" + sale.buyer + "</b> придбав тварину " + sale.petName + " (" + formattedSpecies + ") за <b>" + sale.revenue.toFixed(2) + " грн</b>";
        historyList.appendChild(li);
    });
}

// Прив'язка подій до кнопок всередині динамічних карток
function setupCardEvents() {
    catalogContainer.querySelectorAll(".btn-vac").forEach(btn => {
        btn.addEventListener("click", (e) => {
            const petId = e.target.getAttribute("data-id");
            store.vaccinate(petId);
            renderCatalog();
        });
    });

    catalogContainer.querySelectorAll(".btn-buy").forEach(btn => {
        btn.addEventListener("click", (e) => {
            const petId = e.target.getAttribute("data-id");
            const petName = e.target.getAttribute("data-name");

            selectedPetIdInput.value = petId;
            selectedPetNameSpan.innerText = '"' + petName + '"';
            purchaseModal.style.display = "flex";
        });
    });
}

// Обробка відправки форми покупки з трифакторною валідацією
purchaseForm.addEventListener("submit", (e) => {
    e.preventDefault();

    const lastName = document.getElementById("buyer-lastname").value.trim();
    const firstName = document.getElementById("buyer-firstname").value.trim();
    const middleName = document.getElementById("buyer-middlename").value.trim();
    const petId = selectedPetIdInput.value;

    if (!lastName || !firstName || !middleName) {
        alert("Критична помилка валідації: Всі три поля ПІБ мають бути коректно заповнені!");
        return;
    }

    const fullBuyerName = lastName + " " + firstName + " " + middleName;

    const success = store.sellPet(petId, fullBuyerName);
    if (success) {
        purchaseModal.style.display = "none";
        purchaseForm.reset();
        renderCatalog();
        updateStatsUI();
    }
});

// Закриття модального вікна покупки
cancelPurchaseBtn.addEventListener("click", () => {
    purchaseModal.style.display = "none";
});

// Слухачі подій для полів фільтрації
filterSpecies.addEventListener("change", renderCatalog);
filterBreed.addEventListener("input", renderCatalog);
filterAge.addEventListener("input", renderCatalog);

// Первинний виклик функцій для відображення інтерфейсу
renderCatalog();
updateStatsUI();