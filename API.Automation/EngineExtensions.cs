// Ignore Spelling: SDM

namespace Skyline.DataMiner.SDM.ObjectLinking
{
	using System;

	using Skyline.DataMiner.Automation;

	/// <summary>
	/// Provides extension methods for the <see cref="IEngine"/> interface to support object linking functionality.
	/// </summary>
	public static class EngineExtensions
	{
		/// <summary>
		/// Gets an <see cref="ObjectLinker"/> instance for the specified <see cref="IEngine"/>.
		/// </summary>
		/// <param name="engine">The engine for which to get the <see cref="ObjectLinker"/>.</param>
		/// <returns>An <see cref="ObjectLinker"/> instance associated with the specified engine.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="engine"/> is <c>null</c>.</exception>
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