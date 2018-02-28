using BudgetUnderControl.Common;
using BudgetUnderControl.Droid;
using System;
using System.IO;
using Xamarin.Forms;
[assembly: Dependency(typeof(FileHelper))]
namespace BudgetUnderControl.Droid
{
    public class FileHelper : IFileHelper
    {
        public string GetLocalFilePath(string filename)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(path, filename);
        }

        public string GetExternalFilePath(string filename)
        {
            string path = Android.OS.Environment.ExternalStorageDirectory.Path;
            return Path.Combine(path, filename);
        }

        public void SaveText(string filename, string[] lines)
        {
            var filePath = GetLocalFilePath(filename);
            using (System.IO.StreamWriter file =
           new System.IO.StreamWriter(filePath))
            {
                foreach (string line in lines)
                {
                    file.WriteLine(line);
                }
            }
        }

        public void SaveText(string filename, string text)
        {
            var filePath = GetLocalFilePath(filename);
            System.IO.File.WriteAllText(filePath, text);
        }
        public string LoadText(string filename)
        {
            var filePath = GetLocalFilePath(filename);
            return System.IO.File.ReadAllText(filePath);
        }

        public void SaveTextExternal(string filename, string text)
        {
            var filePath = GetExternalFilePath(filename);
            System.IO.File.WriteAllText(filePath, text);
        }

        public void SaveTextExternal(string filename, string[] lines)
        {
            var filePath = GetExternalFilePath(filename);
            using (System.IO.StreamWriter file =
           new System.IO.StreamWriter(filePath))
            {
                foreach (string line in lines)
                {
                    file.WriteLine(line);
                }
            }
        }

        public string LoadTextExternal(string filename)
        {
            var filePath = GetExternalFilePath(filename);
            return System.IO.File.ReadAllText(filePath);
        }
    }
}
