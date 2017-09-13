﻿using System;
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

namespace TsuManager.Installation
{
    /// <summary>
    /// Interaction logic for InstallStatus.xaml
    /// </summary>
    public partial class InstallStatus : Page
    {
        internal InstallationManager Manager;
        internal MainWindow MainWindow;
        
        public InstallStatus(InstallationManager manager, MainWindow parent)
        {
            Manager = manager;
            MainWindow = parent;
            
            InitializeComponent();
        }
    }
}
