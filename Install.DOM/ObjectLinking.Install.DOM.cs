namespace InstallDOM
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using DomHelpers.SlcObject_Linking;

	using Newtonsoft.Json;

	using ObjectLinking.Install.DOM;

	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.Net.Apps.DataMinerObjectModel;
	using Skyline.DataMiner.Net.Apps.Modules;
	using Skyline.DataMiner.Net.Helper;
	using Skyline.DataMiner.Net.Messages.SLDataGateway;
	using Skyline.DataMiner.SDM;
	using Skyline.DataMiner.SDM.ObjectLinking;

	/// <summary>
	/// Represents a DataMiner Automation script.
	/// </summary>
	public class Script
	{
		/// <summary>
		/// The script entry point.
		/// </summary>
		/// <param name="engine">Link with SLAutomation process.</param>
		public void Run(IEngine engine)
		{
			try
			{
				RunSafe(engine);
			}
			catch (ScriptAbortException)
			{
				// Catch normal abort exceptions (engine.ExitFail or engine.ExitSuccess)
				throw; // Comment if it should be treated as a normal exit of the script.
			}
			catch (ScriptForceAbortException)
			{
				// Catch forced abort exceptions, caused via external maintenance messages.
				throw;
			}
			catch (ScriptTimeoutException)
			{
				// Catch timeout exceptions for when a script has been running for too long.
				throw;
			}
			catch (InteractiveUserDetachedException)
			{
				// Catch a user detaching from the interactive script by closing the window.
				// Only applicable for interactive scripts, can be removed for non-interactive scripts.
				throw;
			}
			catch (Exception e)
			{
				engine.ExitFail("Run|Something went wrong: " + e);
			}
		}

		private static void RunSafe(IEngine engine)
		{
			var amount = 0;
			var success = 0;

			var linker = engine.GetObjectLinker();
			var helper = new DomHelper(engine.SendSLNetMessages, SlcObject_LinkingIds.ModuleId);

			var filter = new ANDFilterElement<DomInstance>(
				DomInstanceExposers.DomDefinitionId.Equal(SlcObject_LinkingIds.Definitions.Link.Id),
				DomInstanceExposers.FieldValues.KeyExists(SlcObject_LinkingIds.Sections.EntityDescriptor.ID.Id.ToString()).Equal(true));
			var pagingHelper = helper.DomInstances.PreparePaging(filter, helper.DomInstances.MaxAmountBulkOperation - 5);
			while (pagingHelper.MoveToNextPage())
			{
				var page = pagingHelper.GetCurrentPage();

				var toBeRemoved = new List<DomInstance>();
				var links = new List<Link>();
				var batch = page.Select(instance => new LinkInstance(instance));
				batch.ForEach(link =>
				{
					try
					{
						var translated = TranslateToNewFormat(link, out var removed);
						links.AddRange(translated);
						amount += translated.Count;
						if (removed is null)
						{
							return;
						}

						toBeRemoved.Add(removed);
					}
					catch
					{
						engine.Log($"Failed to translate link with id {link.ID.Id}, skipping. Link details: {JsonConvert.SerializeObject(link)}");
					}
				});

				if (!links.Any())
				{
					continue;
				}

				try
				{
					linker.Links.CreateOrUpdate(links);
					success += page.Count;
				}
				catch (SdmBulkCrudException<Link> ex)
				{
					engine.Log($"Failed to migrate a batch of links: {ex}");
					success += ex.SuccessfulItems.Count;
				}
			}

			engine.AddOrUpdateScriptOutput("amount", Convert.ToString(amount));
			engine.AddOrUpdateScriptOutput("success", Convert.ToString(success));
		}

		private static List<Link> TranslateToNewFormat(LinkInstance linkInstance, out DomInstance removed)
		{
			removed = null;
			var result = new List<Link>();

			if (linkInstance is null ||
				linkInstance.EntityDescriptor.Count == 0)
			{
				// link is already migrated
				return result;
			}

			// First check if it's an easy translation, meaning only 2 entity descriptors, one source and one target.
			if (linkInstance.EntityDescriptor.Count == 2)
			{
				result.Add(TranslateToNewFormat(linkInstance.EntityDescriptor[0], linkInstance.EntityDescriptor[1], linkInstance.ID.Id.ToString()));
				return result;
			}

			// otherwise create a link per combination
			var seen = new HashSet<string>();
			for (int i = 0; i < linkInstance.EntityDescriptor.Count; i++)
			{
				var section1 = linkInstance.EntityDescriptor[i];
				for (int j = 0; j < linkInstance.EntityDescriptor.Count; j++)
				{
					var section2 = linkInstance.EntityDescriptor[j];
					if (section1 == section2)
					{
						continue;
					}

					var forward = $"{section1.ID} - {section2.ID}";
					var backward = $"{section1.ID} - {section2.ID}";
					if (seen.Contains(forward) ||
						seen.Contains(backward))
					{
						continue;
					}

					result.Add(TranslateToNewFormat(section1, section2));
					seen.Add(forward);
					seen.Add(backward);
				}
			}

			removed = linkInstance;
			return result;
		}

		private static Link TranslateToNewFormat(EntityDescriptorSection section1, EntityDescriptorSection section2, string linkIdentifier = null)
		{
			var identifier = linkIdentifier ?? Guid.NewGuid().ToString();
			return new Link
			{
				Identifier = identifier,
				Direction = Skyline.DataMiner.SDM.ObjectLinking.Models.LinkDirection.Symmetric,
				Source = new EntityDescriptor
				{
					ID = section1.ID,
					DisplayName = section1.DisplayName,
					ModelName = String.IsNullOrEmpty(section1.ModelName) ? "Migrated" : section1.ModelName,
					SolutionID = String.IsNullOrEmpty(section1.SolutionName) ? "Migrated" : section1.SolutionName, // SolutionID is not available in the old format, so we use the SolutionName as a fallback.
					SolutionName = String.IsNullOrEmpty(section1.SolutionName) ? "Migrated" : section1.SolutionName,
					ParentID = section1.ParentID,
					ParentModelName = section1.ParentModelName,
				},
				Target = new EntityDescriptor
				{
					ID = section1.ID,
					DisplayName = section1.DisplayName,
					ModelName = String.IsNullOrEmpty(section1.ModelName) ? "Migrated" : section1.ModelName,
					SolutionID = String.IsNullOrEmpty(section1.SolutionName) ? "Migrated" : section1.SolutionName, // SolutionID is not available in the old format, so we use the SolutionName as a fallback.
					SolutionName = String.IsNullOrEmpty(section1.SolutionName) ? "Migrated" : section1.SolutionName,
					ParentID = section1.ParentID,
					ParentModelName = section1.ParentModelName,
				},
			};
		}
	}
}
