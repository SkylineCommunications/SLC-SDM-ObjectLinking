namespace Skyline.DataMiner.SDM.ObjectLinking
{
	using System;

	using Skyline.DataMiner.Net;

	/// <summary>
	/// Provides extension methods for the <see cref="IConnection"/> interface to support object linking functionality.
	/// </summary>
	public static class ConnectionExtensions
	{
		/// <summary>
		/// Gets an <see cref="ObjectLinker"/> instance for the specified <see cref="IConnection"/>.
		/// </summary>
		/// <param name="connection">The connection for which to get the <see cref="ObjectLinker"/>.</param>
		/// <returns>An <see cref="ObjectLinker"/> instance associated with the specified engine.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="connection"/> is <c>null</c>.</exception>
		public static ObjectLinker GetObjectLinker(this IConnection connection)
		{
			if (connection is null)
			{
				throw new ArgumentNullException(nameof(connection), "Connection cannot be null.");
			}

			return new ObjectLinker(connection);
		}
	}
}
