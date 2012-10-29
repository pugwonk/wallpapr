using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using WallpaperFlickr.MicroMVVM;
using System.Reflection;
using Microsoft.Win32;

namespace WallpaperFlickr
{
    //Todo: Rename me - this is an awful name
    public class Form1ViewModel : INotifyPropertyChanged
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        extern static bool DestroyIcon(IntPtr handle);

        private bool _isNotifyFail;
        private Icon _notifyIconIcon;
        private string _notifyIconBalloonTipTitle;
        private string _notifyIconText;
        private string _notifyIconBalloonTipText;
        private WallpaperFlickrSettings _settings;

        private Timer _timer = new Timer { Interval = 60000 };

        public Form1ViewModel()
        {
            // _ is a variable name, it's becoming a standard for something that is ignored
            RotateNowCommand = new MicroCommand(
                       _ => true,
                       _ => this.GetNewWallpaper()
                   );

            OKCommand = new MicroCommand(
                        _ => true,
                        _ => this.DoOk()
                    );

            _settings = new WallpaperFlickrSettings();
            _settings.ReadSettings();

            _timer.Tick += _timer_Tick;
            _timer.Start();            
        }

        void _timer_Tick(object sender, EventArgs e)
        {
            SaveSettings();
            if (HasExpired())
            {
                GetNewWallpaper();
            }
        }

        private void DoOk()
        {
            SaveSettings();
            GetNewWallpaper();
        }

        public int Frequency
        {
            get { return _settings.Frequency; }
            set
            {
                _settings.Frequency = value;
                NotifyPropertyChanged("Frequency");
            }
        }

        public IEnumerable<string> Intervals
        {
            get
            {
                return new string[] { 
                    "minutes",
                    "hours",
                    "days",
                    "weeks",
                    "months"
                };
            }
        }

        //Todo: Introduce Enum, just like Position
        public int Interval
        {
            get
            {
                var count = 0;
                return (from x in Intervals
                        let index = count++
                        where String.Compare(x, _settings.Interval, true) == 0
                        select index).Single();
            }
            set
            {
                _settings.Interval = Intervals.Skip(value).First();
                NotifyPropertyChanged("Interval");
            }
        }

        public IEnumerable<string> OrderByOptions
        {
            get
            {
                return new[] 
                { 
                    "Newly posted",
                    "Most recently taken",
                    "Most interesting",
                    "None",
                    "Relevance" 
                };
            }
        }

        public int OrderByIndex
        {
            get
            {
                var index = 0;
                var result = (from x in OrderByOptions
                              let count = index++
                              where string.Compare(x, _settings.OrderBy, true) == 0
                              select count).Single();
                return result;
            }
            set
            {
                _settings.OrderBy = OrderByOptions.Skip(value).First();
                NotifyPropertyChanged("OrderBy");
            }
        }

        public string ApiKey
        {
            get { return _settings.ApiKey; }
            set
            {
                _settings.ApiKey = value;
                NotifyPropertyChanged("ApiKey");
            }
        }

        public IEnumerable<string> Positions
        {
            get
            {
                return Enum.GetNames(typeof(winWallpaper.Style));
            }
        }

        public int Position
        {
            get
            {
                return (int)getDisplayStyle();
            }
            set
            {
                _settings.Position = Enum.GetName(typeof(winWallpaper.Style), (winWallpaper.Style)value);
                NotifyPropertyChanged("Position");
            }
        }

        private winWallpaper.Style getDisplayStyle()
        {
            return (winWallpaper.Style)Enum.Parse(typeof(winWallpaper.Style), _settings.Position);
        }

        public string Tags
        {
            get { return _settings.Tags; }
            set
            {
                _settings.Tags = value;
                NotifyPropertyChanged("Tags");
            }
        }

        public string UserId
        {
            get { return _settings.UserId; }
            set
            {
                _settings.UserId = value;
                NotifyPropertyChanged("UserId");
            }
        }

        public string FaveUserId
        {
            get { return _settings.FaveUserId; }
            set
            {
                _settings.FaveUserId = value;
                NotifyPropertyChanged("FaveUserId");
            }
        }

        public bool PhotoSourceIsSearch
        {
            get { return _settings.SearchOrFaves == 0; }
            set
            {
                if (value)
                {
                    _settings.SearchOrFaves = 0;
                    NotifyPhotoSource();
                }
            }
        }

        private void NotifyPhotoSource()
        {
            NotifyPropertyChanged("PhotoSourceIsSearch"); ;
            NotifyPropertyChanged("PhotoSourceIsInteresting");
            NotifyPropertyChanged("PhotoSourceIsFavourites");
        }

        public bool PhotoSourceIsFavourites
        {
            get { return _settings.SearchOrFaves == 1; }
            set
            {
                if (value)
                {
                    _settings.SearchOrFaves = 1;
                    NotifyPhotoSource();
                }
            }
        }

