using System.Collections.Generic;
using SFA.DAS.LevyTransferMatching.Models.ReferenceData;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetLevel
{
    public class GetLevelQueryResult
    {
        public IEnumerable<ReferenceDataItem> Levels { get; set; }
    }
}