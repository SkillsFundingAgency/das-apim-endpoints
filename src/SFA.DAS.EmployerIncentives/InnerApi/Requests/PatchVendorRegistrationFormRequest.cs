using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests
{
    public class PatchVendorRegistrationFormRequest : IPatchApiRequest<VendorRegistrationFormRequest>
    {
        private readonly long _legalEntityId;
        
        public PatchVendorRegistrationFormRequest(long legalEntityId)
        {
            _legalEntityId = legalEntityId;
        }

        public string BaseUrl { get; set; }
        public string PatchUrl => $"{BaseUrl}legalentities/{_legalEntityId}/vendorregistrationform";
        public VendorRegistrationFormRequest Data { get; set; }
    }

    public class VendorRegistrationFormRequest
    {
        public string VendorId { get; set; }
        public string CaseId { get; set; }
        public string CaseStatus { get; set; }
    }
}