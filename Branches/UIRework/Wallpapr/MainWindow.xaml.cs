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

        private void MenuItem_ClickGoFlickURL(object sender, RoutedEventArgs e)
        {
            _viewModel.GotoFlickrURL();
        }

        private void MenuItem_ClickExit(object sender, RoutedEventArgs e)
        {   
            doSaveSettings();            
            Environment.Exit(0);
        }

        //This method doesn't belong here, but is making up for the lack of INotifyPropertyChanged until we implement it in the viewModel
        private void doSaveSettings()
        {            
            ////_viewModel.Frequency = Convert.ToInt32(numFrequency.Value);
            ////_viewModel.Interval = ddInterval.Text;
            ////_viewModel.OrderBy = ddOrderBy.Text;
            ////_viewModel.Position = ddPosition.Text;
            ////if (rbSearch.Checked)
            ////    _viewModel.SearchOrFaves = 0;
            ////else
            ////    if (rbFaves.Checked)
            ////        _viewModel.SearchOrFaves = 1;
            ////    else
            ////        _viewModel.SearchOrFaves = 2;
            ////_viewModel.StartWithWindows = cbStartWithWindows.Checked;
            ////_viewModel.CachePics = cbCache.Checked;
            ////_viewModel.ShowBubbles = cbBubbles.Checked;

            // Also need to actually change the registry here
            RegistryKey myKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (_viewModel.StartWithWindows)
            {
                myKey.SetValue("WallpaperFlickr",
                    System.Reflection.Assembly.GetExecutingAssembly().Location,
                    RegistryValueKind.String);
            }
            else
            {
                try
                {
                    myKey.DeleteValue("WallpaperFlickr");
                }
                catch
                {
                }
            }

            ////if (rbAllTags.Checked)
            ////{
            ////    _viewModel.TagMode = "all";
            ////}
            ////else
            ////{
            ////    _viewModel.TagMode = "any";
            ////}
            ////_viewModel.Tags = txtTags.Text;
            ////_viewModel.UserId = txtUserId.Text;
            ////_viewModel.FaveUserId = txtFaveUserId.Text;
            _viewModel.SaveSettings();
        }


    }
}