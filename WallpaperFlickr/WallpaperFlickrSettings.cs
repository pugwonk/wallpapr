using System;
using System.Xml;
using System.IO;
using System.Windows.Forms;

namespace WallpaperFlickr {
    public class WallpaperFlickrSettings {

        private bool Dirty = false;

        private string _ApiKey = "";
        public string ApiKey {
            get { return _ApiKey; }
            set { if (_ApiKey != value) { _ApiKey = value; Dirty = true; } }
        }

        private bool _SearchOrFaves = false;
        public bool SearchOrFaves
        {
            get { return _SearchOrFaves; }
            set { if (_SearchOrFaves != value) { _SearchOrFaves = value; Dirty = true; } }
        }

        private bool _StartWithWindows = false;
        public bool StartWithWindows
        {
            get { return _StartWithWindows; }
            set { if (_StartWithWindows != value) { _StartWithWindows = value; Dirty = true; } }
        }

        private string _UserId = "";
        public string UserId {
            get { return _UserId; }
            set { if (_UserId != value) { _UserId = value; Dirty = true; } }
        }

        private string _FaveUserId = "";
        public string FaveUserId
        {
            get { return _FaveUserId; }
            set { if (_FaveUserId != value) { _FaveUserId = value; Dirty = true; } }
        }

        private string _Tags = "";
        public string Tags {
            get { return _Tags; }
            set { if (_Tags != value) { _Tags = value; Dirty = true; } }
        }

        private string _Interval = "";
        public string Interval {
            get { return _Interval; }
            set { if (_Interval != value) { _Interval = value; Dirty = true; } }
        }

        private string _OrderBy = "";
        public string OrderBy {
            get { return _OrderBy; }
            set { if (_OrderBy != value) { _OrderBy = value; Dirty = true; } }
        }

        private string _TagMode = "";
        public string TagMode {
            get { return _TagMode; }
            set { if (_TagMode != value) { _TagMode = value; Dirty = true; } }
        }

        private string _Position = "";
        public string Position {
            get { return _Position; }
            set { if (_Position != value) { _Position = value; Dirty = true; } }
        }

        private DateTime _LastChange = DateTime.MinValue;
        public DateTime LastChange {
            get { return _LastChange; }
            set { if (_LastChange != value) { _LastChange = value; Dirty = true; } }
        }

        private string _WebURL = "";
        public string WebURL
        {
            get { return _WebURL; }
            set { if (_WebURL != value) { _WebURL = value; Dirty = true; } }
        }

        private int _Frequency = 1;
        public int Frequency {
            get { return _Frequency; }
            set { if (_Frequency != value) { _Frequency = value; Dirty = true; } }
        }

        public void ReadSettings() {
            XmlDocument xml = new XmlDocument();
            xml.Load(Program.MyPath() + "\\WallpaperFlickrSettings.xml");
            _ApiKey = xml.GetElementsByTagName("apikey").Item(0).InnerText;
            if (xml.GetElementsByTagName("userid").Count > 0)
                _UserId = xml.GetElementsByTagName("userid").Item(0).InnerText;
            if (xml.GetElementsByTagName("faveuserid").Count > 0)
                _FaveUserId = xml.GetElementsByTagName("faveuserid").Item(0).InnerText;
            if (xml.GetElementsByTagName("searchorfaves").Count > 0)
                _SearchOrFaves = bool.Parse(xml.GetElementsByTagName("searchorfaves").Item(0).InnerText);
            _Interval = xml.GetElementsByTagName("interval").Item(0).InnerText;
            _Tags = xml.GetElementsByTagName("tags").Item(0).InnerText;
            if (xml.GetElementsByTagName("tagmode").Count > 0)
                _TagMode = xml.GetElementsByTagName("tagmode").Item(0).InnerText;
            if (xml.GetElementsByTagName("orderby").Count > 0)
                _OrderBy = xml.GetElementsByTagName("orderby").Item(0).InnerText;
            try {
                _Position = xml.GetElementsByTagName("position").Item(0).InnerText;
            } catch (Exception) {
                _Position = "Stretched";
            }
            try
            {
                _StartWithWindows = bool.Parse(xml.GetElementsByTagName("startwithwin").Item(0).InnerText);
            }
            catch (Exception)
            {
                _Position = "Stretched";
            }
            try
            {
                _Frequency = Convert.ToInt32(xml.GetElementsByTagName("frequency").Item(0).InnerText);
            } catch (Exception) {
                _Frequency = 1;
            }
            try {
                _LastChange = Convert.ToDateTime(xml.GetElementsByTagName("lastchange").Item(0).InnerText);
            } catch (Exception) {
                _LastChange = DateTime.MinValue;
            }
            xml = null;
        }

        public void SaveSettings() {
            if (Dirty)
            {
                XmlTextWriter tw = new XmlTextWriter(Program.MyPath() + "\\WallpaperFlickrSettings.xml", System.Text.Encoding.UTF8);
                tw.Formatting = Formatting.Indented;
                tw.WriteStartDocument(false);
                tw.WriteStartElement("settings");
                tw.WriteElementString("lastchange", _LastChange.ToString());
                tw.WriteElementString("frequency", _Frequency.ToString());
                tw.WriteElementString("interval", _Interval);
                tw.WriteElementString("tags", _Tags.Trim());
                tw.WriteElementString("apikey", _ApiKey.Trim());
                tw.WriteElementString("userid", _UserId.Trim());
                tw.WriteElementString("searchorfaves", _SearchOrFaves.ToString());
                tw.WriteElementString("faveuserid", _FaveUserId.Trim());
                tw.WriteElementString("tagmode", _TagMode);
                tw.WriteElementString("orderby", _OrderBy);
                tw.WriteElementString("position", _Position);
                tw.WriteElementString("startwithwin", _StartWithWindows.ToString());
                tw.WriteEndElement();
                tw.WriteEndDocument();
                tw.Close();
                tw = null;
                Dirty = false;
            }
        }

        public bool HasExpired() {
            double multiplier = 0;
            switch (_Interval.ToLower()) {
                case "minutes":
                    multiplier = 1;
                    break;
                case "hours":
                    multiplier = 60;
                    break;
                case "days":
                    multiplier = 60 * 24;
                    break;
                case "weeks":
                    multiplier = 60 * 24 * 7;
                    break;
                case "months":
                    multiplier = 60 * 24 * 7 * 30;
                    break;
                default:
                    break;
            }
            double change = multiplier * _Frequency;
            if (_LastChange.AddMinutes(change) < DateTime.Now) {
                return true;
            } else {
                return false;
            }
        }
    }
}
