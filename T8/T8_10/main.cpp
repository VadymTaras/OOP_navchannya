#include <iostream>
#include <string>
#include <windows.h>

using namespace std;

class Pokupec {
public:
    string prizv;
    string imya;
    string po_batk;
    string adresa;
    string card;
    int rahunok;

    void SetData(string p, string i, string b, string adr, string c, int r) {
        prizv = p;
        imya = i;
        po_batk = b;
        adresa = adr;
        card = c;
        rahunok = r;
    }

    void Print() {
        cout << prizv << " " << imya << " | Картка: " << card << endl;
    }
};

int main() {
    SetConsoleOutputCP(65001);

    Pokupec mas[5];

    mas[0].SetData("Shevchenko", "Taras", "G", "Kyiv", "1111222233334444", 101);
    mas[1].SetData("Boyko", "Ivan", "P", "Lviv", "5555666677778888", 102);
    mas[2].SetData("Antonov", "Oleg", "A", "Odesa", "4444555566667777", 103);
    mas[3].SetData("Melnyk", "Anna", "V", "Kharkiv", "9999888877776666", 104);
    mas[4].SetData("Zelensky", "Volodymyr", "O", "Kyiv", "2222333344445555", 105);

    for (int i = 0; i < 5; i++) {
        for (int j = 0; j < 5 - 1; j++) {
            if (mas[j].prizv > mas[j + 1].prizv) {
                Pokupec temp = mas[j];
                mas[j] = mas[j + 1];
                mas[j + 1] = temp;
            }
        }
    }

    cout << "=== Список за алфавітом ===" << endl;
    for (int i = 0; i < 5; i++) {
        mas[i].Print();
    }

    string min_c, max_c;
    cout << "\nВведіть діапазон карток (від і до): " << endl;
    cin >> min_c >> max_c;

    cout << "\nЗнайдені покупці:" << endl;
    for (int i = 0; i < 5; i++) {
        if (mas[i].card >= min_c && mas[i].card <= max_c) {
            mas[i].Print();
        }
    }

    return 0;
}