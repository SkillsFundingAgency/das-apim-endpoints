using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using System;

namespace SFA.DAS.Recruit.InnerApi.Responses
{
    public class GetStandardsListItem : StandardApiResponseBase
    {
        public string StandardUId { get; set; }
        public int LarsCode { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
    }
}