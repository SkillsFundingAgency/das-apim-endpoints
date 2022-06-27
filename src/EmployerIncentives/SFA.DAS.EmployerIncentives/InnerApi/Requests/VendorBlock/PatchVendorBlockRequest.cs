using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests.VendorBlock
{
    public class PatchVendorBlockRequest : IPatchApiRequest<BlockAccountLegalEntityForPaymentsRequest>
    {
        public PatchVendorBlockRequest(BlockAccountLegalEntityForPaymentsRequest request)
        {
            Data = request;
        }

        public string PatchUrl => "/blockedpayments";
        public BlockAccountLegalEntityForPaymentsRequest Data { get; set; }
    }
}