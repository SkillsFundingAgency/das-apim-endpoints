namespace SFA.DAS.Apprenticeships.Api.Models
{
    public class GetAccountLegalEntityResponse
    {
        public long AccountId { get; set; }
        public long MaLegalEntityId { get; set; }
        public string AccountName { get; set; }
        public string LegalEntityName { get; set; }
    }
}
