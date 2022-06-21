using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetApplications;

namespace SFA.DAS.LevyTransferMatching.Api.Models.Applications
{
    public class GetApplicationsResponse
    {
        public IEnumerable<Application> Applications { get; set; }

        public static implicit operator GetApplicationsResponse(GetApplicationsQueryResult source)
        {
            return new GetApplicationsResponse
            {
                Applications = source.Applications.Select(x => (Application) x)
            };
        }

        public class Application
        {
            public int Id { get; set; }
            public string DasAccountName { get; set; }
            public int PledgeId { get; set; }
            public string Details { get; set; }
            public int NumberOfApprentices { get; set; }
            public int Amount { get; set; }
            public int TotalAmount { get; set; }
            public DateTime CreatedOn { get; set; }
            public bool IsNamePublic { get; set; }
            public string Status { get; set; }

            public static implicit operator Application(GetApplicationsQueryResult.Application application)
            {
                return new Application
                {
                    Id = application.Id,
                    DasAccountName = application.DasAccountName,
                    PledgeId = application.PledgeId,
                    Details = application.Details,
                    NumberOfApprentices = application.NumberOfApprentices,
                    Amount = application.Amount,
                    TotalAmount = application.TotalAmount,
                    CreatedOn = application.CreatedOn,
                    IsNamePublic = application.IsNamePublic,
                    Status = application.Status
                };
            }
        }
    }
}
