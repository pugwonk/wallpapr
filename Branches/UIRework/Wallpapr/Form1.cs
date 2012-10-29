using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Text;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.Drawing.Drawing2D;

namespace WallpaperFlickr {
    public partial class Form1 : Form {

        public Form1() {
            InitializeComponent();
        }

        private Form1ViewModel _viewModel;


        private void Form1_Load(object sender, EventArgs e) {
            _viewModel = new Form1ViewModel();

            ddInterval.Items.Add("minutes");
            ddInterval.Items.Add("hours");
            ddInterval.Items.Add("days");
            ddInterval.Items.Add("weeks");
            ddInterval.Items.Add("months");

            ddPosition.Items.Add("Centered");
            ddPosition.Items.Add("Tiled");
            ddPosition.Items.Add("Stretched");
            ddPosition.Items.Add("Fill");
            ddPosition.Items.Add("Fit");

            //ddOrderBy.Items.Add("Date Posted Asc");
            ddOrderBy.Items.Add("Newly Posted");
            //ddOrderBy.Items.Add("Date Taken Asc");
            ddOrderBy.Items.Add("Most Recently Taken");
            //ddOrderBy.Items.Add("Least Interesting");
            ddOrderBy.Items.Add("Most Interesting");
            ddOrderBy.Items.Add("None");
            ddOrderBy.Items.Add("Relevance");

            lbVersion.Text = "Version " + Assembly.GetExecutingAssembly().GetName().Version.ToString();

            numFrequency.Value = _viewModel.Frequency;
            ddInterval.Text = _viewModel.Interval;
            ddOrderBy.Text = _viewModel.OrderBy;
            ddPosition.Text = _viewModel.Position;
            //txtApiKey.Text = settings.ApiKey;
            txtTags.Text = _viewModel.Tags;
            txtUserId.Text = _viewModel.UserId;
            txtFaveUserId.Text = _viewModel.FaveUserId;
            rbSearch.Checked = (_viewModel.SearchOrFaves == 0);
            rbFaves.Checked = (_viewModel.SearchOrFaves == 1);
            rbExplore.Checked = (_viewModel.SearchOrFaves == 2);
            cbStartWithWindows.Checked = _viewModel.StartWithWindows;
            cbCache.Checked = _viewModel.CachePics;
            cbBubbles.Checked = _viewModel.ShowBubbles;
            EnableSearchTypes();

            rbAllTags.Checked = false;
            rbAnyTags.Checked = false;
            if (_viewModel.TagMode == "all") {
                rbAllTags.Checked = true;
            } else {
                rbAnyTags.Checked = true;
            }

            timer1.Enabled = true;
            timer1.Interval = 60000;
            timer1.Start();

            Hide();

            if (_viewModel.ApiKey.Equals(string.Empty))
            {
                MessageBox.Show("Please read the readme.txt and follow the instructions to get an API key.");
            }

            _viewModel.GetNewWallpaper();
            UpdateBalloon();                
        }

        private void timer1_Tick(object sender, EventArgs e) {
            doSaveSettings();
            if (_viewModel.HasExpired()) {
                _viewModel.GetNewWallpaper();
                UpdateBalloon();
            }
        }

   

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e) {
            WindowState = FormWindowState.Normal;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
            // I prefer the old behaviour, so I uncommented it :) - CLR
            e.Cancel = true;
            this.WindowState = FormWindowState.Minimized;
        }

        private void Form1_Resize(object sender, EventArgs e) {
            if (this.WindowState == FormWindowState.Minimized) {
                Hide();
            }
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e) {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            // Left-click gets new wallpaper. Anything else gets context menu
            {
                doSaveSettings();
                _viewModel.GetNewWallpaper();
                UpdateBalloon();
            }
            else
                contextMenuStrip1.Show();
        }

        private void UpdateBalloon()
        {
            notifyIcon1.BalloonTipText = _viewModel.NotifyIconBalloonTipText;
            notifyIcon1.BalloonTipTitle = _viewModel.NotifyIconBalloonTipTitle;
            notifyIcon1.Icon = _viewModel.NotifyIconIcon;
            notifyIcon1.Text = _viewModel.NotifyIconText;

            if (_viewModel.ShowBubbles
                && !_viewModel.IsNotifyFail)
                notifyIcon1.ShowBalloonTip(3);

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            doSaveSettings();
            //Application.Exit();
            Environment.Exit(0);
        }

        private void getNewWallpaperToolStripMenuItem_Click(object sender, EventArgs e) {
            Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void getNewWallpaperToolStripMenuItem1_Click(object sender, EventArgs e) {
            doSaveSettings();
            _viewModel.GetNewWallpaper();
            UpdateBalloon();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            doSaveSettings();
            _viewModel.GetNewWallpaper();
            UpdateBalloon();
        }

        private void doSaveSettings() {
            //settings.ApiKey = txtApiKey.Text;
            _viewModel.Frequency = Convert.ToInt32(numFrequency.Value);
            _viewModel.Interval = ddInterval.Text;
            _viewModel.OrderBy = ddOrderBy.Text;
            _viewModel.Position = ddPosition.Text;
            if (rbSearch.Checked)
                _viewModel.SearchOrFaves = 0;
            else
                if (rbFaves.Checked)
                    _viewModel.SearchOrFaves = 1;
                else
                    _viewModel.SearchOrFaves = 2;
            _viewModel.StartWithWindows = cbStartWithWindows.Checked;
            _viewModel.CachePics = cbCache.Checked;
            _viewModel.ShowBubbles = cbBubbles.Checked;
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

            if (rbAllTags.Checked) {
                _viewModel.TagMode = "all";
            } else {
                _viewModel.TagMode = "any";
            }
            _viewModel.Tags = txtTags.Text;
            _viewModel.UserId = txtUserId.Text;
            _viewModel.FaveUserId = txtFaveUserId.Text;
            _viewModel.SaveSettings();
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            doSaveSettings();
            Hide();
            _viewModel.GetNewWallpaper();
            UpdateBalloon();
        }

        private void thisPhotoOnFlickrcomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _viewModel.GotoFlickrURL();
        }

     

        private void rbSearch_CheckedChanged(object sender, EventArgs e)
        {
            EnableSearchTypes();
        }

        private void EnableSearchTypes()
        {
            label5.Enabled = label7.Enabled = txtTags.Enabled = rbAllTags.Enabled = rbAnyTags.Enabled
                = label1.Enabled = ddOrderBy.Enabled = label6.Enabled = txtUserId.Enabled = rbSearch.Checked;
            txtFaveUserId.Enabled = rbFaves.Checked;
        }

        private void rbFaves_CheckedChanged(object sender, EventArgs e)
        {
            EnableSearchTypes();
        }

        private void llWebsite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("http://flickrwallpaper.codeplex.com/");  
            Process.Start(sInfo);
        }

        private void rbExplore_CheckedChanged(object sender, EventArgs e)
        {
            EnableSearchTypes();
        }

        private void notifyIcon1_BalloonTipClicked(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(_viewModel.WebURL);
        }
    }
}