using BudgetUnderControl.Common.Contracts;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.ApiInfrastructure.Services
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile file);

        Task<FileDto> GetFileAsync(Guid id, string token);

        Task RemoveFileAsync(Guid id);
    }
}
