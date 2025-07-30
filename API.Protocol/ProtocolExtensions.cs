// Ignore Spelling: SDM

namespace Skyline.DataMiner.SDM.ObjectLinking
{
	using System;

	using Skyline.DataMiner.Scripting;

	public static class ProtocolExtensions
	{
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