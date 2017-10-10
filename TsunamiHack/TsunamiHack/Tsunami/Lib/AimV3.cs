using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using SDG.Unturned;
using TsunamiHack.Tsunami.Manager;
using TsunamiHack.Tsunami.Types;
using TsunamiHack.Tsunami.Util;
using UnityEngine;

namespace TsunamiHack.Tsunami.Lib
{
    internal class AimV3
    {
        public static Menu.Aim Menu;
	    
	    public static DateTime LastLock;
        
	    //aimlock
	    public static float Defsense;    
	    //-------
	    
	    //triggerbot
	    public static int Id;
	    
	    public static FieldInfo Prim;
	    public static FieldInfo Yaw;
	    public static FieldInfo Pitch;
	    //----------
	    
	    //Aimbot
	    public static List<Zombie> Zombies;
	    public static int ZombieUpdate;
	    public static List<SteamPlayer> Players;
	    public static int PlayerUpdate;

	    public static bool ZombieFlag;
	    public static bool PlayerFlag;

	    public static Transform target;
	    
	    //------
	    
        public static void Start()
        {
	        ZombieUpdate = 0;
	        PlayerUpdate = 1;
	        
	        Players = new List<SteamPlayer>();
	        Zombies = new List<Zombie>();
	        
	        Menu = WaveMaker.MenuAim;
	        LastLock = DateTime.Now;
	        
            //aimlock
	        Defsense = Player.player.look.sensitivity;
	        //-------
	        
	        //triggerbot
	        Prim = typeof(PlayerEquipment).GetField("prim", BindingFlags.Instance | BindingFlags.NonPublic);
	        Yaw = typeof(PlayerLook).GetField("_yaw", BindingFlags.Instance | BindingFlags.NonPublic);
	        Pitch = typeof(PlayerLook).GetField("_pitch", BindingFlags.Instance | BindingFlags.NonPublic);
	        //----------
        }

        public static void Update()
        {
	        UpdateAimbot();
	        
	        //Update Lock and Triggerbot on delay
	        if ((LastLock - DateTime.Now).TotalMilliseconds > Menu.AimUpdateRate)
	        {
			    UpdateLock();
			    UpdateTb();

		        LastLock = DateTime.Now;
	        }
	        
	        
        }

		#region Aimbot

	    public static bool CanSee(Transform transform)
	    {
		    var scrnpt = Camera.main.WorldToViewportPoint(transform.position);

		    if (scrnpt.z <= 0f || scrnpt.x <= 0f || scrnpt.x >= 1f || scrnpt.y <= 0f || scrnpt.y >= 1f)
			    return false;
		    
		    return true;
	    }
	    
