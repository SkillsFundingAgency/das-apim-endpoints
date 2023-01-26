using System;
using System.Collections.Generic;

namespace SFA.DAS.EmployerIncentives.InnerApi.Responses.VendorRegistrationForm
{
    public class GetVendorRegistrationCaseStatusUpdateResponse
    {
        public List<VendorRegistrationCase> RegistrationCases { get; set; } = new List<VendorRegistrationCase>();
        public string SkipCode { get; set; }
    }

    public class VendorRegistrationCase
    {
        public string CaseId { get; set; }
        public string ApprenticeshipLegalEntityId { get; set; }
        public string CaseStatus { get; set; }
        public string CaseStatusLastUpdatedDate { get; set; }
        public string CaseType { get; set; }
    }
}