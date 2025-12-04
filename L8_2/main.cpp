#include <iostream>
#include <windows.h>

using namespace std;

int main() {
    SetConsoleOutputCP(65001);

    int* numbers = new int[3];

    numbers[0] = 1;
    numbers[1] = 15;
    numbers[2] = 2;

    cout << "Всі елементи масиву:" << endl;

    for (int i = 0; i < 3; i++) {
        cout << numbers[i] << endl;
    }

    delete[] numbers;

    return 0;
}