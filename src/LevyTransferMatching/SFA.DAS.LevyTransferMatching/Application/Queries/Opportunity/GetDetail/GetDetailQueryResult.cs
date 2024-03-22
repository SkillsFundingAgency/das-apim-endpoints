using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.LevyTransferMatching.Models.ReferenceData;
using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Opportunity.GetDetail
{
    public class GetDetailQueryResult
    {
        public Pledge Opportunity { get; set; }
        public IEnumerable<ReferenceDataItem> Sectors { get; set; }
        public IEnumerable<ReferenceDataItem> JobRoles { get; set; }
        public IEnumerable<ReferenceDataItem> Levels { get; set; }
    }
}
