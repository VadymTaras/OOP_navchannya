#include <iostream>
#include <locale>

class coord {
    int x, y;
public:
    coord() { x = 0; y = 0; }
    coord(int i, int j) { x = i; y = j; }
    void get_xy(int &i, int &j) { i = x; j = y; }

    bool operator==(const coord &other);
};

bool coord::operator==(const coord &other) {
    if (x == other.x && y == other.y) {
        return true;
    }
    return false;
}

int main() {

    system("chcp 65001");
    setlocale(LC_ALL, "uk_UA.UTF-8");

    coord c1(10, 20);
    coord c2(10, 20);
    coord c3(5, 15);

    if (c1 == c2) {
        std::cout << "Координати c1 та c2 рівні." << std::endl;
    } else {
        std::cout << "Координати c1 та c2 не рівні." << std::endl;
    }

    if (c1 == c3) {
        std::cout << "Координати c1 та c3 рівні." << std::endl;
    } else {
        std::cout << "Координати c1 та c3 не рівні." << std::endl;
    }


    return 0;
}