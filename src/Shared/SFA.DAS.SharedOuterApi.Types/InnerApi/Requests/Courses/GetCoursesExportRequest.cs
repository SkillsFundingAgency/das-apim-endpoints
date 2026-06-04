using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;

public class GetCoursesExportRequest : IGetApiRequest
{
    public string GetUrl => "api/Courses/search";
}