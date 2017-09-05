using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Mono.Security;
using Pathfinding;
using SDG.Unturned;
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

		internal static void Start()
		{
			lastLock = DateTime.Now;
			menu = WaveMaker.MenuAim;

			defsense = Player.player.look.sensitivity;

			prim = typeof(PlayerEquipment).GetField("prim", BindingFlags.Instance | BindingFlags.NonPublic);
			yaw = typeof(PlayerLook).GetField("_yaw", BindingFlags.Instance | BindingFlags.NonPublic);
			pitch = typeof(PlayerLook).GetField("_pitch", BindingFlags.Instance | BindingFlags.NonPublic);
		}

		internal static void Update()
		{
			if ((DateTime.Now - lastLock).TotalMilliseconds >= menu.LockUpdateRate)
			{
				UpdateLock();
				UpdateTb();
				UpdateAimbot();

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
					return false;
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

		internal static void UpdateAim()
		{
			//if aimbot is enabled and you are connected to server

			//if player aimbot is enabled
			//setup range
			//go through all players
			//if player is closer than range or inf range 
			//if player is an enemy, or no whitelists are active
			//mark that transform as target

			Transform closestTarget = null;
			float closestDist = 0f;

			if (menu.EnableAimbot && Provider.isConnected)
			{
				if (menu.AimPlayers)
				{
					var range = menu.AimDistance;
					if (menu.AimUseGunDistance && Player.player.equipment.asset is ItemGunAsset)
						range = ((ItemGunAsset) Player.player.equipment.asset).range;

//					foreach (var client in Provider.clients)
//					{
//						if (menu.AimWhitelistFriends && WaveMaker.Friends.Contains(client.playerID.steamID.m_SteamID))
//							goto END;
//
//						if (menu.END:;
//					}

				}

				if (closestTarget != null)
				{

				}

			}
		}

		internal static void UpdateAimbot()
		{
			var tarLimb = menu.Limb == 1 ? "Skull" : "Spine";

			if (menu.EnableAimbot)
			{
				//Setup vars
				var player = Player.player;
				var look = Player.player.look;
				var main = Camera.main;
				var position = main.transform.position;


				if (!menu.AimManualChangeTarget)
				{
					target = null;
				}


				var dist = menu.AimDistance;
				if (menu.AimInfDistance && player.equipment.asset is ItemWeaponAsset)
				{
					dist = ((ItemWeaponAsset) player.equipment.asset).range;
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
							if (!menu.AimManualChangeTarget || !(target == client.player.transform))
							{

								if (!menu.AimWhitelistFriends || !WaveMaker.Friends.Contains(client.playerID.steamID.m_SteamID))
								{
									if (!menu.AimWhitelistAdmins || !client.isAdmin)
									{
										var tarLimbPos = getVectorOfTarget(client.player.transform, tarLimb);

										float sqrMagnitude;
										if (menu.AimClosest)
										{
											var tarScrnPt = main.WorldToScreenPoint(tarLimbPos);
											sqrMagnitude = (new Vector3((float) (Screen.width / 2), (float) (Screen.height / 2)) - tarScrnPt)
												.sqrMagnitude;
										}
										else
										{
											sqrMagnitude = (tarLimbPos - position).sqrMagnitude;
										}

										if (sqrMagnitude <= squarerange && !client.player.life.isDead && Player.player.name != client.player.name)
										{
											if (!menu.Aim360)
											{
												var tarLimbPosScrnPt = main.WorldToViewportPoint(tarLimbPos);
												if (tarLimbPosScrnPt.z <= 0f || tarLimbPosScrnPt.x <= 0f || tarLimbPosScrnPt.x >= 1f ||
												    tarLimbPosScrnPt.y <= 0f || tarLimbPosScrnPt.y >= 1f)
												{
													goto Exit;
												}
											}
											if (!menu.AimIgnoreWalls)
											{
												var distance = tarLimbPos - look.aim.position;
												RaycastHit raycastHit;
												if (!Physics.Raycast(look.aim.position, distance, out raycastHit, dist, RayMasks.DAMAGE_CLIENT) ||
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
							}
						}
						catch (System.Exception)
						{
						}
						Exit:
						;
					}
				}

//				if (menu.AimZombies)
//				{
//					foreach (var current in)
//					{
//						if (!this.manuallyChangeTarget || !(menu_Aimbot.target == current.transform))
//						{
//							tarScrnPt3 vector5 = menu_Aimbot.getVectorOfTarget(current.transform, tarLimb);
//							float sqrMagnitude2;
//							if (this.aimClosestCrosshair)
//							{
//								Vector3 vector6 = main.WorldToScreenPoint(vector5);
//								sqrMagnitude2 = (new Vector3((float) (Screen.width / 2), (float) (Screen.height / 2)) - vector6).sqrMagnitude;
//							}
//							else
//							{
//								sqrMagnitude2 = (vector5 - position).sqrMagnitude;
//							}
//							if (sqrMagnitude2 <= squarerange && !current.isDead)
//							{
//								if (!this.aim360)
//								{
//									Vector3 vector7 = main.WorldToViewportPoint(vector5);
//									if (vector7.z <= 0f || vector7.x <= 0f || vector7.x >= 1f || vector7.y <= 0f || vector7.y >= 1f)
//									{
//										continue;
//									}
//								}
//								if (!this.aimWalls)
//								{
//									Vector3 vector8 = vector5 - look.aim.position;
//									RaycastHit raycastHit2;
//									if (!Physics.Raycast(look.aim.position, vector8, out raycastHit2, dist, RayMasks.DAMAGE_CLIENT) ||
//									    raycastHit2.transform.tag != "Zombie")
//									{
//										continue;
//									}
//								}
//								menu_Aimbot.target = current.transform;
//								squarerange = sqrMagnitude2;
//							}
//						}
//					}
//				}


				if (target != null && !menu.AimSilent)
				{
					if (menu.AimManualChangeTarget && !menu.AimIgnoreWalls)
					{
						var vector9 = getVectorOfTarget(target.transform, tarLimb) - Camera.main.transform.position;
						RaycastHit hit;
						if (!Physics.Raycast(look.aim.position, vector9, out hit, dist, RayMasks.DAMAGE_CLIENT))
						{
							return;
						}
						if (!hit.transform.CompareTag("Zombie") && !hit.transform.CompareTag("Enemy") &&
						    !hit.transform.CompareTag("Animal") && !hit.transform.CompareTag("Vehicle"))
						{
							return;
						}
					}

					var targetVector = getVectorOfTarget(target.transform, tarLimb);
					var num3 = menu.AimSpeed * 100f * Time.deltaTime;
					var quaternion = Quaternion.LookRotation(targetVector - look.aim.position);
					var quaternion2 = Quaternion.RotateTowards(main.transform.rotation, quaternion, num3);
					var xVal = quaternion2.eulerAngles.x;
					if (xVal <= 90f)
					{
						xVal += 90f;
					}
					if (xVal > 180f)
					{
						xVal -= 270f;
					}
					yaw.SetValue(look, quaternion2.eulerAngles.y);
					pitch.SetValue(look, xVal);
				}
			}
		}




		public static Vector3 getVectorOfTarget(Transform target, string objName)
		{
			Transform[] componentsInChildren = target.transform.GetComponentsInChildren<Transform>();
			Vector3 result = Vector3.zero;

			if (componentsInChildren != null)
			{
				Transform[] array = componentsInChildren;
				for (int i = 0; i < array.Length; i++)
				{
					Transform transform = array[i];
					if (transform.name.Trim() == objName)
					{
						result = transform.position + new Vector3(0f, 0.4f, 0f);
						break;
					}
				}
			}
			return result;
		}


	}
}

