from arithmetic_operation import ArithmeticOperation
from number_value import NumberValue
from converter import Converter


class Addition(ArithmeticOperation):
    """Модуль додавання: реалізує логіку total = bit1 + bit2 + carry."""

    def execute(self):
        # Вирівнюємо довжину рядків для зручності
        max_len = max(len(self.num1), len(self.num2))
        s1 = self.num1.get_padded_string(max_len)
        s2 = self.num2.get_padded_string(max_len)

        self._add_step(f"Вирівнюємо числа: {s1} та {s2}")

        res_bits, carry = [], 0
        # Обчислюємо кожен розряд справа наліво
        for i in range(max_len - 1, -1, -1):
            bit1, bit2 = int(s1[i]), int(s2[i])
            total = bit1 + bit2 + carry

            # Поточний біт результату (остача від ділення на 2)
            res_bits.insert(0, str(total % 2))
            # Перенос на наступний розряд (ціла частина від ділення на 2)
            carry = total // 2

            desc = f"Крок на позиції {max_len - i}: {bit1}+{bit2} (перенос {carry})"
            self._add_step(desc, "".join(res_bits))

        if carry:
            res_bits.insert(0, "1")
            self._add_step("Додаємо останній перенос одиниці", "".join(res_bits))

        final_bin = "".join(res_bits)
        return NumberValue(final_bin, Converter.to_decimal(final_bin))