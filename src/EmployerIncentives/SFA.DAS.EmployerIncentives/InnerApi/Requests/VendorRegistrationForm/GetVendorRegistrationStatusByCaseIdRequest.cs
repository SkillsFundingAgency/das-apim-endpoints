using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests.VendorRegistrationForm
{
    public class GetVendorRegistrationStatusByCaseIdRequest : IGetApiRequest
    {
        private readonly string _caseId;
        
        public GetVendorRegistrationStatusByCaseIdRequest(string caseId)
        {
            _caseId = caseId;
        }

        public string BaseUrl { get; set; }
        public string GetUrl => $"{BaseUrl}Finance/Registrations/{_caseId}?api-version=2019-06-01";
    }
}