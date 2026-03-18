using SFA.DAS.Apim.Shared.Interfaces;

using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests;

public class GetCharityRequest : IGetApiRequest
{
    public int RegistrationNumber { get; }
    public string GetUrl => $"api/charities/{RegistrationNumber}";
    public GetCharityRequest(int registrationNumber)
    {
        RegistrationNumber = registrationNumber;
    }
}