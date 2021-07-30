using SFA.DAS.LevyTransferMatching.Api.Models.Shared;
using SFA.DAS.LevyTransferMatching.Models.ReferenceData;
using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Api.Models.Opportunity
{
    public class GetSectorResponse
    {
        public IEnumerable<ReferenceDataItem> Sectors { get; set; }
        public OpportunitySummary Opportunity { get; set; }
        public string Location { get; set; }
    }
}