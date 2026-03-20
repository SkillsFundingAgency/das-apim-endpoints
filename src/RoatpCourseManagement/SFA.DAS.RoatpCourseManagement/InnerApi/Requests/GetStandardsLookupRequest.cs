using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Requests;

public class GetStandardsLookupRequest : IGetApiRequest
{
    public string GetUrl => "api/courses/search?filter=Active";
}