using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerAan.InnerApi.Requests;
public class GetEmployerMemberRequest : IGetApiRequest
{
    public Guid UserRef { get; init; }
    public string GetUrl => Constants.AanHubApiRequestUrls.GetEmployerMember + UserRef;
}
