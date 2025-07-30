// Ignore Spelling: SDM GQI DMS

namespace Skyline.DataMiner.SDM.ObjectLinking
{
	using System;

	using Skyline.DataMiner.Analytics.GenericInterface;

	public static class GqiExtensions
	{
		public static ObjectLinker GetObjectLinker(GQIDMS dms)
		{
			if (dms is null)
			{
				throw new ArgumentNullException(nameof(dms), "dms cannot be null.");
			}

			return new ObjectLinker(dms.GetConnection());
		}
	}
}