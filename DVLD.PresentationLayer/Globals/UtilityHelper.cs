using DVLD.PresentationLayer.People;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.PresentationLayer.Globals
{
    internal static class UtilityHelper
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

            EnsureDirectoryExists(SharedGlobals.ImagesRootDirectory);

            string extension = Path.GetExtension(sourceFilePath);
            string fileName = $"{NewGuidString()}{extension}";
            string destinationPath = Path.Combine(SharedGlobals.ImagesRootDirectory, fileName);

            File.Copy(sourceFilePath, destinationPath, overwrite: true);

            return fileName; // store this in DB
        }

        public static bool DeleteImageFromDirectory(string fileName)
        {
            string fullPath = Path.Combine(SharedGlobals.ImagesRootDirectory, fileName);

            if (!File.Exists(fullPath))
                return true; // Person's old image doesn't exists, accept as deleted

            try
            {
                File.Delete(Path.Combine(SharedGlobals.ImagesRootDirectory, fileName));
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex);
                return false;
            }
        }
    }
}
