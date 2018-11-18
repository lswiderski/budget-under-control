
using Android.Content;
using Android.Content.Res;
using BudgetUnderControl.Common;
using BudgetUnderControl.Droid;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
[assembly: Dependency(typeof(FileHelper))]
namespace BudgetUnderControl.Droid
{
    public class FileHelper : IFileHelper
    {
        private string folderName = "BudgetUnderControl";

        private Context _context = Android.App.Application.Context;

        public string ReadFromAssetsAsString(string filename)
        {
            using (var asset = _context.Assets.Open(filename))
            using (var streamReader = new StreamReader(asset))
            {
                return streamReader.ReadToEnd();
            }
        }

        public string GetLocalFilePath(string filename)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(path, filename);
        }

        public string GetExternalFilePath(string filename)
        {
            string path = Android.OS.Environment.ExternalStorageDirectory.Path;
            return Path.Combine(path, this.folderName, filename);
        }

        private string GetExternalPathWithoutFileName()
        {
            string path = Android.OS.Environment.ExternalStorageDirectory.Path;
            return Path.Combine(path, this.folderName);
        }

        public bool CopyFile(string sourcePath, string destinationPath)
        {
            if (File.Exists(sourcePath))
            {
                File.Copy(sourcePath, destinationPath, true);
                return true;
            }

            return false;
        }

        public void SaveText(string filename, string[] lines)
        {
            var path = this.GetExternalPathWithoutFileName();
            var filePath = GetExternalFilePath(filename);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath))
            {
                foreach (string line in lines)
                {
                    file.WriteLine(line);
                }
            }
        }

        public void SaveText(string filename, string text)
        {
            var path = this.GetExternalPathWithoutFileName();
            var filePath = GetExternalFilePath(filename);
            if(!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath))
            {
                file.Write(text);
            }

            //System.IO.File.WriteAllText(filePath, text);
        }

        public string LoadText(string filename)
        {
            var filePath = GetExternalFilePath(filename);
            var content = "";
            if (File.Exists(filePath))
            {
                using (System.IO.StreamReader file = new System.IO.StreamReader(filePath))
                {
                    content = file.ReadToEnd();
                }
            }

            return content;
            //return System.IO.File.ReadAllText(filePath);
        }
    }
}
