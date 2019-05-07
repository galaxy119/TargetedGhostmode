using Harmony;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using RemoteAdmin;
using Smod2;

namespace TargetedGhostmode
{
	[HarmonyPatch(typeof(PlayerPositionManager)),HarmonyPatch("TransmitData")]
	
	public class Patch
	{
		[HarmonyPrefix]
				static bool Prefix(PlayerPositionManager __instance)
		{
			List<PlayerPositionData> posData = new List<PlayerPositionData>();
			List<GameObject> players = PlayerManager.singleton.players.ToList();
			bool smGhostMode = ConfigFile.GetBool("sm_enable_ghostmode", false);

			foreach (GameObject player in players)
			{
				posData.Add(new PlayerPositionData(player));
			}

			__instance.ReceiveData(posData.ToArray());

			foreach (GameObject gameObject in players)
			{
				CharacterClassManager component = gameObject.GetComponent<CharacterClassManager>();
				int pid1 = gameObject.GetComponent<QueryProcessor>().PlayerId;

				if (smGhostMode && gameObject != __instance.gameObject && component.curClass >= 0)
				{
					for (int i = 0; i < posData.Count; i++)
					{
						if (players[i] == gameObject) continue;
						
						CharacterClassManager component2 = players[i].GetComponent<CharacterClassManager>();
						int pid2 = players[i].GetComponent<QueryProcessor>().PlayerId;
						if (smGhostMode && component2.smGhostMode && component2.curClass >= 0 && component2.curClass != 2 && (component.curClass != 2 || (!component2.smVisibleToSpec && component.curClass == 2)) && (!component2.smVisibleWhenTalking || (component2.smVisibleWhenTalking && !component2.GetComponent<Radio>().NetworkisTransmitting)))
						{
							posData[i] = new PlayerPositionData
							{
								position = Vector3.up * 6000f,
								rotation = 0f,
								playerID = posData[i].playerID
							};
						}

						if (TargetedGhostmode.Hidden.ContainsKey(pid1))
						{
							if (TargetedGhostmode.Hidden[pid1].Contains(pid2))
								posData[i] = new PlayerPositionData
								{
									position = Vector3.up * 6000f, rotation = 0f, playerID = posData[i].playerID
								};
						}
					}
				}
				switch (component.curClass)
				{
					case 16:
					case 17:
					{
						List<PlayerPositionData> posData939 = new List<PlayerPositionData>(posData);

						for (int i = 0; i < posData939.Count; i++)
						{
							CharacterClassManager component2 = players[i].GetComponent<CharacterClassManager>();
							if (posData939[i].position.y < 800f && component2.klasy[component2.curClass].team != Team.SCP && component2.klasy[component2.curClass].team != Team.RIP && !players[i].GetComponent<Scp939_VisionController>().CanSee(component.GetComponent<Scp939PlayerScript>()))
							{
								posData939[i] = new PlayerPositionData
								{
									position = Vector3.up * 6000f,
									rotation = 0f,
									playerID = posData939[i].playerID
								};
							}
						}
						__instance.CallTargetTransmit(gameObject.GetComponent<NetworkIdentity>().connectionToClient, posData939.ToArray());
						break;
					}
					default:
						__instance.CallTargetTransmit(gameObject.GetComponent<NetworkIdentity>().connectionToClient, posData.ToArray());
						break;
				}
			}

			return false;
		}

		internal static void PatchMethodUsingHarmony()
		{
			HarmonyInstance harmony = HarmonyInstance.Create("com.joker119.ghost");
			harmony.PatchAll();
		}
	}
}