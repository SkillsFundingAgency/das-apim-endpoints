using System.Collections.Generic;
using SFA.DAS.LevyTransferMatching.Models.ReferenceData;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetLevels
{
    public class GetLevelsQueryResult
    {
        public IEnumerable<ReferenceDataItem> ReferenceDataItems { get; set; }
    }
}
