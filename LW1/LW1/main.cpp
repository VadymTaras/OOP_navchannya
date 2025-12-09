#include <iostream>
#include <cmath>
#include <windows.h>

using namespace std;

int main() {
    SetConsoleCP(65001);
    SetConsoleOutputCP(65001);

    double x, y;

    cout << "=== Завдання 1 (Варіант 5) ===" << endl;
    cout << "Введіть значення x (x > 0): ";
    cin >> x;

    if (x <= 0) {
        cout << "Помилка: x повинен бути більше 0." << endl;
        return 1;
    }

    double numerator = exp(-x) - 4 * x - pow(log(x), 3);

    double term_ctg = 1.0 / tan(x * x - 1);
    double denominator = log10(fabs(x + 1)) + term_ctg;

    if (fabs(denominator) < 1e-9) {
        cout << "Помилка: Знаменник дорівнює нулю." << endl;
    } else {
        y = numerator / denominator;
        cout << "Результат y = " << y << endl;
    }

    return 0;
}