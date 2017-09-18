using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TsuManager.Installation;

namespace TsuManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string MenuText = "";
        public bool Disabled = true;
        public string Reason = "";

        public db DbAccess;

        public InstallationManager Manager;
        
        
        private bool sideBarState;
        private enum Displayed { Home, Install, About}

        private Displayed current;
        
        private List<Tuple<string,string>> FileList = new List<Tuple<string, string>>();


        public MainWindow()
        {
            FileList.Add(new Tuple<string, string>("Assembly-CSharp-firstpass.dll", "Hook Dependency"));
            FileList.Add(new Tuple<string, string>("I18N.dll", "SQL Dependency"));
            FileList.Add(new Tuple<string, string>("I18N.West.dll", "SQL Dependency (West)"));
            FileList.Add(new Tuple<string, string>("System.Management.dll", "Management Dependency"));
            FileList.Add(new Tuple<string, string>("TsunamiHack.dll", "Function Dependency"));

            Manager = new InstallationManager(this);
            
            InitializeComponent();

            DbAccess = new db(this);
            DbAccess.GetData();

            //Disabled = true;
            
            if (Disabled)
            {
                grdContainer.IsEnabled = false;
                var win = new BlockedWindow(Reason);
                var ret = win.ShowDialog();
                if(ret != null)
                    Environment.Exit(0);
            }
                
            
            
            //set sidebar to out and set lengths
            sideBarState = true;
            cgSidebar.Width = new GridLength(2, GridUnitType.Star);
            cgContent.Width = new GridLength(8, GridUnitType.Star);
            btnResize.Content = "<";
            
            current = Displayed.Home;
            //set container instance
        }

        private void BtnResize_OnClick(object sender, RoutedEventArgs e)
        {
            if (sideBarState)
            {
                btnResize.Content = ">";
                cgSidebar.Width = new GridLength(0.7, GridUnitType.Star);
                cgContent.Width = new GridLength(9.5, GridUnitType.Star);
                sideBarState = false;

                btnHome.Width = 30;
                btnInstall.Width = 30;
                btnAbout.Width = 30;

                btnHome.Content = "H";
                btnInstall.Content = "I";
                btnAbout.Content = "A";

                btnHome.FontSize = 15;
                btnInstall.FontSize = 15;
                btnAbout.FontSize = 15;


            }
            else
            {
                btnResize.Content = "<";
                cgSidebar.Width = new GridLength(2, GridUnitType.Star);
                cgContent.Width = new GridLength(8, GridUnitType.Star);
                sideBarState = true;
                
                btnHome.Width = 120;
                btnInstall.Width = 120;
                btnAbout.Width = 120;

                btnHome.Content = "Home";
                btnInstall.Content = "Install";
                btnAbout.Content = "About";
                
                btnHome.FontSize = 20;
                btnInstall.FontSize = 20;
                btnAbout.FontSize = 20;

            }
        }

        private void btnHome_OnClick(object sender, RoutedEventArgs e)
        {
            
            
        }

        private void btnInstall_OnClick(object sender, RoutedEventArgs e)
        {
            //Check if all file exist
            var output = "You are missing the following dependencies, install is not possible:\n";
            var flag = false;
            foreach (var req in FileList)
            {
                //if they dont exist add them to list of errors
                if (!File.Exists($"\\Dependencies\\{req.Item1}"))
                {
                    output += $"{req.Item2}\n";
                    flag = true;
                }
            }
            
            if (flag)
            {
                //output error file and 
                var desk = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                File.WriteAllText($"{desk}\\InstallerError.txt", output);
                MessageBox.Show(
                    "Missing Dependencies Prevent Tsunami Hack from being Installed. Refer to \"InstallerError.txt\" on your desktop for more information.",
                    "INSTALLER ERROR", MessageBoxButton.OK);
                goto End;
            }  
            
            
            
            
            End:;
        }

        private void btnAbout_OnClick(object sender, RoutedEventArgs e)
        {
            
        }

        private static string ConcatAppData(string ip)
        {
            var indexof = ip.LastIndexOf('\\');
            if (ip.Length - indexof == 7)
                ip = ip.Remove(indexof);
            var apd = "\\LocalLow\\Smartly Dressed Games\\Unturned\\Tsunami\\";
            return string.Concat(ip, apd);
        }
        
    }
}
