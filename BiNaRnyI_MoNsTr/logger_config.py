import logging
import sys
import tkinter as tk
from tkinter import messagebox

def init_logger(root: tk.Tk) -> None:
    # Звичайне логування подій та помилок додатка
    logging.basicConfig(
        filename="app.log",
        level=logging.INFO,
        format="%(asctime)s [%(levelname)s] %(message)s",
        datefmt="%Y-%m-%d %H:%M:%S",
        encoding="utf-8"
    )

    def handle_sys_exception(exc_type, exc_value, exc_traceback):
        if issubclass(exc_type, KeyboardInterrupt):
            sys.__excepthook__(exc_type, exc_value, exc_traceback)
            return
        logging.critical("Невідловлена системна помилка:", exc_info=(exc_type, exc_value, exc_traceback))
        # Вивід помилки в консоль CLion
        sys.__excepthook__(exc_type, exc_value, exc_traceback)

    sys.excepthook = handle_sys_exception

    def handle_tk_exception(exc_type, exc_value, exc_traceback):
        logging.critical("Критичний збій в інтерфейсі UI:", exc_info=(exc_type, exc_value, exc_traceback))
        messagebox.showerror("Критичний збій", "У додатку сталася помилка. Детальний лог збережено в app.log")
        sys.__excepthook__(exc_type, exc_value, exc_traceback)

    root.report_callback_exception = handle_tk_exception
}