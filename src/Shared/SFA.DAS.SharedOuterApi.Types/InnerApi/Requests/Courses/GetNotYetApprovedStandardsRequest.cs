using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;

public class GetNotYetApprovedStandardsRequest : IGetApiRequest
{
    public string GetUrl => "api/courses/standards?filter=NotYetApproved";
}