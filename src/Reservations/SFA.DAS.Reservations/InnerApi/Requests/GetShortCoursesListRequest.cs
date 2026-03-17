using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Reservations.InnerApi.Requests;

public class GetShortCoursesListRequest : IGetApiRequest
{
    public string GetUrl => "api/courses?learningType=ShortCourse";
}