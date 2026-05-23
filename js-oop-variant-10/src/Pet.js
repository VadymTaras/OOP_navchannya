/**
 * Базовий клас тварини.
 */
export class Pet {
    #id;

    constructor(name, species, age, price, photo, vaccinated = false) {
        this.#id = "pet-" + String(Math.floor(1000 + Math.random() * 9000));
        this.name = name || "Без імені";
        this.species = species;
        this.age = Number(age) || 1;
        this.price = Number(price) || 0;
        this.photo = photo || "https://images.unsplash.com/photo-1543466835-00a7907e9de1?w=500";
        this.vaccinated = vaccinated;
    }

    get id() {
        return this.#id;
    }

    getInfo() {
        return "Вид: " + this.species + ", Ім'я: " + this.name + ", Вік: " + this.age + " р.";
    }
}

/**
 * Клас Собака.
 */
export class Dog extends Pet {
    constructor(name, age, price, photo, vaccinated, breed, trained, size) {
        super(name, "dog", age, price, photo, vaccinated);
        this.breed = breed || "Метис";
        this.trained = !!trained;
        this.size = size || "medium";
    }

    getInfo() {
        const trainedStatus = this.trained ? "дресирований" : "не дресирований";

        const sizeMap = {
            small: "маленький",
            medium: "середній",
            large: "великий"
        };
        const ukrSize = sizeMap[this.size] || this.size;

        return super.getInfo() + " Порода: " + this.breed + " (" + trainedStatus + ", розмір: " + ukrSize + ")";
    }
}

/**
 * Клас Кіт.
 */
export class Cat extends Pet {
    constructor(name, age, price, photo, vaccinated, color, indoor) {
        super(name, "cat", age, price, photo, vaccinated);
        this.color = color || "Трьохколірний";
        this.indoor = !!indoor;
    }

    getInfo() {
        const homeStatus = this.indoor ? "домашній" : "вуличний";
        return super.getInfo() + " Забарвлення: " + this.color + " (" + homeStatus + ")";
    }
}

/**
 * Клас Птах.
 */
export class Bird extends Pet {
    constructor(name, age, price, photo, vaccinated, canTalk, cageSize) {
        super(name, "bird", age, price, photo, vaccinated);
        this.canTalk = !!canTalk;
        this.cageSize = cageSize || "medium";
    }

    getInfo() {
        const talkStatus = this.canTalk ? "розмовляє" : "не розмовляє";

        const cageMap = {
            small: "маленька",
            medium: "середня",
            large: "велика"
        };
        const ukrCage = cageMap[this.cageSize] || this.cageSize;

        return super.getInfo() + " Особливість: " + talkStatus + " (клітка: " + ukrCage + ")";
    }
}