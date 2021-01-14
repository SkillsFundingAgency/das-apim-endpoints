using System.Collections.Generic;
using SFA.DAS.Approvals.InnerApi.Responses;

namespace SFA.DAS.Approvals.Application.Epaos.Queries
{
    public class GetEpaosResult
    {
        public IEnumerable<GetEpaosListItem> Epaos { get ; set ; }
    }
}