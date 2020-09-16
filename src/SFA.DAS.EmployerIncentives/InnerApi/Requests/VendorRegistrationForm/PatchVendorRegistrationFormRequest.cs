using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests.VendorRegistrationForm
{
    public class PatchVendorRegistrationCaseStatusRequest : IPatchApiRequest<UpdateVendorRegistrationCaseStatusRequest>
    {
        public PatchVendorRegistrationCaseStatusRequest(UpdateVendorRegistrationCaseStatusRequest data)
        {
            Data = data;
        }
        public string BaseUrl { get; set; }
        public string PatchUrl => $"{BaseUrl}legalentities/{Data.LegalEntityId}/vendorregistrationform/status";
        public UpdateVendorRegistrationCaseStatusRequest Data { get; set; }
    }

    public class UpdateVendorRegistrationCaseStatusRequest
    {
        public string LegalEntityId { get; set; }

        public string Status { get; set; }
        public DateTime CaseStatusLastUpdatedDate { get; set; }
        public string VendorId { get; set; }
        public string CaseId { get; set; }
    }
}