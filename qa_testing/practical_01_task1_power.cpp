/**
 * Дисципліна: Основи тестування програмного забезпечення
 * Практичне заняття №1. Завдання 1
 * Тема: Тестування функції обчислення степеня числа
 * Студент: ТАРАС Вадим (група аІк43)
 * 
 * Еталонна реалізація B (успішно проходить усі тести специфікації)
 */

#include <iostream>
#include <windows.h>
using namespace std;

int main()
{
    SetConsoleOutputCP(1251);
    double x;
    int n;

    cin >> x >> n;
    double result = 1;

    if (n >= 0)
    {
        for (int i = 0; i < n; i++)
            result *= x;

        cout << "Результат: " << result << endl;
    }
    else
    {
        cout << "Помилка: показник степеня має бути невід'ємним." << endl;
    }
    return 0;
}
