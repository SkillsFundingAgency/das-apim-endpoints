using System.Collections.Generic;
using SFA.DAS.LevyTransferMatching.Models.ReferenceData;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetCreate
{
    public class GetCreateQueryResult
    {
        public IEnumerable<ReferenceDataItem> Levels { get; set; }
        public IEnumerable<ReferenceDataItem> Sectors { get; set; }
        public IEnumerable<ReferenceDataItem> JobRoles { get; set; }
    }
}