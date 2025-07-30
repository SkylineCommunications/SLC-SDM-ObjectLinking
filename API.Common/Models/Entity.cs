// Ignore Spelling: Slc SDM

namespace Skyline.DataMiner.SDM.ObjectLinking
{
	using System;

	/// <summary>
	/// Represents a wrapper class for accessing a Entity section.
	/// The <see cref="Entity"/> class provides simplified access to the data and functionality of the underlying DOM section, allowing for easier manipulation and retrieval of data from DOM.
	/// </summary>
	public sealed partial class Entity : IEquatable<Entity>
	{
		public bool Equals(Entity other)
		{
			if (other is null)
			{
				return false;
			}

			if (ReferenceEquals(this, other))
			{
				return true;
			}

			return string.Equals(ID, other.ID, StringComparison.InvariantCulture)
				&& string.Equals(DisplayName, other.DisplayName, StringComparison.InvariantCulture)
				&& string.Equals(ModelName, other.ModelName, StringComparison.InvariantCulture)
				&& string.Equals(SolutionName, other.SolutionName, StringComparison.InvariantCulture)
				&& string.Equals(ParentID, other.ParentID, StringComparison.InvariantCulture)
				&& string.Equals(ParentModelName, other.ParentModelName, StringComparison.InvariantCulture);
		}
	}
}
