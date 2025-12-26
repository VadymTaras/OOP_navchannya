#include <iostream>
#include <windows.h>

template <typename T1, typename T2>
class Pair {
private:
    T1 first;
    T2 second;
public:
    Pair() : first(T1()), second(T2()) {}
    Pair(T1 f, T2 s) : first(f), second(s) {}

    void setFirst(T1 f) { first = f; }
    void setSecond(T2 s) { second = s; }

    void print() const {
        std::cout << "Результат збереження пари: [" << first << ", " << second << "]" << std::endl;
    }
};

int main() {
    SetConsoleCP(65001);
    SetConsoleOutputCP(65001);

    // Створення пари для Цілого та Дробового числа
    int iVal;
    double dVal;

    std::cout << "--- Введення для Pair<int, double> ---" << std::endl;
    std::cout << "Введіть ціле число: ";
    std::cin >> iVal;
    std::cout << "Введіть дробове число: ";
    std::cin >> dVal;

    Pair<int, double> p1(iVal, dVal);
    p1.print();

    std::cout << "\n-----------------------------------\n" << std::endl;

    // Створення пари для двох Дробових чисел
    double dVal1, dVal2;

    std::cout << "--- Введення для Pair<double, double> ---" << std::endl;
    std::cout << "Введіть перше дробове число: ";
    std::cin >> dVal1;
    std::cout << "Введіть друге дробове число: ";
    std::cin >> dVal2;

    Pair<double, double> p2(dVal1, dVal2);
    p2.print();

    return 0;
}