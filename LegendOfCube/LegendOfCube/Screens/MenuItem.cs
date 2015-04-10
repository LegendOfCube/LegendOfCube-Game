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

		public MenuItem(string text, Rectangle rectangle, Action onClick)
		{
			Text = text;
			Rectangle = rectangle;
			Selected = false;
			OnClick = onClick;
		}

		public string Text { get; private set; }

		public Rectangle Rectangle { get; private set; }

		public bool Selected { get; set; }
		public Action OnClick { get; private set; }

		public void Draw(SpriteBatch spriteBatch, SpriteFont font)
		{
			Color shadowColor = Color.Black;
			Color color = Selected ? Color.DarkOrange : Color.White;
			spriteBatch.DrawString(font, Text, new Vector2(Rectangle.Left + 1, Rectangle.Top + 1), shadowColor);
			spriteBatch.DrawString(font, Text, new Vector2(Rectangle.Left, Rectangle.Top), color);
		}

		public void LoadContent()
		{

		}

	}

}