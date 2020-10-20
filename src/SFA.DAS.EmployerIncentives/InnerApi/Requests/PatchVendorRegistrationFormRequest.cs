using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests
{
    public class PatchVendorRegistrationFormRequest : IPatchApiRequest<UpdateVendorRegistrationFormRequest>
    {
        private readonly long _legalEntityId;
        
        public PatchVendorRegistrationFormRequest(long legalEntityId)
        {
            _legalEntityId = legalEntityId;
        }

        public string BaseUrl { get; set; }
        public string PatchUrl => $"{BaseUrl}legalentities/{_legalEntityId}/vendorregistrationform";
        public UpdateVendorRegistrationFormRequest Data { get; set; }
    }

    public class UpdateVendorRegistrationFormRequest
    {
        public string VendorId { get; set; }
        public string CaseId { get; set; }
        public string Status { get; set; }
    }
}