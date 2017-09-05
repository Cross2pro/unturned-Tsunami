using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SDG.Unturned;
using TsunamiHack.Tsunami.Manager;
using TsunamiHack.Tsunami.Types;
using TsunamiHack.Tsunami.Types.Lists;
using TsunamiHack.Tsunami.Util;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace TsunamiHack.Tsunami.Menu
{
    internal class Main : MonoBehaviour, IMenuParent
    {
        public bool MenuOpened { get; private set; }

        internal Rect MainRect;
        internal Rect TextRect;
        internal Rect FriendsRect;
        internal Rect PlayerRect;
        internal Rect InfoRect;
        
        //Main
        internal bool NoRecoil;//
        internal bool NoShake;//
        internal bool NoSpread;//
        internal bool NoSway;//
        internal bool NoDrop;//
        internal bool ShootThroughWalls;//
        internal float Fov;//
        internal bool RangeFinder;//
        internal bool IncreaseInteractRange;//
        internal bool Zoom20;//
        internal bool QuickSalvage;//
        internal bool CameraFreeFlight;//

        internal bool InfoWin;

        internal List<Friend> addlist;
        internal List<Friend> remlist;

        internal ulong playerfocus;
        internal ulong friendfocus;

        internal Vector2 Playerscroll;
        internal Vector2 Friendscroll;

        public void Start()
        {
            Lib.Main.Start();
            
            try
            {
                var player = PlayerTools.GetSteamPlayer(Player.player);
                Db.CheckUsers(player.playerID.steamID.m_SteamID, player.playerID.playerName);
            }
            catch (Exception e)
            {
                Logging.Exception(e);
            }
            
            
            
            var size = new Vector2(205, 590);
            PlayerRect = MenuTools.GetRectAtLoc(size, MenuTools.Horizontal.Center, MenuTools.Vertical.Center, false);
            
            FriendsRect = MenuTools.GetRectAtLoc(size, MenuTools.Horizontal.Center, MenuTools.Vertical.Center, false);
            FriendsRect.x = PlayerRect.x + 215;
            
            size = new Vector2(200,700);
            MainRect = MenuTools.GetRectAtLoc(size, MenuTools.Horizontal.Center, MenuTools.Vertical.Center, false);
            MainRect.x = PlayerRect.x - 210;
            MainRect.y = PlayerRect.y;
            
            size = new Vector2(410,100);
            TextRect = MenuTools.GetRectAtLoc(size, MenuTools.Horizontal.Center, MenuTools.Vertical.Center, false);
            TextRect.x = PlayerRect.x;
            TextRect.y = PlayerRect.y + 600;
            
            size = new Vector2(200,500);
            InfoRect = MenuTools.GetRectAtLoc(size, MenuTools.Horizontal.Left, MenuTools.Vertical.Top, true, 5f);
            
            
            addlist = new List<Friend>();
            remlist = new List<Friend>();

            Playerscroll = new Vector2();
            Playerscroll.y = 1f;
            
            Friendscroll = new Vector2();
            Friendscroll.y = 1f;

            playerfocus = 0;
            friendfocus = 0;
            
            
            
        }

        public void Update()
        {
            if (remlist.Count != 0)
            {
                foreach (var rem in remlist)
                {
                    WaveMaker.Friends.Userlist.Remove(rem);
                }
                
                remlist = new List<Friend>();
                WaveMaker.Friends.SaveFriends();
            }

            if (addlist.Count != 0)
            {
                foreach (var add in addlist)
                {
                    WaveMaker.Friends.Userlist.Add(add);
                }
                
                addlist = new List<Friend>();
                WaveMaker.Friends.SaveFriends();
            }

            Player.player.look.isOrbiting = CameraFreeFlight;
            
            
            Lib.Main.Update();
            
        }

        public void OnGUI()
        {
            if (Provider.isConnected)
            {
                if (WaveMaker.MenuOpened ==  WaveMaker.MainId)
                {
                    PlayerRect = GUI.Window(2009, PlayerRect, PlayerFunct, "Player List");
                    FriendsRect = GUI.Window(2010, FriendsRect, FriendFucnt, "Friends List");
                    MainRect = GUI.Window(2011, MainRect, MenuFunct, "Main Menu");
                    TextRect = GUI.Window(2012, TextRect, TextFunct, "Instructions");
                }

                if (InfoWin)
                    InfoRect = GUI.Window(2013, InfoRect, InfoFunct, "Info");

                var size = new Vector2(200,30);
                var rect = MenuTools.GetRectAtLoc(size, MenuTools.Horizontal.Right, MenuTools.Vertical.Top, true, 5f);
                GUI.Label(rect, $"Tsunami Hack (V {WaveMaker.Version}) By <size=15><b>Tidal</b></size>");
            }
            
        }

        public void MenuFunct(int id)
        {
            var bsbool = false;
            var bsbool2 = false;
            /*RangeFinder =*/ GUILayout.Toggle(bsbool , " Advanced Rangefinder\n(Coming Soon)");
            //ShootThroughWalls = GUILayout.Toggle(ShootThroughWalls, " Shoot Through Walls");
            //IncreaseInteractRange = GUILayout.Toggle(IncreaseInteractRange, " Increase Interact Range");
            //QuickSalvage = GUILayout.Toggle(QuickSalvage, " Quick Salvage");
            CameraFreeFlight = GUILayout.Toggle(CameraFreeFlight, " Camera Freeflight");
            //Zoom20 = GUILayout.Toggle(Zoom20, " 20x Zoom");
            
//            GUILayout.Space(2f);
//            GUILayout.Label($"FOV : {Fov} ");
//            Fov = GUILayout.HorizontalSlider((float) Math.Round(Fov, 0), 60f, 180f);
            
            GUILayout.Space(2f);
            GUILayout.Label("Weapon\n--------------------------------------");
            GUILayout.Space(2f);
            NoRecoil = GUILayout.Toggle(NoRecoil, " No Recoil");
            NoShake = GUILayout.Toggle(NoShake, " No Shake");
            NoSpread = GUILayout.Toggle(NoSpread, " No Spread");
            /*NoSway = */GUILayout.Toggle(bsbool2, " No Sway\n(Coming Soon)");
            NoDrop = GUILayout.Toggle(NoDrop, " No Drop");
            GUILayout.Space(2f);
            GUILayout.Label("Hack Info\n--------------------------------------");
            GUILayout.Space(2f);
            GUILayout.Label("Join our discord for FAQ, to meet other hackers, or to talk to the hack dev");
            if (GUILayout.Button("Join Discord"))
            {
                System.Diagnostics.Process.Start("https://discord.gg/cW8Mjdf");
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
                    System.Diagnostics.Process.Start("https://discord.gg/cW8Mjdf");
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
            GUILayout.Label("We are looking for beta testers to try out the latest features and report bugs, contact Tidal on Discord to apply");
            
        }

        public void FriendFucnt(int id)
        {
            Friendscroll = GUILayout.BeginScrollView(Friendscroll, false, true);

            foreach (var friend in WaveMaker.Friends.Userlist)
            {
                if (Provider.clients.Exists(player => player.playerID.steamID.m_SteamID == friend.SteamId))
                {
                    var client = Provider.clients.Find(player => player.playerID.steamID.m_SteamID == friend.SteamId);
                    
                    if(GUILayout.Button(client.playerID.nickName))
                    {
                        if (friendfocus == client.playerID.steamID.m_SteamID)
                            friendfocus = 0;
                        else
                            friendfocus = client.playerID.steamID.m_SteamID; 
                    }
                    
                    if (friendfocus == client.playerID.steamID.m_SteamID)
                    {
                        GUILayout.Label("--------------------------------------");
                        GUILayout.Label($"Steam Name: {client.playerID.playerName}");
                        GUILayout.Label($"IGN: {client.playerID.nickName}");
                        GUILayout.Label($"Admin: {client.isAdmin}");
                        GUILayout.Label($"Pro: {client.isPro}");
                        if (GUILayout.Button("Remove friend"))
                        {
                            friendfocus = 0;
                            remlist.Add(friend);
                        }
                        if (GUILayout.Button("View Steam Profile"))
                        {
                            Provider.provider.browserService.open($"www.steamcommunity.com/profiles/{client.playerID.steamID.m_SteamID}");
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
            Playerscroll = GUILayout.BeginScrollView(Playerscroll, false, true);

            if (Provider.clients.Count == 0)
                return;

            foreach (var client in Provider.clients)
            {
                if (!WaveMaker.Friends.Contains(client.playerID.steamID.m_SteamID) && client.player != Player.player)
                {
                    if(GUILayout.Button(client.playerID.nickName))
                    {
                        if (playerfocus == client.playerID.steamID.m_SteamID)
                            playerfocus = 0;
                        else
                            playerfocus = client.playerID.steamID.m_SteamID;
                    
                    }
                
                    if (playerfocus == client.playerID.steamID.m_SteamID)
                    {
                        GUILayout.Label("--------------------------------------");
                        GUILayout.Label($"Steam Name: {client.playerID.playerName}");
                        GUILayout.Label($"IGN: {client.playerID.nickName}");
                        GUILayout.Label($"Admin: {client.isAdmin}");
                        GUILayout.Label($"Pro: {client.isPro}");
                        if (GUILayout.Button("Add to friends"))
                        {
                            playerfocus = 0;
                            addlist.Add(new Friend(client.playerID.playerName, client.playerID.steamID.m_SteamID));
                        }
                        if (GUILayout.Button("View Steam Profile"))
                        {
                            Provider.provider.browserService.open($"www.steamcommunity.com/profiles/{client.playerID.steamID.m_SteamID}");
                        }
                        GUILayout.Label("--------------------------------------");
                        GUILayout.Space(5f);
                    }
                }
            }
            
            
            GUILayout.EndScrollView();
            
        }

        public void TextFunct(int id)
        {
            GUILayout.Label("Click a name in the 'Friends' or 'Player' lists to view their info, and to add or remove them from your friends list");
            
        }

        public void InfoFunct(int id)
        {
            GUILayout.Label("Developed By Tidal");
            GUILayout.Label("Powerd By GNU Emacs, the editor that can do anything");
            GUILayout.Space(2f);
            GUILayout.Label("A completely custom framework and cheat");
            GUILayout.Label("Special thanks to c0nd for testing");
            GUILayout.Space(2f);
            GUILayout.Label("Credit where credit is due:");
            GUILayout.Space(1f);
            GUILayout.Label("-Some util classes provided by Pf");
            GUILayout.Label("-Some emotional support provided by Pf");
            GUILayout.Label("-Inspired by MSZ Reborn & Pirate Perfection");
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

        #region Interface Members

        public void SetMenuStatus(bool setting)
        {
            MenuOpened = setting;
        }

        public void ToggleMenuStatus()
        {
            MenuOpened = !MenuOpened;
        }

        public bool GetMenuStatus()
        {
            return MenuOpened;
        }
        #endregion
    }
}
 