#include <iostream>
#include <boost/filesystem.hpp>
#include <SDL.h>
#undef main

#include <string>
#include <memory>
#include "Ogre.h"

int main()
{
	/*SDL_Init(SDL_INIT_EVERYTHING);

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
	SDL_Quit();*/

	std::string resourcePath = ""; // Should possibly be different on Mac OS X.

	std::unique_ptr<Ogre::Root> root{new Ogre::Root{resourcePath + "plugins.cfg", resourcePath + "ogre.cfg", "Ogre.log"}};
	if (!root->showConfigDialog()) return -1;

	/*Ogre::ConfigFile cfg;
	cfg.load(resourcePath + "resources.cfg");

	Ogre::ConfigFile::SectionIterator secItr = cfg.getSectionIterator();

	Ogre::String secName, typeName, archName;
	while (secItr.hasMoreElements()) {
		secName = secItr.peekNextKey();

	}*/
	
	return 0;
}