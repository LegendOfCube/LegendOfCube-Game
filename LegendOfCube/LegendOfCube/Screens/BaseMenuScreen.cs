using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LegendOfCube.Engine.Input;
using LegendOfCube.Engine.Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfCube.Screens
{
	// BaseMenuScreen
	// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

	public abstract class BaseMenuScreen : Screen
	{
		// Members
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		private SpriteBatch spriteBatch;
		private SpriteFont spriteFont;
		private MenuAudioSystem menuAudioSystem;

		private List<MenuItem> menuItems = new List<MenuItem>();
		private int selected = -1;

		private Vector2 nextItemPos = new Vector2(40.0f, 40.0f);

		// Constructors
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public BaseMenuScreen(Game game, ScreenSystem screenSystem) : base(game, screenSystem, false) { }
		internal abstract void InitializeScreen();
		internal abstract void OnExit();

		// Methods
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		protected void AddMenuItem(MenuItem menuItem)
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

		protected void AddMultiChoiceSelector(string name, int currentValue, string[] options, Action<int> applyOption)
		{
			AddMenuItem(new MultiChoiceSelectorMenuItem(name, this.spriteFont, options, currentValue, applyOption, 200.0f));
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
					if (iH.MouseWasMoved())
					{
						Game.IsMouseVisible = true;
						if(iH.MouseWithinRectangle(menuItems.ElementAt(i).ActivationHitBox()))
						{
							selected = i;
						}
					}
				}
			}

			menuAudioSystem.Update(selected);
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
			menuAudioSystem = new MenuAudioSystem(Game.Content);
			InitializeScreen();
		}
	}
}
