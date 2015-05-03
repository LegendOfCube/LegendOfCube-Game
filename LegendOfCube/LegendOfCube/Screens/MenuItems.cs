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
		public bool IsSelectable { get; private set; }

		public float Height { get; private set; }

		public Vector2 Position { get; set; }

		public MenuItem(bool isSelectable, float height)
		{
			IsSelectable = isSelectable;
			Height = height;
		}

		public abstract void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont, bool isSelected);
		public virtual Rectangle ActivationHitBox() { return new Rectangle(); }
		public virtual void OnAction(MenuItemAction action) { }

		protected static float GetScale(float fontHeight, SpriteFont spriteFont)
		{
			return fontHeight / spriteFont.LineSpacing;
		}

		protected static float GetScaledHeight(string text, float fontHeight, SpriteFont spriteFont)
		{
			return GetScale(fontHeight, spriteFont) * spriteFont.MeasureString(text).Y;
		}
	}

	// TextMenuItem (Titles, headings, etc)
	// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

	public class TextMenuItem : MenuItem
	{
		private readonly string text;
		private float scale;

		public TextMenuItem(string text, float fontHeight, SpriteFont spriteFont)
			: base(false, GetScaledHeight(text, fontHeight, spriteFont))
		{
			this.text = text;
			this.scale = GetScale(fontHeight, spriteFont);
		}

		public override void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont, bool unused)
		{
			Vector2 shadowFontPos = new Vector2(Position.X + 1, Position.Y + 1);
			Color shadowColor = Color.Black;
			Color color = Color.White;
			spriteBatch.DrawString(spriteFont, text, shadowFontPos, shadowColor, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0);
			spriteBatch.DrawString(spriteFont, text, Position, color, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0);
		}
	}

	// EmptyMenuItem (for spacing)
	// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

	public class EmptyMenuItem : MenuItem
	{
		public EmptyMenuItem(float height) : base(false, height) {}
		public override void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont, bool unused) { }
	}

	// ClickableTextMenuItem (for text you can click on and make something happen)
	// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

	public class ClickableTextMenuItem : MenuItem
	{
		private string text;
		private float scale;
		private readonly Func<string> onClickAction;

		private SpriteFont spriteFontForMeasuring;

		public ClickableTextMenuItem(string text, float fontHeight, SpriteFont spriteFont, Func<String> onClickAction)
			: base(true, GetScaledHeight(text, fontHeight, spriteFont))
		{
			this.text = text;
			this.scale = GetScale(fontHeight, spriteFont);
			this.onClickAction = onClickAction;
			this.spriteFontForMeasuring = spriteFont;
		}

		public sealed override void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont, bool isSelected)
		{
			Vector2 shadowFontPos = new Vector2(Position.X + 1, Position.Y + 1);
			Color shadowColor = Color.Black;
			Color color = isSelected ? Color.DarkOrange : Color.White;
			spriteBatch.DrawString(spriteFont, text, shadowFontPos, shadowColor, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0);
			spriteBatch.DrawString(spriteFont, text, Position, color, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0);
		}

		public sealed override void OnAction(MenuItemAction menuAction)
		{
			if (menuAction == MenuItemAction.ACTIVATE) this.text = onClickAction();
		}

		public sealed override Rectangle ActivationHitBox()
		{
			return new Rectangle((int)Math.Round(Position.X),
			                     (int)Math.Round(Position.Y),
			                     (int)Math.Round(spriteFontForMeasuring.MeasureString(text).X * scale),
			                     (int)Math.Round(Height));
		}
	}

	// OnOffSelectorMenuItem (For switching on and off something)
	// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

	public class OnOffSelectorMenuItem : MenuItem
	{
		private string text;
		private float scale;
		private Action<bool> onSettingChanged;
		private bool currentValue;
		private readonly float onOffAlign;

		private SpriteFont spriteFontForMeasuring;

		public OnOffSelectorMenuItem(string text, float fontHeight, SpriteFont spriteFont, Action<bool> onSettingChanged, bool currentValue, float onOffAlign)
			: base(true, GetScaledHeight(text, fontHeight, spriteFont))
		{
			this.text = text + ":";
			this.scale = GetScale(fontHeight, spriteFont);
			this.spriteFontForMeasuring = spriteFont;
			this.onSettingChanged = onSettingChanged;
			this.currentValue = currentValue;
			this.onOffAlign = onOffAlign;
		}

		public sealed override void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont, bool isSelected)
		{
			Vector2 shadowFontPos = new Vector2(Position.X + 1, Position.Y + 1);
			Vector2 fontPos = Position;
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

			float onWidth = scale * spriteFontForMeasuring.MeasureString("On  ").X;
			shadowFontPos.X += onWidth;
			fontPos.X += onWidth;

			// Off
			spriteBatch.DrawString(spriteFont, "Off", shadowFontPos, shadowColor, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0);
			spriteBatch.DrawString(spriteFont, "Off", fontPos, !currentValue ? activatedColor : nonActivatedColor, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0);
		}

		public sealed override void OnAction(MenuItemAction menuAction)
		{
			if (menuAction == MenuItemAction.RIGHT && currentValue)
			{
				currentValue = false;
				onSettingChanged(currentValue);
			}
			else if (menuAction == MenuItemAction.LEFT && !currentValue)
			{
				currentValue = true;
				onSettingChanged(currentValue);
			}
			else if (menuAction == MenuItemAction.ACTIVATE)
			{
				currentValue = !currentValue;
				onSettingChanged(currentValue);
			}
		}

		public sealed override Rectangle ActivationHitBox()
		{
			return new Rectangle((int)Math.Round(Position.X),
			                     (int)Math.Round(Position.Y),
			                     (int)Math.Round(onOffAlign + spriteFontForMeasuring.MeasureString(":On  Off").X * scale),
			                     (int)Math.Round(Height));
		}
	}

	// MultiChoiceSelectorMenuItem (For multichoice thingies)
	// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

	public class MultiChoiceSelectorMenuItem : MenuItem
	{
		private readonly string text;
		private string[] options;
		private int currentValue;
		private readonly Action<int> onOptionChanged;
		private readonly float optionsAlign;

		private float scale;
		private SpriteFont spriteFontForMeasuring;
		private readonly float longestChoiceWidth;

		public MultiChoiceSelectorMenuItem(string text, float fontHeight, SpriteFont spriteFont, string[] options, int currentValue, Action<int> onOptionChanged, float optionsAlign)
			: base(true, GetScaledHeight(text, fontHeight, spriteFont))
		{
			this.text = text + ":";
			this.scale = GetScale(fontHeight, spriteFont);
			this.spriteFontForMeasuring = spriteFont;
			this.options = options;
			this.onOptionChanged = onOptionChanged;
			this.currentValue = currentValue;
			this.optionsAlign = optionsAlign;
			this.longestChoiceWidth = options.Select(optionText => spriteFont.MeasureString(optionText).X).Max() * scale;
		}

		public sealed override void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont, bool isSelected)
		{
			Vector2 shadowFontPos = new Vector2(Position.X + 1, Position.Y + 1);
			Vector2 fontPos = Position;
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

		public sealed override void OnAction(MenuItemAction menuAction)
		{
			if (menuAction == MenuItemAction.RIGHT && currentValue < options.Length - 1)
			{
				currentValue++;
				onOptionChanged(currentValue);
			}
			else if (menuAction == MenuItemAction.LEFT && currentValue > 0)
			{
				currentValue--;
				onOptionChanged(currentValue);
			}
			else if (menuAction == MenuItemAction.ACTIVATE)
			{
				currentValue++;
				if (currentValue == options.Length) currentValue = 0;
				onOptionChanged(currentValue);
			}
		}

		public sealed override Rectangle ActivationHitBox()
		{
			return new Rectangle((int)Math.Round(Position.X),
			                     (int)Math.Round(Position.Y),
			                     (int)Math.Round(optionsAlign + longestChoiceWidth),
			                     (int)Math.Round(Height));
		}

	}
}
