using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Roatp;
public class GetAllCourseTypesRequest : IGetApiRequest
{
    public string GetUrl => "course-types";
}