#include <iostream>
#include <string>
#include <windows.h>

template <typename T>
void findInRange(T arr[], int size, T minVal, T maxVal) {
    bool found = false;
    std::cout << "Елементи в діапазоні [" << minVal << ", " << maxVal << "]: ";

    for (int i = 0; i < size; i++) {
        if (arr[i] >= minVal && arr[i] <= maxVal) {
            std::cout << arr[i] << " ";
            found = true;
        }
    }

    if (!found) {
        std::cout << "жодного елемента не знайдено";
    }
    std::cout << std::endl;
}

int main() {
    SetConsoleCP(65001);
    SetConsoleOutputCP(65001);

    // Перевірка для цілих чисел (int)
    int intArr[] = {10, 5, 20, 15, 30, 8};
    int n1 = sizeof(intArr) / sizeof(intArr[0]);
    std::cout << "Масив int: 10, 5, 20, 15, 30, 8" << std::endl;
    findInRange(intArr, n1, 10, 20);
    std::cout << "-----------------------------------" << std::endl;

    // Перевірка для чисел з рухомою комою (double)
    double doubleArr[] = {1.5, 4.2, 7.8, 3.3, 0.9, 5.5};
    int n2 = sizeof(doubleArr) / sizeof(doubleArr[0]);
    std::cout << "Масив double: 1.5, 4.2, 7.8, 3.3, 0.9, 5.5" << std::endl;
    findInRange(doubleArr, n2, 1.0, 5.0);
    std::cout << "-----------------------------------" << std::endl;

    // Перевірка для символів (char)
    char charArr[] = {'a', 'z', 'f', 'm', 'b', 'k'};
    int n3 = sizeof(charArr) / sizeof(charArr[0]);
    std::cout << "Масив char: a, z, f, m, b, k" << std::endl;
    findInRange(charArr, n3, 'c', 'n');

    return 0;
}