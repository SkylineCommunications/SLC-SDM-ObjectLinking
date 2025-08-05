namespace Skyline.DataMiner.SDM.ObjectLinking.Install.DOM
{
	using Skyline.DataMiner.Net.Apps.DataMinerObjectModel;
	using Skyline.DataMiner.SDM.ObjectLinking.Models;
	using Skyline.DataMiner.Utils.DOM.Builders;

	public partial class DomInstaller
	{
		private void InstallModel(DomHelper helper)
		{
			var modelProperties = new SectionDefinitionBuilder()
				.WithName(nameof(ModelRegistrationDomMapper.ModelRegistrationProperties))
				.WithID(ModelRegistrationDomMapper.ModelRegistrationProperties.SectionDefinitionId)
				.AddFieldDescriptor(new FieldDescriptorBuilder()
					.WithID(ModelRegistrationDomMapper.ModelRegistrationProperties.Name)
					.WithName("ID")
					.WithType(typeof(string))
					.WithIsOptional(true)
					.WithTooltip("The unique name for them model."))
				.AddFieldDescriptor(new FieldDescriptorBuilder()
					.WithID(ModelRegistrationDomMapper.ModelRegistrationProperties.DisplayName)
					.WithName("Display Name")
					.WithType(typeof(string))
					.WithIsOptional(true)
					.WithTooltip("The display name of the model."))
				.AddFieldDescriptor(new FieldDescriptorBuilder()
					.WithID(ModelRegistrationDomMapper.ModelRegistrationProperties.ApiScriptName)
					.WithName("API Script Name")
					.WithType(typeof(string))
					.WithIsOptional(true)
					.WithTooltip(""))
				.AddFieldDescriptor(new FieldDescriptorBuilder()
					.WithID(ModelRegistrationDomMapper.ModelRegistrationProperties.ApiEndpoint)
					.WithName("API Endpoint")
					.WithType(typeof(string))
					.WithIsOptional(true)
					.WithTooltip("The base endpoint to interact with the model using the REST API."))
				.AddFieldDescriptor(new FieldDescriptorBuilder()
					.WithID(ModelRegistrationDomMapper.ModelRegistrationProperties.VisualizationEndpoint)
					.WithName("Visualization Endpoint")
					.WithType(typeof(string))
					.WithIsOptional(true)
					.WithTooltip("The endpoint where a user should navigate to, to interact with the model using an UI."))
				.Build();
			Log("Installed section definition for Model Registration");

			var solutionDefinition = new DomDefinitionBuilder()
				.WithID(ModelRegistrationDomMapper.DomDefinitionId)
				.WithName("Model Registration")
				.AddSectionDefinitionLink(new Skyline.DataMiner.Net.Apps.Sections.SectionDefinitions.SectionDefinitionLink
				{
					SectionDefinitionID = ModelRegistrationDomMapper.ModelRegistrationProperties.SectionDefinitionId,
					AllowMultipleSections = false,
					IsOptional = false,
				})
				.Build();
			helper.DomDefinitions.Create(solutionDefinition);
			Log("Installed DOM definition for Model Registration");
		}
	}
}
