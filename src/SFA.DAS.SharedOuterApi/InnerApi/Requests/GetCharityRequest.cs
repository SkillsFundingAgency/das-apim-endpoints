using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests
{
    public class GetCharityRequest : IGetApiRequest
    {
        public int RegistrationNumber { get; }
        public string GetUrl => $"api/charities/{RegistrationNumber}";
        public GetCharityRequest(int registrationNumber)
        {
            RegistrationNumber = registrationNumber;
        }
    }
}
