from abc import ABC, abstractmethod

class ArithmeticOperation(ABC):
    # Базовий клас для операцій

    def __init__(self, num1, num2):
        self.num1, self.num2, self.steps = num1, num2, []

    @abstractmethod
    def execute(self):
        # Метод треба реалізувати в кожній дії
        pass

    def get_steps(self):
        # Повертає список кроків
        return self.steps

    def _add_step(self, desc, intermediate=None):
        # Додає новий крок у журнал
        self.steps.append({"description": desc, "result": intermediate})