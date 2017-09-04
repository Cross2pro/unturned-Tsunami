using System;
using System.Runtime.InteropServices;
using Pathfinding;
using SDG.Unturned;
using TsunamiHack.Tsunami.Manager;
using TsunamiHack.Tsunami.Types;
using TsunamiHack.Tsunami.Util;
using UnityEngine;

namespace TsunamiHack.Tsunami.Lib
{
	internal class AimV2
	{
		private static DateTime lastLock;
		private static Menu.Aim menu;

		private static float defsense;


		internal static void Start()
		{
			Logging.Log("Inside Start");
			lastLock = DateTime.Now;
			menu = WaveMaker.MenuAim;

			defsense = Player.player.look.sensitivity;
		}

		internal static void Update()
		{
			Logging.Log("Inside Update");
			if ((DateTime.Now - lastLock).TotalMilliseconds >= menu.LockUpdateRate)
			{
				Logging.Log("Calling Update");
				UpdateLock();
				lastLock = DateTime.Now;
			}
		}


		internal static void UpdateLock()
		{
			Logging.Log("Checking if connected and aimlock enabled");
			if (menu.EnableAimlock && Provider.isConnected)
			{
				Logging.Log("setting range");
				var range = menu.LockDistance;
				if (menu.LockGunRange && Player.player.equipment.asset is ItemGunAsset)
				{
					range = ((ItemGunAsset) Player.player.equipment.asset).range;
				}

				RaycastHit hit;
				Logging.Log("casting");
				Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, range, RayMasks.DAMAGE_CLIENT);

				if (hit.transform != null)
				{
					Logging.Log("Checking if player");
					Logging.Log($"TAG = {hit.transform.tag} ");
					if (menu.LockPlayers && hit.transform.CompareTag("Enemy"))				//if player
					{
						if (!menu.AimWhitelistFriends && !isPlayerFriend(hit))
							ChangeSense(true);
						else
							ChangeSense(false);
					}
					else if (menu.LockZombies && hit.transform.CompareTag("Zombie"))		//if zombie
						ChangeSense(true);
					else if (menu.LockAnimals && hit.transform.CompareTag("Animal"))		//if animal
						ChangeSense(true);
					else if (menu.LockVehicles && hit.transform.CompareTag("Vehicle"))		//if vehicle
						ChangeSense(true);
					else 																	//if none
						ChangeSense(false);

				}
				else
					ChangeSense(false);														//if transform is null
				
			}
		}

		internal static void ChangeSense(bool enable)
		{
			if (enable)
				Player.player.look.sensitivity = defsense / menu.LockSensitivity;
			else
				Player.player.look.sensitivity = defsense;
		}


		internal static bool isPlayerFriend(RaycastHit rch)
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
						if (ply.isAdmin && menu.LockWhitelistAdmins)
							return true;

						//else return false
						return false;
					}

					//if unable to match the client throw unable tomatch
					throw new UnableToMatchPlayerException();
					return false;
				}

			}	//if cant match log exception
			catch (UnableToMatchPlayerException e)
			{
				Logging.Exception(e);
			}	//suppress general exceptions
			catch (Exception) {}

			return false;
		}
	}
}
