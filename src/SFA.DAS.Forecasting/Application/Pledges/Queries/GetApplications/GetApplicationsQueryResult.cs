using System;
using System.Collections.Generic;

namespace SFA.DAS.Forecasting.Application.Pledges.Queries.GetApplications
{
    public class GetApplicationsQueryResult
    {
        public int TotalApplications { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }

        public IEnumerable<Application> Applications { get; set; }

        public class Application
        {
            public int Id { get; set; }
            public int PledgeId { get; set; }
            public long EmployerAccountId { get; set; }
            public string StandardId { get; set; }
            public string StandardTitle { get; set; }
            public int StandardLevel { get; set; }
            public int StandardDuration { get; set; }
            public int StandardMaxFunding { get; set; }
            public DateTime StartDate { get; set; }
            public int NumberOfApprentices { get; set; }
            public int NumberOfApprenticesUsed { get; set; }
        }
    }
}