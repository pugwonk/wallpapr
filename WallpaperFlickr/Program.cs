using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WallpaperFlickr {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        public static string MyPath()
        {
            //return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WallpaperFlickr";
        }


    }
}