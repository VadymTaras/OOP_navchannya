cmake_minimum_required(VERSION 4.00)
project(sqrt)

set(CMAKE_CXX_STANDARD 20)

add_executable(sqrt main.cpp
        root.h
        root.cpp
        age.cpp
        name.cpp)
