import tkinter as tk
from app_view import AppView
from app_controller import AppController
import logger_config

if __name__ == "__main__":
    root = tk.Tk()

    # Запуск перехоплювача помилок перед ініціалізацією візуальних компонентів
    logger_config.init_logger(root)

    view = AppView(root)
    controller = AppController(view)

    root.mainloop()