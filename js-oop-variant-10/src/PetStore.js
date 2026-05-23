import { Pet } from "./Pet.js";

/**
 * Клас управління зоомагазином.
 */
export class PetStore {
    #inventory;
    #salesHistory;
    #cashBalance;

    constructor(initialBalance = 1000) {
        this.#inventory = [];
        this.#salesHistory = [];
        this.#cashBalance = initialBalance;
    }

    addPet(pet) {
        this.#inventory.push(pet);
    }

    vaccinate(petId) {
        const pet = this.#inventory.find(p => p.id === petId);
        if (pet) {
            pet.vaccinated = true;
            return true;
        }
        return false;
    }

    #calculateFinalPrice(basePrice) {
        const taxRate = 0.10;
        return basePrice * (1 + taxRate);
    }

    sellPet(petId, buyer) {
        if (!buyer || buyer.trim() === "") return null;

        const petIndex = this.#inventory.findIndex(p => p.id === petId);
        if (petIndex === -1) return null;

        const pet = this.#inventory[petIndex];
        const finalPrice = this.#calculateFinalPrice(pet.price);

        const saleRecord = {
            saleId: "sale-" + String(Date.now()),
            petName: pet.name,
            species: pet.species,
            buyer: buyer,
            revenue: finalPrice,
            date: new Date()
        };

        this.#salesHistory.push(saleRecord);
        this.#cashBalance = this.#cashBalance + finalPrice;
        this.#inventory.splice(petIndex, 1);

        return saleRecord;
    }

    getCompatiblePets(species) {
        if (!species) return this.#inventory;
        return this.#inventory.filter(pet => pet.species === species);
    }

    static formatSpeciesName(species) {
        const map = { dog: "Собака", cat: "Кіт", bird: "Птах" };
        return map[species] || "Інший вид";
    }

    getStats() {
        return {
            inventoryCount: this.#inventory.length,
            salesCount: this.#salesHistory.length,
            totalRevenue: this.#salesHistory.reduce((sum, sale) => sum + sale.revenue, 0),
            balance: this.#cashBalance,
            history: [...this.#salesHistory]
        };
    }
}