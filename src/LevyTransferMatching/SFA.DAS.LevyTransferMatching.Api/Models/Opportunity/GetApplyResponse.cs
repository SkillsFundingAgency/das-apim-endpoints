using SFA.DAS.LevyTransferMatching.Api.Models.Shared;
using SFA.DAS.LevyTransferMatching.Models.ReferenceData;
using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Api.Models.Opportunity
{
    public class GetApplyResponse
    {
        public OpportunitySummary Opportunity { get; set; }
        public IEnumerable<ReferenceDataItem> Sectors { get; set; }
        public IEnumerable<ReferenceDataItem> JobRoles { get; set; }
        public IEnumerable<ReferenceDataItem> Levels { get; set; }
        public IEnumerable<PledgeLocation> PledgeLocations { get; set; }
        public class PledgeLocation
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
