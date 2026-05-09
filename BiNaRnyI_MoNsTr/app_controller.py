from messages import Messages
from addition import Addition
from subtraction import Subtraction
from multiplication import Multiplication
from division import Division
from visualizer import Visualizer
from converter import Converter
from debugger import Debugger
from number_value import NumberValue


class AppController:
    """Мозок програми: керує станами, командами та помилками."""

    def __init__(self):
        self.vis, self.db = Visualizer(), Debugger()
        self.mode, self.is_running = "binary", True

    def _get_number(self, prompt):
        # Зчитуємо ввід та чистимо його від сміття (пробіли, невидимі символи)
        raw = input(prompt)
        clean = Converter.clean_input(raw, self.db)
        if not clean: raise ValueError(Messages.ERR_EMPTY)

        if self.mode == "binary":
            # Перевірка на відповідність двійковій системі
            if not all(c in '01' for c in clean): raise ValueError(Messages.ERR_NOT_BIN)
            return NumberValue(clean.lstrip('0') or '0', Converter.to_decimal(clean))
        else:
            return NumberValue(Converter.to_binary(int(clean)), int(clean))

    def handle_command(self, cmd):
        """Обробка вибору в меню з блокуючими вікнами помилок."""
        cmd = cmd.strip()
        if cmd == "0": self.is_running = False; return
        if cmd == "5": self.mode = "decimal" if self.mode == "binary" else "binary"; return
        if cmd == "6": self.db.toggle(); return

        # Якщо ввід не збігається з пунктами меню
        if cmd not in ["1", "2", "3", "4"]:
            err = Messages.ERR_NO_CHOICE if cmd == "" else Messages.ERR_INVALID_CMD.format(cmd)
            self.vis.display_error_block(err)
            return

        try:
            print(Messages.PROMPT_INPUT.format(self.mode.upper()))
            n1, n2 = self._get_number("   A -> "), self._get_number("   B -> ")
            ops = {"1": Addition, "2": Subtraction, "3": Multiplication, "4": Division}
            op = ops[cmd](n1, n2)

            # Виконання розрахунків
            if cmd == "4":
                q, r = op.execute();
                self.vis.show_steps(op.get_steps())
                print(Messages.RES_DIV.format(q, r, q.to_decimal(), r.to_decimal()))
            else:
                res = op.execute();
                self.vis.show_steps(op.get_steps())
                print(Messages.RES_GEN.format(res, res.to_decimal()))
            input(Messages.PRESS_ENTER)
        except Exception as e:
            # Будь-яка помилка зупиняє програму до натискання ENTER
            self.vis.display_error_block(str(e))

    def start(self):
        """Запуск нескінченного циклу програми."""
        while self.is_running:
            self.vis.clear()
            self.vis.display_header(self.mode, self.db)
            self.vis.display_menu()
            self.handle_command(input("\n [?] Твій вибір > "))