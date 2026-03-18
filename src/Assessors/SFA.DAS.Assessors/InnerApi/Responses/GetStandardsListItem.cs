using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Courses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Courses;

namespace SFA.DAS.Assessors.InnerApi.Responses
{
    public class GetStandardsListItem : StandardApiResponseBase
    {
        public string StandardUId { get; set; }
        public string IfateReferenceNumber { get; set; }
        public int LarsCode { get; set; }
        public string Title { get; set; }
        public string Version { get; set; }
        public int Level { get; set; }
        public string Status { get; set; }
    }
}