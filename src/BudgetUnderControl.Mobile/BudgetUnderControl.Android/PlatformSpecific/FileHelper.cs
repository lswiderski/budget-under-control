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

        public string SaveImageToFile(ImageSource imgSrc, string path, string filename)
        {
            string localPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

            string fileDirectory = Path.Combine(localPath, path);
            if(!Directory.Exists(fileDirectory))
            {
                Directory.CreateDirectory(fileDirectory);
            }

            string jpgFilename = Path.Combine(fileDirectory, filename);
            if (File.Exists(jpgFilename))
            {
                File.Delete(jpgFilename);
            }

            ImageToFileAsync(imgSrc, jpgFilename);

            return jpgFilename;
        }

        public ImageSource GetImageSourceFromFile(string path, string filename)
        {
            string localPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            string jpgFilename = Path.Combine(localPath, path, filename);

            var imgSource = ImageSource.FromFile(jpgFilename);

            return imgSource;

        }

        private async void ImageToFileAsync(ImageSource imgSrc, string jpgFilename)
        {
            var photo = await this.GetBitmapFromImageSourceAsync(imgSrc, _context);

            using (FileStream fs = new FileStream(jpgFilename, FileMode.OpenOrCreate))
            {
                if(photo != null)
                {
                    photo.Compress(Android.Graphics.Bitmap.CompressFormat.Jpeg, 100, fs);
                }
                
            }
        }

        public async Task<bool> SaveImageSourceToFile(ImageSource img, string directory, string filename)
        {
            System.IO.Stream outputStream = null;

            var renderer = GetHandler(img);
            Android.Graphics.Bitmap photo = await renderer.LoadImageAsync(img, Forms.Context);
            string localPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var savedImageFilename = System.IO.Path.Combine(localPath, directory, filename);

            System.IO.Directory.CreateDirectory(System.IO.Path.Combine(localPath, directory));

            bool success = false;
            using (outputStream = new System.IO.FileStream(savedImageFilename, System.IO.FileMode.Create))
            {
                //if (System.IO.Path.GetExtension(filename).ToLower() == ".png")
                    //success = await photo.CompressAsync(Android.Graphics.Bitmap.CompressFormat.Png, 100, outputStream);
               // else
                    success = await photo.CompressAsync(Android.Graphics.Bitmap.CompressFormat.Jpeg, 100, outputStream);
            }

            return success;
        }


        public async Task<Android.Graphics.Bitmap> GetBitmapFromImageSourceAsync(ImageSource source, Context context)
        {
            var handler = GetHandler(source);
            var returnValue = (Android.Graphics.Bitmap)null;
            returnValue = await handler.LoadImageAsync(source, context);
            return returnValue;
        }


        public IImageSourceHandler GetHandler(ImageSource source)
        {
            //Image source handler to return 
            IImageSourceHandler returnValue = null;
            //check the specific source type and return the correct image source handler 
            if (source is UriImageSource)
            {
                returnValue = new ImageLoaderSourceHandler();
            }
            else if (source is FileImageSource)
            {
                returnValue = new FileImageSourceHandler();
            }
            else if (source is StreamImageSource)
            {
                returnValue = new StreamImagesourceHandler();
            }
            return returnValue;
        }
    }
}
