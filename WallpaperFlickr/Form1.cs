using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Text;
using System.Diagnostics;
using System.Reflection;

namespace WallpaperFlickr {
    public partial class Form1 : Form {

        public Form1() {
            InitializeComponent();
        }

        private WallpaperFlickrSettings settings;

        private void Form1_Load(object sender, EventArgs e) {
            settings = new WallpaperFlickrSettings();
            settings.ReadSettings();

            ddInterval.Items.Add("minutes");
            ddInterval.Items.Add("hours");
            ddInterval.Items.Add("days");
            ddInterval.Items.Add("weeks");
            ddInterval.Items.Add("months");

            ddPosition.Items.Add("Centered");
            ddPosition.Items.Add("Tiled");
            ddPosition.Items.Add("Stretched");

            //ddOrderBy.Items.Add("Date Posted Asc");
            ddOrderBy.Items.Add("Newly Posted");
            //ddOrderBy.Items.Add("Date Taken Asc");
            ddOrderBy.Items.Add("Most Recently Taken");
            //ddOrderBy.Items.Add("Least Interesting");
            ddOrderBy.Items.Add("Most Interesting");
            ddOrderBy.Items.Add("None");
            ddOrderBy.Items.Add("Relevance");

            lbVersion.Text = "Version " + Assembly.GetExecutingAssembly().GetName().Version.ToString();

            numFrequency.Value = settings.Frequency;
            ddInterval.Text = settings.Interval;
            ddOrderBy.Text = settings.OrderBy;
            ddPosition.Text = settings.Position;
            txtApiKey.Text = settings.ApiKey;
            txtTags.Text = settings.Tags;
            txtUserId.Text = settings.UserId;
            txtFaveUserId.Text = settings.FaveUserId;
            rbSearch.Checked = settings.SearchOrFaves;
            rbFaves.Checked = !rbSearch.Checked;
            EnableSearchTypes();

            rbAllTags.Checked = false;
            rbAnyTags.Checked = false;
            if (settings.TagMode == "all") {
                rbAllTags.Checked = true;
            } else {
                rbAnyTags.Checked = true;
            }

            timer1.Enabled = true;
            timer1.Interval = 60000;
            timer1.Start();

            Hide();

            if (settings.ApiKey.Equals(string.Empty)) {
                MessageBox.Show("Please read the readme.txt and follow the instructions to get an API key.");
            }

            GetNewWallpaper();
        }

        private void GetNewWallpaper()
        {
            notifyIcon1.Text = "Retrieving next picture...";
            notifyIcon1.Icon = WallpaperFlickr.Properties.Resources.flickrwait;
            
            if (settings.ApiKey.Equals(string.Empty))
            {
                notifyIcon1.Text = "API key missing";
                notifyIcon1.Icon = WallpaperFlickr.Properties.Resources.flickrbad;
                return;
            }

            FlickrNet.Flickr flickr = new FlickrNet.Flickr();
            flickr.ApiKey = settings.ApiKey;

            FlickrNet.PhotoCollection photos = null;
            if (settings.SearchOrFaves) // doing normal search
            {
                FlickrNet.PhotoSearchOptions options = new FlickrNet.PhotoSearchOptions();
                if (!settings.Tags.Trim().Equals(string.Empty))
                {
                    options.Tags = settings.Tags;
                    options.TagMode = GetTagMode();
                }
                if (!settings.UserId.Trim().Equals(string.Empty))
                {
                    FlickrNet.FoundUser fuser;
                    string UserName = "";
                    string[] AllUserNames = settings.UserId.Split(',');
                    UserName = AllUserNames[new Random().Next(0, AllUserNames.GetUpperBound(0) + 1)];
                    try
                    { // Exception handler added by CLR 2010-06-11
                        fuser = flickr.PeopleFindByUserName(UserName.Trim());
                    }
                    catch (Exception ex)
                    {
                        FailWithError(ex);
                        return;
                    }
                    if (!fuser.UserId.Equals(string.Empty))
                    {
                        options.UserId = fuser.UserId;
                    }
                }
                options.PrivacyFilter = FlickrNet.PrivacyFilter.PublicPhotos;
                options.SortOrder = GetSortOrder();
                options.PerPage = 365;

                try
                {
                    photos = flickr.PhotosSearch(options);
                    //photos = flickr.PhotosGetRecent(); // this was me trying to do Explore stuff, but failed
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                    FailWithError(ex);
                    return;
                }
                options = null;
            }
            else // getting someone's favourites
            {
                try
                {
                    FlickrNet.FoundUser fuser;
                    fuser = flickr.PeopleFindByUserName(settings.FaveUserId);
                    photos = flickr.FavoritesGetPublicList(fuser.UserId);
                }
                catch (Exception ex)
                {
                    FailWithError(ex);
                    return;
                }
            }

            clsWallpaper wallpaper = new clsWallpaper();

            Random pn = new Random();

            if (photos.Count == 0)
                MessageBox.Show("Specified parameters return no photographs from Flickr.", "WallpaperFlickr");
            else
            {
                int chosePhoto = pn.Next(0, photos.Count);
                //FlickrNet.Sizes fs = flickr.PhotosGetSizes("4570943273");
                FlickrNet.SizeCollection fs;
                try
                {
                    fs = flickr.PhotosGetSizes(photos[chosePhoto].PhotoId);
                }
                catch (Exception ex)
                {
                    FailWithError(ex);
                    return;
                }
                // Load the last size (which should be "Original"). Doing all this
                // because photo.OriginalURL just causes an exception
                bool LoadedWallpaper = wallpaper.Load(fs[fs.Count-1].Source, settings, 
                    getDisplayStyle(), Application.ExecutablePath, photos[chosePhoto].WebUrl);

                FlickrNet.Person fuser;
                string notifyText = "";
                try
                {
                    fuser = flickr.PeopleGetInfo(photos[chosePhoto].UserId);
                    notifyText = fuser.UserName +
                        ": " + photos[chosePhoto].Title;
                }
                catch (Exception ex)
                {
                    FailWithError(ex);
                    return;
                }

                notifyIcon1.Text = notifyText.Substring(0, Math.Min(63, notifyText.Length));
            }

            wallpaper = null;
            flickr = null;
            photos = null;
            notifyIcon1.Icon = WallpaperFlickr.Properties.Resources.flickr;
        }