        public bool PhotoSourceIsInteresting
        {
            get { return _settings.SearchOrFaves == 2; }
            set
            {
                if (value)
                {
                    _settings.SearchOrFaves = 2;
                    NotifyPhotoSource();
                }
            }
        }


        public bool StartWithWindows
        {
            get { return _settings.StartWithWindows; }
            set
            {
                _settings.StartWithWindows = value;
                NotifyPropertyChanged("StartWithWindows");
            }
        }

        public bool CachePics
        {
            get { return _settings.CachePics; }
            set
            {
                _settings.CachePics = value;
                NotifyPropertyChanged("CachePics");
            }
        }

        public bool ShowBubbles
        {
            get { return _settings.ShowBubbles; }
            set
            {
                _settings.ShowBubbles = value;
                NotifyPropertyChanged("ShowBubbles");
            }
        }

        public bool TagModeIsAll
        {
            get { return string.Compare(TagMode, "all", true) == 0; }
            set
            {
                if (value)
                {
                    TagMode = "all";
                    NotifyTagMode();
                }
            }
        }

        public bool TagModeIsAny
        {
            get { return !TagModeIsAll; }
            set
            {
                if (value)
                {
                    TagMode = "any";
                    NotifyTagMode();
                }
            }
        }


        private void NotifyTagMode()
        {
            NotifyPropertyChanged("TagModeIsAll");
            NotifyPropertyChanged("TagModeIsAny");
        }

        public string TagMode
        {
            get { return _settings.TagMode; }
            set
            {
                _settings.TagMode = value;
                NotifyPropertyChanged("TagMode");
            }
        }

        public WallpaperFlickrSettings Settings { get { return _settings; } }

        internal bool HasExpired()
        {
            return _settings.HasExpired();
        }

        internal void SaveSettings()
        {
            _settings.SaveSettings();

            RegistryKey myKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (StartWithWindows)
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

        }

        public string WebURL
        {
            get { return _settings.WebURL; }
            set
            {
                _settings.WebURL = value;
                NotifyPropertyChanged("WebURL");
            }
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

            switch (_settings.SearchOrFaves)
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
                NotifyIconIcon = TinyPictureVersion(FileSystem.MyPath() + "\\wallpaper\\_CurrentPaper.bmp");

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

                if (ShowBubbles)
                    NotifyPropertyChanged("PopupBalloon");

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
            if (ShowBubbles)
                NotifyPropertyChanged("PopupBalloon");
        }

        public string Version
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

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
            switch (_settings.OrderBy)
            {
                case "Newly Posted":
                    return FlickrNet.PhotoSearchSortOrder.DatePostedDescending;
                case "Most Recently Taken":
                    return FlickrNet.PhotoSearchSortOrder.DateTakenDescending;
                case "Most Interesting":
                    return FlickrNet.PhotoSearchSortOrder.InterestingnessDescending;
                case "None":
                    return FlickrNet.PhotoSearchSortOrder.None;
                case "Relevance":
                    return FlickrNet.PhotoSearchSortOrder.Relevance;
                default:
                    //Erm this doesn't match a current option????
                    return FlickrNet.PhotoSearchSortOrder.InterestingnessAscending;
            }
        }

        private FlickrNet.TagMode GetTagMode()
        {
            switch (TagMode)
            {
                case "all": return FlickrNet.TagMode.AllTags;
                case "any":
                default: return FlickrNet.TagMode.AnyTag;
            }
        }

        public bool IsNotifyFail
        {
            get { return _isNotifyFail; }
            set
            {
                _isNotifyFail = value;
                NotifyPropertyChanged("IsNotifyFail");
            }
        }

        public string NotifyIconBalloonTipText
        {
            get { return _notifyIconBalloonTipText; }
            set
            {
                _notifyIconBalloonTipText = value;
                NotifyPropertyChanged("NotifyIconBalloonTipText");
            }
        }

        public string NotifyIconText
        {
            get { return _notifyIconText; }
            set
            {
                _notifyIconText = value;
                NotifyPropertyChanged("NotifyIconText");
            }
        }

        public string NotifyIconBalloonTipTitle
        {
            get { return _notifyIconBalloonTipTitle; }
            set
            {
                _notifyIconBalloonTipTitle = value;
                NotifyPropertyChanged("NotifyIconBalloonTipTitle");
            }
        }

        public Icon NotifyIconIcon
        {
            get { return _notifyIconIcon; }
            set
            {
                _notifyIconIcon = value;
                NotifyPropertyChanged("NotifyIconIcon");
            }
        }

        public void GotoFlickrURL()
        {
            System.Diagnostics.Process.Start(WebURL);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            var temp = PropertyChanged;
            if (temp != null)
            {
                temp(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public MicroCommand RotateNowCommand { get; set; }

        public MicroCommand OKCommand { get; set; }
    }
}
