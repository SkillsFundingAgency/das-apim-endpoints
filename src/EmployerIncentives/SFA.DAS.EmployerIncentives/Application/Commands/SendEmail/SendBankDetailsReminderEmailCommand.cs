using MediatR;

namespace SFA.DAS.EmployerIncentives.Application.Commands.SendEmail
{
    public class SendBankDetailsReminderEmailCommand : IRequest
    {
        public long AccountId { get; }
        public long AccountLegalEntityId { get; }
        public string EmailAddress { get; }
        public string AddBankDetailsUrl { get; }

        public SendBankDetailsReminderEmailCommand(long accountId, long accountLegalEntityId, string emailAddress, string addBankDetailsUrl)
        {
            AccountId = accountId;
            AccountLegalEntityId = accountLegalEntityId;
            EmailAddress = emailAddress;
            AddBankDetailsUrl = addBankDetailsUrl;
        }
    }
}

