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
            var signAgreementRequest = new SignAgreementRequest
            {
                AccountId = request.AccountId,
                AccountLegalEntityId = request.AccountLegalEntityId,
                AgreementVersion = request.AgreementVersion,
                LegalEntityId = request.LegalEntityId,
                LegalEntityName = request.LegalEntityName
            };
            await _legalEntitiesService.SignAgreement(signAgreementRequest);
            
            return Unit.Value;
        }
    }
}