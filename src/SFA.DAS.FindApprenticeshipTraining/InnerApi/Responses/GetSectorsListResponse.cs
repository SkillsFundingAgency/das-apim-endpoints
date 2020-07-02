using System.Collections.Generic;

namespace SFA.DAS.FindApprenticeshipTraining.Application.InnerApi.Responses
{
    public class GetSectorsListResponse
    {
        public IEnumerable<GetSectorsListItem> Sectors { get; set; }
    }
}