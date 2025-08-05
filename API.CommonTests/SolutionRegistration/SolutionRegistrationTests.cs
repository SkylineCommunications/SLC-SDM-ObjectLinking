// Ignore Spelling: SDM

namespace Skyline.DataMiner.SDM.ObjectLinking.Tests
{
	using FluentAssertions;

	using Skyline.DataMiner.Net.Messages.SLDataGateway;
	using Skyline.DataMiner.SDM.ObjectLinking.Exceptions;
	using Skyline.DataMiner.SDM.ObjectLinking.Install.DOM;
	using Skyline.DataMiner.SDM.ObjectLinking.Models;
	using Skyline.DataMiner.Utils.DOM.UnitTesting;

	[TestClass]
	public class SolutionRegistrationTests
	{
		private DomConnectionMock _connection;

		[TestInitialize]
		public void Setup()
		{
			_connection = new DomConnectionMock();
			new DomInstaller(_connection).InstallDefaultContent();
		}

		[TestMethod]
		public void SdmRegistrar_Creation()
		{
			var act = () => new SdmRegistrar(_connection);

			act.Should().NotThrow();
		}

		[TestMethod]
		public void SdmRegistrar_Create()
		{
			var registrar = new SdmRegistrar(_connection);

			var models1 = new List<ModelRegistration>
			{
				new ModelRegistration
				{
					Name = "service",
					DisplayName = "Service",
				},
				new ModelRegistration
				{
					Name = "rack",
					DisplayName = "Rack",
				},
			};
			var solution1 = new SolutionRegistration
			{
				ID = "service_management",
				Models =
				{
					models1[0],
					models1[1],
				},
			};

			var solutionAct = () => registrar.Solutions.Create(solution1);
			var modelAct = () => registrar.Models.Create(models1);

			solutionAct.Should().Throw<ValidationException>();
			solution1.DisplayName = "Service Management";
			solutionAct.Should().NotThrow();

			modelAct.Should().NotThrow();

			registrar.Solutions.Read(new TRUEFilterElement<SolutionRegistration>()).Should().Contain(solution1);
			registrar.Models.Read(new TRUEFilterElement<ModelRegistration>()).Should().Contain(models1);
		}

		[TestMethod]
		public void SdmRegistrar_Count()
		{
			var registrar = new SdmRegistrar(_connection);

			var models1 = new List<ModelRegistration>
			{
				new ModelRegistration
				{
					Name = "service",
					DisplayName = "Service",
				},
				new ModelRegistration
				{
					Name = "rack",
					DisplayName = "Rack",
				},
			};
			var solution1 = new SolutionRegistration
			{
				ID = "service_management",
				DisplayName = "Service Management",
				Models =
				{
					models1[0],
					models1[1],
				},
			};

			registrar.Solutions.Create(solution1);
			registrar.Models.Create(models1);

			long result = -1;
			var act = () => result = registrar.Models.Count(ModelRegistrationExposers.Name.Equal("rack"));

			act.Should().NotThrow();
			result.Should().Be(1);
		}
	}
}