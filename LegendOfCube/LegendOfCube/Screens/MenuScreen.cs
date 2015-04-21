using System;
using System.Collections.Generic;
using LegendOfCube.Engine;
using LegendOfCube.Engine.Events;
using LegendOfCube.Engine.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfCube.Screens
{
	abstract class MenuScreen : Screen
	{
		private readonly MenuInputSystem menuInputSystem;
		private readonly List<MenuItem> menuItems;
		private MenuAudioSystem menuAudioSystem;
		protected readonly InputHelper InputHelper;

		private SpriteBatch spriteBatch;
		private SpriteFont font;
		private int selection;
		private Vector2 nextItemPos = new Vector2(40, 40);

		protected MenuScreen(Game game, ScreenSystem screenSystem, InputHelper inputHelper) : base(game, screenSystem, false)
		{
			menuItems = new List<MenuItem>();
			menuInputSystem = new MenuInputSystem(game, screenSystem, inputHelper);
			this.InputHelper = inputHelper;
			selection = 0;
		}

		protected void AddMenuItem(MenuItem menuItem)
		{
			menuItems.Add(menuItem);
			nextItemPos.X = menuItem.Rectangle.Left;
			nextItemPos.Y = menuItem.Rectangle.Top + menuItem.Rectangle.Height;
		}

		protected void AddItemBelow(String text, Action onClick)
		{
			Vector2 size = font.MeasureString(text);
			AddMenuItem(new MenuItem(text, new Rectangle((int)nextItemPos.X, (int)nextItemPos.Y, (int)size.X, (int)size.Y), onClick));
		}

		protected void AddSpaceBelow(float amount)
		{
			nextItemPos.Y += amount;
		}

		internal override void Update(GameTime gameTime)
		{
			menuInputSystem.ApplyInput(gameTime, menuItems, ref selection);
			menuAudioSystem.Update(selection);
		}

		internal override void Draw(GameTime gameTime)
		{
			spriteBatch.Begin();
			foreach (var menuItem in menuItems)
			{
				menuItem.Draw(spriteBatch, font);
			}
			spriteBatch.End();
		}

		internal override void LoadContent()
		{
			menuAudioSystem = new MenuAudioSystem(Game.Content, InputHelper);
			spriteBatch = new SpriteBatch(Game.GraphicsDevice);
			font = Game.Content.Load<SpriteFont>("Arial");
		}
	}
}
