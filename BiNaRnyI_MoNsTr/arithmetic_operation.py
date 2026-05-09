from abc import ABC, abstractmethod


class ArithmeticOperation(ABC):
    """Базовий клас для всіх математичних операцій."""

    def __init__(self, num1, num2):
        self.num1, self.num2, self.steps = num1, num2, []

    @abstractmethod
    def execute(self):
        """Метод має бути реалізований у кожній конкретній дії."""
        pass

    def get_steps(self):
        # Повертає історію кроків для візуалізатора
        return self.steps

    def _add_step(self, desc, intermediate=None):
        # Додає новий запис у протокол обчислень
        self.steps.append({"description": desc, "result": intermediate})