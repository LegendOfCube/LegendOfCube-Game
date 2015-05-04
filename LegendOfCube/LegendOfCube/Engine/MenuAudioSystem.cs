using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LegendOfCube.Engine.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace LegendOfCube.Engine.Events
{
	class MenuAudioSystem
	{
		private int oldSelection;
		private float pitch;
		private InputHelper inputHelper = InputHelper.Instance;

		public SoundEffect select { get; private set; }
		public SoundEffect select2 { get; private set; }

		public MenuAudioSystem(ContentManager cm)
		{
			oldSelection = 0;
			pitch = 0;

			select = cm.Load<SoundEffect>("SoundEffects/select");
			select2 = cm.Load<SoundEffect>("SoundEffects/select2");
		}
		public void Update(int selection)
		{
			if (selection != oldSelection)
			{
				pitch += 0.1f;
				if (pitch > 1)
				{
					pitch = 0;
				}
				select2.Play(0.15f, pitch, 0);
			}
			if (inputHelper.KeyWasJustPressed(Keys.Space) || inputHelper.ButtonWasJustPressed(Buttons.A))
			{
				pitch += 0.1f;
				if (pitch > 1)
				{
					pitch = 0;
				}
				select.Play(0.15f, pitch, 0);
			}
			oldSelection = selection;
		}
	}
}
