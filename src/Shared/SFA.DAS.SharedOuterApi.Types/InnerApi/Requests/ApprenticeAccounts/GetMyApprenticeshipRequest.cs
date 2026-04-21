using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.ApprenticeAccounts;

public class GetMyApprenticeshipRequest(Guid id) : IGetApiRequest
{
    public Guid Id { get; } = id;

    public string GetUrl => $"apprentices/{Id}/MyApprenticeship";
}