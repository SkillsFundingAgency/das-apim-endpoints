using System.Collections.Generic;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses
{
    public class GetLevelsListResponse
    {
        public IEnumerable<GetLevelsListItem> Levels { get; set; }
    }
}