using MediatR;

namespace SFA.DAS.EmployerIncentives.Application.Commands.SignAgreement
{
    public class SignAgreementCommand : IRequest<Unit>
    {
        public long AccountLegalEntityId { get ; set ; }
        public long AccountId { get ; set ; }
        public int AgreementVersion { get; set; }
    }
}