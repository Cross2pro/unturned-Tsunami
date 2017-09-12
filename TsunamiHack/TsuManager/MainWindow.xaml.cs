using System;
using System.Collections.Generic;
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
using UnityEngine;

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
        
        private bool sideBarState;
        private enum Displayed { Home, Install, About}

        private Displayed current;
        
        public MainWindow()
        {
            InitializeComponent();

            DbAccess = new db(this);
//            DbAccess.GetData();

            if (Disabled)
            {
                grdContainer.IsEnabled = false;
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
            
            //TODO: Get Folder path of \appdata\locallow
            //Environment.GetFolderPath(Special)


        }

        private void btnInstall_OnClick(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnAbout_OnClick(object sender, RoutedEventArgs e)
        {
            
        }
        
        
    }
}