	    public static void UpdateAimbot()
	    {
		    target = null;
		    UpdateAimLists();
		    
		    if (Menu.EnableAimbot)
		    {
			    //set target limb
			    var limb = Menu.AimTargetLimb == TargetLimb.Head ? "Skull" : "Spine";
			    var currClosest = float.PositiveInfinity;
			    
			    //set distance of aimbot
			    var distance = Menu.AimDistance;
			    if (Menu.AimUseGunDistance && Player.player.equipment.asset is ItemGunAsset)
			    {
				    distance = ((ItemGunAsset) Player.player.equipment.asset).range;
			    }
			    if (Menu.AimInfDistance)
			    {
				    distance = float.PositiveInfinity;
			    }

			    if (Menu.AimZombies)
			    {
				    foreach (var zombie in Zombies)
				    {
					    if (zombie.isDead == false)
					    {
						    if (Vector3.Distance(Camera.main.transform.position, zombie.transform.position) <= distance)
						    {
							    //if I can see the player, or if 360 is enabled and I cant see the player
							    if (CanSee(zombie.transform) || Menu.Aim360 && !CanSee(zombie.transform))
							    {
									//if I can see the player, or if ignore walls is enabled and I cant see the player
	        					    RaycastHit hit;
//	        					    Physics.Raycast(Player.player.look.aim.position, zombie.transform.position, out hit, distance, RayMasks.DAMAGE_CLIENT);
								    var pos = GetLimbPosition(zombie.transform, limb);
								    var realpos = pos - Player.player.look.aim.position;
								    Physics.Raycast(Player.player.look.aim.position, realpos, out hit, distance, RayMasks.DAMAGE_CLIENT);
	        					   
								    if (hit.transform != null)
								    {
									    if (hit.transform.CompareTag("Zombie") || (Menu.AimIgnoreWalls && !hit.transform.CompareTag("Zombie")))
									    {

										    var targetpoint = Camera.main.WorldToScreenPoint(GetLimbPosition(zombie.transform, limb));
										    targetpoint.y = Camera.main.pixelHeight - targetpoint.y;
										    var centerpoint = new Vector3((float) Camera.main.pixelWidth / 2, (float) Camera.main.pixelHeight / 2);

										    var ppd = Camera.main.pixelWidth / Camera.main.fieldOfView;
										    var wafov = ((Menu.AimFov * ppd) / 2);

										    var distToXHair = Vector2.Distance(centerpoint, targetpoint);
										    if (!Menu.Aim360)
											    if (distToXHair > wafov) continue;

										    if (distToXHair > currClosest) continue;

										    currClosest = distToXHair;
										    target = zombie.transform;
									    }
								    }
	        					    
							    }
							    
						    }
					    }  
				}
			    
		    }

			    if (Menu.AimPlayers)
			    {
				    foreach (var player in Players)
				    {
					    var id = player.playerID.steamID.m_SteamID;
					    
					    if (player.player != Player.player)
					    {
//						    if (WaveMaker.Friends.Contains(id) && Menu.AimWhitelistFriends) continue;
//						    if (player.isAdmin && Menu.AimWhitelistAdmins) continue;
						    
						    if (Vector3.Distance(Camera.main.transform.position, player.player.transform.position) <= distance)
						    {
							    if (CanSee(player.player.transform) || Menu.Aim360 && !CanSee(player.player.transform))
							    {
								    RaycastHit hit;
								    var pos = GetLimbPosition(player.player.transform, limb);
								    var realpos = pos - Player.player.look.aim.position;
								    Physics.Raycast(Player.player.look.aim.position, realpos, out hit, distance, RayMasks.DAMAGE_CLIENT);
								    
								    if (hit.transform != null)
								    {
									    if (hit.transform.CompareTag("Enemy") || (Menu.AimIgnoreWalls && !hit.transform.CompareTag("Enemy")))
									    {
										    var targetpoint = Camera.main.WorldToScreenPoint(GetLimbPosition(player.player.transform, limb));
										    targetpoint.y = Camera.main.pixelHeight - targetpoint.y;		    
										    var centerpoint = new Vector3((float) Camera.main.pixelWidth / 2, (float) Camera.main.pixelHeight / 2);
										    
										    var ppd = Camera.main.pixelWidth / Camera.main.fieldOfView;
										    var wafov = ((Menu.AimFov * ppd) / 2);
										    
										    var distToXHair = Vector2.Distance(centerpoint, targetpoint);
										    if (!Menu.Aim360)
											    if (distToXHair > wafov) continue;
										    
										    if (distToXHair > currClosest) continue;
										    
										    currClosest = distToXHair;
										    target = player.player.transform;
									    }
								    }
							    }
						    }   
					    }
				    }
			    }
			    
		    if (target != null)
		    {
			    var speed = Menu.AimSpeed / 4;
			    var targetVector = GetLimbPosition(target.transform, limb);
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
					
			    Yaw.SetValue(Player.player.look, quaternion2.eulerAngles.y);
			    Pitch.SetValue(Player.player.look, xVal);
					
		    }
		    
		    }
		    
		    
	    }

