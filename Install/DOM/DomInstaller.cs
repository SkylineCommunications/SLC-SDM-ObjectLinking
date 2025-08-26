// Ignore Spelling: SDM

namespace Skyline.DataMiner.SDM.ObjectLinking.Install.DOM
{
	using System;
	using System.Linq;

	using DomHelpers.SlcObject_Linking;

	using Skyline.DataMiner.Net;
	using Skyline.DataMiner.Net.Apps.DataMinerObjectModel;
	using Skyline.DataMiner.Net.Apps.Modules;
	using Skyline.DataMiner.Net.ManagerStore;
	using Skyline.DataMiner.Net.Messages.SLDataGateway;
	using Skyline.DataMiner.Utils.DOM.Builders;

	public partial class DomInstaller
	{
		private readonly IConnection _connection;
		private readonly Action<string> _logMethod;

		public DomInstaller(IConnection connection, Action<string> logMethod = null)
		{
			_connection = connection;
			_logMethod = logMethod;
		}

		public void InstallDefaultContent()
		{
			Log("Installation for Object Linking started...");

			var moduleHelper = new ModuleSettingsHelper(_connection.HandleMessages);
			var moduleExist = moduleHelper.ModuleSettings.Count(ModuleSettingsExposers.ModuleId.Equal(SlcObject_LinkingIds.ModuleId)) == 0;
			if (!moduleExist)
			{
				Log("Installing Module Settings...");
			}
			else
			{
				Log("Updating Module Settings...");
			}

			var module = new DomModuleBuilder()
				.WithModuleId(SlcObject_LinkingIds.ModuleId)
				.WithInformationEvents(false)
				.WithHistory(false)
				.Build();
			Import(moduleHelper.ModuleSettings, ModuleSettingsExposers.ModuleId.Equal(SlcObject_LinkingIds.ModuleId), module);

			if (!moduleExist)
			{
				Log("Installed Module Settings");
			}
			else
			{
				Log("Updated Module Settings");
			}

			var domHelper = new DomHelper(_connection.HandleMessages, SlcObject_LinkingIds.ModuleId);
			InstallLinks(domHelper);
		}

		internal void Log(string message)
		{
			_logMethod?.Invoke($"ObjectLinking.Installer: {message}");
		}

		private static void Import<T>(ICrudHelperComponent<T> crudHelperComponent, FilterElement<T> equalityFilter, T dataType)
			where T : DataType
		{
			bool exists = crudHelperComponent.Read(equalityFilter).Any();

			if (exists)
			{
				crudHelperComponent.Update(dataType);
			}
			else
			{
				crudHelperComponent.Create(dataType);
			}
		}
	}
}
