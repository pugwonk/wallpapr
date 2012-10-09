using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WallpaperFlickr
{
    static class FileSystem
    {
        public static string MyPath()
        {
            //return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WallpaperFlickr";
            if (Directory.Exists(path) == false)
                Directory.CreateDirectory(path);

            return path;
        }
    }
}
