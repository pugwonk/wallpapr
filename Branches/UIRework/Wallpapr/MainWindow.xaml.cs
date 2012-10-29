using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Microsoft.Win32;
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
            _viewModel.PropertyChanged += _viewModel_PropertyChanged;
        }

        void _viewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "PopupBalloon")
            {
                notifyIcon.ShowBalloonTip(3);
            }
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

        private void MenuItem_ClickGoFlickURL(object sender, RoutedEventArgs e)
        {
            _viewModel.GotoFlickrURL();
        }

        private void MenuItem_ClickExit(object sender, RoutedEventArgs e)
        {
            _viewModel.SaveSettings();
            Environment.Exit(0);
        }

        private void Window_Closing_1(object sender, CancelEventArgs e)
        {
            // I prefer the old behaviour, so I uncommented it :) - CLR
            e.Cancel = true;
            this.WindowState = WindowState.Minimized;
        }

        private void Window_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
            {
                Hide();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
            _viewModel.SaveSettings();

            //Do this after minimising to avoid delay
            _viewModel.GetNewWallpaper();
        }



    }
}