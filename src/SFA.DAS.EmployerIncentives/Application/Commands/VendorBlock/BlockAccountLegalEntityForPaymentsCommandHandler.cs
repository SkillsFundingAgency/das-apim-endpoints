using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Application.Commands.VendorBlock
{
    public class BlockAccountLegalEntityForPaymentsCommandHandler : IRequestHandler<BlockAccountLegalEntityForPaymentsCommand>
    {
        private readonly ILegalEntitiesService _legalEntitiesService;

        public BlockAccountLegalEntityForPaymentsCommandHandler(ILegalEntitiesService legalEntitiesService)
        {
            _legalEntitiesService = legalEntitiesService;
        }
        
        public async Task<Unit> Handle(BlockAccountLegalEntityForPaymentsCommand request, CancellationToken cancellationToken)
        {
            await _legalEntitiesService.BlockAccountLegalEntitiesForPayments(request.VendorBlockRequest);

            return Unit.Value;
        }
    }
}
