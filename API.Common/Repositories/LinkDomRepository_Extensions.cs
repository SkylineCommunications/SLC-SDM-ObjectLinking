
namespace Skyline.DataMiner.SDM
{
	using Skyline.DataMiner.SDM.Middleware;
	using Skyline.DataMiner.SDM.ObjectLinking;
	using Skyline.DataMiner.SDM;

	public static class LinkDomRepository_Extensions
	{

		public static Skyline.DataMiner.SDM.IBulkRepository<Skyline.DataMiner.SDM.ObjectLinking.Link> WithMiddleware(
			this Skyline.DataMiner.SDM.IBulkRepository<Skyline.DataMiner.SDM.ObjectLinking.Link> repository,
			IMiddlewareMarker<Skyline.DataMiner.SDM.ObjectLinking.Link> middleware)
		{
			return new LinkDomRepository_Middleware(repository, middleware);
		}
	}
}
