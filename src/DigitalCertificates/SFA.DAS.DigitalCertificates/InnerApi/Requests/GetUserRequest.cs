using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.DigitalCertificates.InnerApi.Requests
{
    public class GetUserRequest : IGetApiRequest
    {
        public string GovUkIdentifier { get; set; }

        public GetUserRequest(string govUkIdentifier)
        {
            GovUkIdentifier = govUkIdentifier;
        }

        public string GetUrl => $"api/users/{GovUkIdentifier}";
    }
}
