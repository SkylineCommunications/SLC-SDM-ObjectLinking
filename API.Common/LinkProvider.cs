// Ignore Spelling: SDM

namespace Skyline.DataMiner.SDM.ObjectLinking
{
	using System;

	public static class LinkProvider
	{
		public static void Create(this ICreatable<Link> provider, Entity entityA, Entity entityB)
		{
			provider.Create(new Link
			{
				Entities =
				{
					entityA ?? throw new ArgumentNullException(nameof(entityA)),
					entityB ?? throw new ArgumentNullException(nameof(entityB)),
				},
			});
		}
	}
}
