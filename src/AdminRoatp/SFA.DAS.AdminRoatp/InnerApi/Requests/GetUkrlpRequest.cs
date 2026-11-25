using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.AdminRoatp.InnerApi.Requests;

public record GetUkrlpRequest(int Ukprn) : IGetApiRequest
{
    public string GetUrl => $"/ukrlp/providers/{Ukprn}";
}