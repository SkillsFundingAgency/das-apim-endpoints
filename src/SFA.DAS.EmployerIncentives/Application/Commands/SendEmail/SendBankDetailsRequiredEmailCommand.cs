using MediatR;

namespace SFA.DAS.EmployerIncentives.Application.Commands.SendEmail
{
    public class SendBankDetailsRequiredEmailCommand : IRequest
    {
        public long AccountId { get; }
        public long AccountLegalEntityId { get; }
        public string EmailAddress { get; }
        public string AddBankDetailsUrl { get; }

        public SendBankDetailsRequiredEmailCommand(long accountId, long accountLegalEntityId, string emailAddress, string addBankDetailsUrl)
        {
            AccountId = accountId;
            AccountLegalEntityId = accountLegalEntityId;
            EmailAddress = emailAddress;
            AddBankDetailsUrl = addBankDetailsUrl;
        }
    }
}

