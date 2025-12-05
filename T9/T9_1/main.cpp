#include <iostream>
#include <cmath>
#include <windows.h>

using namespace std;

int main() {
    SetConsoleOutputCP(65001);

    double s;
    cout << "S = ";
    cin >> s;

    double a = sqrt(s);
    double p = 4 * a;

    cout << "P = " << p << endl;

    return 0;
}