using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Requests;

public class GetStandardsLookupRequest : IGetApiRequest
{
    public string GetUrl => "api/courses/search?filter=Active";
}