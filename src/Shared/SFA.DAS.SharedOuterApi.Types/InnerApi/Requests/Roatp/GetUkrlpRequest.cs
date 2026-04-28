using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Roatp;

public class GetUkrlpRequest(int ukprn) : IGetApiRequest
{
    int Ukprn { get; set; } = ukprn;

    public string GetUrl => $"/organisations/{Ukprn}/ukrlp-data";
}