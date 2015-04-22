using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LegendOfCube.Engine.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfCube.Screens
{
	// MenuItems
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

	public class TextMenuItem : MenuItem2
	{
		private string text;
		private float height;
		private float scale;
		private Vector2 position;
		public TextMenuItem(string text, float scale, SpriteFont spriteFont) : base(false)
		{
			this.text = text;
			this.scale = scale;
			this.height = spriteFont.MeasureString(text).Y;
		}

		public override float ItemHeight() { return height; }

		public override void SetPosition(Vector2 position) { this.position = position; }

		public override void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont, bool unused)
		{
			Vector2 shadowFontPos = new Vector2(position.X + 1, position.Y + 1);
			Color shadowColor = Color.Black;
			Color color = Color.White;
			spriteBatch.DrawString(spriteFont, text, shadowFontPos, shadowColor, 0.11f, Vector2.Zero, scale, SpriteEffects.None, 0);
			spriteBatch.DrawString(spriteFont, text, position, color, 0.11f, Vector2.Zero, scale, SpriteEffects.None, 0);
		}
	}

	public class EmptyMenuItem : MenuItem2
	{
		private float height;
		private Vector2 position;
		public EmptyMenuItem(float height) : base(false) { this.height = height; }
		public override float ItemHeight() { return height; }
		public override void SetPosition(Vector2 position) { this.position = position; }
		public override void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont, bool unused) { }
	}

	public class ClickableTextMenuItem : MenuItem2
	{
		private string text;
		private float height;
		private float scale;
		private Vector2 position;
		private Action action;

		public ClickableTextMenuItem(string text, float scale, SpriteFont spriteFont, Action action) : base(true)
		{
			this.text = text;
			this.scale = scale;
			this.height = spriteFont.MeasureString(text).Y;
			this.action = action;
		}

		public sealed override float ItemHeight() { return height; }

		public sealed override void SetPosition(Vector2 position) { this.position = position; }

		public sealed override void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont, bool isSelected)
		{
			Vector2 shadowFontPos = new Vector2(position.X + 1, position.Y + 1);
			Color shadowColor = Color.Black;
			Color color = isSelected ? Color.DarkOrange : Color.White;
			spriteBatch.DrawString(spriteFont, text, shadowFontPos, shadowColor, 0.11f, Vector2.Zero, scale, SpriteEffects.None, 0);
			spriteBatch.DrawString(spriteFont, text, position, color, 0.11f, Vector2.Zero, scale, SpriteEffects.None, 0);
		}

		public sealed override void Update(MenuItemAction menuAction)
		{
			if (menuAction == MenuItemAction.ACTIVATE) action();
		}
		
		public sealed override Rectangle ActivationHitBox()
		{
			throw new NotImplementedException();
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

		private Vector2 nextItemPos = new Vector2(40.0f, 40.0f);

		// Constructors
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public BaseMenuScreen(Game game, ScreenSystem screenSystem) : base(game, screenSystem, false) { }
		internal abstract void InitializeScreen();

		// Methods
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		protected void AddMenuItemBelow(MenuItem2 menuItem)
		{
			menuItem.SetPosition(nextItemPos);
			nextItemPos.Y += menuItem.ItemHeight();
			menuItems.Add(menuItem);
			if (selected == -1 && menuItem.IsSelectable)
			{
				selected = menuItems.Count - 1;
			}
		}

		protected void AddTitleBelow(string text)
		{
			AddMenuItemBelow(new TextMenuItem(text, 2.0f, this.spriteFont));
		}

		protected void AddHeadingBelow(string text)
		{
			AddMenuItemBelow(new TextMenuItem(text, 1.5f, this.spriteFont));
		}

		protected void AddDescriptionBelow(string text)
		{
			AddMenuItemBelow(new TextMenuItem(text, 0.8f, this.spriteFont));
		}

		protected void AddSpaceBelow(float amount)
		{
			AddMenuItemBelow(new EmptyMenuItem(amount));
		}

		protected void AddClickableBelow(string text, Action action)
		{
			AddMenuItemBelow(new ClickableTextMenuItem(text, 1.0f, this.spriteFont, action));
		}

		// Inherited functions from Screen
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		internal sealed override void Update(GameTime gameTime)
		{
			InputHelper iH = InputHelper.Instance;

			if (iH.MenuUpPressed())
			{
				if (selected != -1)
				{
					do {
						selected--;
						if (selected < 0) selected = menuItems.Count - 1;
					} while (!menuItems.ElementAt(selected).IsSelectable);
				}
			}
			else if (iH.MenuDownPressed())
			{
				if (selected != -1)
				{
					do
					{
						selected++;
						if (selected >= menuItems.Count) selected = 0;
					} while (!menuItems.ElementAt(selected).IsSelectable);
				}
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
			InitializeScreen();
		}
	}
}
