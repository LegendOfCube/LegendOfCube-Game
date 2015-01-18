#include <iostream>
#include <boost/filesystem.hpp>

int main()
{
	if (boost::filesystem::exists("assets/this_is_not_an_asset.txt")) {
		std::cout << "Found assets\n";
	} else {
		std::cout << "Couldn't find assets\n";
	}
	std::cin.get();
	return 0;
}