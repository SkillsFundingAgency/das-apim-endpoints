using SFA.DAS.LevyTransferMatching.Api.Models.Shared;
using SFA.DAS.LevyTransferMatching.Models.ReferenceData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
