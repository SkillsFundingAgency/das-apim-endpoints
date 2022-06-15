using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Application.Commands.AddLegalEntity
{
    public class CreateAccountLegalEntityCommandHandler : IRequestHandler<CreateAccountLegalEntityCommand, CreateAccountLegalEntityCommandResult>
    {
        private readonly ILegalEntitiesService _service;

        public CreateAccountLegalEntityCommandHandler (ILegalEntitiesService service)
        {
            _service = service;
        }
        public async Task<CreateAccountLegalEntityCommandResult> Handle(CreateAccountLegalEntityCommand request, CancellationToken cancellationToken)
        {
            await _service.CreateLegalEntity(new AccountLegalEntityCreateRequest
            {
                AccountId = request.AccountId,
                OrganisationName = request.OrganisationName,
                LegalEntityId = request.LegalEntityId,
                AccountLegalEntityId = request.AccountLegalEntityId
            });

            return new CreateAccountLegalEntityCommandResult();
        }
    }
}