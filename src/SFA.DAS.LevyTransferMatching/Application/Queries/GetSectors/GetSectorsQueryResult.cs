using System.Collections.Generic;
using SFA.DAS.LevyTransferMatching.Models.Tags;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetSectors
{
    public class GetSectorsQueryResult
    {
        public IEnumerable<ReferenceDataItem> ReferenceDataItems { get; set; }
    }
}
