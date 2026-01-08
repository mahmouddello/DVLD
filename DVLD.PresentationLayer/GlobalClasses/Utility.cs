using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.PresentationLayer.GlobalClasses
{
    public static class Utility
    {
        private static string NewGuidString()
        {
            return Guid.NewGuid().ToString();
        }

        private static void EnsureDirectoryExists(string directoryPath)
        {
            if (string.IsNullOrWhiteSpace(directoryPath))
                throw new ArgumentException("Directory path is invalid.");

            Directory.CreateDirectory(directoryPath);
        }

        public static string CopyImageToDirectory(string sourceFilePath)
        {
            if (!File.Exists(sourceFilePath))
                throw new FileNotFoundException("Source image not found.", sourceFilePath);

            EnsureDirectoryExists(Globals.ImagesRootDirectory);

            string extension = Path.GetExtension(sourceFilePath);
            string fileName = $"{NewGuidString()}{extension}";
            string destinationPath = Path.Combine(Globals.ImagesRootDirectory, fileName);

            File.Copy(sourceFilePath, destinationPath, overwrite: true);

            return fileName; // store this in DB
        }

        public static bool DeleteImageFromDirectory(string fileName)
        {
            string fullPath = Path.Combine(Globals.ImagesRootDirectory, fileName);

            if (!File.Exists(fullPath))
                return true; // Person's old image doesn't exists, accept as deleted

            try
            {
                File.Delete(Path.Combine(Globals.ImagesRootDirectory, fileName));
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex);
                return false;
            }
        }

        public static void PlayBeepSound()
        {
            System.Media.SystemSounds.Beep.Play();
        }

        public static void HandleWrongKey(KeyPressEventArgs e)
        {
            PlayBeepSound();
            e.Handled = true;
        }

        public static bool IsLoggedIn()
        {
            return Globals.CurrentUser != null;
        }

        public static bool Logout()
        {
            if (IsLoggedIn())
            {
                Globals.CurrentUser = null;
                return true;
            }

            return false;
        }
    }
}
