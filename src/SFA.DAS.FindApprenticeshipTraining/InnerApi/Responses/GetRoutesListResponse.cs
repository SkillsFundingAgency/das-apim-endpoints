using System.Collections.Generic;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses
{
    public class GetRoutesListResponse
    {
        public IEnumerable<GetRoutesListItem> Routes { get; set; }
    }
}