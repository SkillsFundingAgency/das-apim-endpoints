using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetApplications
{
    public class GetApplicationsQueryResult
    {
        public static implicit operator GetApplicationsQueryResult(GetApplicationsResponse application)
        {
            return new GetApplicationsQueryResult
            {
                Applications = application.Applications.Select(x => (Application) x)
            };
        }

        public IEnumerable<Application> Applications { get; set; }

        public class Application
        {
            public int Id { get; set; }
            public string DasAccountName { get; set; }
            public int PledgeId { get; set; }
            public int StandardDuration { get; set; }
            public DateTime StartDate { get; set; }
            public int Amount { get; set; }
            public int TotalAmount { get; set; }
            public bool HasTrainingProvider { get; set; }
            public DateTime CreatedOn { get; set; }
            public bool IsNamePublic { get; set; }
            public string Status { get; set; }
            public bool IsLocationMatch { get; set; }
            public bool IsSectorMatch { get; set; }
            public bool IsJobRoleMatch { get; set; }
            public bool IsLevelMatch { get; set; }

            public static implicit operator Application(GetApplicationsResponse.Application x)
            {
                return new Application
                {
                    Id = x.Id,
                    DasAccountName = x.DasAccountName,
                    PledgeId = x.PledgeId,
                    StandardDuration = x.StandardDuration,
                    StartDate = x.StartDate,
                    Amount = x.Amount,
                    TotalAmount = x.TotalAmount,
                    HasTrainingProvider = x.HasTrainingProvider,
                    CreatedOn = x.CreatedOn,
                    IsNamePublic = x.IsNamePublic,
                    Status = x.Status
                };
            }
        }
    }
}