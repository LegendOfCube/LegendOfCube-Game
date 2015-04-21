using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfCube.Screens
{ 
	public class MenuItem
	{
		private const float FONT_SCALE = 1.0f;
		private const float TITLE_FONT_SCALE = 1.5f;

		public MenuItem(string text, Rectangle rectangle, Action onClick, bool isTitle)
		{
			IsTitle = isTitle;
			Text = text;
			Rectangle = rectangle;
			Selected = false;
			OnClick = onClick;
		}

		public bool IsTitle { get; private set; }

		public string Text { get; private set; }

		public Rectangle Rectangle { get; private set; }

		public bool Selected { get; set; }
		public Action OnClick { get; private set; }

		public void Draw(SpriteBatch spriteBatch, SpriteFont font)
		{
			Vector2 fontPos = new Vector2(Rectangle.Left, Rectangle.Top);
			Vector2 shadowFontPos = new Vector2(Rectangle.Left + 1, Rectangle.Top + 1);
			Color shadowColor = Color.Black;
			Color color = Selected ? Color.DarkOrange : Color.White;

			spriteBatch.DrawString(font, Text, shadowFontPos, shadowColor, 0.11f, Vector2.Zero, IsTitle ? TITLE_FONT_SCALE : FONT_SCALE, SpriteEffects.None, 0);
			spriteBatch.DrawString(font, Text, fontPos, color, 0.11f, Vector2.Zero, IsTitle ? TITLE_FONT_SCALE : FONT_SCALE, SpriteEffects.None, 0);
		}
	}
}