#include <iostream>
#include <windows.h>

class Employer {
private:
    size_t vacancies_;

public:
    Employer(size_t initial_vacancies) : vacancies_(initial_vacancies) {}

    void hire() {
        if (vacancies_ > 0) {
            std::cout << "Наймання працівника успішне." << std::endl;
            vacancies_--;
        } else {
            std::cout << "Помилка: Немає вакантних посад." << std::endl;
        }
    }

    void fire(const size_t i) {
        if (i == 0) {
            return;
        }
        vacancies_ += i;
        std::cout << i << " робочих місць звільнено. Поточна кількість вакансій: "
                  << vacancies_ << std::endl;
    }

    size_t get_vacancies() const {
        return vacancies_;
    }
};

int main() {
    SetConsoleCP(65001);
    SetConsoleOutputCP(65001);

    Employer my_company(2);

    std::cout << "--- Employer ---" << std::endl;
    std::cout << "Початкові вакансії: " << my_company.get_vacancies() << std::endl;

    my_company.hire();
    std::cout << "Вакансії після найму: " << my_company.get_vacancies() << std::endl;

    my_company.hire();
    std::cout << "Вакансії після найму: " << my_company.get_vacancies() << std::endl;

    my_company.hire();
    std::cout << "Вакансії після невдалого найму: " << my_company.get_vacancies() << std::endl;

    my_company.fire(3);
    std::cout << "Вакансії після звільнення: " << my_company.get_vacancies() << std::endl;

    my_company.hire();
    std::cout << "Вакансії після найму: " << my_company.get_vacancies() << std::endl;

    return 0;
}