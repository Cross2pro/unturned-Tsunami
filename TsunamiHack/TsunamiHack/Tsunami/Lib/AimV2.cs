using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using Mono.Security;
using Pathfinding;
using SDG.Unturned;
using Steamworks;
using TsunamiHack.Tsunami.Manager;
using TsunamiHack.Tsunami.Types;
using TsunamiHack.Tsunami.Types.Lists;
using TsunamiHack.Tsunami.Util;
using UnityEngine;
using UnityEngine.Networking;

namespace TsunamiHack.Tsunami.Lib
{
	internal class AimV2
	{
		private static DateTime lastLock;
		private static Menu.Aim menu;

		private static float defsense;

		private static FieldInfo prim;
		private static int id;

		private static FieldInfo yaw;
		private static FieldInfo pitch;

		private static Transform target;

		private static List<Zombie> Zombies;
		private static int update;

		internal static void Start()
		{
			lastLock = DateTime.Now;
			menu = WaveMaker.MenuAim;

			update = 50;
			defsense = Player.player.look.sensitivity;

			prim = typeof(PlayerEquipment).GetField("prim", BindingFlags.Instance | BindingFlags.NonPublic);
			yaw = typeof(PlayerLook).GetField("_yaw", BindingFlags.Instance | BindingFlags.NonPublic);
			pitch = typeof(PlayerLook).GetField("_pitch", BindingFlags.Instance | BindingFlags.NonPublic);
		}

		internal static void Update()
		{
			UpdateAimbot();
			
			if ((DateTime.Now - lastLock).TotalMilliseconds >= menu.LockUpdateRate)
			{
				UpdateLock();
				UpdateTb();
				

				lastLock = DateTime.Now;
			}
		}

		// aimlock

		internal static void UpdateLock()
		{
			if (menu.EnableAimlock && Provider.isConnected)
			{
				var range = menu.LockDistance;
				if (menu.LockGunRange && Player.player.equipment.asset is ItemGunAsset)
				{
					range = ((ItemGunAsset) Player.player.equipment.asset).range;
				}

				RaycastHit hit;
				Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, range,
					RayMasks.DAMAGE_CLIENT);

				if (hit.transform != null)
				{
					if (menu.LockPlayers && hit.transform.CompareTag("Enemy")) //if player
					{
						if (!menu.AimWhitelistFriends && !IsPlayerFriend(hit, 1))
							ChangeSense(true);
						else
							ChangeSense(false);
					}
					else if (menu.LockZombies && hit.transform.CompareTag("Zombie")) //if zombie
						ChangeSense(true);
					else if (menu.LockAnimals && hit.transform.CompareTag("Animal")) //if animal
						ChangeSense(true);
					else if (menu.LockVehicles && hit.transform.CompareTag("Vehicle")) //if vehicle
						ChangeSense(true);
					else //if none
						ChangeSense(false);

				}
				else
					ChangeSense(false); //if transform is null

			}
		}

		internal static void ChangeSense(bool enable)
		{
			if (enable)
				Player.player.look.sensitivity = defsense / menu.LockSensitivity;
			else
				Player.player.look.sensitivity = defsense;
		}

		internal static bool IsPlayerFriend(RaycastHit rch, int type)
		{
			try
			{
				//check if hit is a player
				if (rch.transform != null && rch.transform.CompareTag("Enemy"))
				{
					//go through clients and match hit to client
					SteamPlayer ply = null;
					foreach (var client in Provider.clients)
					{
						//if `n client is the hit, return that client
						if (client.player.gameObject == rch.transform.gameObject)
							ply = client;
						else
							// else return null
							ply = null;
					}

					//if not null
					if (ply != null)
					{
						//if the player is in the friends return true
						if (WaveMaker.Friends.Contains(ply.playerID.steamID.m_SteamID))
							return true;

						//if the player isnt a friend, but admin && admins are whitelisted return true

						switch (type)
						{
							case 1:
								if (ply.isAdmin && menu.LockWhitelistAdmins)
									return true;
								break;
							case 2:
								if (ply.isAdmin && menu.TriggerWhiteListAdmins)
									return true;
								break;
							case 3:
								if (ply.isAdmin && menu.AimWhitelistAdmins)
									return true;
								break;
						}

						//else return false
						return false;
					}

					//if unable to match the client throw unable tomatch
					throw new UnableToMatchPlayerException();
				}

			} //if cant match log exception
			catch (UnableToMatchPlayerException e)
			{
				Logging.Exception(e);
			} //suppress general exceptions
			catch (Exception)
			{
			}

			return false;
		}

