using System.Collections.Generic;
using Smod2;
using Smod2.Attributes;

namespace TargetedGhostmode
{
	[PluginDetails(author = "Joker119",
		name = "Targeted Ghostmode",
		description = "An api for creating targeted ghostmode.",
		id = "joker.TargetGhostmode",
		version = "1.0.0",
		SmodMajor = 3,
		SmodMinor = 4,
		SmodRevision = 0
		)]

	public class TargetedGhostmode : Plugin
	{
		public Methods Functions { get;  private set; }
		public static Dictionary<int, List<int>> Hidden = new Dictionary<int, List<int>>();
		
		public override void Register()
		{
			Patch.PatchMethodUsingHarmony();
			
			
			Functions = new Methods(this);
			
			AddCommands(new string[] { "tghost" }, new Commands(this));
		}

		public override void OnEnable()
		{
			Info(Details.name + " v." + Details.version + " enabled.");
		}

		public override void OnDisable()
		{
			Info(Details.name + " disabled.");
		}
	}
}