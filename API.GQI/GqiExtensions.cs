// Ignore Spelling: SDM GQI DMS

namespace Skyline.DataMiner.SDM.ObjectLinking
{
	using System;

	using Skyline.DataMiner.Analytics.GenericInterface;

	/// <summary>
	/// Provides extension methods for working with GQI and object linking.
	/// </summary>
	public static class GqiExtensions
	{
		/// <summary>
		/// Creates a new <see cref="ObjectLinker"/> instance using the specified <see cref="GQIDMS"/> connection.
		/// </summary>
		/// <param name="dms">The GQI DMS connection.</param>
		/// <returns>An <see cref="ObjectLinker"/> instance initialized with the provided DMS connection.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="dms"/> is <c>null</c>.</exception>
		public static ObjectLinker GetObjectLinker(GQIDMS dms)
		{
			if (dms is null)
			{
				throw new ArgumentNullException(nameof(dms), "dms cannot be null.");
			}

			return new ObjectLinker(dms.GetConnection());
		}

		/// <summary>
		/// Creates a new <see cref="ObjectLinker"/> instance using the DMS connection from the specified <see cref="OnInitInputArgs"/>.
		/// </summary>
		/// <param name="args">The initialization arguments containing the DMS connection.</param>
		/// <returns>An <see cref="ObjectLinker"/> instance initialized with the DMS connection from <paramref name="args"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="args"/> is <c>null</c>.</exception>
		public static ObjectLinker GetObjectLinker(OnInitInputArgs args)
		{
			if (args is null)
			{
				throw new ArgumentNullException(nameof(args), "args cannot be null.");
			}

			return new ObjectLinker(args.DMS.GetConnection());
		}
	}
}