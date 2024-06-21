using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Charities
{
    public class GetCharityByRegistrationNumberRequest : IGetApiRequest
    {
        public int RegistrationNumber { get; }

        public GetCharityByRegistrationNumberRequest(int registrationNumber)
        {
            RegistrationNumber = registrationNumber;
        }

        public string GetUrl => $"/api/Charities/{RegistrationNumber}";
    }
}