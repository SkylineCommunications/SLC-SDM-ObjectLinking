namespace ObjectLinkingInstallRegistration
{
	using System;

	using Shared;

	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.SDM.Registration;

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
				var context = new ScriptContext(engine);
				RunSafe(context);
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

		private void RunSafe(ScriptContext context)
		{
			// Register the solution
			var registrar = context.Engine.GetSdmRegistrar();
			var solution = new SolutionRegistration
			{
				Identifier = "a5e01a18-7704-40fc-b2d4-b4b02b096ba9",
				ID = Constants.CatalogIdentifier,
				DisplayName = "SDM Object Linking",
				Version = context.Version,
			};

			registrar.Solutions.CreateOrUpdate(new[] { solution });

			var linkModel = new ModelRegistration
			{
				Identifier = "ddebbe81-7f39-41d0-aed6-6c4079baaa96",
				Name = "standard_data_model_object_link",
				DisplayName = "SDM Object Link",
				Version = "1.0.1",
				Solution = solution,
			};

			registrar.Models.CreateOrUpdate(new[] { linkModel });
		}
	}
}
