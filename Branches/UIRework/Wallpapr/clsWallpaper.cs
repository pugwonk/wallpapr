using System;

using System.Net;
using System.Drawing;
using System.IO;
using WallpaperFlickr;


public class clsWallpaper {

    public clsWallpaper() {
    }

	private String _URL;
    public String URL {
        get { return _URL; }
        set { _URL = value; }
    }

	public void AddRecord() {
        try
        {
            StreamWriter sw = new StreamWriter(FileSystem.MyPath()+ "\\wallpapers.db", true, System.Text.Encoding.UTF8);
            sw.WriteLine(URL);
            sw.Dispose();
        }
        catch
        {
        }
    }

    public bool AlreadyDownloaded() {
        return AlreadyDownloaded(_URL);
    }

    public bool AlreadyDownloaded(string url) {
        bool returnvalue = false;
        if (File.Exists(FileSystem.MyPath()+ "\\wallpapers.db"))
        {
            StreamReader sr = new StreamReader(FileSystem.MyPath()+ "\\wallpapers.db", System.Text.Encoding.UTF8);
            while (sr.Peek() >= 0) {
                if (sr.ReadLine().ToLower().Trim() == url.ToLower().Trim()) {
                    returnvalue = true;
                    break;
                }
            }
            sr.Dispose();
        }
        return returnvalue;
    }

    public bool Load(string fname, WallpaperFlickr.WallpaperFlickrSettings settings, 
        WallpaperFlickr.winWallpaper.Style sty, string path, string webpath)
    {
        bool Displayable = false;
        URL = fname;
        bool DownloadedOK = true;
        int spot = URL.LastIndexOf("/");
        string FileName = URL.Substring(spot + 1, URL.Length - spot - 1);
        if (!AlreadyDownloaded())
        {
            if (!Directory.Exists(FileSystem.MyPath()+ "\\wallpaper"))
            {
                Directory.CreateDirectory(FileSystem.MyPath()+ "\\wallpaper");
            }
            if (!settings.CachePics)
            {
                // Empty the cache folder if we're not supposed to be caching
                DirectoryInfo dir = new DirectoryInfo(FileSystem.MyPath()+ "\\wallpaper");
                dir.Delete(true);
                Directory.CreateDirectory(FileSystem.MyPath()+ "\\wallpaper");
            } 

            WebClient wc = new WebClient();
            try
            {
                
                wc.DownloadFile(URL, FileSystem.MyPath()+ "\\wallpaper\\" + FileName);
            }
            catch
            {
                DownloadedOK = false;
            }
            wc.Dispose();
        }

        if (DownloadedOK)
        {
            FileInfo fi = new FileInfo(FileSystem.MyPath()+ "\\wallpaper\\" + FileName);
            if (!fi.Exists) // for some reason this sometimes doesn't appear
                DownloadedOK = false;
            else
            {
                //don't save those stupid photo not available images
                if (fi.Length > 3500)
                {
                    Bitmap bitmap = new Bitmap(FileSystem.MyPath()+ "\\wallpaper\\" + FileName);
                    bitmap.Save(FileSystem.MyPath()+ "\\wallpaper\\_CurrentPaper.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
                    bitmap.Dispose();

                    AddRecord();
                    settings.LastChange = DateTime.Now;
                    settings.WebURL = webpath;

                    //spot = path.LastIndexOf("\\");
                    //string appPath = path.Substring(0, spot);

                    WallpaperFlickr.winWallpaper.ChangeWallpaper(FileSystem.MyPath()+ "\\wallpaper\\_CurrentPaper.bmp", sty);
                    Displayable = true;
                }
                else
                {
                    // CLR 2010-06-22: I don't think we actually do want to save records for the broken ones
                    //wallpaper.AddRecord();
                    File.Delete(FileSystem.MyPath()+ "\\wallpaper\\" + FileName);
                    Displayable = false;
                }
            }
            fi = null;
        }
        return (DownloadedOK && Displayable);
    }

}