        private void FailWithError(Exception ex)
        {
            notifyIcon1.Text = ex.Message.Substring(0, Math.Min(ex.Message.Length, 63));
            notifyIcon1.Icon = WallpaperFlickr.Properties.Resources.flickrbad;
        }


        private void timer1_Tick(object sender, EventArgs e) {
            doSaveSettings();
            if (settings.HasExpired()) {
                GetNewWallpaper();
            }
        }

        private FlickrNet.PhotoSearchSortOrder GetSortOrder() {
            switch (settings.OrderBy) {
                //case "Date Posted Asc": return FlickrNet.PhotoSearchSortOrder.DatePostedAsc;
                case "Newly Posted": return FlickrNet.PhotoSearchSortOrder.DatePostedDescending;
                //case "Date Taken Asc": return FlickrNet.PhotoSearchSortOrder.DateTakenAsc;
                case "Most Recently Taken": return FlickrNet.PhotoSearchSortOrder.DateTakenDescending;
                //case "Interestingness Asc": return FlickrNet.PhotoSearchSortOrder.InterestingnessAsc;
                case "Most Interesting": return FlickrNet.PhotoSearchSortOrder.InterestingnessDescending;
                case "None": return FlickrNet.PhotoSearchSortOrder.None;
                case "Relevance": return FlickrNet.PhotoSearchSortOrder.Relevance;
                default: return FlickrNet.PhotoSearchSortOrder.InterestingnessAscending;
            }
        }

        private FlickrNet.TagMode GetTagMode() {
            switch (settings.TagMode) {
                case "all": return FlickrNet.TagMode.AllTags;
                case "any": return FlickrNet.TagMode.AnyTag;
                default: return FlickrNet.TagMode.AnyTag;
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
            contextMenuStrip1.Show();
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
            GetNewWallpaper();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            doSaveSettings();
            GetNewWallpaper();
        }

        private void doSaveSettings() {
            settings.ApiKey = txtApiKey.Text;
            settings.Frequency = Convert.ToInt32(numFrequency.Value);
            settings.Interval = ddInterval.Text;
            settings.OrderBy = ddOrderBy.Text;
            settings.Position = ddPosition.Text;
            settings.SearchOrFaves = rbSearch.Checked;
            if (rbAllTags.Checked) {
                settings.TagMode = "all";
            } else {
                settings.TagMode = "any";
            }
            settings.Tags = txtTags.Text;
            settings.UserId = txtUserId.Text;
            settings.FaveUserId = txtFaveUserId.Text;
            settings.SaveSettings();
        }

        private winWallpaper.Style getDisplayStyle() {
            switch (settings.Position.ToLower()) {
                case "centered":    return winWallpaper.Style.Centered;
                case "tiled":       return winWallpaper.Style.Tiled;
                case "stretched":   return winWallpaper.Style.Stretched;
                default:            return winWallpaper.Style.Stretched;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            doSaveSettings();
            Hide();
            GetNewWallpaper();
        }

        private void thisPhotoOnFlickrcomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(settings.WebURL);
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
    }
}