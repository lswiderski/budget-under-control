using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetUnderControl.Domain
{
    public interface ISyncable
    {
        DateTime? ModifiedOn { get; }
        Guid ExternalId { get; }
    }
}
