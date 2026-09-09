using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.EmployerAccounts;

public class GetAccountMinimumSignedAgreementVersionRequest(long accountId) : IGetApiRequest
{
    public long AccountId { get; } = accountId;

    public string GetUrl => $"api/accounts/internal/{AccountId}/minimum-signed-agreement-version";
}