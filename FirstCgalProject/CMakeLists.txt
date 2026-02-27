cmake_minimum_required(VERSION 3.15)
project(FirstCgalProject)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

# Find CGAL
find_package(CGAL REQUIRED)

# Add executable
add_executable(FirstCgalProject FirstCgalProject.cpp)

# Link CGAL
target_link_libraries(FirstCgalProject CGAL::CGAL)
