#include <iostream>
#include <windows.h>

using namespace std;

int main() {
    SetConsoleOutputCP(65001);

    int count = 3;

    int* numbers = new int[count];

    numbers[0] = 1;
    numbers[1] = 15;
    numbers[2] = 2;

    cout << "Кількість елементів масиву: " << count << endl;

    delete[] numbers;

    return 0;
}