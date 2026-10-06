/**
 * Дисципліна: Основи тестування програмного забезпечення
 * Практичне заняття №3. Завдання 1
 * Тема: Тестування програми розрахунку знижок за допомогою таблиці рішень
 * Студент: ТАРАС Вадим (група аІк43)
 * 
 * Еталонна реалізація B (коректне розмежування граничного значення 1000 грн)
 */

#include <iostream>
using namespace std;

int main()
{
    double price;
    int card;
    cin >> price >> card;
    double discount;

    if (price < 1000)
    {
        if (card == 1) discount = 5;
        else discount = 0;
    }
    else
    {
        if (card == 1) discount = 15;
        else discount = 10;
    }

    double finalPrice = price - price * discount / 100;
    cout << "Discount = " << discount << "%" << endl;
    cout << "Final price = " << finalPrice << endl;

    return 0;
}
