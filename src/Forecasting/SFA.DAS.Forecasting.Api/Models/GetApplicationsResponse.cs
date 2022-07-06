using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Forecasting.Application.Pledges.Queries.GetApplications;

namespace SFA.DAS.Forecasting.Api.Models
{
    public class GetApplicationsResponse
    {
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
            public string Status { get; set; }
        }

        public static implicit operator GetApplicationsResponse(GetApplicationsQueryResult source)
        {
            return new GetApplicationsResponse
            {
                Applications = source.Applications.Select(a => new Application
                {
                    Id = a.Id,
                    PledgeId = a.PledgeId,
                    EmployerAccountId = a.EmployerAccountId,
                    StandardId = a.StandardId,
                    StandardTitle = a.StandardTitle,
                    StandardLevel = a.StandardLevel,
                    StandardDuration = a.StandardDuration,
                    StandardMaxFunding = a.StandardMaxFunding,
                    StartDate = a.StartDate,
                    NumberOfApprentices = a.NumberOfApprentices,
                    NumberOfApprenticesUsed = a.NumberOfApprenticesUsed,
                    Status = a.Status
                })
            };
        }

    }
}
