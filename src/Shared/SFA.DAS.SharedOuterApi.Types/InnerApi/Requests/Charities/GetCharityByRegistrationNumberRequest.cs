using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Charities;

public class GetCharityByRegistrationNumberRequest(int registrationNumber) : IGetApiRequest
{
    public int RegistrationNumber { get; } = registrationNumber;

    public string GetUrl => $"/api/Charities/{RegistrationNumber}";
}