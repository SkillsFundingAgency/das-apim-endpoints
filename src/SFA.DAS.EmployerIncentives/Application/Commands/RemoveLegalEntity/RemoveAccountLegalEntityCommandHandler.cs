using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Application.Commands.RemoveLegalEntity
{
    public class RemoveAccountLegalEntityCommandHandler : IRequestHandler<RemoveAccountLegalEntityCommand, Unit>
    {
        private readonly IEmployerIncentivesService _employerIncentivesService;

        public RemoveAccountLegalEntityCommandHandler (IEmployerIncentivesService employerIncentivesService)
        {
            _employerIncentivesService = employerIncentivesService;
        }
        public async Task<Unit> Handle(RemoveAccountLegalEntityCommand request, CancellationToken cancellationToken)
        {
            await _employerIncentivesService.DeleteAccountLegalEntity(request.AccountId, request.AccountLegalEntityId);
            
            return Unit.Value;
        }
    }
}