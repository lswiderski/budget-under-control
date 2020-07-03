using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.CommonInfrastructure;
using BudgetUnderControl.CommonInfrastructure.Settings;
using BudgetUnderControl.Domain;
using BudgetUnderControl.Infrastructure;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.ApiInfrastructure.Services
{
    public class FileService : BaseModel, IFileService
    {
        private string _uploadCatalog = "uploads";

        private readonly IUserIdentityContext userIdentityContext;
        private readonly GeneralSettings settings;

        public FileService(IContextFacade context, IUserIdentityContext userIdentityContext, GeneralSettings settings) : base(context)
        {
            this.userIdentityContext = userIdentityContext;
            this.settings = settings;
        }
        public async Task<string> SaveFileAsync(IFormFile file)
        {
            var createdOn = DateTime.UtcNow;
            var rootPath = settings.FileRootPath;
            var uploadsRootFolder = Path.Combine(rootPath, _uploadCatalog, userIdentityContext.ExternalId.ToString(), createdOn.Year.ToString(), createdOn.Month.ToString());
            if (!Directory.Exists(uploadsRootFolder))
            {
                Directory.CreateDirectory(uploadsRootFolder);
            }

            var id = Guid.NewGuid();

            var fileEntity = new Domain.File
            {
                ContentType = file.ContentType,
                FileName = file.FileName,
                UserId = userIdentityContext.ExternalId,
                CreatedOn = createdOn,
                ExternalId = id,
                Id = id,
            };

            this.Context.Files.Add(fileEntity);
            await this.Context.SaveChangesAsync();
            fileEntity.ExternalId = fileEntity.Id;
            await this.Context.SaveChangesAsync();

            var filePath = Path.Combine(uploadsRootFolder, fileEntity.Id.ToString());
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream).ConfigureAwait(false);
            }

            return fileEntity.Id.ToString();
        }

        public async Task<FileDto> GetFileAsync(Guid id, string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return null;
            }
            var rootPath = settings.FileRootPath;
            var fileEntity = this.Context.Files.Where(x => x.Id == id).FirstOrDefault();

            if(fileEntity != null)
            {
                var filePath = Path.Combine(rootPath, _uploadCatalog, userIdentityContext.ExternalId.ToString(), fileEntity.CreatedOn.Year.ToString(), fileEntity.CreatedOn.Month.ToString(), id.ToString());

                var file = new FileDto
                {
                    ContentType = fileEntity.ContentType,
                    Name = fileEntity.FileName,
                    Id = id,
                    FilePath = filePath,
                };
                return file;
            }

            return null;
          
        }

        public async Task RemoveFileAsync(Guid id)
        {
            //check if user hass access

            //remove connections with other objects like transactions
            var fileEntity = this.Context.Files.Where(x => x.Id == id).FirstOrDefault();

            if(fileEntity == null)
            {
                return;
            }

            if(fileEntity.UserId != userIdentityContext.ExternalId)
            {
                throw new UnauthorizedAccessException();
            }
            var f2t = this.Context.FilesToTransactions.Where(x => x.FileId == id).ToList();
            f2t.ForEach(x => x.Delete());
            fileEntity.Delete();
            await this.Context.SaveChangesAsync();

            var rootPath = settings.FileRootPath;
            var filePath = Path.Combine(rootPath, _uploadCatalog, userIdentityContext.ExternalId.ToString(), fileEntity.CreatedOn.Year.ToString(), fileEntity.CreatedOn.Month.ToString(), id.ToString());
            System.IO.File.Delete(filePath);
        }
    }
}
