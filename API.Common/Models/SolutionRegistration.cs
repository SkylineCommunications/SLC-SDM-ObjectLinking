// Ignore Spelling: SDM Api

namespace Skyline.DataMiner.SDM.ObjectLinking.Models
{
	using System.Collections.Generic;

	using DomHelpers.SlcObject_Linking;

	[DataMinerObject]
	[SdmDomStorage(SlcObject_LinkingIds.ModuleId)]
	public class SolutionRegistration : SdmObject<SolutionRegistration>
	{
		public string ID { get; set; }

		public string DisplayName { get; set; }

		public string DefaultApiScriptName { get; set; }

		public string DefaultApiEndpoint { get; set; }

		public string VisualizationEndpoint { get; set; }

		public List<SdmObjectReference<ModelRegistration>> Models { get; internal set; } = new List<SdmObjectReference<ModelRegistration>>();
	}
}
