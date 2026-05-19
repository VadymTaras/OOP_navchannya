from number_value import NumberValue

class Subtraction:
    # Клас для віднімання чисел та виводу на дошку
    def __init__(self, num1: NumberValue, num2: NumberValue) -> None:
        self.num1 = num1
        self.num2 = num2
        self.steps = []
        self.canvas_data = {}

    def execute(self) -> NumberValue:
        bin1 = self.num1.binary_string
        bin2 = self.num2.binary_string
        borrow = 0
        result_bits = []
        borrow_bits = []

        self.steps.append({
            "description": f"Початок віднімання: {self.num1.to_decimal()} (BIN: {bin1}) мінус {self.num2.to_decimal()} (BIN: {bin2})",
            "result": ""
        })

        for i in range(7, -1, -1):
            bit1 = int(bin1[i])
            bit2 = int(bin2[i])

            borrow_bits.insert(0, str(borrow))

            sub_val = bit1 - bit2 - borrow
            if sub_val < 0:
                res_bit = (sub_val + 2) % 2
                borrow = 1
            else:
                res_bit = sub_val % 2
                borrow = 0

            result_bits.insert(0, str(res_bit))
            self.steps.append({
                "description": f"Розряд {7 - i}: віднімаю біт {bit2} від {bit1} з урахуванням позики {borrow_bits[0]}",
                "result": f"Результат розряду: {res_bit}, нова позика: {borrow}"
            })

        final_bin = "".join(result_bits)

        val1 = self.num1.to_decimal()
        val2 = self.num2.to_decimal()
        res_val = val1 - val2
        # Обрізання під межі від -128 до 127
        res_val = ((res_val + 128) % 256) - 128

        self.canvas_data = {
            "type": "-",
            "s1": bin1,
            "s2": bin2,
            "res": final_bin,
            "carries": "".join(borrow_bits)
        }

        self.steps.append({
            "description": "Формування остаточного 8-бітного результату",
            "result": final_bin
        })

        return NumberValue(final_bin, res_val)

    def get_steps(self) -> list:
        return self.steps