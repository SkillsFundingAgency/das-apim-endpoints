namespace SFA.DAS.EmployerIncentives.InnerApi.Responses.VendorRegistrationForm
{
    public class GetVendorByApprenticeshipLegalEntityIdResponse
    {
        public class GetVendorByApprenticeshipLegalEntityIdResponseVendor
        {
            public string VendorIdentifier { get; set; }
            public string ErrorText { get; set; }
        }

        public GetVendorByApprenticeshipLegalEntityIdResponseVendor Vendor { get; set; }
    }
}