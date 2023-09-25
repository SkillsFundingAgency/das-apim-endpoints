using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests.VendorBlock
{
    public class PatchVendorBlockRequest : IPatchApiRequest<List<BlockAccountLegalEntityForPaymentsRequest>>
    {
        public PatchVendorBlockRequest(List<BlockAccountLegalEntityForPaymentsRequest> request)
        {
            Data = request;
        }

        public string PatchUrl => "/blockedpayments";
        public List<BlockAccountLegalEntityForPaymentsRequest> Data { get; set; }
    }
}