using SFA.DAS.Apim.Shared.Interfaces;

using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Roatp;
public class GetUkrlpRequest : IGetApiRequest
{
    int Ukprn { get; set; }

    public GetUkrlpRequest(int ukprn)
    {
        Ukprn = ukprn;
    }
    public string GetUrl => $"/organisations/{Ukprn}/ukrlp-data";
}