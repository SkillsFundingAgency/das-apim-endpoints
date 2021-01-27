using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.Recruit.InnerApi.Responses
{
    public class GetStandardsListItem : StandardApiResponseBase
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
    }
}