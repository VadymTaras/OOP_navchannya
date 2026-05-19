import logging
import tkinter as tk
from tkinter import messagebox
import json
import os

from number_value import NumberValue
from converter import Converter
import addition
import subtraction
import multiplication
import division
import bitwise_ops
from app_view import AppView

class AppController:
    # Сполучний контролер між інтерфейсом UI та обчисленнями
    def __init__(self, view: AppView) -> None:
        self.view = view
        self.current_theme = "cyberpunk"
        self.is_binary_mode = True

        self._setup_bindings()

        self.view.update_colors(self.current_theme)
        self.view.board.draw_placeholder(self.current_theme)
        logging.info("Контролер успішно ініціалізовано")

    def _setup_bindings(self) -> None:
        self.view.header_btns["toggle_mode"].config(command=self._toggle_mode)
        self.view.header_btns["toggle_theme"].config(command=self._toggle_theme)
        self.view.header_btns["show_history"].config(command=self._show_history)
        self.view.header_btns["clear_history"].config(command=self._clear_history)

        for op, btn in self.view.op_buttons.items():
            btn.config(command=lambda o=op: self._on_operation_click(o))

    def _toggle_mode(self) -> None:
        self.is_binary_mode = not self.is_binary_mode
        mode_str = "BINARY" if self.is_binary_mode else "DECIMAL"
        self.view.mode_label.config(text=f"РЕЖИМ: {mode_str}")
        logging.info(f"Змінено режим обчислень на {mode_str}")

    def _toggle_theme(self) -> None:
        theme_keys = list(self.view.themes.keys())
        try:
            current_idx = theme_keys.index(self.current_theme)
            next_idx = (current_idx + 1) % len(theme_keys)
        except ValueError:
            next_idx = 0

        self.current_theme = theme_keys[next_idx]
        self.view.update_colors(self.current_theme)

        if self.view.board.last_data:
            self.view.board.draw_canvas(self.view.board.last_data, self.current_theme)
        else:
            self.view.board.draw_placeholder(self.current_theme)

        logging.info(f"Змінено кольорову тему додатка на {self.current_theme}")

    def _show_history(self) -> None:
        logging.info("Запит відображення історії операцій")
        self.view.steps_area.config(state="normal")
        self.view.steps_area.delete("1.0", "end")

        if os.path.exists("history.json"):
            try:
                with open("history.json", "r", encoding="utf-8") as f:
                    history_data = json.load(f)
                if not history_data:
                    self.view.steps_area.insert("end", "Історія обчислень порожня\n", "info")
                for idx, entry in enumerate(history_data):
                    op = entry.get("op", "?")
                    a = entry.get("a", "?")
                    b = entry.get("b", "?")
                    result = entry.get("result", "?")

                    prefix = f"{idx + 1}. Операція [{op}] A: {a}, B: {b} | "
                    self.view.steps_area.insert("end", prefix, "info")
                    self.view.steps_area.insert("end", f"{result}\n", "result")
            except Exception as e:
                self.view.steps_area.insert("end", f"Помилка читання історії: {str(e)}\n", "error")
        else:
            self.view.steps_area.insert("end", "Файл історії не знайдено\n", "info")
        self.view.steps_area.config(state="disabled")

    def _clear_history(self) -> None:
        logging.info("Запит на очищення історії")
        try:
            with open("history.json", "w", encoding="utf-8") as f:
                json.dump([], f)
            self.view.steps_area.config(state="normal")
            self.view.steps_area.delete("1.0", "end")
            self.view.steps_area.insert("end", "Історію успішно очищено\n", "info")
            self.view.steps_area.config(state="disabled")
            messagebox.showinfo("Успіх", "Історію обчислень було повністю очищено")
        except Exception as e:
            logging.error(f"Не вдалося очистити історію: {str(e)}")

    def _save_to_history_file(self, op: str, a: str, b: str, res_txt: str) -> None:
        entry = {"op": op, "a": a, "b": b, "result": res_txt.replace("\n", " ")}
        history_data = []
        if os.path.exists("history.json"):
            try:
                with open("history.json", "r", encoding="utf-8") as f:
                    history_data = json.load(f)
            except Exception:
                history_data = []
        history_data.append(entry)
        try:
            with open("history.json", "w", encoding="utf-8") as f:
                json.dump(history_data, f, ensure_ascii=False, indent=4)
        except Exception as e:
            logging.error(f"Помилка запису в історію: {str(e)}")

    def _on_operation_click(self, op: str) -> None:
        a_txt = self.view.entry_a.get().strip()
        b_txt = self.view.entry_b.get().strip()

        logging.info(f"Запущено розрахунок операції {op} для значень A='{a_txt}', B='{b_txt}'")

        if self.is_binary_mode:
            clean_a = Converter.clean_input(a_txt, "binary", None)
            clean_b = Converter.clean_input(b_txt, "binary", None) if op != "~" else "00000000"
            if not clean_a or not clean_b:
                messagebox.showerror("Помилка", "Некоректний формат двійкових чисел")
                return
            num_a = NumberValue(clean_a)
            num_b = NumberValue(clean_b) if op != "~" else None
        else:
            clean_a = Converter.clean_input(a_txt, "decimal", None)
            clean_b = Converter.clean_input(b_txt, "decimal", None) if op != "~" else "0"
            if not clean_a or not clean_b:
                messagebox.showerror("Помилка", "Число має бути в діапазоні від -128 до 127")
                return
            num_a = NumberValue(Converter.to_binary(int(clean_a)), int(clean_a))
            num_b = NumberValue(Converter.to_binary(int(clean_b)), int(clean_b)) if op != "~" else None

        try:
            if op == "+":
                op_obj = addition.Addition(num_a, num_b)
                res_num = op_obj.execute()
                result_text = f"Значення: {res_num.to_decimal()}\nBIN: {res_num.binary_string}"
            elif op == "-":
                op_obj = subtraction.Subtraction(num_a, num_b)
                res_num = op_obj.execute()
                result_text = f"Значення: {res_num.to_decimal()}\nBIN: {res_num.binary_string}"
            elif op == "*":
                op_obj = multiplication.Multiplication(num_a, num_b)
                res_num = op_obj.execute()
                result_text = f"Значення: {res_num.to_decimal()}\nBIN: {res_num.binary_string}"
            elif op == "/":
                op_obj = division.Division(num_a, num_b)
                res_q, res_r = op_obj.execute()
                result_text = f"Частка: {res_q.to_decimal()} (BIN: {res_q.binary_string})\nОстача: {res_r.to_decimal()} (BIN: {res_r.binary_string})"
            elif op == "&":
                op_obj = bitwise_ops.BitwiseAnd(num_a, num_b)
                res_num = op_obj.execute()
                result_text = f"Значення: {res_num.to_decimal()}\nBIN: {res_num.binary_string}"
            elif op == "|":
                op_obj = bitwise_ops.BitwiseOr(num_a, num_b)
                res_num = op_obj.execute()
                result_text = f"Значення: {res_num.to_decimal()}\nBIN: {res_num.binary_string}"
            elif op == "^":
                op_obj = bitwise_ops.BitwiseXor(num_a, num_b)
                res_num = op_obj.execute()
                result_text = f"Значення: {res_num.to_decimal()}\nBIN: {res_num.binary_string}"
            elif op == "~":
                op_obj = bitwise_ops.BitwiseNot(num_a)
                res_num = op_obj.execute()
                result_text = f"Значення: {res_num.to_decimal()}\nBIN: {res_num.binary_string}"
            elif op == "<<":
                op_obj = bitwise_ops.ShiftLeft(num_a, num_b)
                res_num = op_obj.execute()
                dec_val = Converter.to_decimal(res_num.binary_string)
                result_text = f"Значення: {dec_val}\nBIN: {res_num.binary_string}"
            elif op == ">>":
                op_obj = bitwise_ops.ShiftRight(num_a, num_b)
                res_num = op_obj.execute()
                dec_val = Converter.to_decimal(res_num.binary_string)
                result_text = f"Значення: {dec_val}\nBIN: {res_num.binary_string}"

            self.view.res_label.config(text=f"РЕЗУЛЬТАТ:\n{result_text}")
            self.view.board.draw_canvas(op_obj.canvas_data, self.current_theme)

            self.view.steps_area.config(state="normal")
            self.view.steps_area.delete("1.0", "end")

            for step in op_obj.get_steps():
                desc = step.get("description", "")
                res = step.get("result", "")
                self.view.steps_area.insert("end", desc, "info")
                if res:
                    self.view.steps_area.insert("end", " -> ", "info")
                    self.view.steps_area.insert("end", res, "result")
                self.view.steps_area.insert("end", "\n", "info")

            self.view.steps_area.config(state="disabled")

            self._save_to_history_file(op, a_txt, b_txt, result_text)
            logging.info(f"Операція {op} успішно прорахована")

        except Exception as e:
            logging.error(f"Помилка при виконанні дії {op}: {str(e)}")
            raise e