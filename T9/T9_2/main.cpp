#include <iostream>
#include <windows.h>

using namespace std;

int main() {
    SetConsoleOutputCP(65001);

    double R, r;
    const double pi = 3.14;

    cout << "R = ";
    cin >> R;
    cout << "r = ";
    cin >> r;

    double S = pi * (R * R - r * r);

    cout << "S = " << S << endl;

    return 0;
}