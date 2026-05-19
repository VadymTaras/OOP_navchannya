from typing import Optional, Any
from number_value import NumberValue

class BitwiseAnd:
    # Побітове І (AND) з описом кожного розряду
    def __init__(self, num1: NumberValue, num2: NumberValue) -> None:
        self.num1 = num1
        self.num2 = num2
        self.steps = []
        self.canvas_data = {}

    def execute(self) -> NumberValue:
        b1, b2 = self.num1.binary_string, self.num2.binary_string
        res_bits = []

        self.steps.append({
            "description": f"Початок операції побітового І (AND):\n   A: {b1}\n   B: {b2}",
            "result": ""
        })

        for i in range(8):
            bit1 = int(b1[i])
            bit2 = int(b2[i])
            res_bit = bit1 & bit2
            res_bits.append(str(res_bit))
            self.steps.append({
                "description": f"Розряд {7 - i}: логічне І для бітів {bit1} та {bit2}",
                "result": f"відповідь {res_bit}"
            })

        final_bin = "".join(res_bits)
        self.canvas_data = {"type": "&", "s1": b1, "s2": b2, "res": final_bin, "carries": "00000000"}

        val1 = self.num1.to_decimal()
        val2 = self.num2.to_decimal()
        res_val = (val1 & val2) & 0xFF
        res_val = (res_val ^ 0x80) - 0x80

        return NumberValue(final_bin, res_val)

    def get_steps(self) -> list:
        return self.steps

class BitwiseOr:
    # Побітове АБО (OR) по розрядах
    def __init__(self, num1: NumberValue, num2: NumberValue) -> None:
        self.num1 = num1
        self.num2 = num2
        self.steps = []
        self.canvas_data = {}

    def execute(self) -> NumberValue:
        b1, b2 = self.num1.binary_string, self.num2.binary_string
        res_bits = []

        self.steps.append({
            "description": f"Початок операції побітового АБО (OR):\n   A: {b1}\n   B: {b2}",
            "result": ""
        })

        for i in range(8):
            bit1 = int(b1[i])
            bit2 = int(b2[i])
            res_bit = bit1 | bit2
            res_bits.append(str(res_bit))
            self.steps.append({
                "description": f"Розряд {7 - i}: логічне АБО для бітів {bit1} та {bit2}",
                "result": f"відповідь {res_bit}"
            })

        final_bin = "".join(res_bits)
        self.canvas_data = {"type": "|", "s1": b1, "s2": b2, "res": final_bin, "carries": "00000000"}

        val1 = self.num1.to_decimal()
        val2 = self.num2.to_decimal()
        res_val = (val1 | val2) & 0xFF
        res_val = (res_val ^ 0x80) - 0x80

        return NumberValue(final_bin, res_val)

    def get_steps(self) -> list:
        return self.steps

class BitwiseXor:
    # Побітове виключне АБО (XOR)
    def __init__(self, num1: NumberValue, num2: NumberValue) -> None:
        self.num1 = num1
        self.num2 = num2
        self.steps = []
        self.canvas_data = {}

    def execute(self) -> NumberValue:
        b1, b2 = self.num1.binary_string, self.num2.binary_string
        res_bits = []

        self.steps.append({
            "description": f"Початок операції побітового XOR:\n   A: {b1}\n   B: {b2}",
            "result": ""
        })

        for i in range(8):
            bit1 = int(b1[i])
            bit2 = int(b2[i])
            res_bit = bit1 ^ bit2
            res_bits.append(str(res_bit))
            self.steps.append({
                "description": f"Розряд {7 - i}: логічне виключне АБО для бітів {bit1} та {bit2}",
                "result": f"відповідь {res_bit}"
            })

        final_bin = "".join(res_bits)
        self.canvas_data = {"type": "^", "s1": b1, "s2": b2, "res": final_bin, "carries": "00000000"}

        val1 = self.num1.to_decimal()
        val2 = self.num2.to_decimal()
        res_val = (val1 ^ val2) & 0xFF
        res_val = (res_val ^ 0x80) - 0x80

        return NumberValue(final_bin, res_val)

    def get_steps(self) -> list:
        return self.steps

