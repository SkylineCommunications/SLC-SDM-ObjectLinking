// Ignore Spelling: SDM

namespace Skyline.DataMiner.SDM.ObjectLinking
{
	using System;

	using Skyline.DataMiner.Automation;

	public static class EngineExtensions
	{
		public static ObjectLinker GetObjectLinker(this IEngine engine)
		{
			if (engine is null)
			{
				throw new ArgumentNullException(nameof(engine), "Engine cannot be null.");
			}

			return new ObjectLinker(engine.GetUserConnection());
		}
	}
}