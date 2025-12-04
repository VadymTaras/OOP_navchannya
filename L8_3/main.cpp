#include <iostream>
#include <windows.h>

using namespace std;

int main() {
    SetConsoleOutputCP(65001);

    int* numbers = new int[3];

    numbers[0] = 1;
    numbers[1] = 15;
    numbers[2] = 2;

    int sum = 0;

    for (int i = 0; i < 3; i++) {
        sum = sum + numbers[i]; // Або можна скорочено: sum += numbers[i];
    }

    cout << "Сума елементів масиву: " << sum << endl;

    delete[] numbers;

    return 0;
}