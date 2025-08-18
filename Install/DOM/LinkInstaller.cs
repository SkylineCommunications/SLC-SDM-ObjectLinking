namespace Skyline.DataMiner.SDM.ObjectLinking.Install.DOM
{
	using DomHelpers.SlcObject_Linking;

	using Skyline.DataMiner.Net.Apps.DataMinerObjectModel;
	using Skyline.DataMiner.Net.Sections;
	using Skyline.DataMiner.Utils.DOM.Builders;

	public partial class DomInstaller
	{
		private void InstallLinks(DomHelper helper)
		{
			var entitySection = new SectionDefinitionBuilder()
				.WithName("Entity")
				.WithID(SlcObject_LinkingIds.Sections.Entity.Id)
				.AddFieldDescriptor(new FieldDescriptorBuilder()
					.WithID(SlcObject_LinkingIds.Sections.Entity.ID)
					.WithName("ID")
					.WithType(typeof(string))
					.WithIsOptional(true)
					.WithTooltip("The unique identifier of the entity."))
				.AddFieldDescriptor(new FieldDescriptorBuilder()
					.WithID(SlcObject_LinkingIds.Sections.Entity.DisplayName)
					.WithName("Display Name")
					.WithType(typeof(string))
					.WithIsOptional(true)
					.WithTooltip("The display name of the entity."))
				.AddFieldDescriptor(new FieldDescriptorBuilder()
					.WithID(SlcObject_LinkingIds.Sections.Entity.ModelName)
					.WithName("Model Name")
					.WithType(typeof(string))
					.WithIsOptional(true)
					.WithTooltip("The name of the model the enitity is associated with."))
				.AddFieldDescriptor(new FieldDescriptorBuilder()
					.WithID(SlcObject_LinkingIds.Sections.Entity.SolutionName)
					.WithName("Solution Name")
					.WithType(typeof(string))
					.WithIsOptional(true)
					.WithTooltip("The name of the solution the entity is associated with."))
				.AddFieldDescriptor(new FieldDescriptorBuilder()
					.WithID(SlcObject_LinkingIds.Sections.Entity.ParentID)
					.WithName("Parent ID")
					.WithType(typeof(string))
					.WithIsOptional(true)
					.WithTooltip("The unique identifier of the parent enitity."))
				.AddFieldDescriptor(new FieldDescriptorBuilder()
					.WithID(SlcObject_LinkingIds.Sections.Entity.ParentModelName)
					.WithName("Parent Model Name")
					.WithType(typeof(string))
					.WithIsOptional(true)
					.WithTooltip("The model name of the parent entity"))
				.Build();

			Import(helper.SectionDefinitions, SectionDefinitionExposers.ID.Equal(SlcObject_LinkingIds.Sections.Entity.Id), entitySection);
			Log("Installed section definition for Entity");

			var linkDefinition = new DomDefinitionBuilder()
				.WithID(SlcObject_LinkingIds.Definitions.Link.Id)
				.WithName("Link")
				.AddSectionDefinitionLink(new Skyline.DataMiner.Net.Apps.Sections.SectionDefinitions.SectionDefinitionLink
				{
					SectionDefinitionID = SlcObject_LinkingIds.Sections.Entity.Id,
					AllowMultipleSections = true,
					IsOptional = true,
				})
				.Build();

			Import(helper.DomDefinitions, DomDefinitionExposers.Id.Equal(SlcObject_LinkingIds.Definitions.Link), linkDefinition);
			Log("Installed DOM definition for Link");
		}
	}
}
