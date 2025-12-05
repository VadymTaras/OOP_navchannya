#include <iostream>
#include <cmath>
#include <windows.h>

using namespace std;

int main() {
    SetConsoleOutputCP(65001);

    double a, b;

    cout << "Введіть катет a: ";
    cin >> a;
    cout << "Введіть катет b: ";
    cin >> b;

    if (a > 0 && b > 0) {
        double c = sqrt(a * a + b * b);

        double P = a + b + c;

        cout << "Периметр = " << P << endl;
    }
    else {
        cout << "Помилка: сторони мають бути > 0" << endl;
    }

    return 0;
}