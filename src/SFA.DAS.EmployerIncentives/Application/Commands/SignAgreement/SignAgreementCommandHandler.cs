using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Application.Commands.SignAgreement
{
    public class SignAgreementCommandHandler : IRequestHandler<SignAgreementCommand, Unit>
    {
        private readonly IEmployerIncentivesService _employerIncentivesService;

        public SignAgreementCommandHandler(IEmployerIncentivesService employerIncentivesService)
        {
            _employerIncentivesService = employerIncentivesService;
        }
        public async Task<Unit> Handle(SignAgreementCommand request, CancellationToken cancellationToken)
        {
            await _employerIncentivesService.SignAgreement(request.AccountId, request.AccountLegalEntityId, new SignAgreementRequest { AgreementVersion = request.AgreementVersion });
            
            return Unit.Value;
        }
    }
}