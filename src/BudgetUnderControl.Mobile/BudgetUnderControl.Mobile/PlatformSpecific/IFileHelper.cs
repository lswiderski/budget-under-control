
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

        Task<string> SaveToLocalFolderAsync(Stream dataStream, string fileName);
        Task<string> SaveToLocalFolderAsync(byte[] dataBytes, string fileName);
        Task<Stream> LoadFileStreamAsync(string fileName);
        Task<byte[]> LoadFileBytesAsync(string fileName);
        Task TaskRemoveFileAsync(string fileName);
    }
}
