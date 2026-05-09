#include <iostream>
#include <string>
#include <windows.h>
#include <cmath>

class RevolutionBody {
protected:
    std::string type;
    double radius;
public:
    RevolutionBody(std::string t, double r) : type(t), radius(r) {}

    virtual double getVolume() {
        return 0;
    }

    virtual void printInfo() {
        std::cout << "Тип: " << type << std::endl;
        std::cout << "Радіус основи: " << radius << std::endl;
    }

    virtual ~RevolutionBody() {}
};

class Cone : public RevolutionBody {
private:
    double height;
public:
    Cone(double r, double h) : RevolutionBody("Конус", r), height(h) {}

    double getVolume() override {
        return (1.0 / 3.0) * M_PI * pow(radius, 2) * height;
    }

    void printInfo() override {
        RevolutionBody::printInfo();
        std::cout << "Висота: " << height << std::endl;
        std::cout << "Об'єм конуса: " << getVolume() << std::endl;
    }
};

int main() {
    SetConsoleCP(65001);
    SetConsoleOutputCP(65001);

    const int size = 3;
    RevolutionBody* shapes[size];

    shapes[0] = new RevolutionBody("Загальне тіло обертання", 5.5);
    shapes[1] = new Cone(3.0, 10.0);
    shapes[2] = new Cone(7.2, 4.5);

    for (int i = 0; i < size; i++) {
        shapes[i]->printInfo();
        std::cout << "-----------------------------------" << std::endl;
    }

    for (int i = 0; i < size; i++) {
        delete shapes[i];
    }

    return 0;
}