using System.Collections.Generic;
using Smod2.API;

namespace TargetedGhostmode
{
	public class Methods
	{
		private readonly TargetedGhostmode plugin;
		public Methods(TargetedGhostmode plugin) => this.plugin = plugin;
		
		public void Hide(Player victim, Player target)
		{
			Hide(victim.PlayerId, target.PlayerId);
		}

		public void Hide(int victim, int target)
		{
			if (!TargetedGhostmode.Hidden.ContainsKey(victim))
			{
				TargetedGhostmode.Hidden.Add(victim, new List<int>());
				plugin.Info("Creating new Hidden dictionary.");
			}

			if (TargetedGhostmode.Hidden[victim].Contains(target))
			{
				plugin.Info("Target already hidden from victim.");
				return;
			}

			TargetedGhostmode.Hidden[victim].Add(target);
			plugin.Info("Hiding " + target + " from " + victim);
		}

		public void Unhide(int victim, int target)
		{
			if (!TargetedGhostmode.Hidden.ContainsKey(victim))
			{
				plugin.Info("Unable to find dictionary entry for " + victim);
				return;
			}

			if (TargetedGhostmode.Hidden[victim].Contains(target))
			{
				plugin.Info("Unhiding " + target + " from " + victim);
				TargetedGhostmode.Hidden[victim].Remove(target);
			}
			else
				plugin.Info(target + " is not hidden from " + victim);
		}

		public void Unhide(Player victim, Player target)
		{
			Unhide(victim.PlayerId, target.PlayerId);
		}

		public static bool CheckHidden(int victim, int target) =>
			TargetedGhostmode.Hidden.ContainsKey(victim) && TargetedGhostmode.Hidden[victim].Contains(target);
	}
}