using SFA.DAS.Approvals.InnerApi.Interfaces;
using System;

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
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Uln { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Version { get; set; }
        public string Option { get; set; }
        public DateTime? EmploymentEndDate { get; set; }
        public int? EmploymentPrice { get; set; }
    }
}
