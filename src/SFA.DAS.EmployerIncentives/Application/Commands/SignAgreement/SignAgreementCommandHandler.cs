using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Application.Commands.SignAgreement
{
    public class SignAgreementCommandHandler : IRequestHandler<SignAgreementCommand, Unit>
    {
        private readonly ILegalEntitiesService _legalEntitiesService;
        private readonly ILogger<SignAgreementCommandHandler> _logger;

        public SignAgreementCommandHandler(ILegalEntitiesService legalEntitiesService, ILogger<SignAgreementCommandHandler> logger)
        {
            _legalEntitiesService = legalEntitiesService;
            _logger = logger;
        }

        public async Task<Unit> Handle(SignAgreementCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("[SignAgreementCommandHandler] request {@request}", request);

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