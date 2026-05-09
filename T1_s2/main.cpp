#include <iostream>
#include <list>
#include <Windows.h>

using namespace std;

int main() {
    // Фіксимо кракозябри в консолі Windows
    SetConsoleCP(65001);
    SetConsoleOutputCP(65001);

    int n;
    cout << "Введіть кількість елементів n: ";
    cin >> n;

    if (n <= 0) {
        cout << "Помилка: кількість елементів має бути більшою за нуль." << endl;
        return 1;
    }

    list<double> my_list;
    cout << "Введіть " << n << " дійсних чисел:" << endl;

    // Заповнюємо список
    for (int i = 0; i < n; i++) {
        double temp;
        cin >> temp;
        my_list.push_back(temp);
    }

    // Оголошуємо класичний ітератор для проходу по списку
    list<double>::iterator it;

    // Шукаємо, чи є хоча б один елемент, менший за -3
    bool condition_met = false;
    for (it = my_list.begin(); it != my_list.end(); it++) {
        if (*it < -3.0) {
            condition_met = true;
            break; // Знайшли — зупиняємо цикл
        }
    }

    // Залежно від результату пошуку, модифікуємо список
    if (condition_met == true) {
        // Якщо знайшли, то всі від'ємні підносимо до квадрата
        for (it = my_list.begin(); it != my_list.end(); it++) {
            if (*it < 0.0) {
                *it = (*it) * (*it);
            }
        }
    } else {
        // Якщо не знайшли, множимо всі елементи на 0.1
        for (it = my_list.begin(); it != my_list.end(); it++) {
            *it = (*it) * 0.1;
        }
    }

    // Виводимо список у зворотному порядку
    cout << "Результат у зворотньому порядку:" << endl;

    // Використовуємо класичний реверсивний ітератор
    list<double>::reverse_iterator rit;
    for (rit = my_list.rbegin(); rit != my_list.rend(); rit++) {
        cout << *rit << " ";
    }
    cout << endl;

    return 0;
}