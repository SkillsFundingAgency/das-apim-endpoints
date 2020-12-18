using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests.VendorRegistrationForm
{
    public class GetVrfVendorIdRequest : IGetApiRequest
    {
        public GetVrfVendorIdRequest(string hashedLegalEntityId)
        {
            HashedLegalEntityId = hashedLegalEntityId;
        }

        public string HashedLegalEntityId { get; private set; }
        public string GetUrl => "/legalentities/{hashedLegalEntityId}/employervendorid";
    }
}
