namespace SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses
{
    public class GetApprenticeshipResponse
    {
        public long Id { get; set; }
        public long ProviderId { get; set; }
        public string ProviderName { get; set; }
        public long EmployerAccountId { get; set; }
        public long AccountLegalEntityId { get; set; }
        public string EmployerName { get; set; }
        public string DeliveryModel { get; set; }
        public string CourseCode { get; set; }
        public long? ContinuationOfId { get; set; }
        public long? TransferSenderId { get; set; }
    }
}
