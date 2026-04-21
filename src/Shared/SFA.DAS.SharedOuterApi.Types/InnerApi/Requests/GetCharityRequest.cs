using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests;

public class GetCharityRequest(int registrationNumber) : IGetApiRequest
{
    public int RegistrationNumber { get; } = registrationNumber;
    public string GetUrl => $"api/charities/{RegistrationNumber}";
}