namespace SFA.DAS.Approvals.Application.Apprentices.Queries.ChangeEmployer.ConfirmEmployer
{
    public class GetConfirmEmployerQueryResult
    {
        public string LegalEntityName { get; set; }
        public string AccountLegalEntityName { get; set; }
        public string AccountName { get; set; }
        public bool IsFlexiJobAgency { get; set; }
        public string DeliveryModel { get; set; }
    }
}