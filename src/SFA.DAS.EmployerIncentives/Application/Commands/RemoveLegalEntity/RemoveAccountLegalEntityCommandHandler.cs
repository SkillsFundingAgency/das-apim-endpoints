using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Application.Commands.RemoveLegalEntity
{
    public class RemoveAccountLegalEntityCommandHandler : IRequestHandler<RemoveAccountLegalEntityCommand, Unit>
    {
        private readonly ILegalEntitiesService _legalEntitiesService;

        public RemoveAccountLegalEntityCommandHandler (ILegalEntitiesService legalEntitiesService)
        {
            _legalEntitiesService = legalEntitiesService;
        }
        public async Task<Unit> Handle(RemoveAccountLegalEntityCommand request, CancellationToken cancellationToken)
        {
            await _legalEntitiesService.DeleteAccountLegalEntity(request.AccountId, request.AccountLegalEntityId);
            
            return Unit.Value;
        }
    }
}