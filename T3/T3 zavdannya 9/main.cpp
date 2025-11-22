#include <iostream>
#include <windows.h>


using namespace std;

int main() {
    SetConsoleOutputCP(65001);
    int n;
    cout << "Введіть число n: ";
    cin >> n;

    long long dobutok = 1;

    if (n % 2 == 0) {

        for (int i = 2; i <= n; i += 2) {
            dobutok = dobutok * i;
        }
    }
    else {

        for (int i = 1; i <= n; i += 2) {
            dobutok = dobutok * i;
        }
    }

    cout << "Результат n!! = " << dobutok << endl;

    return 0;
}