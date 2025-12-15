#include <iostream>
#include <cmath>
#include <windows.h>

using namespace std;

int main() {
    SetConsoleCP(65001);
    SetConsoleOutputCP(65001);

    // Константи
    const double a = 1.75;
    const double b = 0.4;
    const double e_const = exp(1.0);

    double x, y, z;

    cout << "=== Завдання 2 (Варіант 5) ===" << endl;

    y = 2 * a * pow(a + b, 1.0 / 3.0);

    double arg_x = (exp(a) + exp(b)) / sqrt(a + e_const);
    x = atan(1.0 / arg_x);

    if (x <= 0) {
        cout << "Помилка обчислення Z: x <= 0" << endl;
    } else {
        double numerator_z = fabs(x - 1) + exp(-y);
        double denominator_z = 12.34 - log10(sqrt(x));

        if (denominator_z == 0) {
            cout << "Помилка: ділення на нуль (Z)." << endl;
        } else {
            z = numerator_z / denominator_z;

            cout << "Вхідні дані:" << endl;
            cout << "a = " << a << ", b = " << b << endl;
            cout << "--------------------" << endl;
            cout << "Результати:" << endl;
            cout << "y = " << y << endl;
            cout << "x = " << x << endl;
            cout << "z = " << z << endl;
        }
    }

    return 0;
}