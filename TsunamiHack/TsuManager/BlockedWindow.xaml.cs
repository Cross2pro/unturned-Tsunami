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
using System.Windows.Shapes;

namespace TsuManager
{
    /// <summary>
    /// Interaction logic for BlockedWindow.xaml
    /// </summary>
    public partial class BlockedWindow : Window
    {
        public BlockedWindow(string reason)
        {
            InitializeComponent();

            lblBlockReason.Text = reason;
        }

        private void BtnBlockDiscord_OnClick(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://discord.gg/cW8Mjdf");
        }

        private void BtnBlockClose_OnClick(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);   
        }

        private void BtnBlockUninstall_OnClick(object sender, RoutedEventArgs e)
        {
            //TODO: add uninstall code
        }
    }
}
