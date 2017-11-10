using System;
using System.Collections.Generic;
using System.Reflection;
using SDG.Framework.UI.Sleek2;
using SDG.Unturned;
using TsunamiHack.Tsunami.Manager;
using TsunamiHack.Tsunami.Types;
using TsunamiHack.Tsunami.Util;
using UnityEngine;

namespace TsunamiHack.Tsunami.Menu
{
    internal class Main : MonoBehaviour
    {
        public bool MenuOpened { get; private set; }
        public bool PremWindowOpen;

        internal Rect MainRect;
        internal Rect TextRect;
        internal Rect FriendsRect;
        internal Rect PlayerRect;
        internal Rect InfoRect;
        internal Rect PremRect;

        //Main
        internal bool NoRecoil; //

        internal bool NoShake; //
        internal bool NoSpread; //
        internal bool NoSway; //
        internal bool NoDrop; //
        internal bool ShootThroughWalls; //
        internal float Fov; //
        internal bool RangeFinder; //
        internal bool IncreaseInteractRange; //
        internal bool Zoom20; //
        internal bool QuickSalvage; //
        internal bool CameraFreeFlight = false; //

        internal bool InfoWin;

        internal List<Friend> Addlist;
        internal List<Friend> Remlist;

        internal ulong Playerfocus;
        internal ulong Friendfocus;

        internal Vector2 Playerscroll;
        internal Vector2 Friendscroll;

        internal FieldInfo VehicleLocked;

        public void Start()
        {
            Lib.Main.Start();

            VehicleLocked =
                typeof(InteractableVehicle).GetField("_isLocked", BindingFlags.NonPublic | BindingFlags.Instance);

            try
            {
                var player = PlayerTools.GetSteamPlayer(Player.player);
//                Db.CheckUsers(player.playerID.steamID.m_SteamID, player.playerID.playerName);
            }
            catch (Exception e)
            {
                Logging.Exception(e);
            }

            var size = new Vector2(205, 590);
            PlayerRect = MenuTools.GetRectAtLoc(size, MenuTools.Horizontal.Center, MenuTools.Vertical.Center, false);

            FriendsRect = MenuTools.GetRectAtLoc(size, MenuTools.Horizontal.Center, MenuTools.Vertical.Center, false);
            FriendsRect.x = PlayerRect.x + 215;

            size = new Vector2(200, 700);
            MainRect = MenuTools.GetRectAtLoc(size, MenuTools.Horizontal.Center, MenuTools.Vertical.Center, false);
            MainRect.x = PlayerRect.x - 210;
            MainRect.y = PlayerRect.y;

            size = new Vector2(410, 100);
            TextRect = MenuTools.GetRectAtLoc(size, MenuTools.Horizontal.Center, MenuTools.Vertical.Center, false);
            TextRect.x = PlayerRect.x;
            TextRect.y = PlayerRect.y + 600;

            size = new Vector2(200, 500);
            InfoRect = MenuTools.GetRectAtLoc(size, MenuTools.Horizontal.Left, MenuTools.Vertical.Top, true, 5f);

            size = new Vector2(200, 700);
            PremRect = MenuTools.GetRectAtLoc(size, MenuTools.Horizontal.Left, MenuTools.Vertical.Top, true, 5f);

            Addlist = new List<Friend>();
            Remlist = new List<Friend>();

            Playerscroll = new Vector2();
            Playerscroll.y = 1f;

            Friendscroll = new Vector2();
            Friendscroll.y = 1f;

            Playerfocus = 0;
            Friendfocus = 0;

        }

        public void Update()
        {
            if (Provider.isConnected)
            {
                if (Remlist.Count != 0)
                {
                    foreach (var rem in Remlist)
                    {
                        WaveMaker.Friends.Userlist.Remove(rem);
                    }

                    Remlist = new List<Friend>();
                    WaveMaker.Friends.SaveFriends();
                }

                if (Addlist.Count != 0)
                {
                    foreach (var add in Addlist)
                    {
                        WaveMaker.Friends.Userlist.Add(add);
                    }

                    Addlist = new List<Friend>();
                    WaveMaker.Friends.SaveFriends();
                }

                Player.player.look.isOrbiting = CameraFreeFlight;

                Lib.Main.Update();

            }    
        }

