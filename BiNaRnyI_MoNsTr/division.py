from number_value import NumberValue

class Division:
    # Клас для покрокового ділення куточком
    def __init__(self, num1: NumberValue, num2: NumberValue) -> None:
        self.num1 = num1
        self.num2 = num2
        self.steps = []
        self.canvas_data = {}

    def execute(self) -> tuple[NumberValue, NumberValue]:
        val1 = self.num1.to_decimal()
        val2 = self.num2.to_decimal()
        bin1 = self.num1.binary_string
        bin2 = self.num2.binary_string

        if val2 == 0:
            self.steps.append({
                "description": "Помилка: дільник рівний нулю",
                "result": "Ділення перервано"
            })
            raise ZeroDivisionError("Ділення на нуль неможливе!")

        self.steps.append({
            "description": f"Початок ділення:\n   Ділене (A): {val1} (BIN: {bin1})\n   Дільник (B): {val2} (BIN: {bin2})",
            "result": ""
        })

        q_val = int(val1 / val2)
        r_val = val1 % val2 if val1 >= 0 else val1 - (q_val * val2)

        from converter import Converter
        bin_q = Converter.to_binary(q_val)
        bin_r = Converter.to_binary(r_val)

        abs_a = abs(val1)
        abs_b = abs(val2)
        current_remainder = 0
        bin_abs_a = bin(abs_a)[2:].zfill(8)
        generated_q_bits = []

        self.steps.append({
            "description": "Ініціалізація покрокового алгоритму порівняння та віднімання",
            "result": ""
        })

        for i in range(8):
            next_bit = int(bin_abs_a[i])
            current_remainder = (current_remainder << 1) | next_bit

            self.steps.append({
                "description": f"Такт {i + 1}: Знос біта {next_bit}. Поточна частина: {current_remainder}",
                "result": f"BIN: {bin(current_remainder)[2:]}"
            })

            if current_remainder >= abs_b:
                current_remainder -= abs_b
                generated_q_bits.append("1")
                self.steps.append({
                    "description": f"   Частина >= дільника ({abs_b}). Віднімаю дільник. Біт частки 1",
                    "result": f"нова остача: {current_remainder}"
                })
            else:
                generated_q_bits.append("0")
                self.steps.append({
                    "description": f"   Частина < дільника ({abs_b}). Віднімання неможливе. Біт частки 0",
                    "result": "залишок без змін"
                })

        self.steps.append({
            "description": f"Завершення процесу. Частка = {abs(q_val)}, Остача = {r_val}",
            "result": ""
        })

        self.steps.append({
            "description": f"Корекція знаків результатів відповідно до початкових чисел",
            "result": f"Q (DEC): {q_val} | R (DEC): {r_val}"
        })

        self.canvas_data = {
            "type": "/",
            "s1": bin1,
            "s2": bin2,
            "res": bin_q,
            "carries": "00000000"
        }

        return NumberValue(bin_q, q_val), NumberValue(bin_r, r_val)

    def get_steps(self) -> list:
        return self.steps