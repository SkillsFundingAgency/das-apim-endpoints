using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Requests;

public class DeleteExpiredShortlistsRequest : IDeleteApiRequest
{
    private const string deletePath = "api/shortlists/expired";

    public string DeleteUrl => deletePath;
}
