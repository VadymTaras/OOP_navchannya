class Debugger:
    """Інструмент діагностики для перевірки 'брудного' вводу."""

    def __init__(self):
        self.is_active = False

    def toggle(self):
        # Вмикає або вимикає логування
        self.is_active = not self.is_active

    def log_input(self, original, cleaned):
        # repr() дозволяє побачити спецсимволи (\r, \n)
        if self.is_active:
            print(f"\n[DEBUG] Отримано (сирий): {repr(original)}")
            print(f"[DEBUG] Після фільтрації: '{cleaned}'")