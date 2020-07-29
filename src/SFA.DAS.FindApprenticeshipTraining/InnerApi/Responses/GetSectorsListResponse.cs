using System.Collections.Generic;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses
{
    public class GetSectorsListResponse
    {
        public IEnumerable<GetSectorsListItem> Sectors { get; set; }
    }
}