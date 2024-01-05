using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses
{
    public class GetLevelsListResponse
    {
        public IEnumerable<GetLevelsListItem> Levels { get; set; }
    }

    public class GetLevelsListItem
    {
        public int Code { get; set; }
        public string Name { get; set; }
    }
}