        public void OnGUI()
        {
            if (Provider.isConnected)
            {

                if (WaveMaker.MenuOpened == WaveMaker.MainId && !WaveMaker.SoftDisable)
                {
                    PlayerRect = GUI.Window(2009, PlayerRect, PlayerFunct, "Player List");
                    FriendsRect = GUI.Window(2010, FriendsRect, FriendFucnt, "Friends List");
                    MainRect = GUI.Window(2011, MainRect, MenuFunct, "Main Menu");
                    TextRect = GUI.Window(2012, TextRect, TextFunct, "Instructions");
                    if (PremWindowOpen)
                        PremRect = GUI.Window(2013, PremRect, PremiumFunct, "Premium Features");

                }

                if (InfoWin)
                    InfoRect = GUI.Window(2013, InfoRect, InfoFunct, "Info");

                var size = new Vector2(200, 60);
                var rect = MenuTools.GetRectAtLoc(size, MenuTools.Horizontal.Right, MenuTools.Vertical.Top, true, 5f);
                GUI.Label(rect,
                    $"<b><color=#00ffffff>Tsunami Hack <color=#c0c0c0ff>(V {WaveMaker.Version})</color> By <color=#0000a0ff><size=15><i>Tidal</i></size></color>\n               Featuring <i>Deus Myke</i></color></b>");
                
                    
            }

        }

        public void MenuFunct(int id)
        {
            var bsbool = false;
            var bsbool2 = false;
//            /*RangeFinder =*/
            GUILayout.Toggle(bsbool, " Advanced Rangefinder\n(Coming Soon)");
//            ShootThroughWalls = GUILayout.Toggle(ShootThroughWalls, " Shoot Through Walls");
//            IncreaseInteractRange = GUILayout.Toggle(IncreaseInteractRange, " Increase Interact Range");
//            QuickSalvage = GUILayout.Toggle(QuickSalvage, " Quick Salvage");
            CameraFreeFlight = GUILayout.Toggle(CameraFreeFlight, " Camera Freeflight");
//            Zoom20 = GUILayout.Toggle(Zoom20, " 20x Zoom");

//            GUILayout.Space(2f);
//            GUILayout.Label($"FOV : {Fov} ");
//            Fov = GUILayout.HorizontalSlider((float) Math.Round(Fov, 0), 60f, 180f);

            GUILayout.Space(2f);
            GUILayout.Label("Weapon\n--------------------------------------");
            GUILayout.Space(2f);
            NoRecoil = GUILayout.Toggle(NoRecoil, " No Recoil");
            NoShake = GUILayout.Toggle(NoShake, " No Shake");
            NoSpread = GUILayout.Toggle(NoSpread, " No Spread");
            /*NoSway = */
            GUILayout.Toggle(bsbool2, " No Sway\n(Coming Soon)");
            NoDrop = GUILayout.Toggle(NoDrop, " No Drop");
            GUILayout.Space(2f);
            GUILayout.Label("Hack Info\n--------------------------------------");
            GUILayout.Space(2f);
            GUILayout.Label("Join our discord for FAQ, to meet other players, or to talk to the hack dev");
            if (GUILayout.Button("Join Discord"))
            {
                System.Diagnostics.Process.Start("https://discord.gg/QhakXeK");
            }
            GUILayout.Space(2f);

            var local = Provider.clients.Find(player => player.player == Player.player);
            var localid = local.playerID.steamID.m_SteamID;

            if (localid == WaveMaker.Controller.Dev || localid == ulong.Parse("76561198308025096"))
            {
                GUILayout.Label("You are: Tidal, Developer ");
            }
            else if (WaveMaker.Prem.Contain(localid.ToString()) && WaveMaker.Beta.Contain(localid.ToString()))
            {
                GUILayout.Label($"You are: {local.playerID.playerName}, Premium User & Beta Tester");
            }
            else if (WaveMaker.Prem.Contain(localid.ToString()))
            {
                GUILayout.Label($"You are: {local.playerID.playerName}, Premium User");
            }
            else if (WaveMaker.Beta.Contain(localid.ToString()))
            {
                GUILayout.Label($"You are: {local.playerID.playerName}, Beta Tester");
            }
            else
            {
                GUILayout.Label($"You are: {local.playerID.playerName}, Free user");

                GUILayout.Label($"To upgrade to premium, contact Tidal on discord");

                if (GUILayout.Button("Join TsuHack Discord"))
                {
                    System.Diagnostics.Process.Start("https://discord.gg/QhakXeK");
                }

            }
            GUILayout.Space(2f);
            GUILayout.Label("INFO\n--------------------------------------");
            GUILayout.Space(2f);
            if (GUILayout.Button("Dev Info"))
            {
                InfoWin = !InfoWin;
            }
            GUILayout.Space(2f);
            GUILayout.Label(
                "We are looking for beta testers to try out the latest features and report bugs, contact Tidal on Discord to apply");

            if (WaveMaker.isBeta)
            {
                if (GUILayout.Button("Premium Cheats"))
                {
                    PremWindowOpen = !PremWindowOpen;
                }
            }

        }

