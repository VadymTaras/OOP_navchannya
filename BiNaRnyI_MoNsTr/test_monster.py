import unittest
from unittest.mock import patch, mock_open
import tkinter as tk

from number_value import NumberValue
from addition import Addition
from subtraction import Subtraction
from multiplication import Multiplication
from division import Division
from bitwise_ops import BitwiseAnd, BitwiseOr, BitwiseXor, BitwiseNot, ShiftLeft, ShiftRight
from converter import Converter
from app_view import AppView
from app_controller import AppController


class TestNumberValue(unittest.TestCase):
    # Перевіряю чи правильно створюється об'єкт числа

    def test_init_binary_only(self):
        # Якщо передаю тільки двійковий рядок
        num = NumberValue("00000101")
        self.assertEqual(num.to_decimal(), 5)
        self.assertEqual(str(num), "00000101")

    def test_init_with_decimal(self):
        # Якщо передаю рядок і готове десяткове значення
        num = NumberValue("11111111", -1)
        self.assertEqual(num.to_decimal(), -1)

    def test_negative_binary_parsing(self):
        # Перевірка парсингу від'ємного числа через старший біт
        num = NumberValue("10000000")
        self.assertEqual(num.to_decimal(), -128)


class TestConverterValidation(unittest.TestCase):
    # Тести валідації вводу в конвертері

    def test_clean_input_binary_padding(self):
        # Дописування нулів зліва до 8 біт
        res = Converter.clean_input("101", "binary", None)
        self.assertEqual(res, "00000101")

    def test_clean_input_invalid_binary(self):
        # Відхилення літер у двійковому режимі
        res = Converter.clean_input("102a", "binary", None)
        self.assertFalse(res)

    def test_clean_input_invalid_decimal(self):
        # Відхилення тексту у десятковому режимі
        res = Converter.clean_input("text", "decimal", None)
        self.assertFalse(res)

    def test_clean_input_out_of_range(self):
        # Перевірка виходу числа за межі байту
        res = Converter.clean_input("128", "decimal", None)
        self.assertFalse(res)

    def test_to_decimal_conversion(self):
        # Перевід з двійкового коду в десятковий зі знаком
        self.assertEqual(Converter.to_decimal("11111111"), -1)
        self.assertEqual(Converter.to_decimal("01111111"), 127)

    def test_to_binary_conversion(self):
        # Перевід з десяткового формату в двійковий
        self.assertEqual(Converter.to_binary(-128), "10000000")
        self.assertEqual(Converter.to_binary(5), "00000101")


class TestArithmeticOperations(unittest.TestCase):
    # Тести математичних операцій калькулятора та генерації кроків

    def test_addition(self):
        n1 = NumberValue("00000101", 5)
        n2 = NumberValue("00000011", 3)
        op = Addition(n1, n2)
        res = op.execute()
        self.assertEqual(res.to_decimal(), 8)
        self.assertTrue(len(op.get_steps()) > 0)

    def test_subtraction_negative(self):
        n1 = NumberValue("00000010", 2)
        n2 = NumberValue("00000011", 3)
        op = Subtraction(n1, n2)
        res = op.execute()
        self.assertEqual(res.to_decimal(), -1)
        self.assertEqual(res.binary_string, "11111111")
        self.assertTrue(len(op.get_steps()) > 0)

    def test_multiplication(self):
        n1 = NumberValue("00000100", 4)
        n2 = NumberValue("00000011", 3)
        op = Multiplication(n1, n2)
        res = op.execute()
        self.assertEqual(res.to_decimal(), 12)
        self.assertTrue(len(op.get_steps()) > 0)

    def test_division_normal(self):
        n1 = NumberValue("00001010", 10)
        n2 = NumberValue("00000010", 2)
        op = Division(n1, n2)
        q, r = op.execute()
        self.assertEqual(q.to_decimal(), 5)
        self.assertEqual(r.to_decimal(), 0)
        self.assertTrue(len(op.get_steps()) > 0)

    def test_division_by_zero(self):
        n1 = NumberValue("00001010", 10)
        n2 = NumberValue("00000000", 0)
        with self.assertRaises(ZeroDivisionError):
            Division(n1, n2).execute()


