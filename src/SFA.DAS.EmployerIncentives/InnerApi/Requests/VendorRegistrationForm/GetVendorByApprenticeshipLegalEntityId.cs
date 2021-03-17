using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests.VendorRegistrationForm
{
    public class GetVendorByApprenticeshipLegalEntityId : IGetApiRequest
    {
        private readonly string _companyName;
        private readonly string _hashedLegalEntityId;
        private readonly string _apiVersion;

        public GetVendorByApprenticeshipLegalEntityId(string companyName, string hashedLegalEntityId, IDateTimeService dateTimeService)
        {
            _companyName = companyName;
            _hashedLegalEntityId = hashedLegalEntityId;
            _apiVersion = dateTimeService.Today >= new DateTime(2021, 4, 6) ? "2021-04-06" : "2019-06-01";
        }

        public string BaseUrl { get; set; }
        public string GetUrl => $"{BaseUrl}Finance/{_companyName}/vendor/aleid={_hashedLegalEntityId}?api-version={_apiVersion}";
    }
}