using SFA.DAS.LevyTransferMatching.Infrastructure;
using SFA.DAS.LevyTransferMatching.Models.ReferenceData;
using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Opportunity.GetIndex
{
    public class GetIndexQueryResult : PagedQueryResult<GetIndexQueryResult.Opportunity>
    {
        public IEnumerable<ReferenceDataItem> Sectors { get; set; }
        public IEnumerable<ReferenceDataItem> JobRoles { get; set; }
        public IEnumerable<ReferenceDataItem> Levels { get; set; }

        public class Opportunity
        {
            public int Id { get; set; }
            public int Amount { get; set; }
            public bool IsNamePublic { get; set; }
            public string DasAccountName { get; set; }
            public IEnumerable<string> Sectors { get; set; }
            public IEnumerable<string> JobRoles { get; set; }
            public IEnumerable<string> Levels { get; set; }
            public IEnumerable<string> Locations { get; set; }
        }
    }
}
