using MediatR;

namespace SFA.DAS.EmployerIncentives.Application.Commands.AddLegalEntity
{
    public class CreateAccountLegalEntityCommand : IRequest<CreateAccountLegalEntityCommandResult>
    {
        public long AccountId { get ; set ; }
        public long LegalEntityId { get ; set ; }
        public string OrganisationName { get ; set ; }
        public long AccountLegalEntityId { get ; set ; }
    }
}