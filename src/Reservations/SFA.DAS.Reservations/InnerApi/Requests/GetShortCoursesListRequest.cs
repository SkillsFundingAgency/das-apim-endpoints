using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Reservations.InnerApi.Requests;

public class GetShortCoursesListRequest : IGetApiRequest
{
    public string GetUrl => "api/courses?learningType=ShortCourse";
}