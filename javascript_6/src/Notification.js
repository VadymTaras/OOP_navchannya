/**
 * Клас, що представляє окреме сповіщення на екрані.
 */
class Notification {
    /**
     * @param {Object} options - Конфігурація сповіщення.
     * @param {string} options.id - Унікальний ID.
     * @param {string} options.title - Заголовок.
     * @param {string} options.message - Повідомлення.
     * @param {string} [options.type='info'] - Тип: success, error, warning, info.
     * @param {number} [options.duration=5000] - Час показу у мс.
     * @param {boolean} [options.persistent=false] - Чи є сповіщення постійним.
     * @param {boolean} [options.playSound=false] - Чи відтворювати звук.
     * @param {Object[]} [options.actions=[]] - Масив кнопок дій [{text, callback}].
     * @param {function} options.onClose - Колбек-функція при закритті.
     */
    constructor(options) {
        this.id = options.id;
        this.title = options.title;
        this.message = options.message;
        this.type = options.type || "info";
        this.duration = options.duration || 5000;
        this.persistent = options.persistent || false;
        this.playSound = options.playSound || false;
        this.actions = options.actions || [];
        this.onClose = options.onClose;

        this.element = null;
        this.remainingTime = this.duration;
        this.startTime = null;
        this.timerId = null;
        this.progressBar = null;

        this.init();
    }

    /**
     * Ініціалізація та рендеринг елемента.
     */
    init() {
        this.element = document.createElement("div");
        this.element.className = "notification notification-" + this.type;
        this.element.id = this.id;

        const iconMap = {
            success: "✓",
            error: "✕",
            warning: "⚠",
            info: "ℹ"
        };

        let actionsHtml = "";
        if (this.actions.length > 0) {
            actionsHtml = "<div class='notification-actions'>";
            this.actions.forEach((action, index) => {
                actionsHtml = actionsHtml + "<button class='action-btn' data-index='" + index + "'>" + action.text + "</button>";
            });
            actionsHtml = actionsHtml + "</div>";
        }

        let progressHtml = "";
        if (!this.persistent) {
            progressHtml = "<div class='notification-progress'><div class='progress-bar'></div></div>";
        }

        this.element.innerHTML =
            "<div class='notification-content'>" +
            "<span class='notification-icon'>" + (iconMap[this.type] || "ℹ") + "</span>" +
            "<div class='notification-text'>" +
            "<div class='notification-title'>" + this.title + "</div>" +
            "<div class='notification-message'>" + this.message + "</div>" +
            "</div>" +
            "<button class='notification-close'>&times;</button>" +
            "</div>" +
            actionsHtml +
            progressHtml;

        this.progressBar = this.element.querySelector(".progress-bar");
        this.setupEvents();

        if (this.playSound) {
            this.playNotificationSound();
        }

        if (!this.persistent) {
            this.startTimer();
        }
    }

    /**
     * Налаштування обробників подій.
     */
    setupEvents() {
        const closeBtn = this.element.querySelector(".notification-close");
        closeBtn.addEventListener("click", () => this.dismiss());

        const actionButtons = this.element.querySelectorAll(".action-btn");
        actionButtons.forEach((btn) => {
            btn.addEventListener("click", (e) => {
                const index = Number.parseInt(e.target.getAttribute("data-index"));
                if (this.actions[index] && this.actions[index].callback) {
                    this.actions[index].callback();
                }
                this.dismiss();
            });
        });

        if (!this.persistent) {
            this.element.addEventListener("mouseenter", () => this.pauseTimer());
            this.element.addEventListener("mouseleave", () => this.resumeTimer());
        }
    }

    /**
     * Запуск таймера автозакриття.
     */
    startTimer() {
        this.startTime = Date.now();
        this.timerId = setTimeout(() => this.dismiss(), this.remainingTime);
        this.animateProgressBar();
    }

    /**
     * Призупинення таймера при наведенні курсора.
     */
    pauseTimer() {
        clearTimeout(this.timerId);
        this.remainingTime = this.remainingTime - (Date.now() - this.startTime);
        if (this.progressBar) {
            this.progressBar.style.animationPlayState = "paused";
        }
    }

    /**
     * Відновлення таймера після відведення курсора.
     */
    resumeTimer() {
        this.startTime = Date.now();
        this.timerId = setTimeout(() => this.dismiss(), this.remainingTime);
        if (this.progressBar) {
            this.progressBar.style.animationPlayState = "running";
        }
    }

    /**
     * Оновлення стилю прогрес-бару через CSS-анімацію.
     */
    animateProgressBar() {
        if (!this.progressBar) return;
        this.progressBar.style.animation = "none";
        // Тригер рефлоу для перезапуску анімації у браузері
        this.progressBar.offsetHeight;
        this.progressBar.style.animation = "notification-progress-anim " + String(this.remainingTime) + "ms linear forwards";
    }

    /**
     * Відтворення аудіосигналу (за допомогою Web Audio API).
     */
    playNotificationSound() {
        const audioContext = new (window.AudioContext || window.webkitAudioContext)();
        const oscillator = audioContext.createOscillator();
        const gainNode = audioContext.createGain();

        oscillator.connect(gainNode);
        gainNode.connect(audioContext.destination);

        oscillator.type = "sine";
        oscillator.frequency.setValueAtTime(587.33, audioContext.currentTime);
        gainNode.gain.setValueAtTime(0.05, audioContext.currentTime);

        oscillator.start();
        oscillator.stop(audioContext.currentTime + 0.1);
    }

    /**
     * Видалення сповіщення з екрану з анімацією згасання.
     */
    dismiss() {
        clearTimeout(this.timerId);
        this.element.classList.add("dismissed");
        this.element.addEventListener("animationend", () => {
            if (this.element.parentNode) {
                this.element.parentNode.removeChild(this.element);
            }
            this.onClose(this.id);
        });
    }
}

export default Notification;