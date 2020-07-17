using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BudgetUnderControl.MobileDomain
{
    public class FileToTransaction : ISyncable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int FileId { get; set; }
        public int TransactionId { get; set; }

        public File File { get; set; }
        public Transaction Transaction { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string ExternalId { get; set; }

        public bool IsDeleted { get; set; }

        public void Delete(bool delete = true)
        {
            this.IsDeleted = delete;
            this.UpdateModify();
        }

        public void UpdateModify()
        {
            this.SetModifiedOn(DateTime.UtcNow);
        }

        /// <summary>
        /// Use for sync/imports
        /// </summary>
        /// <param name="date"></param>
        public void SetModifiedOn(DateTime? date)
        {
            this.ModifiedOn = date;
        }

    }
}
