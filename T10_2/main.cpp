#include <iostream>
#include <cmath>
#include <windows.h>

using namespace std;

int main() {
    SetConsoleOutputCP(65001);

    int k;
    double a, b;
    int n;

    cout << "Виберіть функцію:" << endl;
    cout << "1. x^2" << endl;
    cout << "2. sin(x)" << endl;
    cout << "3. cos(x)" << endl;
    cout << "4. e^x" << endl;
    cin >> k;

    cout << "a = "; cin >> a;
    cout << "b = "; cin >> b;
    cout << "n = "; cin >> n;

    double h = (b - a) / n;
    double s = 0;
    double x;
    double y;

    if (k == 1) s = a * a;
    else if (k == 2) s = sin(a);
    else if (k == 3) s = cos(a);
    else if (k == 4) s = exp(a);


    if (k == 1) s = s + b * b;
    else if (k == 2) s = s + sin(b);
    else if (k == 3) s = s + cos(b);
    else if (k == 4) s = s + exp(b);

    for (int i = 1; i < n; i++) {
        x = a + i * h;

        if (k == 1) y = x * x;
        else if (k == 2) y = sin(x);
        else if (k == 3) y = cos(x);
        else if (k == 4) y = exp(x);

        if (i % 2 != 0) {
            s = s + 4 * y;
        } else {
            s = s + 2 * y;
        }
    }

    double integral = s * h / 3;

    cout << "Integral = " << integral << endl;

    return 0;
}