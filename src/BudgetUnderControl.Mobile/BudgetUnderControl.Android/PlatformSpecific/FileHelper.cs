using Android.Content;
using Android.Content.Res;
using BudgetUnderControl.Droid;
using BudgetUnderControl.Mobile;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: Dependency(typeof(FileHelper))]
namespace BudgetUnderControl.Droid
{
    public class FileHelper : IFileHelper
    {
        private string folderName = "BudgetUnderControl";
        private readonly string LocalFolder;

        private Context _context = Android.App.Application.Context;

        public FileHelper()
        {
            LocalFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),"files");

            if (!Directory.Exists(LocalFolder))
            {
                Directory.CreateDirectory(LocalFolder);
            }
        }

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

        public async Task<string> SaveToLocalFolderAsync(byte[] dataBytes, string fileName)
        {
            return await Task.Run(() =>
            {
                // Use Combine so that the correct file path slashes are used
                var filePath = Path.Combine(LocalFolder, fileName);

                if (File.Exists(filePath))
                    File.Delete(filePath);

                File.WriteAllBytes(filePath, dataBytes);

                return filePath;
            });
        }

        public async Task<byte[]> LoadFileBytesAsync(string fileName)
        {
            var filePath = Path.Combine(LocalFolder, fileName);
            if (File.Exists(filePath))
            {
                return await Task.Run(() => File.ReadAllBytes(filePath));
            }
            else
            {
                return null;
            }
        }

        public async Task<string> SaveToLocalFolderAsync(Stream dataStream, string fileName)
        {
            // Use Combine so that the correct file path slashes are used
            var filePath = Path.Combine(LocalFolder, fileName);

            if (File.Exists(filePath))
                File.Delete(filePath);

            using (var fileStream = File.OpenWrite(filePath))
            {
                if (dataStream.CanSeek)
                    dataStream.Position = 0;

                await dataStream.CopyToAsync(fileStream);

                return filePath;
            }
        }

        public async Task<Stream> LoadFileStreamAsync(string fileName)
        {
            var filePath = Path.Combine(LocalFolder, fileName);

            return await Task.Run(() =>
            {
                if (File.Exists(filePath))
                {
                    using (var fileStream = File.OpenRead(filePath))
                    {
                        return fileStream;
                    }
                }
                else
                {
                    return null;
                }
               
            });
        }

        public async Task TaskRemoveFileAsync(string fileName)
        {
            var filePath = Path.Combine(LocalFolder, fileName);

            if (File.Exists(filePath))
            {
                 await Task.Run(() => File.Delete(filePath));
            }

        }
    }
}
