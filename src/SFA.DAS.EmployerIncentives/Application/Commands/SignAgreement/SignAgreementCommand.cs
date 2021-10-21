using MediatR;

namespace SFA.DAS.EmployerIncentives.Application.Commands.SignAgreement
{
    public class SignAgreementCommand : IRequest<Unit>
    {
        public long AccountId { get; set; }
        public long AccountLegalEntityId { get; set; }
        public string LegalEntityName { get; set; }
        public long LegalEntityId { get; set; }
        public int AgreementVersion { get; set; }
    }
}