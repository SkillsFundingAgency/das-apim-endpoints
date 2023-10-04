using System.Collections.Generic;
using MediatR;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.VendorBlock;

namespace SFA.DAS.EmployerIncentives.Application.Commands.VendorBlock
{
    public class BlockAccountLegalEntityForPaymentsCommand : IRequest
    {
        public BlockAccountLegalEntityForPaymentsCommand(
            List<BlockAccountLegalEntityForPaymentsRequest> vendorBlockRequest)
        {
            VendorBlockRequest = vendorBlockRequest;
        }

        public List<BlockAccountLegalEntityForPaymentsRequest> VendorBlockRequest { get; }
    }
}