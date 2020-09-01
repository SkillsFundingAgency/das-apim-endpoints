using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests.VendorRegistrationForm
{
    public class GetVendorByApprenticeshipLegalEntityId : IGetApiRequest
    {
        private readonly string _companyName;
        private readonly string _hashedLegalEntityId;

        public GetVendorByApprenticeshipLegalEntityId(string companyName, string hashedLegalEntityId)
        {
            _companyName = companyName;
            _hashedLegalEntityId = hashedLegalEntityId;
        }

        public string BaseUrl { get; set; }
        public string GetUrl => $"{BaseUrl}Finance/{_companyName}/vendor/{_hashedLegalEntityId}";
    }
}