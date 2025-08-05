// Ignore Spelling: SDM

namespace Skyline.DataMiner.SDM.ObjectLinking
{
	using System.Linq;

	using Skyline.DataMiner.Net;
	using Skyline.DataMiner.SDM.Middleware;
	using Skyline.DataMiner.SDM.ObjectLinking.Middleware;

	public class ObjectLinker
	{
		public ObjectLinker(IConnection connection)
		{
			Links = Sdm.CreateProviderBuilder(new LinkDomStorageProvider(connection))
				.AddMiddleware(new LinkValidationMiddleware())
				.AddMiddleware(new SdmTracingMiddleware<Link>())
				.Build();
		}

		public IObservableBulkStorageProvider<Link> Links { get; }
	}
}
