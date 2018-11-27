using BudgetUnderControl.Common;
using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.Common.Enums;
using BudgetUnderControl.Domain;
using BudgetUnderControl.Domain.Repositiories;
using BudgetUnderControl.Infrastructure.Commands;
using BudgetUnderControl.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Mobile.Services
{
    public class SyncMobileService : ISyncMobileService
    {
        private static string BACKUP_FILE_NAME = "buc_backup.json";

        private readonly IFileHelper fileHelper;
        private readonly ISyncService syncService;
        private readonly ITransactionRepository transactionRepository;
        private readonly ISyncRequestBuilder syncRequestBuilder;
        private readonly ISynchroniser synchroniser;
        public SyncMobileService(IFileHelper fileHelper,
            ISyncService syncService,
            ITransactionRepository transactionRepository,
            ISyncRequestBuilder syncRequestBuilder)
        {
            this.fileHelper = fileHelper;
            this.syncService = syncService;
            this.transactionRepository = transactionRepository;
            this.syncRequestBuilder = syncRequestBuilder;
        }


        public async Task<string> GetBackUpJSONAsync()
        {
            var backUp = await this.syncService.GetBackUpAsync();

            string output = Newtonsoft.Json.JsonConvert.SerializeObject(backUp);
            return output;
        }

        private async Task ImportBackUpJSONAsync(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return;
            }
            try
            {
                var deserialized = Newtonsoft.Json.JsonConvert.DeserializeObject<BackUpDTO>(json);

                await syncService.ImportBackUpAsync(deserialized);
            }
            catch (Exception e)
            {
                //Problem with JSON file
                throw;
            }

        }

        public async Task SaveBackupFileAsync()
        {
            var json = await GetBackUpJSONAsync();

            fileHelper.SaveText(BACKUP_FILE_NAME, json);
        }

        public async Task LoadBackupFileAsync()
        {
            var json = fileHelper.LoadText(BACKUP_FILE_NAME);
            await ImportBackUpJSONAsync(json);
        }

        public async Task ExportCSVAsync()
        {
            var fileName = string.Format("{0}_{1}.txt", "buc", DateTime.UtcNow.Ticks);
            var lines = await syncService.GenerateCSV();
            fileHelper.SaveText(fileName, lines.ToArray());
        }

        public async Task ExportDBAsync()
        {
            //temporary
            ExtractDB();
        }

        public void ExtractDB()
        {
            var sourcePath = fileHelper.GetLocalFilePath(Settings.DB_SQLite_NAME);

            var databaseBackupPath = fileHelper.GetExternalFilePath(string.Format("{0}_{1}.db3", "buc_Backup", DateTime.UtcNow.Ticks));
            fileHelper.CopyFile(sourcePath, databaseBackupPath);

        }

        public async Task SyncAsync()
        {
            //get request
            var syncRequestDto = await this.syncRequestBuilder.CreateSyncRequestAsync(SynchronizationComponent.Mobile, SynchronizationComponent.Api);

            //call api
            var apiResponse = new SyncRequest();
            //do sync with responsedto
            await this.synchroniser.SynchroniseAsync(apiResponse);
        }
    }
}
