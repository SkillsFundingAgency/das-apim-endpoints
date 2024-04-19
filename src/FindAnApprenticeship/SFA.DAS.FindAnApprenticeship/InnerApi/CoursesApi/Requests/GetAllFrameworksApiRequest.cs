using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CoursesApi.Requests;

public class GetAllFrameworksApiRequest : IGetApiRequest
{
    public string GetUrl => "api/courses/frameworks";
}