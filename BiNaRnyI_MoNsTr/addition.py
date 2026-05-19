from number_value import NumberValue

class Addition:
    # Клас для додавання чисел та збору кроків для дошки
    def __init__(self, num1: NumberValue, num2: NumberValue) -> None:
        self.num1 = num1
        self.num2 = num2
        self.steps = []
        self.canvas_data = {}

    def execute(self) -> NumberValue:
        bin1 = self.num1.binary_string
        bin2 = self.num2.binary_string
        carry = 0
        result_bits = []
        carry_bits = []

        self.steps.append({
            "description": f"Початок додавання: {self.num1.to_decimal()} (BIN: {bin1}) плюс {self.num2.to_decimal()} (BIN: {bin2})",
            "result": ""
        })

        for i in range(7, -1, -1):
            bit1 = int(bin1[i])
            bit2 = int(bin2[i])

            carry_bits.insert(0, str(carry))

            sum_val = bit1 + bit2 + carry
            res_bit = sum_val % 2
            carry = sum_val // 2

            result_bits.insert(0, str(res_bit))
            self.steps.append({
                "description": f"Розряд {7 - i}: додаю {bit1} і {bit2} з переносом {carry_bits[0]}",
                "result": f"біт {res_bit}, новий перенос {carry}"
            })

        final_bin = "".join(result_bits)

        # Рахуємо десяткове значення з урахуванням знаку
        val1 = self.num1.to_decimal()
        val2 = self.num2.to_decimal()
        res_val = val1 + val2
        # Обрізання під межі від -128 до 127
        res_val = ((res_val + 128) % 256) - 128

        self.canvas_data = {
            "type": "+",
            "s1": bin1,
            "s2": bin2,
            "res": final_bin,
            "carries": "".join(carry_bits)
        }

        self.steps.append({
            "description": "Формування остаточного 8-бітного результату",
            "result": final_bin
        })

        return NumberValue(final_bin, res_val)

    def get_steps(self) -> list:
        return self.steps