namespace SFA.DAS.EmployerAccounts.Api.Models
{
    public class AddProviderDetailsPostRequest
    {
        public long AccountId { get; set; }
        public string CorrelationId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
