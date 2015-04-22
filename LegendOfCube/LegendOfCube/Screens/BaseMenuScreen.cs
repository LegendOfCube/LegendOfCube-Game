using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LegendOfCube.Engine.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfCube.Screens
{
	// MenuItem
	// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

	public enum MenuItemAction
	{
		ACTIVATE, LEFT, RIGHT
	}

	public abstract class MenuItem2
	{
		public MenuItem2(bool isSelectable)
		{
			this.IsSelectable = isSelectable;
		}

		public abstract float ItemHeight();
		public abstract void SetPosition(Vector2 position);
		public abstract void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont, bool isSelected);
		public bool IsSelectable { get; private set; }
		public virtual void Update(MenuItemAction action) { throw new NotImplementedException(); }
		public virtual Rectangle ActivationHitBox() { throw new NotImplementedException(); }
	}

	public class TitleMenuItem : MenuItem2
	{
		private string text;
		private float height;
		private const float SCALE = 2.0f;
		private Vector2 position;
		public TitleMenuItem(string text, SpriteFont spriteFont) : base(false)
		{
			this.text = text;
			this.height = spriteFont.MeasureString(text).Y;
		}

		public override float ItemHeight() { return height; }

		public override void SetPosition(Vector2 position) { this.position = position; }

		public override void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont, bool unused)
		{
			Vector2 shadowFontPos = new Vector2(position.X + 1, position.Y + 1);
			Color shadowColor = Color.Black;
			Color color = Color.White;
			spriteBatch.DrawString(spriteFont, text, shadowFontPos, shadowColor, 0.11f, Vector2.Zero, SCALE, SpriteEffects.None, 0);
			spriteBatch.DrawString(spriteFont, text, position, color, 0.11f, Vector2.Zero, SCALE, SpriteEffects.None, 0);
		}
	}

	// BaseMenuScreen
	// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

	public abstract class BaseMenuScreen : Screen
	{
		// Members
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		private SpriteBatch spriteBatch;
		private SpriteFont spriteFont;

		private List<MenuItem2> menuItems = new List<MenuItem2>();
		private int selected = -1;

		// Constructors
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public BaseMenuScreen(Game game, ScreenSystem screenSystem) : base(game, screenSystem, false) { }

		// Public functions
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		protected void AddTitleBelow(string text)
		{

		}

		public void AddMenuItemBelow(MenuItem2 menuItem)
		{
			
		}

		// Inherited functions from Screen
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		internal sealed override void Update(GameTime gameTime)
		{
			InputHelper iH = InputHelper.Instance;

			if (iH.MenuUpPressed())
			{

			}
			else if (iH.MenuDownPressed())
			{

			}
			else if (iH.MenuLeftPressed())
			{
				if (selected != -1) menuItems.ElementAt(selected).Update(MenuItemAction.LEFT);
			}
			else if (iH.MenuRightPressed())
			{
				if (selected != -1) menuItems.ElementAt(selected).Update(MenuItemAction.RIGHT);
			}
			else if (iH.MenuActivatePressed())
			{
				if (selected != -1) menuItems.ElementAt(selected).Update(MenuItemAction.ACTIVATE);
			}
			else if (iH.MenuCancelPressed())
			{
				ScreenSystem.RemoveCurrentScreen();
			}
		}

		internal sealed override void Draw(GameTime gameTime)
		{
			spriteBatch.Begin();
			for (int i = 0; i < menuItems.Count; i++)
			{
				menuItems.ElementAt(i).Draw(spriteBatch, spriteFont, i == selected);
			}
			spriteBatch.End();
		}

		internal sealed override void LoadContent()
		{
			spriteBatch = new SpriteBatch(Game.GraphicsDevice);
			spriteFont = Game.Content.Load<SpriteFont>("Arial");
		}
	}
}
