using System;

namespace SFA.DAS.EmployerIncentives.InnerApi.Responses.VendorRegistrationForm
{
    public class GetVendorByApprenticeshipLegalEntityIdResponse
    { 
        public string VendorIndetifier { get; set; }
        public string VendorUKPRN { get; set; }
        public string VendorUPIN { get; set; }
        public string VendorURN { get; set; }
        public string VendorCompanyRegistrationNo { get; set; }
        public string VendorCPIDCode { get; set; }
        public string VendorEdubaseCode { get; set; }
        public string VendorDUNSNumber { get; set; }
        public string RegistrationCaseID { get; set; }
        public string ApprenticeshipLegalEntityID { get; set; }
        public string VendorOrganisationUnit { get; set; }
        public string VendorLAIdentifier { get; set; }
        public string VendorRemittanceDelMethod { get; set; }
        public string VendorCategorisation { get; set; }
        public string VendorVASISNumber { get; set; }
        public string VendorBlocked { get; set; }
        public string VendorBlockedForBACS { get; set; }
        public string VendorBlockedReason { get; set; }
        public DateTime VendorLastUpdated { get; set; }
    }
}