using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.ReferenceData
{
    public class GetCharityRequest : IGetApiRequest
    {
        public int RegistrationNumber { get; }

        public GetCharityRequest(int registrationNumber)
        {
            RegistrationNumber = registrationNumber;
        }

        public string GetUrl => $"charities/{RegistrationNumber}";
    }
}