using System.Collections.Generic;
using SFA.DAS.LevyTransferMatching.Models.ReferenceData;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetSector
{
    public class GetSectorQueryResult
    {
        public IEnumerable<ReferenceDataItem> Sectors { get; set; }
    }
}