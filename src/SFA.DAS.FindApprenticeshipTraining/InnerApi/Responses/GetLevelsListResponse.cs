using System.Collections.Generic;

namespace SFA.DAS.FindApprenticeshipTraining.Application.InnerApi.Responses
{
    public class GetLevelsListResponse
    {
        public IEnumerable<GetLevelsListItem> Levels { get; set; }
    }
}