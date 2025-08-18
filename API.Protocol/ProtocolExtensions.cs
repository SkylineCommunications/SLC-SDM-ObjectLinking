// Ignore Spelling: SDM

namespace Skyline.DataMiner.SDM.ObjectLinking
{
	using System;

	using Skyline.DataMiner.Scripting;

	/// <summary>
	/// Provides extension methods for the <see cref="SLProtocol"/> class to support object linking functionality.
	/// </summary>
	public static class ProtocolExtensions
	{
		/// <summary>
		/// Gets an <see cref="ObjectLinker"/> instance for the specified <see cref="SLProtocol"/>.
		/// </summary>
		/// <param name="protocol">The protocol instance for which to get the object linker.</param>
		/// <returns>
		/// An <see cref="ObjectLinker"/> that provides access to object linking operations using the protocol's connection.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// Thrown when <paramref name="protocol"/> is <c>null</c>.
		/// </exception>
		public static ObjectLinker GetObjectLinker(this SLProtocol protocol)
		{
			if (protocol is null)
			{
				throw new ArgumentNullException(nameof(protocol), "protocol cannot be null.");
			}

			return new ObjectLinker(protocol.SLNet.RawConnection);
		}
	}
}