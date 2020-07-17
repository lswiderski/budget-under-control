using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.MobileDomain.Repositiories
{
    public interface IFileRepository
    {
        Task AddAsync(File file);
        Task AddAsync(List<File> files);
        Task<ICollection<File>> GetAsync();
        Task<ICollection<File>> GetAsync(List<int> fileIds);
        Task<ICollection<File>> GetAsync(List<string> fileIds);
        Task<File> GetAsync(int id);
        Task<File> GetAsync(string id);
        Task UpdateAsync(File file);
        Task RemoveAsync(File file);
        Task RemoveAsync(IEnumerable<File> files);

        Task AddAsync(FileToTransaction fileToTransaction);
        Task AddAsync(IEnumerable<FileToTransaction> filesToTransaction);
        Task<ICollection<FileToTransaction>> GetFileToTransactionsAsync();
        Task<ICollection<FileToTransaction>> GetFileToTransactionsAsync(int transactionId);
        Task<FileToTransaction> GetFileToTransactionAsync(int fileToTransactionId);
        Task UpdateAsync(FileToTransaction fileToTransaction);
        Task RemoveAsync(FileToTransaction fileToTransaction);
        Task RemoveAsync(IEnumerable<FileToTransaction> filesToTransaction);
    }
}
