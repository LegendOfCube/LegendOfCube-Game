using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfCube.Screens
{
	// MenuItems
	// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

	// MenuItemAction enum (for input to MenuItems)
	// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

	public enum MenuItemAction
	{
		ACTIVATE, LEFT, RIGHT
	}

	// Abstract MenuItem class
	// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

	public abstract class MenuItem
	{
		public MenuItem(bool isSelectable)
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

	// TextMenuItem (Titles, headings, etc)
	// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

	public class TextMenuItem : MenuItem
	{
		private string text;
		private float height;
		private float scale;
		private Vector2 position;

		public TextMenuItem(string text, float scale, SpriteFont spriteFont)
			: base(false)
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

	// EmptyMenuItem (for spacing)
	// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

	public class EmptyMenuItem : MenuItem
	{
		private float height;
		private Vector2 position;
		public EmptyMenuItem(float height) : base(false) { this.height = height; }
		public override float ItemHeight() { return height; }
		public override void SetPosition(Vector2 position) { this.position = position; }
		public override void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont, bool unused) { }
	}

	// ClickableTextMenuItem (for text you can click on and make something happen)
	// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

	public class ClickableTextMenuItem : MenuItem
	{
		private string text;
		private float height;
		private float scale;
		private Vector2 position;
		private Func<string> func;

		private SpriteFont spriteFontForMeasuring;

		public ClickableTextMenuItem(string text, float scale, SpriteFont spriteFont, Func<String> func)
			: base(true)
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

	// OnOffSelectorMenuItem (For switching on and off something)
	// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

	public class OnOffSelectorMenuItem : MenuItem
	{
		private string text;
		private float height;
		private float scale;
		private Vector2 position;
		private Action<bool> setValue;
		private bool currentValue;
		float onOffAlign;

		private SpriteFont spriteFontForMeasuring;

		public OnOffSelectorMenuItem(string text, SpriteFont spriteFont, Action<bool> setValue, bool currentValue, float onOffAlign)
			: base(true)
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
			spriteBatch.DrawString(spriteFont, "On  ", fontPos, currentValue ? activatedColor : nonActivatedColor, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0);

			float onWidth = spriteFontForMeasuring.MeasureString("On  ").X;
			shadowFontPos.X += onWidth;
			fontPos.X += onWidth;

			// Off
			spriteBatch.DrawString(spriteFont, "Off", shadowFontPos, shadowColor, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0);
			spriteBatch.DrawString(spriteFont, "Off", fontPos, !currentValue ? activatedColor : nonActivatedColor, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0);
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

	// MultiChoiceSelectorMenuItem (For multichoice thingies)
	// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

	public class MultiChoiceSelectorMenuItem : MenuItem
	{
		private string text;
		private float height;
		private float scale;
		private Vector2 position;
		string[] options;
		int currentValue;
		Action<int> applyOption;
		float optionsAlign;

		private SpriteFont spriteFontForMeasuring;

		public MultiChoiceSelectorMenuItem(string text, SpriteFont spriteFont, string[] options, int currentValue, Action<int> applyOption, float optionsAlign)
			: base(true)
		{
			this.text = text + ":";
			this.scale = 1.0f;
			this.spriteFontForMeasuring = spriteFont;
			this.height = spriteFont.MeasureString(text).Y;
			this.options = options;
			this.applyOption = applyOption;
			this.currentValue = currentValue;
			this.optionsAlign = optionsAlign;
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

			// Name
			spriteBatch.DrawString(spriteFont, text, shadowFontPos, shadowColor, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0);
			spriteBatch.DrawString(spriteFont, text, fontPos, color, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0);

			shadowFontPos.X += optionsAlign;
			fontPos.X += optionsAlign;

			// Option
			spriteBatch.DrawString(spriteFont, options[currentValue], shadowFontPos, shadowColor, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0);
			spriteBatch.DrawString(spriteFont, options[currentValue], fontPos, normalColor, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0);
		}

		public sealed override void Update(MenuItemAction menuAction)
		{
			if (menuAction == MenuItemAction.RIGHT && currentValue < options.Length - 1)
			{
				currentValue++;
				applyOption(currentValue);
			}
			else if (menuAction == MenuItemAction.LEFT && currentValue > 0)
			{
				currentValue--;
				applyOption(currentValue);
			}
			else if (menuAction == MenuItemAction.ACTIVATE)
			{
				currentValue++;
				if (currentValue == options.Length) currentValue = 0;
				applyOption(currentValue);
			}
		}

		public sealed override Rectangle ActivationHitBox()
		{
			return new Rectangle((int)Math.Round(position.X), (int)Math.Round(position.Y),
						 (int)Math.Round((spriteFontForMeasuring.MeasureString(text + ":On  Off").X + optionsAlign) * scale), (int)Math.Round(ItemHeight()));
		}
	}
}
