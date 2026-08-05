using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CoursesApi.Requests;

public class GetAllFrameworksApiRequest : IGetApiRequest
{
    public string GetUrl => "api/courses/frameworks";
}