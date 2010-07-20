using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Text;

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

            numFrequency.Value = settings.Frequency;
            ddInterval.Text = settings.Interval;
            ddOrderBy.Text = settings.OrderBy;
            ddPosition.Text = settings.Position;
            txtApiKey.Text = settings.ApiKey;
            txtTags.Text = settings.Tags;
            txtUserId.Text = settings.UserId;

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
        }

        private void GetNewWallpaperBACKUPDELETEME() {

            if (settings.ApiKey.Equals(string.Empty)) {
                return;
            }

            FlickrNet.Flickr flickr = new FlickrNet.Flickr();
            flickr.ApiKey = settings.ApiKey;

            FlickrNet.PhotoSearchOptions options = new FlickrNet.PhotoSearchOptions();
            if (!settings.Tags.Trim().Equals(string.Empty)) {
                options.Tags = settings.Tags;
                options.TagMode = GetTagMode();
            }
            if (!settings.UserId.Trim().Equals(string.Empty)) {
                FlickrNet.FoundUser fuser;
                string UserName = "";
                string[] AllUserNames = settings.UserId.Split(',');
                UserName = AllUserNames[new Random().Next(0, AllUserNames.GetUpperBound(0) + 1)];
                try
                { // Exception handler added by CLR 2010-06-11
                    fuser = flickr.PeopleFindByUsername(UserName.Trim());
                }
                catch (Exception ex)
                {
                    return;
                }
                if (!fuser.UserId.Equals(string.Empty)) {
                    options.UserId = fuser.UserId;
                }
            }
            options.PrivacyFilter = FlickrNet.PrivacyFilter.PublicPhotos;
            options.SortOrder = GetSortOrder();
            options.PerPage = 365;


            FlickrNet.Photos photos = null;
            try {
                photos = flickr.PhotosSearch(options);
            } catch (Exception ex) {
                //MessageBox.Show(ex.Message);
                return;
            }
            

            clsWallpaper wallpaper = new clsWallpaper();

            for(int i = 0; i < photos.PhotoCollection.Length; i++) {
                wallpaper.URL = photos.PhotoCollection[i].LargeUrl;
                if(!wallpaper.AlreadyDownloaded()) {

                    /* StringBuilder sb = new StringBuilder();
                    sb.Append("Accuracy: " + photos.PhotoCollection[i].Accuracy.ToString() + "\n");
                    sb.Append("CleanTags: " + photos.PhotoCollection[i].CleanTags + "\n");
                    sb.Append("DateAdded: " + photos.PhotoCollection[i].DateAdded.ToString() + "\n");
                    sb.Append("DateTaken: " + photos.PhotoCollection[i].DateTaken.ToString() + "\n");
                    sb.Append("DateUploaded: " + photos.PhotoCollection[i].DateUploaded.ToString() + "\n");
                    sb.Append("Farm: " + photos.PhotoCollection[i].Farm + "\n");
                    sb.Append("IconServer: " + photos.PhotoCollection[i].IconServer + "\n");
                    sb.Append("IsFamily: " + photos.PhotoCollection[i].IsFamily.ToString() + "\n");
                    sb.Append("IsFriend: " + photos.PhotoCollection[i].IsFriend.ToString() + "\n");
                    sb.Append("IsPrimary: " + photos.PhotoCollection[i].IsPrimary.ToString() + "\n");
                    sb.Append("IsPublic: " + photos.PhotoCollection[i].IsPublic.ToString() + "\n");
                    sb.Append("LargeUrl: " + photos.PhotoCollection[i].LargeUrl + "\n");
                    sb.Append("LastUpdated: " + photos.PhotoCollection[i].LastUpdated.ToString() + "\n");
                    sb.Append("Latitude: " + photos.PhotoCollection[i].Latitude.ToString() + "\n");
                    sb.Append("License: " + photos.PhotoCollection[i].License + "\n");
                    sb.Append("Longitude: " + photos.PhotoCollection[i].Longitude.ToString() + "\n");
                    sb.Append("MachineTags: " + photos.PhotoCollection[i].MachineTags + "\n");
                    sb.Append("MediumUrl: " + photos.PhotoCollection[i].MediumUrl + "\n");
                    sb.Append("OriginalFormat: " + photos.PhotoCollection[i].OriginalFormat + "\n");
                    sb.Append("OriginalSecret: " + photos.PhotoCollection[i].OriginalSecret + "\n");
                    //sb.Append("OriginalUrl: " + photos.PhotoCollection[i].OriginalUrl + "\n");
                    sb.Append("OwnerName: " + photos.PhotoCollection[i].OwnerName + "\n");
                    sb.Append("PhotoId: " + photos.PhotoCollection[i].PhotoId + "\n");
                    sb.Append("RawTags: " + photos.PhotoCollection[i].RawTags + "\n");
                    sb.Append("Secret: " + photos.PhotoCollection[i].Secret + "\n");
                    sb.Append("Server: " + photos.PhotoCollection[i].Server + "\n");
                    sb.Append("SmallUrl: " + photos.PhotoCollection[i].SmallUrl + "\n");
                    sb.Append("SquareThumbnailUrl: " + photos.PhotoCollection[i].SquareThumbnailUrl + "\n");
                    sb.Append("ThumbnailUrl: " + photos.PhotoCollection[i].ThumbnailUrl + "\n");
                    sb.Append("Title: " + photos.PhotoCollection[i].Title + "\n");
                    sb.Append("UserId: " + photos.PhotoCollection[i].UserId + "\n");
                    sb.Append("WebUrl: " + photos.PhotoCollection[i].WebUrl + "\n");
                    MessageBox.Show(sb.ToString()); */

                    if (!Directory.Exists("wallpaper")) {
                        Directory.CreateDirectory("wallpaper");
                    }

                    int spot = wallpaper.URL.LastIndexOf("/");
                    string FileName = wallpaper.URL.Substring(spot + 1, wallpaper.URL.Length - spot - 1);

                    WebClient wc = new WebClient();
                    bool DownloadedOK = true;
                    try
                    {
                        wc.DownloadFile(wallpaper.URL, "wallpaper\\" + FileName);
                    }
                    catch
                    {
                        DownloadedOK = false;
                    }
                    wc.Dispose();

                    if (DownloadedOK)
                    {
                        FileInfo fi = new FileInfo("wallpaper\\" + FileName);
                        //don't save those stupid photo not available images
                        if (fi.Length > 3500)
                        {
                            Bitmap bitmap = new Bitmap("wallpaper\\" + FileName);
                            bitmap.Save("wallpaper\\_CurrentPaper.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
                            bitmap.Dispose();

                            wallpaper.AddRecord();
                            settings.LastChange = DateTime.Now;

                            spot = Application.ExecutablePath.LastIndexOf("\\");
                            string appPath = Application.ExecutablePath.Substring(0, spot);

                            winWallpaper.ChangeWallpaper(appPath + "\\wallpaper\\_CurrentPaper.bmp", getDisplayStyle());

                            break;
                        }
                        else
                        {
                            wallpaper.AddRecord();
                            File.Delete("wallpaper\\" + FileName);
                        }
                        fi = null;
                    }
                }
            }
            options = null;
            wallpaper = null;
            flickr = null;
            photos = null;
        }

        private void GetNewWallpaper()
        {

            if (settings.ApiKey.Equals(string.Empty))
            {
                return;
            }

            FlickrNet.Flickr flickr = new FlickrNet.Flickr();
            flickr.ApiKey = settings.ApiKey;

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
                    fuser = flickr.PeopleFindByUsername(UserName.Trim());
                }
                catch (Exception ex)
                {
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


            FlickrNet.Photos photos = null;
            try
            {
                photos = flickr.PhotosSearch(options);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return;
            }

            clsWallpaper wallpaper = new clsWallpaper();

            Random pn = new Random();

            if (photos.PhotoCollection.Length == 0)
                MessageBox.Show("Specified parameters return no photographs from Flickr.", "WallpaperFlickr");
            else
            {
                int chosePhoto = pn.Next(0, photos.PhotoCollection.Length);
                //chosePhoto = 1;

                //wallpaper.URL = photos.PhotoCollection[chosePhoto].LargeUrl;
                //wallpaper.URL = photos.PhotoCollection[chosePhoto].MediumUrl;
                bool LoadedWallpaper = wallpaper.Load(photos.PhotoCollection[chosePhoto].LargeUrl, settings, 
                    getDisplayStyle(), Application.ExecutablePath, photos.PhotoCollection[chosePhoto].WebUrl);
                if (!LoadedWallpaper) // try medium
                    LoadedWallpaper = wallpaper.Load(photos.PhotoCollection[chosePhoto].MediumUrl, settings,
                        getDisplayStyle(), Application.ExecutablePath, photos.PhotoCollection[chosePhoto].WebUrl);
            }

            options = null;
            wallpaper = null;
            flickr = null;
            photos = null;
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
                case "Newly Posted": return FlickrNet.PhotoSearchSortOrder.DatePostedDesc;
                //case "Date Taken Asc": return FlickrNet.PhotoSearchSortOrder.DateTakenAsc;
                case "Most Recently Taken": return FlickrNet.PhotoSearchSortOrder.DateTakenDesc;
                //case "Interestingness Asc": return FlickrNet.PhotoSearchSortOrder.InterestingnessAsc;
                case "Most Interesting": return FlickrNet.PhotoSearchSortOrder.InterestingnessDesc;
                case "None": return FlickrNet.PhotoSearchSortOrder.None;
                case "Relevance": return FlickrNet.PhotoSearchSortOrder.Relevance;
                default: return FlickrNet.PhotoSearchSortOrder.InterestingnessAsc;
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
            if (rbAllTags.Checked) {
                settings.TagMode = "all";
            } else {
                settings.TagMode = "any";
            }
            settings.Tags = txtTags.Text;
            settings.UserId = txtUserId.Text;
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
            Hide();
        }

        private void thisPhotoOnFlickrcomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(settings.WebURL);
        }
    }
}