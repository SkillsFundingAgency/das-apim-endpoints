using SFA.DAS.LevyTransferMatching.Models.ReferenceData;
using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Opportunity.GetSector
{
    public class GetSectorQueryResult
    {
        public IEnumerable<ReferenceDataItem> Sectors { get; set; }
        public string Location { get; set; }
    }
}
