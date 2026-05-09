import re


class Converter:
    """Сервіс для очищення рядків та зміни систем числення."""

    @staticmethod
    def clean_input(value, debugger=None):
        # Регулярний вираз вилучає тільки цифри 0-9
        raw_str = str(value).strip()
        clean_val = "".join(re.findall(r'[0-9]', raw_str))
        if debugger: debugger.log_input(value, clean_val)
        return clean_val

    @staticmethod
    def to_binary(decimal_val):
        """Перетворення DEC у бінарний рядок."""
        if decimal_val == 0: return "0"
        return bin(decimal_val)[2:]

    @staticmethod
    def to_decimal(binary_str):
        """Перетворення BIN у десяткове число."""
        return int(binary_str, 2)