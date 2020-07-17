using System.Collections.Generic;
using SFA.DAS.FindApprenticeshipTraining.Application.Application;

namespace SFA.DAS.FindApprenticeshipTraining.Application.InnerApi.Responses
{
    public class GetStandardsListResponse
    {
        public IEnumerable<GetStandardsListItem> Standards { get; set; }
        public int TotalFiltered { get ; set ; }
        public int Total { get ; set ; }
    }
}
