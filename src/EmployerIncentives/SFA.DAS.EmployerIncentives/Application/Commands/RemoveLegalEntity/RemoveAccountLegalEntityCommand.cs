using MediatR;

namespace SFA.DAS.EmployerIncentives.Application.Commands.RemoveLegalEntity
{
    public class RemoveAccountLegalEntityCommand : IRequest<Unit>
    {
        public long AccountLegalEntityId { get ; set ; }
        public long AccountId { get ; set ; }
    }
}