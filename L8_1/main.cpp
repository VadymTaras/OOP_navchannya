#include <iostream>
#include <string>
#include <windows.h>

using namespace std;

int main() {
    SetConsoleOutputCP(65001);
    SetConsoleCP(65001);


    string* myArray = new string[5];


    myArray[0] = "Перший рядок";
    myArray[1] = "Другий рядок";
    myArray[2] = "Третій рядок";
    myArray[3] = "Четвертий рядок";
    myArray[4] = "П'ятий рядок";

    cout << "Вміст масиву:" << endl;
    for (int i = 0; i < 5; i++) {
        cout << "[" << i << "]: " << myArray[i] << endl;
    }

    delete[] myArray;

    return 0;
}