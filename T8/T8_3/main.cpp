#include <iostream>
#include <cmath>
#include <windows.h>

using namespace std;

class Complex {
public:
    double re;
    double im;


    Complex() {
        re = 0;
        im = 0;
    }

    void input() {
        cout << "Введіть дійсну частину: ";
        cin >> re;
        cout << "Введіть уявну частину: ";
        cin >> im;
    }

    void print() {
        if (im >= 0)
            cout << re << " + " << im << "i" << endl;
        else
            cout << re << " - " << abs(im) << "i" << endl;
    }

    Complex plus(Complex b) {
        Complex res;
        res.re = re + b.re;
        res.im = im + b.im;
        return res;
    }

    Complex minus(Complex b) {
        Complex res;
        res.re = re - b.re;
        res.im = im - b.im;
        return res;
    }

    Complex mult(Complex b) {
        Complex res;
        res.re = re * b.re - im * b.im;
        res.im = re * b.im + im * b.re;
        return res;
    }

    Complex div(Complex b) {
        Complex res;
        double znam = b.re * b.re + b.im * b.im;

        if (znam == 0) {
            cout << "Помилка! Ділення на нуль." << endl;
            return res;
        }

        res.re = (re * b.re + im * b.im) / znam;
        res.im = (im * b.re - re * b.im) / znam;
        return res;
    }

    Complex sopryazh() {
        Complex res;
        res.re = re;
        res.im = -im;
        return res;
    }

    Complex power(int n) {
        Complex res;
        double r = sqrt(re * re + im * im);
        double phi = atan2(im, re);

        double new_r = pow(r, n);
        double new_phi = phi * n;

        res.re = new_r * cos(new_phi);
        res.im = new_r * sin(new_phi);
        return res;
    }

    Complex sq_root() {
        Complex res;
        double r = sqrt(re * re + im * im);
        double phi = atan2(im, re);

        double new_r = sqrt(r);
        double new_phi = phi / 2;

        res.re = new_r * cos(new_phi);
        res.im = new_r * sin(new_phi);
        return res;
    }
};

int main() {
    SetConsoleOutputCP(65001);

    Complex c1;
    Complex c2;

    cout << "--- Введіть перше число ---" << endl;
    c1.input();

    cout << "--- Введіть друге число ---" << endl;
    c2.input();

    cout << "\nРезультати:" << endl;

    cout << "Сума: ";
    Complex sum = c1.plus(c2);
    sum.print();

    cout << "Різниця: ";
    Complex diff = c1.minus(c2);
    diff.print();

    cout << "Добуток: ";
    Complex m = c1.mult(c2);
    m.print();

    cout << "Частка: ";
    Complex d = c1.div(c2);
    d.print();

    cout << "Спряжене до першого: ";
    Complex s = c1.sopryazh();
    s.print();

    int n;
    cout << "\nВведіть ступінь для першого числа: ";
    cin >> n;
    cout << "Перше число в ступені " << n << ": ";
    Complex p = c1.power(n);
    p.print();

    cout << "Корінь з першого числа: ";
    Complex root = c1.sq_root();
    root.print();

    return 0;
}