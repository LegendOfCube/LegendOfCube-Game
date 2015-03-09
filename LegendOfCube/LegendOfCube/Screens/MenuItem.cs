using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LegendOfCube.Screens
{ 
	class MenuItem
	{
		string text;
		Vector2 pos;

		public MenuItem(string text)
		{
			this.text = text;
		}

		public string Text
		{
			set {text = value;}
			get {return text;}
		}

		public Vector2 position
		{
			set {pos = value;}
			get {return pos;}
		}

		public void update()
		{

		}

		public void draw()
		{

		}

	}
}