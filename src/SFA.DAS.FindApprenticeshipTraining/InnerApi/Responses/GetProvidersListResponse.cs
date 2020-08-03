using System.Collections.Generic;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses
{
    public class GetProvidersListResponse
    {
        public IEnumerable<GetProvidersListItem> Providers { get ; set ; }
        public int TotalResults { get; set; }
    }
}