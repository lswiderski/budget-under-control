
using System.Threading.Tasks;

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
    }
}
