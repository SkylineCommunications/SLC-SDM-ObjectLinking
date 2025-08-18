namespace Skyline.DataMiner.SDM.ObjectLinking.Tests
{
	using FluentAssertions;

	using Skyline.DataMiner.Net.Messages.SLDataGateway;
	using Skyline.DataMiner.SDM.ObjectLinking.Exceptions;
	using Skyline.DataMiner.SDM.ObjectLinking.Install.DOM;
	using Skyline.DataMiner.Utils.DOM.UnitTesting;

	[TestClass]
	public class ObjectLinkerTests
	{
		private DomConnectionMock _connection;

		[TestInitialize]
		public void Setup()
		{
			_connection = new DomConnectionMock();
			new DomInstaller(_connection).InstallDefaultContent();
		}

		[TestMethod]
		public void ObjectLinker_Creation()
		{
			var act = () => new ObjectLinker(_connection);

			act.Should().NotThrow();
		}

		[TestMethod]
		public void ObjectLinker_Create()
		{
			var linker = new ObjectLinker(_connection);

			var entity1 = new Entity
			{
				ID = "entity1",
				DisplayName = "Entity 1",
				ModelName = "Model A",
				SolutionName = "Solution X",
				ParentID = "parent1",
				ParentModelName = "Parent Model A",
			};
			var entity2 = new Entity
			{
				ID = "entity2",
				DisplayName = "Entity 2",
				ModelName = "Model B",
				SolutionName = "Solution Y",
				ParentID = "parent2",
				ParentModelName = "Parent Model B",
			};

			var link = new Link
			{
				Entities =
				{
					entity1,
				},
			};

			var act = () => linker.Links.Create(link);

			act.Should().Throw<ValidationException>();

			link.Entities.Add(entity2);

			act.Should().NotThrow();

			linker.Links.Read(new TRUEFilterElement<Link>()).Should().ContainSingle(l => l.Entities.Contains(entity1));
		}

		[TestMethod]
		public void ObjectLinker_Count()
		{
			var linker = new ObjectLinker(_connection);

			var entity1 = new Entity
			{
				ID = "entity1",
				DisplayName = "Entity 1",
				ModelName = "Model A",
				SolutionName = "Solution X",
				ParentID = "parent1",
				ParentModelName = "Parent Model A",
			};
			var entity2 = new Entity
			{
				ID = "entity2",
				DisplayName = "Entity 2",
				ModelName = "Model B",
				SolutionName = "Solution Y",
				ParentID = "parent2",
				ParentModelName = "Parent Model B",
			};
			var entity3 = new Entity
			{
				ID = "entity3",
				DisplayName = "Entity 3",
				ModelName = "Model C",
				SolutionName = "Solution Z",
				ParentID = "parent3",
				ParentModelName = "Parent Model C",
			};

			linker.Create(entity1, entity2);
			linker.Create(entity2, entity3);
			linker.Create(entity3, entity1);

			long result = -1;
			var act = () => result = linker.Links.Count(LinkExposers.Entities.SolutionName.Equal("Solution Z"));

			act.Should().NotThrow();
			result.Should().Be(2);
		}
	}
}