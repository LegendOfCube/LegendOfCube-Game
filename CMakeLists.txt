cmake_minimum_required(VERSION 3.0 FATAL_ERROR)
project("Legend of Cube")

# Directories
set(SRC_DIR ${CMAKE_CURRENT_SOURCE_DIR}/src)
set(EXTERNALS_DIR ${CMAKE_CURRENT_SOURCE_DIR}/externals)

# Main Executable
add_executable(
	LegendOfCube

	${SRC_DIR}/Main.cpp
)

