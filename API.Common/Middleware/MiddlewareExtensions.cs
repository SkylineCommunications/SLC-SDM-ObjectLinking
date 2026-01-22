namespace Skyline.DataMiner.SDM.ObjectLinking.Middleware
{
	using Skyline.DataMiner.SDM;

	public static class ModelRegistrationDomRepository_Extensions
	{

		public static IBulkRepository<Link> WithMiddleware(
			this IBulkRepository<Link> repository,
			IMiddlewareMarker<Link> middleware)
		{
			return new LinkRepository_Middleware(repository, middleware);
		}
	}
}
