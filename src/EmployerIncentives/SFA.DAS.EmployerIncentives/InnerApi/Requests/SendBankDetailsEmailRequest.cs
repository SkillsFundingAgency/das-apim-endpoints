
namespace SFA.DAS.EmployerIncentives.InnerApi.Requests
{
    public class SendBankDetailsEmailRequest 
    {
        public SendBankDetailsEmailRequest(long accountId, long accountLegalEntityId, string emailAddress, string addBankDetailsUrl)
        {
            AccountId = accountId;
            AccountLegalEntityId = accountLegalEntityId;
            EmailAddress = emailAddress;
            AddBankDetailsUrl = addBankDetailsUrl;
        }

        public long AccountId { get; private set; }
        public long AccountLegalEntityId { get; private set; }
        public string EmailAddress { get; private set; }
        public string AddBankDetailsUrl { get; private set; }

    }

   
}
