using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SDG.Framework.UI.Components;
using SDG.Unturned;
using TsunamiHack.Tsunami.Manager;
using TsunamiHack.Tsunami.Types;
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

		private static System.Collections.Generic.List<Zombie> Zombies;
		private static int update;

		internal static void Start()
		{
			lastLock = DateTime.Now;
			menu = WaveMaker.MenuAim;

			update = 0;
			defsense = Player.player.look.sensitivity;

			prim = typeof(PlayerEquipment).GetField("prim", BindingFlags.Instance | BindingFlags.NonPublic);
			yaw = typeof(PlayerLook).GetField("_yaw", BindingFlags.Instance | BindingFlags.NonPublic);
			pitch = typeof(PlayerLook).GetField("_pitch", BindingFlags.Instance | BindingFlags.NonPublic);

			preaimpitch = float.NaN;
			preaimyaw = float.NaN;
			
			Zombies = new List<Zombie>();
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

		private static float preaimyaw;
		private static float preaimpitch;
		

		internal static void UpdateAimbot()
		{
			var tarLimb = menu.Limb == 1 ? "Skull" : "Spine";
			var currClosest = menu.AimDistance;
			target = null;
			
			if (menu.EnableAimbot)
			{
				var dist = menu.AimDistance;
				if (menu.AimUseGunDistance && Player.player.equipment.asset is ItemWeaponAsset)
				{
					dist = ((ItemWeaponAsset) Player.player.equipment.asset).range;
					currClosest = dist;
				}

				/*if (menu.AimPlayers)
				{

					foreach (var player in Provider.clients)
					{
						try
						{
							//elimiated yourself, dead players,  admins if whitelisting, and friends if whitelisting
							if (player.player == Player.player) continue;
							if (player.player.life.isDead) continue;
							if (player.isAdmin && menu.AimWhitelistAdmins) continue;
							if (WaveMaker.Friends.Contains(player.playerID.steamID.m_SteamID) && menu.AimWhitelistFriends) continue;

							//elimiate people who are further than your aimdistance
							var distance = Vector3.Distance(Camera.main.transform.position, player.player.transform.position);
							if (distance > menu.AimDistance) continue;

							//if you cant see the player and you arent doing 360 aim, skip	
							if (!menu.Aim360)
							{
								var renderer = player.player.gameObject.GetComponent<Renderer>();
								if (!renderer.isVisible) continue;
							}

							//if you arent ignoring walls and there is things between you and the player, skip
							if (!menu.AimIgnoreWalls)
							{
								RaycastHit hit;
								Physics.Raycast(Camera.main.transform.position, player.player.transform.position, out hit,
									float.PositiveInfinity, RayMasks.DAMAGE_CLIENT);
								if (!hit.transform.CompareTag("Enemy")) continue;
							}

							//Get target screen point and center point
							var targetpoint = Camera.main.WorldToScreenPoint(GetLimbPosition(player.player.transform, tarLimb));
							targetpoint.y = Camera.main.pixelHeight - targetpoint.y;
							var centerpoint = new Vector3((float) Camera.main.pixelWidth / 2, (float) Camera.main.pixelHeight / 2);

							//Calculate the pixels in specified 
							var ppd = Camera.main.pixelWidth / Camera.main.fieldOfView;
							var wafov = ((menu.AimFov * ppd) / 2);

							//elimnate targets if they are not in the fov
							var distToXHair = Vector2.Distance(centerpoint, targetpoint);
							if (distToXHair > wafov) continue;

							//elminate targets if they are further than the current lowest distance;
							if (distToXHair > currClosest) continue;
							currClosest = distToXHair;

							target = player.player.transform;



						}
						catch (Exception)
						{
						}
					}
				}*/

				if (menu.AimZombies)
				{
					
//Logging.Log("1");
					if (update == 0)
					{
						Zombies = new List<Zombie>();
						foreach (var region in ZombieManager.regions)
						{
							foreach (var zombie in region.zombies)
							{
								Zombies.Add(zombie);
							}
						}
						update = 75;
					}

//Logging.Log("2");
					
					foreach (var zombie in Zombies)
					{
						//TODO: Fix the aiming not working with the fov selector and the aim 360

						if (zombie.isDead == false)
						{
							if (Vector3.Distance(Camera.main.transform.position, zombie.transform.position) <= menu.AimDistance)
							{
								var limbpos = GetLimbPosition(zombie.transform, tarLimb);
								
								if (menu.Aim360 == false)
								{
									var scrnpt = Camera.main.WorldToViewportPoint(GetLimbPosition(zombie.transform, tarLimb));
									if (scrnpt.z <= 0f || scrnpt.x <= 0f || scrnpt.y <= 0f || scrnpt.z >= 1f || scrnpt.x >= 1f ||
									    scrnpt.y >= 1f) continue;
								}
								
								if (menu.AimIgnoreWalls == false)
								{
									RaycastHit hit;
									Physics.Raycast(Camera.main.transform.position, limbpos, out hit, dist, RayMasks.DAMAGE_CLIENT);
									if (!hit.transform.CompareTag("Zombie")) continue;
								}
								
								var targetpoint = Camera.main.WorldToScreenPoint(GetLimbPosition(zombie.transform, tarLimb));
								targetpoint.y = Camera.main.pixelHeight - targetpoint.y;
								var centerpoint = new Vector3((float) Camera.main.pixelWidth / 2, (float) Camera.main.pixelHeight / 2);

								Logging.Log($"CENTER POINT: X: {centerpoint.x} Y: {centerpoint.y}"); //840/525
						
//Logging.Log("E");
								//Calculate the pixels in specified 
								var ppd = Camera.main.pixelWidth / Camera.main.fieldOfView;
								var wafov = ((menu.AimFov * ppd) / 2);

								Logging.Log($"PPD: {ppd}");
								Logging.Log($"WAFOV: {wafov}");
						
//Logging.Log("F");
								//elimnate targets if they are not in the fov
								var distToXHair = Vector2.Distance(centerpoint, targetpoint);
								Logging.Log($"Dist to Xhair: {distToXHair}");
								if(!menu.Aim360)
									if (distToXHair > wafov) continue;
						
//Logging.Log("G");
								//elminate targets if they are further than the current lowest distance;
								if (distToXHair > currClosest) continue;
								currClosest = distToXHair;

//Logging.Log("H");
								target = zombie.transform;
								
								
								
							}
						}
						
						
//						if (zombie.isDead) continue;
//						if (Vector3.Distance(Camera.main.transform.position, zombie.transform.position) > menu.AimDistance) continue;
//						if (!menu.Aim360)
//						{
//							
//						}
//						if (!menu.AimIgnoreWalls) // if you are not aiming through walls
//						{
//							RaycastHit hit;
//							var distance = GetLimbPosition(zombie.transform, tarLimb) - Player.player.look.aim.position;
//							if (!Physics.Raycast(Player.player.look.aim.position, distance, out hit, dist, RayMasks.DAMAGE_CLIENT) ||
//							    hit.transform.CompareTag("Zombie")) continue;
//						}
//					
//						
//						Logging.Log("A");
//						//remove dead zombies and those who are further than the aim distance
//						if (zombie.isDead) continue;
//						if (Vector3.Distance(Camera.main.transform.position, zombie.transform.position) > menu.AimDistance) continue;
//
//						Logging.Log("B");
//						//remove those zombies who you cant see if you arent doing 360
//						if (!menu.Aim360)
//						{
//							var tarLimbPosScrnPt = Camera.main.WorldToViewportPoint(GetLimbPosition(zombie.transform, tarLimb));
//							if (tarLimbPosScrnPt.z <= 0f || tarLimbPosScrnPt.x <= 0f || tarLimbPosScrnPt.x >= 1f || tarLimbPosScrnPt.y <= 0f || tarLimbPosScrnPt.y >= 1f) continue;
//						}
//
//						Logging.Log("C");
//
//						if (menu.AimIgnoreWalls)
//						{
//							RaycastHit hit;
//							Physics.Raycast(Camera.main.transform.position, zombie.transform.position, out hit, float.PositiveInfinity, RayMasks.DAMAGE_CLIENT);
//							if(hit.transform != null)
//								if (!hit.transform.CompareTag("Zombie")) continue;
//						}
						
//Logging.Log("D");
							
					}
					
					
				}

				if (target != null)
				{
					var speed = menu.AimSpeed * 5f;
					var targetVector = GetLimbPosition(target.transform, tarLimb);
					var quaternion = Quaternion.LookRotation(targetVector - Player.player.look.aim.position);
					var quaternion2 = Quaternion.RotateTowards(Camera.main.transform.rotation, quaternion, speed);
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
				
				
//			var tarLimb = menu.Limb == 1 ? "Skull" : "Spine";
//
//			if (menu.EnableAimbot)
//			{
//
//				if (!menu.AimManualChangeTarget)
//				{
//					target = null;
//				}
//
//
//				var dist = menu.AimDistance;
//				if (menu.AimInfDistance && Player.player.equipment.asset is ItemWeaponAsset)
//				{
//					dist = ((ItemWeaponAsset) Player.player.equipment.asset).range;
//				}
//
//
//				float squarerange;
//				if (!menu.AimClosest)
//				{
//					squarerange = dist * dist;
//				}
//				else
//				{
//					squarerange = float.PositiveInfinity;
//				}
//
//
//
//				if (menu.AimPlayers)
//				{
//					foreach (var client in Provider.clients)
//					{
//						try
//						{
//
//							//if client != local client
//							//if player isnt friend or player is friend and whitelist friends is disabled
//							//if player isnt admin or player is admin and whitelist admins is disabled
//
//							if (client.player != Player.player)
//							{
//								var isFriend = WaveMaker.Friends.Contains(client.playerID.steamID.m_SteamID);
//								
//								if (!isFriend || (isFriend && !menu.AimWhitelistFriends))
//								{
//									if (!client.isAdmin || (client.isAdmin && !menu.AimWhitelistAdmins))
//									{
//										var tarLimbPos = GetLimbPosition(client.player.transform, tarLimb);
//
//										float sqrMagnitude;
//										if (menu.AimClosest)
//										{
//											var tarScrnPt = Camera.main.WorldToScreenPoint(tarLimbPos);
//											sqrMagnitude = (new Vector3(Screen.width / 2f, Screen.height / 2f) - tarScrnPt).sqrMagnitude;
//										}
//										else
//										{
//											sqrMagnitude = (tarLimbPos - Camera.main.transform.position).sqrMagnitude;
//										}
//
//										if (sqrMagnitude <= squarerange && !client.player.life.isDead && Player.player.name != client.player.name)
//										{
//											if (!menu.Aim360)
//											{
//												var tarLimbPosScrnPt = Camera.main.WorldToViewportPoint(tarLimbPos);
//												if (tarLimbPosScrnPt.z <= 0f || tarLimbPosScrnPt.x <= 0f || tarLimbPosScrnPt.x >= 1f ||
//												    tarLimbPosScrnPt.y <= 0f || tarLimbPosScrnPt.y >= 1f)
//												{
//													goto Exit;
//												}
//											}
//											if (!menu.AimIgnoreWalls)
//											{
//												var distance = tarLimbPos - Player.player.look.aim.position;
//												RaycastHit raycastHit;
//												if (!Physics.Raycast(Player.player.look.aim.position, distance, out raycastHit, dist,
//													    RayMasks.DAMAGE_CLIENT) ||
//												    !raycastHit.transform.CompareTag("Enemy"))
//												{
//													goto Exit;
//												}
//											}
//											target = client.player.transform;
//											squarerange = sqrMagnitude;
//										}
//									}
//								}
//								else
//								{
//									target = null;
//									goto Exit;
//
//								}
//							}
//
//						}
//						catch (System.Exception)
//						{
//						}
//
//						Exit:
//						;
//					}
//				}
//
//				if (menu.AimZombies)
//				{
//
//					update++;
//					if (update > 50)
//					{
//						Zombies = new List<Zombie>();
//						foreach (var region in ZombieManager.regions)
//						{
//							foreach (var zom in region.zombies)
//							{
//								Zombies.Add(zom);
//							}
//						}
//
//						update = 0;
//					}
//
//
//					foreach (var zombie in Zombies)
//					{
//						if (menu.AimManualChangeTarget && target == zombie.transform) continue;
//
//						var targetVectorPos = GetLimbPosition(zombie.transform, tarLimb);
//						float distance;
//						if (menu.AimClosest)
//						{
//							var scrnpt = Camera.main.WorldToScreenPoint(targetVectorPos);
//							distance = (new Vector3(Screen.width / 2f, Screen.height / 2f) - scrnpt).sqrMagnitude;
//						}
//						else
//						{
//							distance = (targetVectorPos - Camera.main.transform.position).sqrMagnitude;
//						}
//
//						if (!(distance <= squarerange) || zombie.isDead) continue;
//
//						if (!menu.Aim360)
//						{
//							var targetScrnPt = Camera.main.WorldToViewportPoint(targetVectorPos);
//							if (targetScrnPt.z <= 0f || targetScrnPt.x <= 0f || targetScrnPt.x >= 1f || targetScrnPt.y <= 0f ||
//							    targetScrnPt.y >= 1f)
//							{
//								continue;
//							}
//						}
//						if (!menu.AimIgnoreWalls)
//						{
//							var vector8 = targetVectorPos - Player.player.look.aim.position;
//							RaycastHit raycastHit2;
//							if (!Physics.Raycast(Player.player.look.aim.position, vector8, out raycastHit2, dist, RayMasks.DAMAGE_CLIENT) ||
//							    !raycastHit2.transform.CompareTag("Zombie"))
//							{
//								continue;
//							}
//						}
//						target = zombie.transform;
//						squarerange = distance;
//					}
//				}
//
//
//				if (target == null || menu.AimSilent) return;
//
//				if (menu.AimManualChangeTarget && !menu.AimIgnoreWalls)
//				{
//					var vector9 = GetLimbPosition(target.transform, tarLimb) - Camera.main.transform.position;
//					RaycastHit hit;
//					if (!Physics.Raycast(Player.player.look.aim.position, vector9, out hit, dist, RayMasks.DAMAGE_CLIENT))
//					{
//						return;
//					}
//					if (!hit.transform.CompareTag("Zombie") && !hit.transform.CompareTag("Enemy") &&
//					    !hit.transform.CompareTag("Animal") && !hit.transform.CompareTag("Vehicle"))
//					{
//						return;
//					}
//				}
//
//				var num3 = menu.AimSpeed * 5f;
//				var targetVector = GetLimbPosition(target.transform, tarLimb);
//				var quaternion = Quaternion.LookRotation(targetVector - Player.player.look.aim.position);
//				var quaternion2 = Quaternion.RotateTowards(Camera.main.transform.rotation, quaternion, num3);
//
//				var xVal = quaternion2.eulerAngles.x;
//				if (xVal <= 90f)
//				{
//					xVal += 90f;
//				}
//				if (xVal > 180f)
//				{
//					xVal -= 270f;
//				}

////				Logging.Log("checking if slient is enabled");
//				if (menu.AimSilent)
//				{
//					Logging.Log("Silent is enabled");
//					var state = (bool) prim.GetValue(Player.player.equipment);
//					Logging.Log($"Current mouse button value:{state}");
//					
//					if (state)
//					{
//						Logging.Log("State is true");
//						
//						Logging.Log("Collecting current yaw and pitch");
//						preaimyaw = (float) yaw.GetValue(Player.player.look);
//						preaimpitch = (float) pitch.GetValue(Player.player.look);
//
//						Logging.Log("Creating new quat");
//						var silentquat = Quaternion.RotateTowards(Camera.main.transform.rotation, quaternion, 100f);
//						Logging.Log("Setting yaw to new quat");
//						yaw.SetValue(Player.player.look, silentquat.eulerAngles.y);
//						Logging.Log("Setting pitch to new value");
//						pitch.SetValue(Player.player.look, xVal);
//
//						Logging.Log("Setting state to false");
//						state = false;
//					}
//					else if (!state && !float.IsNaN(preaimyaw) && !float.IsNaN(preaimpitch))
//					{
//						Logging.Log("State is false and floats hold values");
//						
//						Logging.Log("Setting yaw to old value");
//						yaw.SetValue(Player.player.look, preaimyaw);
//						Logging.Log("Setting pitch to old value");
//						pitch.SetValue(Player.player.look, preaimpitch);
//
//						Logging.Log("Setting floats to nan");
//						preaimyaw = float.NaN;
//						preaimpitch = float.NaN;
//					}
//				}
//				else if(!menu.AimSilent)
//				{
//					Logging.Log("Using Normal Aimbot");
					
					
//				}
				
				
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


