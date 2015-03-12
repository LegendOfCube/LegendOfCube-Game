using System;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

namespace LegendOfCubePipeline
{
	[ContentProcessor(DisplayName = "Standard Effect Model Processor")]
	public class StandardEffectModelProcessor : ModelProcessor
	{
		public override ModelContent Process(NodeContent input,
			ContentProcessorContext context)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}

			// TODO: Add additional texture types and model properties somehow
			// input.OpaqueData["something"] = normalMap;
			return base.Process(input, context);
		}
	}
}