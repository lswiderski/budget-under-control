using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetUnderControl.MobileDomain
{
    public interface ISyncable
    {
        DateTime? ModifiedOn { get; }
        string ExternalId { get; }
        bool IsDeleted { get; }

        void UpdateModify();
        void Delete(bool delete = true);
    }
}
