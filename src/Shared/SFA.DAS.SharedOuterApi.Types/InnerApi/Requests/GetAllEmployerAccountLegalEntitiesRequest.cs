using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests;

public class GetAllEmployerAccountLegalEntitiesRequest(string encodedAccountId) : IGetApiRequest
{
    public string EncodedAccountId { get; } = encodedAccountId;

    public string GetUrl => $"api/accounts/{EncodedAccountId}";
}