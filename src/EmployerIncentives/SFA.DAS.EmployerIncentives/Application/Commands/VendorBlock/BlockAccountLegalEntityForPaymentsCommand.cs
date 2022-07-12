using MediatR;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.VendorBlock;

namespace SFA.DAS.EmployerIncentives.Application.Commands.VendorBlock
{
    public class BlockAccountLegalEntityForPaymentsCommand : IRequest
    {
        public BlockAccountLegalEntityForPaymentsRequest VendorBlockRequest { get; }

        public BlockAccountLegalEntityForPaymentsCommand(BlockAccountLegalEntityForPaymentsRequest vendorBlockRequest)
        {
            VendorBlockRequest = vendorBlockRequest;
        }
    }
}