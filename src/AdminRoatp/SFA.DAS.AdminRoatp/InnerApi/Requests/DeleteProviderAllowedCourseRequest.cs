using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.AdminRoatp.InnerApi.Requests;

public class DeleteProviderAllowedCourseRequest : IDeleteApiRequest
{
    public int Ukprn { get; set; }
    public string LarsCode { get; set; }
    public string UserId { get; set; }
    public string UserDisplayName { get; set; }
    public string DeleteUrl => $"providers/{Ukprn}/allowed-courses/{LarsCode}?userId={Uri.EscapeDataString(UserId)}&userDisplayName={Uri.EscapeDataString(UserDisplayName)}";

    public DeleteProviderAllowedCourseRequest(int ukprn, string larsCode, string userId, string userDisplayName)
    {
        Ukprn = ukprn;
        LarsCode = larsCode;
        UserId = userId;
        UserDisplayName = userDisplayName;
    }
}
