#include <iostream>
#include <string>
#include <windows.h>

class University {
protected:
    std::string name;
public:
    University(std::string n) : name(n) {}
    virtual void printInfo() {
        std::cout << "Університет: " << name << std::endl;
    }
    virtual ~University() {}
};

class Faculty : public University {
protected:
    std::string facultyName;
    int studentsCount;
public:
    Faculty(std::string uName, std::string fName, int sCount)
        : University(uName), facultyName(fName), studentsCount(sCount) {}
    void printInfo() override {
        std::cout << "Факультет: " << facultyName << std::endl;
        std::cout << "Університет: " << name << std::endl;
        std::cout << "Кількість студентів: " << studentsCount << std::endl;
    }
};

class Department : public Faculty {
private:
    std::string departmentName;
    std::string headName;
public:
    Department(std::string uName, std::string fName, int sCount, std::string dName, std::string hName)
        : Faculty(uName, fName, sCount), departmentName(dName), headName(hName) {}
    void printInfo() override {
        std::cout << "Кафедра: " << departmentName << std::endl;
        std::cout << "Завідувач: " << headName << std::endl;
        std::cout << "Факультет: " << facultyName << " (" << name << ")" << std::endl;
    }
};

int main() {
    SetConsoleCP(65001);
    SetConsoleOutputCP(65001);

    University* list[3];

    list[0] = new University("Київський політехнічний інститут");
    list[1] = new Faculty("Львівська політехніка", "ІТ-технологій", 1200);
    list[2] = new Department("Харківський університет", "Фізичний", 450, "Кафедра астрономії", "проф. Іваненко О.П.");

    for (int i = 0; i < 3; i++) {
        list[i]->printInfo();
        std::cout << "-----------------------------------" << std::endl;
    }

    for (int i = 0; i < 3; i++) {
        delete list[i];
    }

    return 0;
}