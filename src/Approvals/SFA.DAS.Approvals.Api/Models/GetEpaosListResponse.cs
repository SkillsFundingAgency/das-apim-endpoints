using System.Collections.Generic;

namespace SFA.DAS.Approvals.Api.Models
{
    public class GetEpaosListResponse
    {
        public IEnumerable<GetEpaoResponse> Epaos { get; set; }
    }
}