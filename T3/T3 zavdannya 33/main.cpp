#include <iostream>
#include <windows.h>

using namespace std;

int main() {

    SetConsoleOutputCP(65001);

    const int MAX_SIZE = 100;
    double arr[MAX_SIZE];
    int n;

    cout << "Введіть кількість елементів масиву (не більше 100): ";
    cin >> n;

    if (n > MAX_SIZE) {
        cout << "Помилка: занадто велике число!" << endl;
        return 1;
    }

    cout << "Введіть " << n << " дійсних чисел (можна з комою, наприклад 5.5):" << endl;

    for (int i = 0; i < n; i++) {
        cout << "Елемент [" << i << "]: ";
        cin >> arr[i];
    }

    cout << "\nОтриманий масив:" << endl;

    for (int i = 0; i < n; i++) {
        cout << arr[i] << " ";
    }

    cout << endl;

    return 0;
}