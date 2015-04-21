using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LegendOfCube.Engine
{
	public class Highscore
	{
		private static readonly Highscore INSTANCE = new Highscore();

		public static Highscore Instance
		{
			get { return INSTANCE; }
		}

		private readonly Dictionary<string, List<float>> highScores; 

		public Highscore()
		{
			if (!LoadFromFile())
			{
				highScores = new Dictionary<string, List<float>>();
			}
		}

		public void AddHighScore(string name, float time)
		{
			List<float> results;
			if (highScores.TryGetValue(name, out results))
			{
				results.Add(time);
				results.Sort();
				highScores[name] = results;
			}
			else
			{
				highScores.Add(name, new List<float>{time});
			}
		}

		public List<float> GetHighScoresForLevel(string name)
		{
			List<float> results;
			if (highScores.TryGetValue(name, out results))
			{
				return results;
			}
			return null;
		}

		public void SaveToFile()
		{
			
		}

		public bool LoadFromFile()
		{
			return false;
		}
	}
}
