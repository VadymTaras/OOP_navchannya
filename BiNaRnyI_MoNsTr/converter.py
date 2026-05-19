class Converter:
    # Перевірка вводу та конвертація числення

    @staticmethod
    def clean_input(raw: str, mode: str, db) -> str | bool:
        cleaned = raw.strip()
        if not cleaned:
            return False

        if mode == "binary":
            for char in cleaned:
                if char not in "01":
                    if db and db.is_active:
                        db.log(f"Відхилено: '{cleaned}' містить не двійкові символи.")
                    return False

            if len(cleaned) > 8:
                if db and db.is_active:
                    db.log(f"Відхилено: '{cleaned}' більше 8 біт.")
                return False

            padded = cleaned.zfill(8)
            if db and db.is_active:
                db.log(f"Ввід '{raw}' очищено та доповнено: {padded}")
            return padded

        elif mode == "decimal":
            try:
                val = int(cleaned)
                if val < -128 or val > 127:
                    if db and db.is_active:
                        db.log(f"Відхилено: {val} виходить за межі від -128 до 127.")
                    return False
                return str(val)
            except ValueError:
                if cleaned == "-":
                    return "-"
                if db and db.is_active:
                    db.log(f"Відхилено: '{cleaned}' не є десятковим числом.")
                return False

        return False

    @staticmethod
    def to_decimal(binary_str: str) -> int:
        # Доповнюємо нулями зліва до 8 біт
        padded_str = binary_str.zfill(8)

        if padded_str[0] == '1':
            inverted = "".join("1" if b == "0" else "0" for b in padded_str)
            return -(int(inverted, 2) + 1)
        return int(padded_str, 2)

    @staticmethod
    def to_binary(decimal_val: int) -> str:
        if decimal_val < 0:
            return bin((1 << 8) + decimal_val)[2:]
        return bin(decimal_val)[2:].zfill(8)