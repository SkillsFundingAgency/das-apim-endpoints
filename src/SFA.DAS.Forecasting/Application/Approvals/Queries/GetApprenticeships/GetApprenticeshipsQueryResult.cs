using System;
using System.Collections.Generic;

namespace SFA.DAS.Forecasting.Application.Approvals.Queries.GetApprenticeships
{
    public class GetApprenticeshipsQueryResult
    {
        public int TotalApprenticeshipsFound { get; set; }

        public IEnumerable<Apprenticeship> Apprenticeships { get; set; }

        public class Apprenticeship
        {
            public long Id { get; set; }
            public long? TransferSenderId { get; set; }
            public string Uln { get; set; }
            public long ProviderId { get; set; }
            public string ProviderName { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string CourseCode { get; set; }
            public string CourseName { get; set; }
            public int CourseLevel { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public decimal? Cost { get; set; }
            public int? PledgeApplicationId { get; set; }
            public bool HasHadDataLockSuccess { get; set; }
        }
    }
}