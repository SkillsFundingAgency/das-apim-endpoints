using System;
using System.Collections.Generic;

namespace SFA.DAS.EmployerIncentives.InnerApi.Responses.VendorRegistrationForm
{
    public class GetVendorRegistrationStatusByCaseIdResponse
    {
        public class CaseStatusReason
        {
            public string Code { get; set; }
            public string Text { get; set; }
        }

        public class GetVendorRegistrationStatusByCaseIdRegistrationCase
        {
            public string CaseID { get; set; }
            public string VendorType { get; set; }
            public string SubmittedVendorIdentifier { get; set; }
            public string ApprenticeshipLegalEntityID { get; set; }
            public string UKPRN { get; set; }
            public string URN { get; set; }
            public string VendorCompanyRegistrationNo { get; set; }
            public string CaseType { get; set; }
            public string VendorName { get; set; }
            public IEnumerable<CaseStatusReason> CaseStatusReasons { get; set; }
            public string CaseStatus { get; set; }
            public DateTime CaseReceivedDate { get; set; }
            public DateTime CaseRequestedEffectiveDate { get; set; }
            public DateTime CaseStatusLastUpdatedDate { get; set; }
        }

        public GetVendorRegistrationStatusByCaseIdRegistrationCase RegistrationCase { get; set; }
    }
}