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
    }
}
