import os
from messages import Messages


class Visualizer:
    """Модуль візуалізації: малює меню та великі блоки помилок."""

    def clear(self):
        # Очищення консолі (Windows/Unix)
        os.system("cls" if os.name == "nt" else "clear")

    def display_header(self, mode, debugger):
        status = "ON" if debugger.is_active else "OFF"
        print("============================================")
        print("|             БіНаРниЙ МоНсТр              |")
        print(f"|  РЕЖИМ: {mode.upper():<10} | DEBUG: {status:<5}  |")
        print("============================================")

    def display_menu(self):
        print("| [1] Додавання (+)    | [2] Віднімання (-) |")
        print("| [3] Множення (*)     | [4] Ділення (/)    |")
        print("--------------------------------------------")
        print("| [5] ЗМІНИТИ РЕЖИМ    | [6] DEBUG ON/OFF   |")
        print("| [0] ВИХІД            |                    |")
        print("============================================")

    def display_error_block(self, message):
        """Блокуюче вікно помилки: зупиняє все до натискання ENTER."""
        print("\n" + "!" * 44)
        print("!!" + " " * 12 + " УВАГА! ПОМИЛКА " + " " * 12 + "!!")
        print("!" * 44)
        print(f"  Повідомлення: {message}")
        print("!" * 44)
        # Яскрава інструкція для повернення
        print(Messages.RETURN_TO_MENU)
        input(" ------------------------------------------")

    def show_steps(self, steps):
        print("\n[ КРОКИ ОБЧИСЛЕННЯ ]")
        for i, s in enumerate(steps):
            res_val = f" -> Поточний стан: {s['result']}" if s['result'] else ""
            print(f"{i + 1}. {s['description']}{res_val}")