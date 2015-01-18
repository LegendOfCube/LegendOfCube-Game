#include <iostream>
#include <boost/filesystem.hpp>
#include <SDL.h>
#undef main

int main()
{
	SDL_Init(SDL_INIT_EVERYTHING);

	SDL_Window* window = SDL_CreateWindow("Legend Of Cube", SDL_WINDOWPOS_UNDEFINED,
	                                     SDL_WINDOWPOS_UNDEFINED, 500, 500, SDL_WINDOW_RESIZABLE);
	if (window == NULL) {
		std::cout << "Couldn't create window: " << SDL_GetError() << std::endl;
		return 1;
	}

	if (boost::filesystem::exists("assets/this_is_not_an_asset.txt")) {
		std::cout << "Found assets\n";
	} else {
		std::cout << "Couldn't find assets\n";
	}

	SDL_Delay(3000);

	SDL_DestroyWindow(window);
	SDL_Quit();
	return 0;
}