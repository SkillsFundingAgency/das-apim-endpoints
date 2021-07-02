using System.Collections.Generic;
using SFA.DAS.LevyTransferMatching.Models.ReferenceData;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetJobRoles
{
    public class GetJobRolesQueryResult
    {
        public IEnumerable<ReferenceDataItem> ReferenceDataItems { get; set; }
    }
}
