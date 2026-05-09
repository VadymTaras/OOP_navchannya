from arithmetic_operation import ArithmeticOperation
from number_value import NumberValue
from converter import Converter


class Subtraction(ArithmeticOperation):
    """Віднімання стовпчиком: реалізує логіку позики розряду."""

    def execute(self):
        # Валідація на від'ємний результат
        if self.num1 < self.num2: raise ValueError("Результат не може бути від'ємним!")

        max_len = max(len(self.num1), len(self.num2))
        s1, s2 = self.num1.get_padded_string(max_len), self.num2.get_padded_string(max_len)
        self._add_step(f"Вирівнюємо: {s1} - {s2}")

        res_bits, borrow, bits = [], 0, list(map(int, s1))
        # Проходимо справа наліво
        for i in range(max_len - 1, -1, -1):
            v1, v2 = bits[i] - borrow, int(s2[i])
            # Визначаємо, чи потрібно позичати одиницю
            cur_borrow = 1 if v1 < v2 else 0
            cur_res = (v1 + 2) - v2 if cur_borrow else v1 - v2
            res_bits.insert(0, str(cur_res))
            self._add_step(f"Позиція {max_len - i}: позика {cur_borrow}", "".join(res_bits))
            borrow = cur_borrow

        final_bin = "".join(res_bits).lstrip("0") or "0"
        return NumberValue(final_bin, Converter.to_decimal(final_bin))