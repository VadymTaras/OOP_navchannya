from arithmetic_operation import ArithmeticOperation
from number_value import NumberValue
from converter import Converter


class Multiplication(ArithmeticOperation):
    """Множення: перемножує значення через десяткову базу."""

    def execute(self):
        self._add_step(f"Множимо {self.num1} на {self.num2}")
        # Множимо десяткові еквіваленти чисел
        res_dec = self.num1.to_decimal() * self.num2.to_decimal()
        return NumberValue(Converter.to_binary(res_dec), res_dec)