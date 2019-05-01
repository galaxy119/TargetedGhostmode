using System.Collections.Generic;
using Smod2.API;

namespace TargetedGhostmode
{
	public class Methods
	{
		public static void Hide(Player target, Player victim)
		{
			if (!TargetedGhostmode.Hidden.ContainsKey(target.PlayerId))
				TargetedGhostmode.Hidden.Add(target.PlayerId, new List<int>());
			
			TargetedGhostmode.Hidden[target.PlayerId].Add(victim.PlayerId);
		}

		public static void Unhide(Player target, Player victim)
		{
			if (!TargetedGhostmode.Hidden.ContainsKey(target.PlayerId))
				return;
			
			if (TargetedGhostmode.Hidden[target.PlayerId].Contains(victim.PlayerId))
				TargetedGhostmode.Hidden[target.PlayerId].Remove(victim.PlayerId);
		}

		public static bool CheckHidden(Player target, Player victim)
		{
			if (TargetedGhostmode.Hidden.ContainsKey(target.PlayerId))
				if (TargetedGhostmode.Hidden[target.PlayerId].Contains(victim.PlayerId))
					return true;
			return false;
		}

		public static bool CheckHidden(int target, int victim)
		{
			if (TargetedGhostmode.Hidden.ContainsKey(target))
				if (TargetedGhostmode.Hidden[target].Contains(victim))
					return true;
			return false;
		}
	}
}