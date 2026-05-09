class NumberValue:
    """Об'єкт-контейнер: зберігає значення числа в BIN та DEC одночасно."""

    def __init__(self, binary_string, decimal_value):
        self.binary_string = binary_string
        self.decimal_value = decimal_value

    def __len__(self): return len(self.binary_string)

    def __str__(self): return self.binary_string

    def to_decimal(self): return self.decimal_value

    def get_padded_string(self, length): return self.binary_string.zfill(length)

    # Перевантаження порівняння для математичної логіки
    def __lt__(self, other): return self.decimal_value < other.decimal_value

    def __ge__(self, other): return self.decimal_value >= other.decimal_value