	    public static void UpdateAim()
	    {
		    
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

	    public static void UpdateAimLists()
	    {
		    if (Menu.AimZombies)
		    {
			    if (ZombieUpdate > Menu.AimListUpdateRate)
			    {
				    
				    var newest = new List<Zombie>();
				    foreach (var region in ZombieManager.regions)
				    {
					    foreach (var zombie in region.zombies)
					    {
						    newest.Add(zombie);
					    }
				    }

				    //remove zombies that are no longer in the game
				    foreach (var zombie in Zombies)
				    {
					    var index = newest.IndexOf(zombie);
					    if (index == -1)
						    Zombies.Remove(zombie);
				    }

				    //add zombies that are in the new list but not in the active list
				    foreach (var newzombie in newest)
				    {
					    var index = Zombies.IndexOf(newzombie);
					    if(index == -1)
						    Zombies.Add(newzombie);
				    }
				    
				    ZombieUpdate = 0;
			    }
			    else
				    ZombieUpdate++;
		    }
		    else if (!Menu.AimZombies && Zombies.Count > 0)
		    {
			    if (!ZombieFlag)
				    ZombieFlag = true;
			    else if (ZombieFlag)
			    {
				    Zombies = new List<Zombie>();
				    ZombieFlag = false;
			    }
		    }

		    if (Menu.AimPlayers)
		    {
			    if (PlayerUpdate > Menu.AimListUpdateRate)
			    {

				    if (Players.Count == 0)
				    {
					    Players = Provider.clients;
				    }
				    else
				    {
					    //Add players that are in new list but not in old
					    foreach (var client in Provider.clients)
					    {
						    var index = Players.IndexOf(client);
						    if (index == -1)
							    Players.Add(client);
					    }

					    //remove players that are in active list but arent in new list
					    foreach (var player in Players)
					    {
						    var index = Provider.clients.IndexOf(player);
						    if(index == -1)
							    Players.Remove(player);
					    }

				    }
				    
				    ZombieUpdate = 0;
			    }
			    else
				    ZombieUpdate++;
		    }
		    else if (!Menu.AimPlayers && Players.Count > 0)
		    {
			    if (!PlayerFlag)
				    PlayerFlag = true;
			    else if (PlayerFlag)
			    {
				    Players = new List<SteamPlayer>();
				    PlayerFlag = false;
			    }
		    }


	    }
	    
	    
	    #endregion
	    
        #region aimlock
        
		public static void UpdateLock()
		{
			if (Menu.EnableAimlock && Provider.isConnected)
			{
				var range = Menu.LockDistance;
				if (Menu.LockGunRange && Player.player.equipment.asset is ItemGunAsset)
				{
					range = ((ItemGunAsset) Player.player.equipment.asset).range;
				}

				RaycastHit hit;
				Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, range,
					RayMasks.DAMAGE_CLIENT);

				if (hit.transform != null)
				{
					if (Menu.LockPlayers && hit.transform.CompareTag("Enemy")) //if player
					{
						if (!Menu.AimWhitelistFriends && !IsPlayerFriend(hit, 1))
							ChangeSense(true);
						else
							ChangeSense(false);
					}
					else if (Menu.LockZombies && hit.transform.CompareTag("Zombie")) //if zombie
						ChangeSense(true);
					else if (Menu.LockAnimals && hit.transform.CompareTag("Animal")) //if animal
						ChangeSense(true);
					else if (Menu.LockVehicles && hit.transform.CompareTag("Vehicle")) //if vehicle
						ChangeSense(true);
					else //if none
						ChangeSense(false);

				}
				else
					ChangeSense(false); //if transform is null

			}
		}

		public static void ChangeSense(bool enable)
		{
			if (enable)
				Player.player.look.sensitivity = Defsense / Menu.LockSensitivity;
			else
				Player.player.look.sensitivity = Defsense;
		}

		public static bool IsPlayerFriend(RaycastHit rch, int type)
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
								if (ply.isAdmin && Menu.LockWhitelistAdmins)
									return true;
								break;
							case 2:
								if (ply.isAdmin && Menu.TriggerWhiteListAdmins)
									return true;
								break;
							case 3:
								if (ply.isAdmin && Menu.AimWhitelistAdmins)
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

        #endregion

	    #region triggerbot

	    internal static void UpdateTb()
	    {
		    if (Menu.EnableTriggerbot && Provider.isConnected)
		    {
			    var range = Menu.TriggerDistance;
			    if (Menu.TriggerGunRange && Player.player.equipment.asset is ItemGunAsset)
				    range = ((ItemGunAsset) Player.player.equipment.asset).range;


			    RaycastHit hit;
			    Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, range,
				    RayMasks.DAMAGE_CLIENT);

			    bool fire = false;

			    if (hit.transform != null)
			    {
				    if (Menu.TriggerPlayers && hit.transform.CompareTag("Enemy"))
				    {
					    //if we are not whitelisting and player isnt friend
					    if (!Menu.TriggerWhiteListFriends && !IsPlayerFriend(hit, 2))
						    fire = true;
				    }

				    if (Menu.TriggerZombies && hit.transform.CompareTag("Zombie"))
				    {
					    fire = true;
				    }

				    if (Menu.TriggerAnimals && hit.transform.CompareTag("Animal"))
				    {
					    fire = true;
				    }

				    if (Menu.TriggerVehicles && hit.transform.CompareTag("Vehicle"))
				    {
					    fire = true;
				    }

				    if (fire)
				    {
					    Prim.SetValue(Player.player.equipment, Id <= 3);
					    Id++;
					    if (Id > 6)
						    Id = 0;
				    }


			    }
		    }


	    }

	    #endregion
    }
}