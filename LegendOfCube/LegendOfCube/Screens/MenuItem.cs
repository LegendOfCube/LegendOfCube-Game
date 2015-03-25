using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfCube.Screens
{ 
	class MenuItem
	{
		private string text;
		private Vector2 pos;
		private bool selected;

		public MenuItem(string text)
		{
			this.text = text;
			selected = false;
		}

		public string Text
		{
			set {text = value;}
			get {return text;}
		}

		public Vector2 Position
		{
			set {pos = value;}
			get {return pos;}
		}

		public bool Selected
		{
			set { selected = value; }
			get { return selected; }
		}

		public void Update()
		{

		}

		public void Draw(SpriteBatch spriteBatch, SpriteFont font)
		{
			Color color = selected ? Color.DarkOrange : Color.Black;
			spriteBatch.DrawString(font, text, pos, color);
		}

		public void loadContent()
		{

		}

	}
}