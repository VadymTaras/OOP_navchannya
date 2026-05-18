import unittest
from number_value import NumberValue
from addition import Addition
from subtraction import Subtraction
from multiplication import Multiplication
from division import Division
from bitwise_ops import BitwiseAnd, BitwiseOr, BitwiseXor, BitwiseNot, ShiftLeft, ShiftRight
from converter import Converter
from debugger import Debugger


class TestConverterValidation(unittest.TestCase):
    # тестую як конвертер обробляє кривий ввод

    def setUp(self):
        # створюю об'єкт дебагера для передачі в методи конвертера
        self.db = Debugger()

    def test_clean_input_binary_padding(self):
        # перевіряю чи програма дописує нулі зліва до 8 біт
        res = Converter.clean_input("101", "binary", self.db)
        self.assertEqual(res, "00000101")

    def test_clean_input_invalid_binary(self):
        # літери та зайві цифри у двійковому режимі мають відхилятися
        res = Converter.clean_input("102a", "binary", self.db)
        self.assertFalse(res)

    def test_clean_input_invalid_decimal(self):
        # перевіряю реакцію на текст у десятковому режимі
        res = Converter.clean_input("text", "decimal", self.db)
        self.assertFalse(res)

    def test_clean_input_out_of_range(self):
        # число 128 не поміщається у знаковий 8-бітний регістр
        res = Converter.clean_input("128", "decimal", self.db)
        self.assertFalse(res)

    def test_to_decimal_conversion(self):
        # тест конвертації з двійкового коду в десятковий з урахуванням знаку
        self.assertEqual(Converter.to_decimal("11111111"), -1)
        self.assertEqual(Converter.to_decimal("01111111"), 127)

    def test_to_binary_conversion(self):
        # тест конвертації з десяткового формату в двійковий
        self.assertEqual(Converter.to_binary(-128), "10000000")
        self.assertEqual(Converter.to_binary(5), "00000101")


class TestArithmeticOperations(unittest.TestCase):
    # перевіряю математичне ядро калькулятора

    def test_addition(self):
        # звичайне додавання двох додатних чисел
        n1 = NumberValue("00000101", 5)
        n2 = NumberValue("00000011", 3)
        res = Addition(n1, n2).execute()
        self.assertEqual(res.to_decimal(), 8)

    def test_subtraction_negative(self):
        # віднімання з формуванням від'ємного результату
        n1 = NumberValue("00000010", 2)
        n2 = NumberValue("00000011", 3)
        res = Subtraction(n1, n2).execute()
        self.assertEqual(res.to_decimal(), -1)
        self.assertEqual(res.binary_string, "11111111")

    def test_multiplication(self):
        # множення із перевіркою правильного десяткового значення
        n1 = NumberValue("00000100", 4)
        n2 = NumberValue("00000011", 3)
        res = Multiplication(n1, n2).execute()
        self.assertEqual(res.to_decimal(), 12)

    def test_division_normal(self):
        # ділення без остачі для перевірки цілочисельного формату
        n1 = NumberValue("00001010", 10)
        n2 = NumberValue("00000010", 2)
        q, r = Division(n1, n2).execute()
        self.assertEqual(q.to_decimal(), 5)
        self.assertEqual(r.to_decimal(), 0)

    def test_division_by_zero(self):
        # краш-тест на ділення на нуль
        n1 = NumberValue("00001010", 10)
        n2 = NumberValue("00000000", 0)
        with self.assertRaises(ZeroDivisionError):
            Division(n1, n2).execute()


class TestBitwiseOperations(unittest.TestCase):
    # тестую логіку побітових зсувів та вентилів

    def test_and(self):
        n1 = NumberValue("00001100", 12)
        n2 = NumberValue("00001010", 10)
        res = BitwiseAnd(n1, n2).execute()
        self.assertEqual(res.binary_string, "00001000")

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
        # інверсія нулів має дати мінус один у додатковому коді
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
    # перевірка екстремальних ситуацій та меж пам'яті

    def test_zero_operations(self):
        # нуль має працювати адекватно у всіх базових операціях
        n1 = NumberValue("00000101", 5)
        n0 = NumberValue("00000000", 0)

        res_add = Addition(n1, n0).execute()
        self.assertEqual(res_add.to_decimal(), 5)

        res_sub = Subtraction(n0, n1).execute()
        self.assertEqual(res_sub.to_decimal(), -5)

        q, r = Division(n0, n1).execute()
        self.assertEqual(q.to_decimal(), 0)

    def test_overflow_positive(self):
        # перевірка переходу через максимум (127 + 1 має стати -128)
        n1 = NumberValue("01111111", 127)
        n2 = NumberValue("00000001", 1)
        res = Addition(n1, n2).execute()
        # якщо математика правильно обрізає байти, старший біт стане одиницею
        self.assertEqual(res.binary_string, "10000000")

    def test_overflow_negative(self):
        # перехід через мінімум (-128 - 1 має стати 127)
        n1 = NumberValue("10000000", -128)
        n2 = NumberValue("00000001", 1)
        res = Subtraction(n1, n2).execute()
        self.assertEqual(res.binary_string, "01111111")

    def test_double_inversion(self):
        # подвійне заперечення має повертати початкове число
        n1 = NumberValue("00110011", 51)
        step1 = BitwiseNot(n1, None).execute()
        step2 = BitwiseNot(step1, None).execute()
        self.assertEqual(step2.binary_string, "00110011")


if __name__ == "__main__":
    unittest.main()