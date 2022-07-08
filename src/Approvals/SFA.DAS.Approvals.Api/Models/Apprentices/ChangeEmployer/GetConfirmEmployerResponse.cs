
namespace SFA.DAS.Approvals.Api.Models.Apprentices.ChangeEmployer
{
    public class GetConfirmEmployerResponse
    {
        public string LegalEntityName { get; set; }
        public string AccountLegalEntityName { get; set; }
        public string AccountName { get; set; }
        public bool IsFlexiJobAgency { get; set; }
        public string DeliveryModel { get; set; }
    }
}
