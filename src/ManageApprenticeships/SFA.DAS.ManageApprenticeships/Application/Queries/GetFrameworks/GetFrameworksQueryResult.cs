using System.Collections.Generic;
using SFA.DAS.ManageApprenticeships.InnerApi.Responses;

namespace SFA.DAS.ManageApprenticeships.Application.Queries.GetFrameworks
{
    public class GetFrameworksQueryResult
    {
        public IEnumerable<GetFrameworksListItem> Frameworks { get ; set ; }
    }
}