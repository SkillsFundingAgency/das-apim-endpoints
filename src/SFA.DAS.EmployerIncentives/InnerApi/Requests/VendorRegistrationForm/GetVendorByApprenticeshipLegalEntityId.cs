using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests.VendorRegistrationForm
{
    public class GetVendorByApprenticeshipLegalEntityId : IGetApiRequest
    {
        private readonly string _companyName;
        private readonly string _hashedLegalEntityId;
        private readonly string _apiVersion;

        public GetVendorByApprenticeshipLegalEntityId(
            string companyName, 
            string hashedLegalEntityId, 
            string apiVersion)
        {
            _companyName = companyName;
            _hashedLegalEntityId = hashedLegalEntityId;
            _apiVersion = apiVersion;
        }

        public string BaseUrl { get; set; }
        public string GetUrl => $"{BaseUrl}Finance/{_companyName}/vendor/aleid={_hashedLegalEntityId}?api-version={_apiVersion}";
    }
}