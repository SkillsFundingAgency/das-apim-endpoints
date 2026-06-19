using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Requests;

public class GetAllRestrictedCoursesRequest : IGetApiRequest
{
    public string GetUrl => $"restricted-courses?restricted={Restricted}";
    public bool Restricted { get; }

    public GetAllRestrictedCoursesRequest(bool restricted)
    {
        Restricted = restricted;
    }
}