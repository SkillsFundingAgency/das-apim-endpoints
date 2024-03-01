using SFA.DAS.LevyTransferMatching.Api.Models.Shared;
using SFA.DAS.LevyTransferMatching.Models.ReferenceData;
using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Api.Models.Opportunity
{
    public class GetDetailResponse
    {
        public OpportunitySummary Opportunity { get; set; }
        public IEnumerable<ReferenceDataItem> Sectors { get; set; }
        public IEnumerable<ReferenceDataItem> JobRoles { get; set; }
        public IEnumerable<ReferenceDataItem> Levels { get; set; }
    }
}