class BitwiseNot:
    # Побітова інверсія НЕ (NOT)
    def __init__(self, num1: NumberValue, num2: Optional[NumberValue] = None) -> None:
        self.num1 = num1
        self.steps = []
        self.canvas_data = {}

    def execute(self) -> NumberValue:
        b1 = self.num1.binary_string
        res_bits = []

        self.steps.append({
            "description": f"Початок інверсії НЕ (NOT) для регістра A: {b1}",
            "result": ""
        })

        for i in range(8):
            bit1 = b1[i]
            res_bit = "1" if bit1 == "0" else "0"
            res_bits.append(res_bit)
            self.steps.append({
                "description": f"Розряд {7 - i}: інвертую біт {bit1}",
                "result": f"отримано {res_bit}"
            })

        final_bin = "".join(res_bits)
        self.canvas_data = {"type": "~", "s1": b1, "s2": "00000000", "res": final_bin, "carries": "00000000"}

        val1 = self.num1.to_decimal()
        res_val = (~val1) & 0xFF
        res_val = (res_val ^ 0x80) - 0x80

        return NumberValue(final_bin, res_val)

    def get_steps(self) -> list:
        return self.steps

class ShiftLeft:
    # Логічний зсув бітів вліво
    def __init__(self, num1: NumberValue, num2: NumberValue) -> None:
        self.num1 = num1
        self.num2 = num2
        self.steps = []
        self.canvas_data = {}

    def execute(self) -> NumberValue:
        b1 = self.num1.binary_string
        shift = self.num2.to_decimal()

        self.steps.append({
            "description": f"Початок логічного зсуву вліво (<<) регістра {b1} на {shift} розрядів",
            "result": ""
        })

        if shift >= 8:
            final_bin = "00000000"
            self.steps.append({
                "description": f"Величина зсуву {shift} >= 8 біт. Весь регістр заповнюється нулями",
                "result": final_bin
            })
        else:
            final_bin = b1[shift:] + ("0" * shift)
            self.steps.append({
                "description": f"Виштовхую зліва {shift} старших бітів: '{b1[:shift]}'",
                "result": "видалено"
            })
            self.steps.append({
                "description": f"Зсуваю проміжні біти вліво: '{b1[shift:]}'",
                "result": ""
            })
            self.steps.append({
                "description": f"Дописую справа {shift} нулів розрядів",
                "result": final_bin
            })

        self.canvas_data = {"type": "<<", "s1": b1, "s2": self.num2.binary_string, "res": final_bin, "carries": "00000000"}
        return NumberValue(final_bin, None)

    def get_steps(self) -> list:
        return self.steps

class ShiftRight:
    # Логічний зсув бітів вправо
    def __init__(self, num1: NumberValue, num2: NumberValue) -> None:
        self.num1 = num1
        self.num2 = num2
        self.steps = []
        self.canvas_data = {}

    def execute(self) -> NumberValue:
        b1 = self.num1.binary_string
        shift = self.num2.to_decimal()

        self.steps.append({
            "description": f"Початок логічного зсуву вправо (>>) регістра {b1} на {shift} розрядів",
            "result": ""
        })

        if shift >= 8:
            final_bin = "00000000"
            self.steps.append({
                "description": f"Величина зсуву {shift} >= 8 біт. Весь регістр очищується нулями",
                "result": final_bin
            })
        else:
            final_bin = ("0" * shift) + b1[:8 - shift]
            self.steps.append({
                "description": f"Виштовхую справа {shift} молодших бітів: '{b1[8 - shift:]}'",
                "result": "видалено"
            })
            self.steps.append({
                "description": f"Зсуваю проміжні біти вправо: '{b1[:8 - shift]}'",
                "result": ""
            })
            self.steps.append({
                "description": f"Дописую зліва {shift} нулів для відновлення сітки",
                "result": final_bin
            })

        self.canvas_data = {"type": ">>", "s1": b1, "s2": self.num2.binary_string, "res": final_bin, "carries": "00000000"}
        return NumberValue(final_bin, None)

    def get_steps(self) -> list:
        return self.steps