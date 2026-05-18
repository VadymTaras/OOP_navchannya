class NumberValue:
    # Клас для збереження 8-бітного двійкового числа та його десяткового значення
    def __init__(self, binary_string: str, decimal_value: int = None) -> None:
        self.binary_string = binary_string

        if decimal_value is None:
            self._decimal_value = self._parse_binary_to_decimal(binary_string)
        else:
            self._decimal_value = decimal_value

    def _parse_binary_to_decimal(self, bin_str: str) -> int:
        # Примусово доповнюємо нулями до 8 біт для точного аналізу розрядів
        padded = bin_str.zfill(8)

        # Перевіряємо старший знаковий біт (8-бітна знакова арифметика)
        if padded[0] == "1":
            # Якщо старший біт 1 - це від'ємне число у додатковому коді
            inverted = "".join("1" if b == "0" else "0" for b in padded)
            return -(int(inverted, 2) + 1)
        else:
            # Якщо старший біт 0 - це звичайне додатне число
            return int(padded, 2)

    def to_decimal(self) -> int:
        return self._decimal_value

    def __str__(self) -> str:
        return self.binary_string