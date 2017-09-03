cmake_minimum_required (VERSION 2.6)
project (simpleexample)

add_library(sum sum.cpp)

add_executable(test test.cpp)
target_link_libraries(test sum)
