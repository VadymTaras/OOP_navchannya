import tkinter as tk
from app_view import AppView
from app_controller import AppController
import logger_config

print("Старт скрипта...")

if __name__ == "__main__":
    print("Ввійшли в головний блок ініціалізації")

    # Створюємо корінь
    root = tk.Tk()
    print("Створено базове вікно Tkinter")

    # Ініт логера
    logger_config.init_logger(root)
    print("Логер успішно підключено")

    # Створюємо вигляд
    view = AppView(root)
    print("Інтерфейс завантажено в пам'ять")

    # Підключаємо логіку
    controller = AppController(view)
    print("Контролер підключено до кнопок")

    print("Запускаю нескінченний цикл mainloop...")
    root.mainloop()
    print("Програму штатно закрито")