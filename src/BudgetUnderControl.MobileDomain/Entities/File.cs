using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.MobileDomain
{
    public class File : ISyncable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string FileName { get; set; }

        public DateTime CreatedOn { get; set; }

        public string MimeType { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string ExternalId { get; set; }

        public bool IsDeleted { get; set; }

        public ICollection<FileToTransaction> FileToTransactions { get; set; }

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
