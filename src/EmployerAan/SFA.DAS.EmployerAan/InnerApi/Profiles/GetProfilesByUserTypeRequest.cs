using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerAan.InnerApi.Profiles;

public class GetProfilesByUserTypeRequest : IGetApiRequest
{
    public string GetUrl => Constants.AanHubApiRequestUrls.GetProfilesUrl + UserType;
    public string UserType { get; }
    public GetProfilesByUserTypeRequest(string userType) => UserType = userType;
}
