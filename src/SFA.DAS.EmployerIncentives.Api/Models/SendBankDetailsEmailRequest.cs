
namespace SFA.DAS.EmployerIncentives.Api.Models
{
    public class SendBankDetailsEmailRequest
    {
        public long AccountId { get; set; }
        public long AccountLegalEntityId { get; set; }
        public string EmailAddress { get; set; }
        public string AddBankDetailsUrl { get; set; }
    }
}
