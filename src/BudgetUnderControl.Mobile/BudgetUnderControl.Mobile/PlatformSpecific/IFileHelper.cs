
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BudgetUnderControl.Mobile
{
    public interface IFileHelper
    {
        string GetLocalFilePath(string filename);
        string GetExternalFilePath(string filename);
        void SaveText(string filename, string text);
        string LoadText(string filename);
        void SaveText(string filename, string[] lines);
        bool CopyFile(string sourcePath, string destinationPath);
        string ReadFromAssetsAsString(string filename);
        string SaveImageToFile(ImageSource imgSrc, string path, string filename);
        ImageSource GetImageSourceFromFile(string path, string filename);
        Task<bool> SaveImageSourceToFile(ImageSource img, string directory, string filename);
    }
}
