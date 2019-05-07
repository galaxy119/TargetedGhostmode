using System.Collections.Generic;
using System.Linq;
using Smod2;
using Smod2.API;
using Smod2.Commands;

namespace TargetedGhostmode
{
	public class Commands : ICommandHandler
	{
		private readonly TargetedGhostmode plugin;
		public Commands(TargetedGhostmode plugin) => this.plugin = plugin;
		
		public string GetUsage() => 
			"ghost hide (target) (victim) - Hides \"target\" from \"victim\" \n"+
			"ghost unhide (target) (victim) - Unhides hiding \"target\" from \"victim\"";

		public string GetCommandDescription() => "";

		public string[] OnCall(ICommandSender sender, string[] args)
		{
			if (args.Length <= 0) return new[] { GetUsage() };

			switch (args[0].ToLower())
			{
				case "help":
					return new[] { GetUsage() };
				case "hide":
				{
					if (args.Length < 2)
						return new[] { "You must specify a target (to be hidden) and a victim (to hide from)" };

					List<Player> victims = plugin.Server.GetPlayers(args[1]);
					List<Player> targets = plugin.Server.GetPlayers(args[2]);
					if (targets.Count == 0) return new[] { "Target not found." };
					if (victims.Count == 0) return new[] { "Victim not found" };
					Player target = targets.OrderBy(ply => ply.Name.Length).First();
					Player victim = victims.OrderBy(ply => ply.Name.Length).First();

					plugin.Functions.Hide(victim, target);
					

					return new[] { target.Name + " has been hidden from " + victim.Name };
				}
				case "unhide":
				{
					if (args.Length < 2)
						return new[] { "You must specify a target (to be hidden) and a victim (to hide from)" };

					List<Player> victims = plugin.Server.GetPlayers(args[1]);
					List<Player> targets = plugin.Server.GetPlayers(args[2]);
					if (targets.Count == 0) return new[] { "Target not found." };
					if (victims.Count == 0) return new[] { "Victim not found" };
					Player target = targets.OrderBy(ply => ply.Name.Length).First();
					Player victim = victims.OrderBy(ply => ply.Name.Length).First();

					plugin.Functions.Unhide(victim, target);

					return new[] { target.Name + " has been un-hidden from " + victim.Name };
				}
				case "unhideall":
				{
					List<Player> players = plugin.Server.GetPlayers();
					
					foreach (Player player1 in players)
						foreach (Player player2 in players)
							if (player2 != player1)
								plugin.Functions.Unhide(player1, player2);

					return new[] { "Unhiding everyone from everyone." };
				}

				case "debug":
				{
					List<Player> victims = plugin.Server.GetPlayers(args[1]);
					List<Player> targets = plugin.Server.GetPlayers(args[2]);

					Player target = targets.OrderBy(ply => ply.Name.Length).First();
					Player victim = victims.OrderBy(ply => ply.Name.Length).First();

					foreach (int pid in TargetedGhostmode.Hidden.Keys)
					{
						plugin.Info("Key: " + pid);
						foreach (int pid2 in TargetedGhostmode.Hidden[pid])
							plugin.Info("Value: " + pid2);
					}
					
					return new[] { Methods.CheckHidden(victim.PlayerId, target.PlayerId).ToString() };

				}
				default:
					return new[] { GetUsage() };
			}
		}
	}
}