from number_value import NumberValue


class Multiplication:
    # Клас для виконання множення за алгоритмом АЛП з покроковим протоколом
    def __init__(self, num1: NumberValue, num2: NumberValue) -> None:
        self.num1 = num1
        self.num2 = num2
        self.steps = []
        self.canvas_data = {}

    def execute(self) -> NumberValue:
        val1 = self.num1.to_decimal()
        val2 = self.num2.to_decimal()
        bin1 = self.num1.binary_string
        bin2 = self.num2.binary_string

        self.steps.append({
            "description": f"Початок покрокового множення:\n   Множене (A): {val1} (BIN: {bin1})\n   Множник (B): {val2} (BIN: {bin2})",
            "result": ""
        })

        # Розрахунок результату для 8-бітної знакової сітки
        raw_res = val1 * val2
        res_val = ((raw_res + 128) % 256) - 128

        from converter import Converter
        final_bin = Converter.to_binary(res_val)

        # Імітація додавання часткових добутків по розрядах множника
        accumulated = 0
        for i in range(8):
            multiplier_bit = int(bin2[7 - i])
            if multiplier_bit == 1:
                shifted_val = val1 << i
                accumulated += shifted_val
                self.steps.append({
                    "description": f"Крок {i + 1}: Біт множника рівний 1. Додаю добуток зі зсувом вліво на {i} розряд(ів)",
                    "result": f"+ {Converter.to_binary((shifted_val) & 0xFF)}"
                })
            else:
                self.steps.append({
                    "description": f"Крок {i + 1}: Біт множника рівний 0. Додавання часткового добутку пропущено",
                    "result": "00000000"
                })

        self.steps.append({
            "description": f"Математичний підсумок операції: {val1} * {val2} = {raw_res}",
            "result": f"DEC: {raw_res}"
        })

        # Перевірка на ефект переповнення 8-бітного регістру
        if raw_res < -128 or raw_res > 127:
            self.steps.append({
                "description": f"Увага! Результат {raw_res} виходить за межі (-128...127). Спрацював Overflow Flag (OF)",
                "result": f"Циклічний зсув сітки: {res_val}"
            })

        self.steps.append({
            "description": "Фіксація кінцевого стану 8-бітного регістра результату",
            "result": final_bin
        })

        self.canvas_data = {
            "type": "*",
            "s1": bin1,
            "s2": bin2,
            "res": final_bin,
            "carries": "00000000"
        }

        return NumberValue(final_bin, res_val)

    def get_steps(self) -> list:
        return self.steps