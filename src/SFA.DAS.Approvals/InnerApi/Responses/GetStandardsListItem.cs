using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.Approvals.InnerApi.Responses
{
    public class GetStandardsListItem : StandardApiResponseBase
    {
        public string StandardUId { get; set; }
        public int LarsCode { get; set; }

        public string Title { get; set; }
        public int Level { get; set; }
    }
}