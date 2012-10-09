using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Avalon.Windows.Controls;
using WallpaperFlickr;

namespace WallPapr
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new Form1ViewModel();
            DataContext = _viewModel;
        }

        private Form1ViewModel _viewModel;

        private void _notifyIcon_Click(object sender, RoutedEventArgs e)
        {            
            var notifyIcon = this.FindName("notifyIcon") as NotifyIcon;
            notifyIcon.ContextMenu.IsOpen = true;
        }

        private void MenuItem_ClickSettings(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Normal;
            this.BringIntoView();
        }

        private void MenuItem_ClickGetNewPaper(object sender, RoutedEventArgs e)
        {
            _viewModel.GetNewWallpaper();
        }

    }
}