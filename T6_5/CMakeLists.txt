cmake_minimum_required(VERSION 3.5)
project(T6_5)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

add_executable(T6_5 main.cpp)

# Підключити WinAPI та GDI+
target_link_libraries(T6_5 PRIVATE
        gdiplus
        gdi32
        user32
)
