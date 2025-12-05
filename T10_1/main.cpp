#include <iostream>
#include <cmath>
#include <windows.h>

using namespace std;

int main() {
    SetConsoleOutputCP(65001);

    int k;
    double a, b, h;
    double x, y;

    cout << "1. sin(x)" << endl;
    cout << "2. cos(x)" << endl;
    cout << "3. x^2" << endl;
    cout << "Виберіть (1-3): ";
    cin >> k;

    cout << "a = "; cin >> a;
    cout << "b = "; cin >> b;
    cout << "h = "; cin >> h;

    cout << "\nResult:" << endl;

    x = a;
    while (x <= b) {


        switch (k) {
            case 1:
                y = sin(x);
                break;
            case 2:
                y = cos(x);
                break;
            case 3:
                y = pow(x, 2);
            default:
                cout << "Error" << endl;
                return 0;
        }

        cout << "x=" << x << " \t y=" << y << endl;

        x = x + h;
    }

    return 0;
}