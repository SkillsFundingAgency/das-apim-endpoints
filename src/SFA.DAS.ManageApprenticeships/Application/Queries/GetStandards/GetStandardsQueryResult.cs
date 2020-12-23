using System.Collections.Generic;
using SFA.DAS.ManageApprenticeships.InnerApi.Responses;

namespace SFA.DAS.ManageApprenticeships.Application.Queries.GetStandards
{
    public class GetStandardsQueryResult
    {
        public IEnumerable<GetStandardsListItem> Standards { get ; set ; }
    }
}