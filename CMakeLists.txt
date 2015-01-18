cmake_minimum_required(VERSION 3.0 FATAL_ERROR)
project("Legend of Cube")

# Directories
set(SRC_DIR ${CMAKE_CURRENT_SOURCE_DIR}/src)
set(ASSETS_DIR ${CMAKE_CURRENT_SOURCE_DIR}/assets)
# set(EXTERNALS_DIR ${CMAKE_CURRENT_SOURCE_DIR}/externals)
set(INSTALL_BIN_DIR ${CMAKE_BINARY_DIR}/bin)

# Adds our cmake modules to module path.
list(APPEND CMAKE_MODULE_PATH "${CMAKE_CURRENT_LIST_DIR}/cmake")

# Boost
set(Boost_USE_STATIC_LIBS OFF)
set(Boost_USE_MULTITHREADED ON)
set(Boost_USE_STATIC_RUNTIME OFF)
set(BOOST_ALL_DYN_LINK ON)
find_package(Boost REQUIRED COMPONENTS filesystem system)
link_directories(${Boost_LIBRARY_DIRS})

# SDL2 (sets ${SDL2_INCLUDE_DIR}, ${SDL2_LIBRARY})
find_package(SDL2 REQUIRED)

# Include directories
include_directories(
	${SRC_DIR}
	${Boost_INCLUDE_DIRS}
	${SDL2_INCLUDE_DIR}
)

# Main Executable
add_executable(
	LegendOfCube

	${SRC_DIR}/Main.cpp
)

# Linking libraries to main executable
target_link_libraries(
	LegendOfCube

	${Boost_LIBRARIES}
	${SDL2_LIBRARY}
)

# Copies assets to '/bin' dir in build directory. Only run when CMakeLists.txt is invoked.
INSTALL(TARGETS LegendOfCube DESTINATION ${INSTALL_BIN_DIR})
file(COPY ${ASSETS_DIR} DESTINATION ${INSTALL_BIN_DIR})

# Visual Studio specific file copying
if(MSVC)
	# Copies assets to directory where solution is generated.
	file(COPY ${ASSETS_DIR} DESTINATION ${CMAKE_BINARY_DIR})

	# TODO: Copy runtime binaries

endif()