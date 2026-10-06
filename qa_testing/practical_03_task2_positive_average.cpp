/**
 * Дисципліна: Основи тестування програмного забезпечення
 * Практичне заняття №3. Завдання 2
 * Тема: Комплексне тестування програми та локалізація дефекту (середнє додатних з логуванням)
 * Студент: ТАРАС Вадим (група аІк43)
 * 
 * Еталонна реалізація A (ізоляція логування у валідній гілці, захист від ділення на нуль)
 */

#include <iostream>
using namespace std;

int main()
{
    double value;
    double sum = 0;
    int count = 0;

    while (true)
    {
        cin >> value;
        if (value == 0) break;
        if (value > 0)
        {
            sum += value;
            count++;

            cout << "Value = " << value << endl;
            cout << "Sum = " << sum << endl;
            cout << "Count = " << count << endl;
        }
    }

    if (count == 0)
    {
        cout << "No positive numbers" << endl;
    }
    else
    {
        double average = sum / count;
        cout << "Average = " << average << endl;
    }
    return 0;
}
