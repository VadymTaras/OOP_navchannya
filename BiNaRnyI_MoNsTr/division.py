from arithmetic_operation import ArithmeticOperation
from number_value import NumberValue
from converter import Converter


class Division(ArithmeticOperation):
    """Ділення двійкових чисел: частка та залишок."""

    def execute(self):
        # Захист від ділення на нуль
        if self.num2.to_decimal() == 0: raise ZeroDivisionError("Ділення на нуль!")
        q_dec = self.num1.to_decimal() // self.num2.to_decimal()
        r_dec = self.num1.to_decimal() % self.num2.to_decimal()
        self._add_step(f"Ділимо {self.num1} на {self.num2}")
        return (NumberValue(Converter.to_binary(q_dec), q_dec),
                NumberValue(Converter.to_binary(r_dec), r_dec))