using SFA.DAS.Approvals.InnerApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses
{
    public class GetApprenticeshipResponse : IPartyResource
    {
        public long Id { get; set; }
        public long ProviderId { get; set; }
        public string ProviderName { get; set; }
        public long EmployerAccountId { get; set; }
        public long AccountId => EmployerAccountId;
        public long AccountLegalEntityId { get; set; }
        public string EmployerName { get; set; }
        public string DeliveryModel { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public long? ContinuationOfId { get; set; }
        public long? TransferSenderId { get; set; }
        public bool HasHadDataLockSuccess { get; set; }
    }
}
