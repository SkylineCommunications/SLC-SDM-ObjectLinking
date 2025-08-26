// Ignore Spelling: Slc SDM

namespace Skyline.DataMiner.SDM.ObjectLinking
{
	using System;
	using System.Linq;

	/// <summary>
	/// Represents a wrapper class for accessing a Link DOM instance.
	/// The <see cref="Link"/> class provides simplified access to the data and functionality of the underlying DOM instance, allowing for easier manipulation and retrieval of data from DOM.
	/// </summary>
	public sealed partial class Link : IEquatable<Link>
	{
		public bool Equals(Link other)
		{
			if (other is null)
			{
				return false;
			}

			if (ReferenceEquals(this, other))
			{
				return true;
			}

			// Compare Entities property by sorting on ID to ensure order does not matter
			if (EntityDescriptors is null && other.EntityDescriptors is null)
			{
				return true;
			}

			if (EntityDescriptors is null || other.EntityDescriptors is null)
			{
				return false;
			}

			if (EntityDescriptors.Count != other.EntityDescriptors.Count)
			{
				return false;
			}

			var thisEntitiesSorted = EntityDescriptors.OrderBy(e => e.ID).ToList();
			var otherEntitiesSorted = other.EntityDescriptors.OrderBy(e => e.ID).ToList();

			for (int i = 0; i < thisEntitiesSorted.Count; i++)
			{
				if (!Equals(thisEntitiesSorted[i], otherEntitiesSorted[i]))
				{
					return false;
				}
			}

			return true;
		}
	}
}
