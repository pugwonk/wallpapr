using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
