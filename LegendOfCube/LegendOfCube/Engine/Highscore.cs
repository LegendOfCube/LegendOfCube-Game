using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LegendOfCube.Engine
{
	public class Highscore
	{
		private static readonly Highscore INSTANCE = new Highscore();
		private readonly string path;

		public static Highscore Instance
		{
			get { return INSTANCE; }
		}

		private readonly Dictionary<string, List<float>> highScores; 

		public Highscore()
		{
			path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
			           @"\My Games\Legend of Cube\HighScores.txt";
			highScores = new Dictionary<string, List<float>>();
			LoadFromFile();
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
			SaveToFile();
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
			StringBuilder sb = new StringBuilder();
			foreach (var level in highScores.Keys)
			{
				sb.Append(level);
				foreach (var time in highScores[level])
				{
					sb.Append(",");
					sb.Append(time.ToString("R"));
				}
				sb.Append("\n");
			}
			File.WriteAllText(path, sb.ToString());
		}

		public bool LoadFromFile()
		{
			string textFromFile;
			try
			{
				textFromFile = File.ReadAllText(path);
			}
			catch (FileNotFoundException)
			{
				return false;
			}
			var lines = textFromFile.Split('\n');
			foreach (var line in lines)
			{
				var scores = line.Split(',');
				for (int i = 1; i < scores.Length; i++)
				{
					AddHighScore(scores[0],float.Parse(scores[i]));
				}
			}
			return true;
		}
	}
}
