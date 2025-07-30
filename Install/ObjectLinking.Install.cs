/*
****************************************************************************
*  Copyright (c) 2025,  Skyline Communications NV  All Rights Reserved.    *
****************************************************************************

By using this script, you expressly agree with the usage terms and
conditions set out below.
This script and all related materials are protected by copyrights and
other intellectual property rights that exclusively belong
to Skyline Communications.

A user license granted for this script is strictly for personal use only.
This script may not be used in any way by anyone without the prior
written consent of Skyline Communications. Any sublicensing of this
script is forbidden.

Any modifications to this script by the user are only allowed for
personal use and within the intended purpose of the script,
and will remain the sole responsibility of the user.
Skyline Communications will not be responsible for any damages or
malfunctions whatsoever of the script resulting from a modification
or adaptation by the user.

The content of this script is confidential information.
The user hereby agrees to keep this confidential information strictly
secret and confidential and not to disclose or reveal it, in whole
or in part, directly or indirectly to any person, entity, organization
or administration without the prior written consent of
Skyline Communications.

Any inquiries can be addressed to:

	Skyline Communications NV
	Ambachtenstraat 33
	B-8870 Izegem
	Belgium
	Tel.	: +32 51 31 35 69
	Fax.	: +32 51 31 01 29
	E-mail	: info@skyline.be
	Web		: www.skyline.be
	Contact	: Ben Vandenberghe

****************************************************************************
Revision History:

DATE		VERSION		AUTHOR			COMMENTS

25/07/2025	1.0.0.1		AMA, Skyline	Initial version
****************************************************************************
*/

using System;

using DomHelpers.SlcObject_Linking;

using Skyline.AppInstaller;
using Skyline.DataMiner.Automation;
using Skyline.DataMiner.Net;
using Skyline.DataMiner.Net.AppPackages;
using Skyline.DataMiner.Net.Apps.DataMinerObjectModel;
using Skyline.DataMiner.Net.Apps.Modules;
using Skyline.DataMiner.Utils.DOM.Builders;

/// <summary>
/// DataMiner Script Class.
/// </summary>
public class Script
{
	/// <summary>
	/// The script entry point.
	/// </summary>
	/// <param name="engine">Provides access to the Automation engine.</param>
	/// <param name="context">Provides access to the installation context.</param>
	[AutomationEntryPoint(AutomationEntryPointType.Types.InstallAppPackage)]
	public void Install(IEngine engine, AppInstallContext context)
	{
		try
		{
			engine.Timeout = new TimeSpan(0, 10, 0);
			engine.GenerateInformation("Starting installation");
			var installer = new AppInstaller(Engine.SLNetRaw, context);
			installer.InstallDefaultContent();

			InstallDomModule(engine.GetUserConnection(), installer.Log);
		}
		catch (Exception e)
		{
			engine.ExitFail($"Exception encountered during installation: {e}");
		}
	}

	public static void InstallDomModule(IConnection connection, Action<string> logMethod = null)
	{
		var moduleHelper = new ModuleSettingsHelper(connection.HandleMessages);
		var module = new DomModuleBuilder()
			.WithModuleId(SlcObject_LinkingIds.ModuleId)
			.WithInformationEvents(false)
			.WithHistory(false)
			.Build();
		moduleHelper.ModuleSettings.Create(module);
		logMethod?.Invoke("Installed module settings for Object Linking");

		var domHelper = new DomHelper(connection.HandleMessages, SlcObject_LinkingIds.ModuleId);
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
		domHelper.SectionDefinitions.Create(entitySection);
		logMethod?.Invoke("Installed section definition for Entity");

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
		domHelper.DomDefinitions.Create(linkDefinition);
		logMethod?.Invoke("Installed DOM definition for Link");
	}
}