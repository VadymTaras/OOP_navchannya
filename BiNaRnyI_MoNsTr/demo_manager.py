import random
import tkinter as tk


def run_demo(entry_a: tk.Entry, entry_b: tk.Entry, mode_text: str, op_buttons: dict) -> None:
    # Функція автоматичного заповнення з урахуванням від'ємних десяткових чисел
    entry_a.delete(0, tk.END)
    entry_b.delete(0, tk.END)

    if "BINARY" in mode_text:
        # У бінарному режимі знак закладено у самих 8 бітах (старший біт)
        rand_bin_a = "".join(random.choice(["0", "1"]) for _ in range(8))
        rand_bin_b = "".join(random.choice(["0", "1"]) for _ in range(8))
        entry_a.insert(0, rand_bin_a)
        entry_b.insert(0, rand_bin_b)
    else:
        # Генеруємо числа у межах знакового байту від -128 до 127
        rand_dec_a = str(random.randint(-128, 127))

        # Створюємо список усіх чисел байту окрім нуля для безпечного ділення
        valid_b_choices = [i for i in range(-128, 128) if i != 0]
        rand_dec_b = str(random.choice(valid_b_choices))

        entry_a.insert(0, rand_dec_a)
        entry_b.insert(0, rand_dec_b)

    # Запускаємо випадкову операцію із доступних кнопок
    if op_buttons:
        all_operations = list(op_buttons.keys())
        random_op = random.choice(all_operations)
        op_buttons[random_op].invoke()