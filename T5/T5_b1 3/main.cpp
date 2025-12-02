#include <iostream>
#include <cstdlib>
#include <ctime>
#include <windows.h>

using namespace std;

int main() {
    SetConsoleCP(65001);
    SetConsoleOutputCP(65001);

    srand(time(0));

    int arr[15];
    int count = 0;

    cout << "Масив: ";

    for (int i = 0; i < 15; i++) {
        arr[i] = rand() % 10;
        cout << arr[i] << " ";
    }

    cout << endl;

    for (int i = 0; i < 15; i++) {
        if (arr[i] % 2 == 0) {
            count++;
        }
    }

    cout << "Кількість парних елементів: " << count << endl;

    return 0;
}