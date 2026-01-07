using DVLD.EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.PresentationLayer.Globals
{
    public static class SharedGlobals
    {
        public static string ImagesRootDirectory = @"C:\DVLD-People-Images";

        public static User CurrentUser = null;

        public static bool DotEnvLoaded = false;

        public static bool IsLoggedIn()
        {
            return CurrentUser != null;
        }

        public static void Logout()
        {
            CurrentUser = null;
        }
    }
}
