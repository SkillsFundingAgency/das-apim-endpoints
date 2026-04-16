using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests;
public class GetCoursesExportRequest : IGetApiRequest
{
    public string GetUrl => "api/Courses/search";
}