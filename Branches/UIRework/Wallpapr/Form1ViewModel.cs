using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WallpaperFlickr
{
    //Todo: Rename me - this is an awful name
    public class Form1ViewModel
    {
        public Form1ViewModel()
        {
            _settings = new WallpaperFlickrSettings();
            _settings.ReadSettings();
        }

        private WallpaperFlickrSettings _settings;


        public int Frequency
        {
            get { return _settings.Frequency; }
            set { _settings.Frequency = value; }
        }

        public string Interval
        {
            get { return _settings.Interval; }
            set { _settings.Interval = value; }
        }

        public string OrderBy
        {
            get { return _settings.OrderBy; }
            set { _settings.OrderBy = value; }
        }


        public string ApiKey
        {
            get { return _settings.ApiKey; }
            set { _settings.ApiKey = value; }
        }

        public string Position
        {
            get { return _settings.Position; }
            set { _settings.Position = value; }
        }

        public string Tags
        {
            get { return _settings.Tags; }
            set { _settings.Tags = value; }
        }

        public string UserId
        {
            get { return _settings.UserId; }
            set { _settings.UserId = value; }
        }

        public string FaveUserId
        {
            get { return _settings.FaveUserId; }
            set { _settings.FaveUserId = value; }
        }

        public int SearchOrFaves
        {
            get { return _settings.SearchOrFaves; }
            set { _settings.SearchOrFaves = value; }
        }

        public bool StartWithWindows
        {
            get { return _settings.StartWithWindows; }
            set { _settings.StartWithWindows = value; }
        }

        public bool CachePics      {
            get { return _settings.CachePics; }
            set { _settings.CachePics = value; }
        }

        public bool ShowBubbles
        {
            get { return _settings.ShowBubbles; }
            set { _settings.ShowBubbles = value; }
        }

        public string TagMode
        {
            get { return _settings.TagMode; }
            set { _settings.TagMode = value; }
        }

        public WallpaperFlickrSettings Settings { get { return _settings; } }

        internal bool HasExpired()
        {

            return _settings.HasExpired();
        }

        internal void SaveSettings()
        {
            _settings.SaveSettings();
        }

        public string WebURL
        {
            get { return _settings.WebURL; }
            set { _settings.WebURL = value; }
        }


        public void GetNewWallpaper()
        {
            NotifyIconText = "Retrieving next picture...";
            NotifyIconIcon = WallpaperFlickr.Properties.Resources.flickrwait;
            IsNotifyFail = false;

            if (ApiKey.Equals(string.Empty))
            {
                NotifyIconText = "API key missing";
                NotifyIconIcon = WallpaperFlickr.Properties.Resources.flickrbad;
                IsNotifyFail = true;
                return;
            }

            FlickrNet.Flickr flickr = new FlickrNet.Flickr();
            flickr.ApiKey = ApiKey;

            FlickrNet.PhotoCollection photos = null;

            switch (SearchOrFaves)
            {
                case 0:
                    FlickrNet.PhotoSearchOptions options = new FlickrNet.PhotoSearchOptions();
                    if (!Tags.Trim().Equals(string.Empty))
                    {
                        options.Tags = Tags;
                        options.TagMode = GetTagMode();
                    }
                    if (!UserId.Trim().Equals(string.Empty))
                    {
                        FlickrNet.FoundUser fuser;
                        string UserName = "";
                        string[] AllUserNames = UserId.Split(',');
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
                    break;
                case 1:
                    try
                    {
                        FlickrNet.FoundUser fuser;
                        fuser = flickr.PeopleFindByUserName(FaveUserId);
                        photos = flickr.FavoritesGetPublicList(fuser.UserId);
                    }
                    catch (Exception ex)
                    {
                        FailWithError(ex);
                        return;
                    }
                    break;
                case 2:
                    // do explore
                    try
                    {
                        photos = flickr.InterestingnessGetList();
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show(ex.Message);
                        FailWithError(ex);
                        return;
                    }
                    break;
                default:
                    break;
            }


            clsWallpaper wallpaper = new clsWallpaper();

            Random pn = new Random();

            if (photos.Count == 0)
            {
                NotifyIconText = "Specified parameters return no photographs from Flickr";
                NotifyIconIcon = WallpaperFlickr.Properties.Resources.flickrbad;
                IsNotifyFail = true;
                return;
            }
            else
            {
                int chosePhoto = pn.Next(0, photos.Count);
                //FlickrNet.Sizes fs = flickr.PhotosGetSizes("4570943273");
                FlickrNet.SizeCollection fs;
                bool LoadedWallpaper = false;
                try
                {
                    fs = flickr.PhotosGetSizes(photos[chosePhoto].PhotoId);
                    // Load the last size (which should be "Original"). Doing all this
                    // because photo.OriginalURL just causes an exception
                    LoadedWallpaper = wallpaper.Load(fs[fs.Count - 1].Source, _settings,
                        getDisplayStyle(), Application.ExecutablePath, photos[chosePhoto].WebUrl);
                }
                catch (Exception ex) // load failed with an exception
                {
                    FailWithError(ex);
                    return;
                }

                if (!LoadedWallpaper) // load failed, but didn't cause an exception
                {
                    NotifyIconText = "Failed to load wallpaper";
                    NotifyIconIcon = WallpaperFlickr.Properties.Resources.flickrbad;
                    IsNotifyFail = true;
                    return;
                }

                // Get further info about the photo to display in the tooltip
                FlickrNet.PhotoInfo fi;
                try
                {
                    fi = flickr.PhotosGetInfo(photos[chosePhoto].PhotoId);
                }
                catch (Exception ex)
                {
                    FailWithError(ex);
                    return;
                }

                // Set thumbnail
                NotifyIconIcon = TinyPictureVersion(Program.MyPath() + "\\wallpaper\\_CurrentPaper.bmp");

                FlickrNet.Person fuser;
                string notifyText = "";
                fuser = flickr.PeopleGetInfo(photos[chosePhoto].UserId);
                notifyText = fuser.UserName + ": " + photos[chosePhoto].Title;
                string description = fi.Description;
                string location = "\n";
                if (fi.Location != null)
                {
                    if (fi.Location.County != null)
                        location += fi.Location.County.Description + ", " + fi.Location.Country.Description;
                    else
                        location += fi.Location.Country.Description;
                }
                description = System.Web.HttpUtility.HtmlDecode(Regex.Replace(description, "<[^>]*>", ""));

                NotifyIconText = notifyText.Substring(0, Math.Min(63, notifyText.Length));
                NotifyIconBalloonTipText = fi.DateTaken.ToLongDateString() +
                    location + "\n" + description;
                NotifyIconBalloonTipTitle = photos[chosePhoto].Title;
                //notifyIcon1.Visible = true; //Always visible

            }

            wallpaper = null;
            flickr = null;
            photos = null;
            //notifyIcon1.Icon = WallpaperFlickr.Properties.Resources.flickr;
        }

        private void FailWithError(Exception ex)
        {
            NotifyIconText = ex.Message.Substring(0, Math.Min(ex.Message.Length, 63));
            NotifyIconIcon = WallpaperFlickr.Properties.Resources.flickrbad;
            IsNotifyFail = true;
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        extern static bool DestroyIcon(IntPtr handle);

        private Icon TinyPictureVersion(string p)
        {
            // Scale the main bitmap down into an icon-sized one
            Bitmap newImage = new Bitmap(16, 16);
            using (Graphics gr = Graphics.FromImage(newImage))
            {
                gr.SmoothingMode = SmoothingMode.AntiAlias;
                gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gr.PixelOffsetMode = PixelOffsetMode.HighQuality;

                //Image mainicon = Properties.Resources.flickr.ToBitmap;
                //Image mainicon = Properties.Resources.flickr.ToBitmap();
                Image waiticon = WallpaperFlickr.Properties.Resources.flickrwait.ToBitmap();
                gr.DrawImage(waiticon, new Rectangle(0, 0, 16, 16));

                Image original = Image.FromFile(p);
                //Rectangle srcbit = new Rectangle(original.Width / 4, original.Height / 4, original.Width / 2, original.Height / 2);
                Rectangle srcbit = new Rectangle(0, 0, original.Width, original.Height);
                gr.DrawImage(original, new Rectangle(6, 1, 9, 11), srcbit, GraphicsUnit.Pixel);

                // Copy the bottom line
                //gr.DrawImage(mainicon, new Rectangle(0, 13, 16, 3), new Rectangle(0, 26, 32, 6), GraphicsUnit.Pixel);
                // Copy the right hand side
                //gr.DrawImage(mainicon, new Rectangle(10, 0, 6, 13), new Rectangle(8, 0, 12, 26), GraphicsUnit.Pixel);

                //// Get an Hicon for myBitmap. 
                //IntPtr Hicon = newImage.GetHicon();
                //// Create a new icon from the handle. 
                //Icon newIcon = Icon.FromHandle(Hicon);
                ////Write Icon to File Stream
                //System.IO.FileStream fs = new System.IO.FileStream("c:\\temp.ico", System.IO.FileMode.OpenOrCreate);
                //newIcon.Save(fs);
                //fs.Close();
                //DestroyIcon(Hicon);
            }
            return FlimFlan.IconEncoder.Converter.BitmapToIcon(newImage);
            //newImage.Save("c:\\temp.bmp");
            //Icon retv = new Icon("c:\\temp2.ico");
            //return retv;
        }


        private FlickrNet.PhotoSearchSortOrder GetSortOrder()
        {
            switch (OrderBy)
            {
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

        private FlickrNet.TagMode GetTagMode()
        {
            switch (TagMode)
            {
                case "all": return FlickrNet.TagMode.AllTags;
                case "any": return FlickrNet.TagMode.AnyTag;
                default: return FlickrNet.TagMode.AnyTag;
            }
        }

        private winWallpaper.Style getDisplayStyle()
        {
            switch (Position.ToLower())
            {
                case "centered": return winWallpaper.Style.Centered;
                case "tiled": return winWallpaper.Style.Tiled;
                case "stretched": return winWallpaper.Style.Stretched;
                case "fill": return winWallpaper.Style.Fill;
                case "fit": return winWallpaper.Style.Fit;
                default: return winWallpaper.Style.Stretched;
            }
        }


        public bool IsNotifyFail { get; set; }

        public string NotifyIconBalloonTipText { get; set; }

        public string NotifyIconText { get; set; }

        public string NotifyIconBalloonTipTitle { get; set; }

        public Icon NotifyIconIcon { get; set; }
    }
}
