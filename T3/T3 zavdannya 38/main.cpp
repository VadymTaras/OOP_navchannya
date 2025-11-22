#include <iostream>
#include <string>
#include <windows.h>

using namespace std;

int main() {
    SetConsoleOutputCP(65001);

    string text;

    cout << "Введіть рядок тексту: ";
    getline(cin, text);

    int len = text.length();


    for (int i = 0; i < len / 2; i++) {
        char temp = text[i];
        text[i] = text[len - 1 - i];
        text[len - 1 - i] = temp;
    }

    cout << "Інвертований рядок: " << text << endl;

    return 0;
}