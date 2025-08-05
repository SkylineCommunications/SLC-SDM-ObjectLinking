// Ignore Spelling: SDM

namespace Skyline.DataMiner.SDM.ObjectLinking
{
	using Skyline.DataMiner.Net;
	using Skyline.DataMiner.SDM.Middleware;
	using Skyline.DataMiner.SDM.ObjectLinking.Middleware;
	using Skyline.DataMiner.SDM.ObjectLinking.Models;

	public class SdmRegistrar
    {
		public SdmRegistrar(IConnection connection)
		{
			Solutions = Sdm.CreateProviderBuilder(new SolutionRegistrationDomStorageProvider(connection))
				.AddMiddleware(new SolutionValidationMiddleware())
				.AddMiddleware(new SdmTracingMiddleware<SolutionRegistration>())
				.Build();

			Models = Sdm.CreateProviderBuilder(new ModelRegistrationDomStorageProvider(connection))
				.AddMiddleware(new ModelValidationMiddleware())
				.AddMiddleware(new SdmTracingMiddleware<ModelRegistration>())
				.Build();
		}

		public IObservableBulkStorageProvider<SolutionRegistration> Solutions { get; }

		public IObservableBulkStorageProvider<ModelRegistration> Models { get; }
	}
}