        public void FriendFucnt(int id)
        {
            Friendscroll = GUILayout.BeginScrollView(Friendscroll, false, true);

            foreach (var friend in WaveMaker.Friends.Userlist)
            {
                if (Provider.clients.Exists(player => player.playerID.steamID.m_SteamID == friend.SteamId))
                {
                    var client = Provider.clients.Find(player => player.playerID.steamID.m_SteamID == friend.SteamId);

                    if (GUILayout.Button(client.playerID.nickName))
                    {
                        if (Friendfocus == client.playerID.steamID.m_SteamID)
                            Friendfocus = 0;
                        else
                            Friendfocus = client.playerID.steamID.m_SteamID;
                    }

                    if (Friendfocus == client.playerID.steamID.m_SteamID)
                    {
                        GUILayout.Label("--------------------------------------");
                        GUILayout.Label($"Steam Name: {client.playerID.playerName}");
                        GUILayout.Label($"IGN: {client.playerID.nickName}");
                        GUILayout.Label($"Admin: {client.isAdmin}");
                        GUILayout.Label($"Pro: {client.isPro}");
                        if (GUILayout.Button("Remove friend"))
                        {
                            Friendfocus = 0;
                            Remlist.Add(friend);
                        }
                        if (GUILayout.Button("View Steam Profile"))
                        {
                            Provider.provider.browserService.open(
                                $"www.steamcommunity.com/profiles/{client.playerID.steamID.m_SteamID}");
                        }
                        GUILayout.Label("--------------------------------------");
                        GUILayout.Space(5f);
                    }

                }
            }
            GUILayout.EndScrollView();
        }

        public void PlayerFunct(int id)
        {
            if (Provider.clients.Count == 1 && Provider.clients[0].player == Player.player)
            {
                GUILayout.Space(15f);
                GUILayout.Button("No Players On Server");
            }
            else
            {
                Playerscroll = GUILayout.BeginScrollView(Playerscroll, false, true);

                foreach (var player in Provider.clients)
                {
                    if (WaveMaker.Friends.IsFriend(player.playerID.steamID.m_SteamID) && player.player != Player.player)
                    {
                        if (GUILayout.Button(player.playerID.playerName))
                        {
                            Playerfocus = player.playerID.steamID.m_SteamID;
                        }

                        if (Playerfocus == player.playerID.steamID.m_SteamID)
                        {
                            GUILayout.Label("--------------------------------------");
                            GUILayout.Label($"Steam Name: {player.playerID.playerName}");
                            GUILayout.Label($"IGN: {player.playerID.nickName}");
                            GUILayout.Label($"Admin: {player.isAdmin}");
                            GUILayout.Label($"Pro: {player.isPro}");
                            if (GUILayout.Button("Add to friends"))
                            {
                                Playerfocus = 0;
                                Addlist.Add(new Friend(player.playerID.playerName, player.playerID.steamID.m_SteamID));                         //todo: redo this part
                            }
                            if (GUILayout.Button("View Steam Profile"))
                            {
                                Provider.provider.browserService.open(  $"www.steamcommunity.com/profiles/{player.playerID.steamID.m_SteamID}");
                            }
                            GUILayout.Label("--------------------------------------");
                            GUILayout.Space(5f);
                        }
                    }
                }
                
                GUILayout.EndScrollView();
            }
            
            

        }

        public void TextFunct(int id)
        {
            GUILayout.Label(
                "Click a name in the 'Friends' or 'Player' lists to view their info, and to add or remove them from your friends list");

        }

        public void InfoFunct(int id)
        {
            GUILayout.Label("Developed By Tidal");
            GUILayout.Label("Powerd By GNU Emacs, the editor that can do anything");
            GUILayout.Label("Praise lord ic3 for enlightening me, may he forever be divine");
            GUILayout.Space(2f);
            GUILayout.Label("A completely custom framework and cheat");
            GUILayout.Label("Special thanks to c0nd for testing");
            GUILayout.Label("Thanks to Deus Myke, for hosting this hack on your channel");
            GUILayout.Space(2f);
            GUILayout.Label("Credit where credit is due:");
            GUILayout.Space(1f);
            GUILayout.Label("-Some util classes provided by Pf");
            GUILayout.Label("-Some emotional support provided by Pf");
            GUILayout.Label("-Thanks to stack overflow and the unity scripting api, obv");
            GUILayout.Label("-And thank god for Rider IDE");
            GUILayout.Space(5f);
            GUILayout.Label("Interested in working with me? Contact on discord");
            GUILayout.Space(5f);
            if (GUILayout.Button("Close"))
            {
                InfoWin = false;
            }
        }

        internal bool UnlockCar;
        internal InteractableVehicle TargetCar;

        public void PremiumFunct(int id)
        {
            if (GUILayout.Button("Check COmponents"))
            {
                var zomlist = new List<Zombie>();
                foreach (var region in ZombieManager.regions)
                {
                    foreach (var zombie in region.zombies)
                    {
                        zomlist.Add(zombie);
                    }
                }

                var compintrans = zomlist[0].transform.GetComponentsInChildren<Transform>();

                foreach (var comp in compintrans)
                {
                    Logging.Log(comp.name.Trim());
                }
            }


            
        GUI.DragWindow();
        
        }
    }

}
 