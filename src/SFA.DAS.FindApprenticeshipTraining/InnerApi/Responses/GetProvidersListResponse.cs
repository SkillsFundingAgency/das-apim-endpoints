using System.Collections.Generic;

namespace SFA.DAS.FindApprenticeshipTraining.Application.InnerApi.Responses
{
    public class GetProvidersListResponse
    {
        public IEnumerable<GetProvidersListItem> Providers { get ; set ; }
        public int TotalResults { get; set; }
    }
}