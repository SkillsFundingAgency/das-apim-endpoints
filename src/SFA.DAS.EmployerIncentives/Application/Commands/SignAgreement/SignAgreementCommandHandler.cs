using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Application.Commands.SignAgreement
{
    public class SignAgreementCommandHandler : IRequestHandler<SignAgreementCommand, Unit>
    {
        private readonly ILegalEntitiesService _legalEntitiesService;

        public SignAgreementCommandHandler(ILegalEntitiesService legalEntitiesService)
        {
            _legalEntitiesService = legalEntitiesService;
        }
        public async Task<Unit> Handle(SignAgreementCommand request, CancellationToken cancellationToken)
        {
            await _legalEntitiesService.SignAgreement(request.AccountId, request.AccountLegalEntityId, new SignAgreementRequest { AgreementVersion = request.AgreementVersion });
            
            return Unit.Value;
        }
    }
}