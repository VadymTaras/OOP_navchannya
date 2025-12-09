#include <iostream>
#include <windows.h>

class Calculator {
private:
    double accumulator;

public:
    Calculator() : accumulator(0.0) {}

    void add(double operand) {
        accumulator += operand;
    }

    void subtract(double operand) {
        accumulator -= operand;
    }

    void multiply(double operand) {
        accumulator *= operand;
    }

    void divide(double operand) {
        if (operand != 0.0) {
            accumulator /= operand;
        } else {
            std::cerr << "Помилка: Ділення на нуль! Акумулятор не змінено." << std::endl;
        }
    }

    double get_accumulator() const {
        return accumulator;
    }
};

void exe(
    Calculator& calc,
    void (Calculator::*operation)(double),
    double operand
) {
    (calc.*operation)(operand);
}


int main() {
    SetConsoleOutputCP(CP_UTF8);
    SetConsoleCP(CP_UTF8);

    std::cout << "--- exe() для Calculator ---" << std::endl;
    Calculator my_calc;

    std::cout << "Початкове значення акумулятора: " << my_calc.get_accumulator() << std::endl;

    exe(my_calc, &Calculator::add, 10.5);
    std::cout << "Після додавання 10.5: " << my_calc.get_accumulator() << std::endl;

    exe(my_calc, &Calculator::subtract, 3.0);
    std::cout << "Після віднімання 3.0: " << my_calc.get_accumulator() << std::endl;

    exe(my_calc, &Calculator::divide, 0.0);
    std::cout << "Після ділення на 0.0: " << my_calc.get_accumulator() << std::endl;

    return 0;
}