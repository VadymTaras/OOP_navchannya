#include <iostream>
#include <windows.h>

using namespace std;

int main() {
    SetConsoleOutputCP(65001);

    int n;
    cout << "n = ";
    cin >> n;

    int a = n / 10;
    int b = n % 10;

    cout << "Десятків: " << a << endl;
    cout << "Одиниць: " << b << endl;
    cout << "Сума: " << a + b << endl;
    cout << "Добуток: " << a * b << endl;

    return 0;
}