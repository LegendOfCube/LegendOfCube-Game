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
		public virtual Rectangle ActivationHitBox() { return new Rectangle(-1000000, -1000000, 1, 1); }
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
			this.height = spriteFont.MeasureString(text).Y * scale;
		}

		public override float ItemHeight() { return height; }

		public override void SetPosition(Vector2 position) { this.position = position; }

		public override void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont, bool unused)
		{
			Vector2 shadowFontPos = new Vector2(position.X + 1, position.Y + 1);
			Color shadowColor = Color.Black;
			Color color = Color.White;
			spriteBatch.DrawString(spriteFont, text, shadowFontPos, shadowColor, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0);
			spriteBatch.DrawString(spriteFont, text, position, color, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0);
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
		private Func<string> func;

		private SpriteFont spriteFontForMeasuring;

		public ClickableTextMenuItem(string text, float scale, SpriteFont spriteFont, Func<String> func) : base(true)
		{
			this.text = text;
			this.scale = scale;
			this.spriteFontForMeasuring = spriteFont;
			this.height = spriteFont.MeasureString(text).Y;
			this.func = func;
		}

		public sealed override float ItemHeight() { return height; }

		public sealed override void SetPosition(Vector2 position) { this.position = position; }

		public sealed override void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont, bool isSelected)
		{
			Vector2 shadowFontPos = new Vector2(position.X + 1, position.Y + 1);
			Color shadowColor = Color.Black;
			Color color = isSelected ? Color.DarkOrange : Color.White;
			spriteBatch.DrawString(spriteFont, text, shadowFontPos, shadowColor, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0);
			spriteBatch.DrawString(spriteFont, text, position, color, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0);
		}

		public sealed override void Update(MenuItemAction menuAction)
		{
			if (menuAction == MenuItemAction.ACTIVATE) this.text = func();
		}
		
		public sealed override Rectangle ActivationHitBox()
		{
			return new Rectangle((int)Math.Round(position.X), (int)Math.Round(position.Y),
			             (int)Math.Round(spriteFontForMeasuring.MeasureString(text).X * scale), (int)Math.Round(ItemHeight()));
		}
	}

	public class OnOffSelectorMenuItem : MenuItem2
	{
		private string text;
		private float height;
		private float scale;
		private Vector2 position;
		private Action<bool> setValue;
		private bool currentValue;
		float onOffAlign;

		private SpriteFont spriteFontForMeasuring;

		public OnOffSelectorMenuItem(string text, SpriteFont spriteFont, Action<bool> setValue, bool currentValue, float onOffAlign) : base(true)
		{
			this.text = text + ":";
			this.scale = 1.0f;
			this.spriteFontForMeasuring = spriteFont;
			this.height = spriteFont.MeasureString(text).Y;
			this.setValue = setValue;
			this.currentValue = currentValue;
			this.onOffAlign = onOffAlign;
		}

		public sealed override float ItemHeight() { return height; }

		public sealed override void SetPosition(Vector2 position) { this.position = position; }

		public sealed override void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont, bool isSelected)
		{
			Vector2 shadowFontPos = new Vector2(position.X + 1, position.Y + 1);
			Vector2 fontPos = position;
			Color shadowColor = Color.Black;
			Color normalColor = Color.White;
			Color color = isSelected ? Color.DarkOrange : normalColor;
			Color activatedColor = Color.White;
			Color nonActivatedColor = Color.Gray;

			// Name
			spriteBatch.DrawString(spriteFont, text, shadowFontPos, shadowColor, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0);
			spriteBatch.DrawString(spriteFont, text, fontPos, color, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0);

			//float nameWidth = spriteFontForMeasuring.MeasureString(text).X;
			//shadowFontPos.X += nameWidth;
			//fontPos.X += nameWidth;
			shadowFontPos.X += onOffAlign;
			fontPos.X += onOffAlign;

			// On
			spriteBatch.DrawString(spriteFont, "On  ", shadowFontPos, shadowColor, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0);
			spriteBatch.DrawString(spriteFont, "On  ", fontPos, currentValue ? activatedColor : nonActivatedColor, 0.03f, Vector2.Zero, scale, SpriteEffects.None, 0);

			float onWidth = spriteFontForMeasuring.MeasureString("On  ").X;
			shadowFontPos.X += onWidth;
			fontPos.X += onWidth;

			// Off
			spriteBatch.DrawString(spriteFont, "Off", shadowFontPos, shadowColor, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0);
			spriteBatch.DrawString(spriteFont, "Off", fontPos, !currentValue ? activatedColor : nonActivatedColor, 0.03f, Vector2.Zero, scale, SpriteEffects.None, 0);
		}

		public sealed override void Update(MenuItemAction menuAction)
		{
			if (menuAction == MenuItemAction.RIGHT && currentValue)
			{
				currentValue = false;
				setValue(currentValue);
			}
			else if (menuAction == MenuItemAction.LEFT && !currentValue)
			{
				currentValue = true;
				setValue(currentValue);
			}
			else if (menuAction == MenuItemAction.ACTIVATE)
			{
				currentValue = !currentValue;
				setValue(currentValue);
			}
		}
		
		public sealed override Rectangle ActivationHitBox()
		{
			return new Rectangle((int)Math.Round(position.X), (int)Math.Round(position.Y),
			             (int)Math.Round((spriteFontForMeasuring.MeasureString(text + ":On  Off").X + onOffAlign) * scale), (int)Math.Round(ItemHeight()));
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
		internal abstract void OnExit();

		// Methods
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		protected void AddMenuItem(MenuItem2 menuItem)
		{
			menuItem.SetPosition(nextItemPos);
			nextItemPos.Y += menuItem.ItemHeight();
			menuItems.Add(menuItem);
			if (selected == -1 && menuItem.IsSelectable)
			{
				selected = menuItems.Count - 1;
			}
		}

		protected void AddTitle(string text)
		{
			AddMenuItem(new TextMenuItem(text, 2.0f, this.spriteFont));
		}

		protected void AddHeading(string text)
		{
			AddMenuItem(new TextMenuItem(text, 1.5f, this.spriteFont));
		}

		protected void AddDescription(string text)
		{
			AddMenuItem(new TextMenuItem(text, 0.9f, this.spriteFont));
		}

		protected void AddSpace(float amount)
		{
			AddMenuItem(new EmptyMenuItem(amount));
		}

		protected void AddClickable(string text, Func<string> func)
		{
			AddMenuItem(new ClickableTextMenuItem(text, 1.0f, this.spriteFont, func));
		}

		protected void AddOnOffSelector(string name, bool currentValue, Action<bool> setValueFunc)
		{
			AddMenuItem(new OnOffSelectorMenuItem(name, this.spriteFont, setValueFunc, currentValue, 200.0f));
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
			else if (iH.MenuActivatePressed(menuItems.ElementAt(selected).ActivationHitBox()))
			{
				if (selected != -1) menuItems.ElementAt(selected).Update(MenuItemAction.ACTIVATE);
			}
			else if (iH.MenuCancelPressed())
			{
				OnExit();
				ScreenSystem.RemoveCurrentScreen();
			}
			else
			{
				for (int i = 0; i < menuItems.Count; i++)
				{
					if (iH.MouseWasMoved() && iH.MouseWithinRectangle(menuItems.ElementAt(i).ActivationHitBox()))
					{
						selected = i;
					}
				}
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
