namespace LegendOfCube
{
#if WINDOWS || XBOX
	static class Program
	{
		static void Main(string[] args)
		{
			using (var game = new LegendOfCubeGame())
			{
				game.Run();
			}
		}
	}
#endif
}
