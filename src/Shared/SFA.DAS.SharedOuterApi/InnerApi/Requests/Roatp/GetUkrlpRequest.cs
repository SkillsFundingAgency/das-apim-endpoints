using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Roatp;
public class GetUkrlpRequest : IGetApiRequest
{
    int Ukprn { get; set; }

    public GetUkrlpRequest(int ukprn)
    {
        Ukprn = ukprn;
    }
    public string GetUrl => $"/organisations/{Ukprn}/ukrlp-data";
}