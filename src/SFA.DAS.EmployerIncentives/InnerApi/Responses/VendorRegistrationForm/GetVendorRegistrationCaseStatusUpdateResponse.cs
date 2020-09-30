using System;
using System.Collections.Generic;

namespace SFA.DAS.EmployerIncentives.InnerApi.Responses.VendorRegistrationForm
{
    public class GetVendorRegistrationCaseStatusUpdateResponse
    {
        public List<VendorRegistrationCase> RegistrationCases { get; set; } = new List<VendorRegistrationCase>();
    }

    public class VendorRegistrationCase
    {
        public string CaseId { get; set; }
        public string ApprenticeshipLegalEntityId { get; set; }
        public string SubmittedVendorIdentifier { get; set; }
        public string CaseStatus { get; set; }
        public DateTime CaseStatusLastUpdatedDate { get; set; }
        public string CaseType { get; set; }
    }
}