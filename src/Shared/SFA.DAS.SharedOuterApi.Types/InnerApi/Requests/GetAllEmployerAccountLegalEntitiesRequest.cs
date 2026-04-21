using SFA.DAS.Apim.Shared.Interfaces;

using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests;

public class GetAllEmployerAccountLegalEntitiesRequest : IGetApiRequest
{
    public string EncodedAccountId { get; }

    public GetAllEmployerAccountLegalEntitiesRequest(string encodedAccountId)
    {
        EncodedAccountId = encodedAccountId;
    }

    public string GetUrl => $"api/accounts/{EncodedAccountId}";
}