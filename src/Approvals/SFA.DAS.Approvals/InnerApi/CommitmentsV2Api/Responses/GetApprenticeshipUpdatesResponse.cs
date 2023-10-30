using System.Collections.Generic;
using System;

namespace SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses
{
    public class GetApprenticeshipUpdatesResponse
    {
        public IReadOnlyCollection<ApprenticeshipUpdate> ApprenticeshipUpdates { get; set; }

        public class ApprenticeshipUpdate
        {
            public long Id { get; set; }
            public long ApprenticeshipId { get; set; }
            public short OriginatingParty { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public DateTime? DateOfBirth { get; set; }
            public Decimal? Cost { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public string TrainingCode { get; set; }
            public string Version { get; set; }
            public string TrainingName { get; set; }
            public string Option { get; set; }
            public DeliveryModel? DeliveryModel { get; set; }
            public DateTime? EmploymentEndDate { get; set; }
            public int? EmploymentPrice { get; set; }
        }
    }
}
