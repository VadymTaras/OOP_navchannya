#include <iostream>
#include <cmath>
#include <windows.h>

using namespace std;

class Triangle {
public:
    double x1, y1;
    double x2, y2;
    double x3, y3;

    void Init(double ax, double ay, double bx, double by, double cx, double cy) {
        x1 = ax; y1 = ay;
        x2 = bx; y2 = by;
        x3 = cx; y3 = cy;
    }

    double getPerimeter() {

        double a = sqrt(pow(x2 - x1, 2) + pow(y2 - y1, 2));
        double b = sqrt(pow(x3 - x2, 2) + pow(y3 - y2, 2));
        double c = sqrt(pow(x1 - x3, 2) + pow(y1 - y3, 2));

        return a + b + c;
    }

    double getArea() {

        double a = sqrt(pow(x2 - x1, 2) + pow(y2 - y1, 2));
        double b = sqrt(pow(x3 - x2, 2) + pow(y3 - y2, 2));
        double c = sqrt(pow(x1 - x3, 2) + pow(y1 - y3, 2));

        double p = (a + b + c) / 2.0;
        return sqrt(p * (p - a) * (p - b) * (p - c));
    }

    void printMedianPoint() {
        double mx = (x1 + x2 + x3) / 3.0;
        double my = (y1 + y2 + y3) / 3.0;
        cout << "Медіана перетинається в: " << mx << " " << my << endl;
    }
};

int main() {
    SetConsoleOutputCP(65001);

    Triangle t;
    cout << "Введіть координати (x1 y1 x2 y2 x3 y3): ";

    double a, b, c, d, e, f;
    cin >> a >> b >> c >> d >> e >> f;

    t.Init(a, b, c, d, e, f);

    cout << "Периметр: " << t.getPerimeter() << endl;
    cout << "Площа: " << t.getArea() << endl;
    t.printMedianPoint();

    return 0;
}