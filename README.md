# Legend Of Cube
Legend of Cube is a platformer in 3D written in C++ and using Ogre3D for rendering.

## Building
It is advisable to let CMake generate the wanted build solution in a directory called `build` inside the project root directory as this directory is ignored by git.

### Requirements
- __CMake__ (v3.0 or newer) for generating build solution
- Windows: __Visual Studio 2013__ or newer
- Mac OS X: __Xcode 6__ or newer
- (Other compilers and setups may work, but aren't officially supported)
- __Boost__ (v1.57)
- __SDL2__
- __Ogre3D__ (v1.9)

### Windows
#### Installing dependencies
##### CMake
Install latest version of CMake and add it to your `Path` variable. Warning: Backup your `Path` before installing. The installer is buggy and has on occasion deleted all the contents of my `Path` variable.
##### Boost
Download and install the pre-built Boost binaries (as of writing you want the file called `boost_1_57_0-msvc-12.0-64.exe`). Then you need create an environment variable called `BOOST_ROOT` pointing to the root of the Boost installation. You also need to create an environment variable called `BOOST_LIBRARYDIR` which points to the directory containing the compiled libraries (`.lib` and `.dll`), this will by default be a directory called `lib64-msvc-12.0` inside the Boost root directory.
##### SDL 2
Download and install the development libraries for Visual C++ from the official website. Then create an environment variable called `SDL2` and point it to the root of the SDL2 installation.

#### Generating Visual Studio solution
Create a directory called `build` inside the project root directory and then open `cmd` inside this `build` directory. Run the following command:

	cmake .. -G "Visual Studio 12 2013 Win64"

TODO: Add more instructions

### Mac OS X
#### Installing dependencies
- Install Homebrew
- CMake: `brew install cmake`
- Boost: `brew install boost`
- SDL2: `brew install sdl2`
- Ogre3D: Download binaries from official website and install manually.

#### Generating Xcode solution and building
Open a terminal and goto the root directory of the project. Then run the following commands:

	mkdir build
	cd build
	cmake .. -GXcode

Open the generated Xcode solution and try to run. The program will likely crash as it can't find its assets. Copy the `assets` folder to the directory of the binary built by Xcode (likely called `Debug` or `Release` depending on build configuration). NB: If assets are changed or updated you have to manually copy them from the root of the project again.

#### (Alt) Generate makefile and build via terminal
Open a terminal and goto the root directory of the project. Then run the following commands:

	mkdir build
	cd build
	cmake ..

You can then build with the following command:

	make install

The generated binary will be placed in the `bin` directory. CMake should automatically copy the assets to this folder when first run. Unfortunately it won't do this when assets are changed so you still have to copy the assets manually when they are changed.

## License
TBD