		// triggerbot

		internal static void UpdateTb()
		{
			if (menu.EnableTriggerbot && Provider.isConnected)
			{
				var range = menu.TriggerDistance;
				if (menu.TriggerGunRange && Player.player.equipment.asset is ItemGunAsset)
					range = ((ItemGunAsset) Player.player.equipment.asset).range;


				RaycastHit hit;
				Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, range,
					RayMasks.DAMAGE_CLIENT);

				bool fire = false;

				if (hit.transform != null)
				{
					if (menu.TriggerPlayers && hit.transform.CompareTag("Enemy"))
					{
						//if we are not whitelisting and player isnt friend
						if (!menu.TriggerWhiteListFriends && !IsPlayerFriend(hit, 2))
							fire = true;
					}

					if (menu.TriggerZombies && hit.transform.CompareTag("Zombie"))
					{
						fire = true;
					}

					if (menu.TriggerAnimals && hit.transform.CompareTag("Animal"))
					{
						fire = true;
					}

					if (menu.TriggerVehicles && hit.transform.CompareTag("Vehicle"))
					{
						fire = true;
					}

					if (fire)
					{
						prim.SetValue(Player.player.equipment, id <= 3);
						id++;
						if (id > 6)
							id = 0;
					}


				}
			}


		}

		// aimbot


		internal static void UpdateAimbot()
		{
			var tarLimb = menu.Limb == 1 ? "Skull" : "Spine";

			if (menu.EnableAimbot)
			{

				if (!menu.AimManualChangeTarget)
				{
					target = null;
				}


				var dist = menu.AimDistance;
				if (menu.AimInfDistance && Player.player.equipment.asset is ItemWeaponAsset)
				{
					dist = ((ItemWeaponAsset) Player.player.equipment.asset).range;
				}


				float squarerange;
				if (!menu.AimClosest)
				{
					squarerange = dist * dist;
				}
				else
				{
					squarerange = float.PositiveInfinity;
				}



				if (menu.AimPlayers)
				{
					foreach (var client in Provider.clients)
					{
						try
						{
							
							//if client != local client
							//if player isnt friend or player is friend and whitelist friends is disabled
							//if player isnt admin or player is admin and whitelist admins is disabled

							if (client.player != Player.player)
							{
								if (!WaveMaker.Friends.Contains(client.playerID.steamID.m_SteamID) ||
								    (WaveMaker.Friends.Contains(client.playerID.steamID.m_SteamID) && !menu.AimWhitelistFriends))
								{
									if (!client.isAdmin || (client.isAdmin && !menu.AimWhitelistAdmins))
									{
										var tarLimbPos = GetLimbPosition(client.player.transform, tarLimb);

										float sqrMagnitude;
										if (menu.AimClosest)
										{
											var tarScrnPt = Camera.main.WorldToScreenPoint(tarLimbPos);
											sqrMagnitude = (new Vector3(Screen.width / 2f, Screen.height / 2f) - tarScrnPt).sqrMagnitude;
										}
										else
										{
											sqrMagnitude = (tarLimbPos - Camera.main.transform.position).sqrMagnitude;
										}

										if (sqrMagnitude <= squarerange && !client.player.life.isDead && Player.player.name != client.player.name)
										{
											if (!menu.Aim360)
											{
												var tarLimbPosScrnPt = Camera.main.WorldToViewportPoint(tarLimbPos);
												if (tarLimbPosScrnPt.z <= 0f || tarLimbPosScrnPt.x <= 0f || tarLimbPosScrnPt.x >= 1f ||
												    tarLimbPosScrnPt.y <= 0f || tarLimbPosScrnPt.y >= 1f)
												{
													goto Exit;
												}
											}
											if (!menu.AimIgnoreWalls)
											{
												var distance = tarLimbPos - Player.player.look.aim.position;
												RaycastHit raycastHit;
												if (!Physics.Raycast(Player.player.look.aim.position, distance, out raycastHit, dist,
													    RayMasks.DAMAGE_CLIENT) ||
												    !raycastHit.transform.CompareTag("Enemy"))
												{
													goto Exit;
												}
											}
											target = client.player.transform;
											squarerange = sqrMagnitude;
										}
									}
								}
								else
								{
									target = null;
									goto Exit;

								}
							}
							
						} catch (System.Exception){}
						
						Exit:;
					}
				}

				if (menu.AimZombies)
				{

					update++;
					if (update > 50)
					{
						Zombies = new List<Zombie>();
						foreach (var region in ZombieManager.regions)
						{
							foreach (var zom in region.zombies)
							{
								Zombies.Add(zom);
							}
						}

						update = 0;
					}
					
					
					foreach (var zombie in Zombies)
					{
						if (menu.AimManualChangeTarget && target == zombie.transform) continue;
						
						var targetVectorPos = GetLimbPosition(zombie.transform, tarLimb);
						float distance;
						if (menu.AimClosest)
						{
							var scrnpt = Camera.main.WorldToScreenPoint(targetVectorPos);
							distance = (new Vector3(Screen.width / 2f, Screen.height / 2f) - scrnpt).sqrMagnitude;
						}
						else
						{
							distance = (targetVectorPos - Camera.main.transform.position).sqrMagnitude;
						}
						
						if (!(distance <= squarerange) || zombie.isDead) continue;
						
						if (!menu.Aim360)
						{
							var targetScrnPt = Camera.main.WorldToViewportPoint(targetVectorPos);
							if (targetScrnPt.z <= 0f || targetScrnPt.x <= 0f || targetScrnPt.x >= 1f || targetScrnPt.y <= 0f || targetScrnPt.y >= 1f)
							{
								continue;
							}
						}
						if (!menu.AimIgnoreWalls)
						{
							var vector8 = targetVectorPos - Player.player.look.aim.position;
							RaycastHit raycastHit2;
							if (!Physics.Raycast(Player.player.look.aim.position, vector8, out raycastHit2, dist, RayMasks.DAMAGE_CLIENT) || !raycastHit2.transform.CompareTag("Zombie"))
							{
								continue;
							}
						}
						target = zombie.transform;
						squarerange = distance;
					}
				}


				if (target == null || menu.AimSilent) return;
				
				if (menu.AimManualChangeTarget && !menu.AimIgnoreWalls)
				{
					var vector9 = GetLimbPosition(target.transform, tarLimb) - Camera.main.transform.position;
					RaycastHit hit;
					if (!Physics.Raycast(Player.player.look.aim.position, vector9, out hit, dist, RayMasks.DAMAGE_CLIENT))
					{
						return;
					}
					if (!hit.transform.CompareTag("Zombie") && !hit.transform.CompareTag("Enemy") &&
					    !hit.transform.CompareTag("Animal") && !hit.transform.CompareTag("Vehicle"))
					{
						return;
					}
				}

				var num3 = menu.AimSpeed * 5f;
				var targetVector = GetLimbPosition(target.transform, tarLimb);
				var quaternion = Quaternion.LookRotation(targetVector - Player.player.look.aim.position);
				var quaternion2 = Quaternion.RotateTowards(Camera.main.transform.rotation, quaternion, num3);
				
				var xVal = quaternion2.eulerAngles.x;
				if (xVal <= 90f)
				{
					xVal += 90f;
				}
				if (xVal > 180f)
				{
					xVal -= 270f;
				}
				
				yaw.SetValue(Player.player.look, quaternion2.eulerAngles.y);
				pitch.SetValue(Player.player.look, xVal);
			}
		}




		public static Vector3 GetLimbPosition(Transform target, string objName)
		{
			var componentsInChildren = target.transform.GetComponentsInChildren<Transform>();
			var result = Vector3.zero;

			if (componentsInChildren == null) return result;
			
			foreach (var transform in componentsInChildren)
			{
				if (transform.name.Trim() != objName) continue;
					
				result = transform.position + new Vector3(0f, 0.4f, 0f);
				break;
			}
			
			return result;
		}


	}
}

