#include <iostream>
#include <windows.h>
#include <ctime>

using namespace std;

int main() {
    SetConsoleCP(65001);
    SetConsoleOutputCP(65001);

    srand(time(0));

    int arr[7][4];

    cout << "Масив:" << endl;

    for (int i = 0; i < 7; i++) {
        for (int j = 0; j < 4; j++) {
            arr[i][j] = rand() % 11 - 5;
            cout << arr[i][j] << "\t";
        }
        cout << endl;
    }

    int maxDobutok = -1;
    int indexRow = 0;

    for (int i = 0; i < 7; i++) {
        int currentDobutok = 1;

        for (int j = 0; j < 4; j++) {
            currentDobutok = currentDobutok * arr[i][j];
        }

        if (currentDobutok < 0) {
            currentDobutok = -currentDobutok;
        }

        if (currentDobutok > maxDobutok) {
            maxDobutok = currentDobutok;
            indexRow = i;
        }
    }

    cout << "\nРезультат:" << endl;
    cout << "Індекс рядка: " << indexRow << endl;
    cout << "Найбільший добуток (по модулю): " << maxDobutok << endl;

    return 0;
}