class TestBitwiseOperations(unittest.TestCase):
    # Тести побітових операцій та зсувів

    def test_and(self):
        n1 = NumberValue("00001100", 12)
        n2 = NumberValue("00001010", 10)
        op = BitwiseAnd(n1, n2)
        res = op.execute()
        self.assertEqual(res.binary_string, "00001000")
        self.assertTrue(len(op.get_steps()) > 0)

    def test_or(self):
        n1 = NumberValue("00001100", 12)
        n2 = NumberValue("00001010", 10)
        res = BitwiseOr(n1, n2).execute()
        self.assertEqual(res.binary_string, "00001110")

    def test_xor(self):
        n1 = NumberValue("00001100", 12)
        n2 = NumberValue("00001010", 10)
        res = BitwiseXor(n1, n2).execute()
        self.assertEqual(res.binary_string, "00000110")

    def test_not(self):
        n1 = NumberValue("00000000", 0)
        res = BitwiseNot(n1, None).execute()
        self.assertEqual(res.binary_string, "11111111")
        self.assertEqual(res.to_decimal(), -1)

    def test_shift_left(self):
        n1 = NumberValue("00000001", 1)
        n2 = NumberValue("00000010", 2)
        res = ShiftLeft(n1, n2).execute()
        self.assertEqual(res.to_decimal(), 4)

    def test_shift_right(self):
        n1 = NumberValue("00000100", 4)
        n2 = NumberValue("00000001", 1)
        res = ShiftRight(n1, n2).execute()
        self.assertEqual(res.to_decimal(), 2)


class TestEdgeCases(unittest.TestCase):
    # Тести граничних ситуацій

    def test_zero_operations(self):
        n1 = NumberValue("00000101", 5)
        n0 = NumberValue("00000000", 0)

        res_add = Addition(n1, n0).execute()
        self.assertEqual(res_add.to_decimal(), 5)

        res_sub = Subtraction(n0, n1).execute()
        self.assertEqual(res_sub.to_decimal(), -5)

        q, r = Division(n0, n1).execute()
        self.assertEqual(q.to_decimal(), 0)

    def test_overflow_positive(self):
        # 127 + 1 має перейти в -128
        n1 = NumberValue("01111111", 127)
        n2 = NumberValue("00000001", 1)
        res = Addition(n1, n2).execute()
        self.assertEqual(res.binary_string, "10000000")

    def test_overflow_negative(self):
        # -128 - 1 має перейти в 127
        n1 = NumberValue("10000000", -128)
        n2 = NumberValue("00000001", 1)
        res = Subtraction(n1, n2).execute()
        self.assertEqual(res.binary_string, "01111111")

    def test_double_inversion(self):
        # Подвійна інверсія повертає початкове число
        n1 = NumberValue("00110011", 51)
        step1 = BitwiseNot(n1, None).execute()
        step2 = BitwiseNot(step1, None).execute()
        self.assertEqual(step2.binary_string, "00110011")


class TestUIAndFiles(unittest.TestCase):
    # Тестування інтерфейсу та роботи з файлами через заглушки

    def setUp(self):
        # Створюю приховане вікно, щоб воно не заважало під час тестів
        self.root = tk.Tk()
        self.root.withdraw()
        self.view = AppView(self.root)
        self.controller = AppController(self.view)

    def tearDown(self):
        # Обов'язково закриваю вікно після кожного тесту
        self.root.destroy()

    def test_theme_switching(self):
        # Перевіряю чи змінюється тема при виклику функції
        initial_theme = self.controller.current_theme
        self.controller._toggle_theme()
        new_theme = self.controller.current_theme

        self.assertNotEqual(initial_theme, new_theme)
        self.assertEqual(self.view.current_theme_key, new_theme)

    @patch("builtins.open", new_callable=mock_open)
    @patch("json.dump")
    def test_save_to_history(self, mock_json, mock_file):
        # Тестую збереження історії без реального створення файлу
        self.controller._save_to_history_file("+", "10", "5", "15")

        mock_file.assert_called_with("history.json", "w", encoding="utf-8")
        self.assertTrue(mock_json.called)

    @patch("builtins.open", new_callable=mock_open)
    @patch("tkinter.messagebox.showinfo")
    def test_save_protocol(self, mock_msgbox, mock_file):
        # Закидаю текст у поле і викликаю функцію збереження лога
        self.view.steps_area.config(state="normal")
        self.view.steps_area.insert("1.0", "Тестовий запис протоколу")
        self.view.steps_area.config(state="disabled")

        self.view._save_log_to_file()

        mock_file.assert_called_with("protocol.txt", "w", encoding="utf-8")
        mock_file().write.assert_called_with("Тестовий запис протоколу")
        self.assertTrue(mock_msgbox.called)

    @patch("builtins.open", new_callable=mock_open)
    @patch("json.dump")
    @patch("tkinter.messagebox.showinfo")
    def test_clear_history(self, mock_msgbox, mock_json, mock_file):
        # Перевіряю чи функція очищення записує порожній список у файл
        self.controller._clear_history()

        mock_file.assert_called_with("history.json", "w", encoding="utf-8")
        mock_json.assert_called_with([], mock_file())
        self.assertTrue(mock_msgbox.called)


if __name__ == "__main__":
    unittest.main()