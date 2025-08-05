// Ignore Spelling: SDM Api

namespace Skyline.DataMiner.SDM.ObjectLinking.Models
{
	using DomHelpers.SlcObject_Linking;

	[DataMinerObject]
	[SdmDomStorage(SlcObject_LinkingIds.ModuleId)]
	public class ModelRegistration : SdmObject<ModelRegistration>
	{
		public string Name { get; set; }

		public string DisplayName { get; set; }

		public string ApiScriptName { get; set; }

		public string ApiEndpoint { get; set; }

		public string VisualizationEndpoint { get; set; }
	}
}
