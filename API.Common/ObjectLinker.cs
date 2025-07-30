// Ignore Spelling: SDM

namespace Skyline.DataMiner.SDM.ObjectLinking
{
#if NETSTANDARD2_0_OR_GREATER
	using Skyline.DataMiner.SDM.Middleware.Tracing;
#endif

	using DomHelpers.SlcObject_Linking;

	using Skyline.DataMiner.Net;
	using Skyline.DataMiner.SDM.ObjectLinking.Middleware;

	public class ObjectLinker
	{
		public ObjectLinker(IConnection connection)
		{
			Links = Sdm.CreateProviderBuilder(new LinkDomStorageProvider(connection))
				.AddMiddleware(new LinkValidationMiddleware())

#if NETSTANDARD2_0_OR_GREATER
				.AddMiddleware(new SdmTracingMiddleware<Link>())
#endif
				.Build();
		}

		public IObservableBulkStorageProvider<Link> Links { get; }
	}
}
