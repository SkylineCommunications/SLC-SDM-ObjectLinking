// Ignore Spelling: Slc SDM

namespace Skyline.DataMiner.SDM.ObjectLinking
{
	using System;

	/// <summary>
	/// Represents a wrapper class for accessing a Entity section.
	/// The <see cref="EntityDescriptor"/> class provides simplified access to the data and functionality of the underlying DOM section, allowing for easier manipulation and retrieval of data from DOM.
	/// </summary>
	public sealed partial class EntityDescriptor : IEquatable<EntityDescriptor>
	{
		public bool Equals(EntityDescriptor other)
		{
			if (other is null)
			{
				return false;
			}

			if (ReferenceEquals(this, other))
			{
				return true;
			}

			return String.Equals(ID, other.ID, StringComparison.InvariantCulture)
				&& String.Equals(DisplayName, other.DisplayName, StringComparison.InvariantCulture)
				&& String.Equals(ModelName, other.ModelName, StringComparison.InvariantCulture)
				&& String.Equals(SolutionName, other.SolutionName, StringComparison.InvariantCulture)
				&& String.Equals(ParentID, other.ParentID, StringComparison.InvariantCulture)
				&& String.Equals(ParentModelName, other.ParentModelName, StringComparison.InvariantCulture);
		}
	}
}
