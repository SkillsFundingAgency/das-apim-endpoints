using System;
using System.Collections.Generic;
using SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetApplications;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.LevyTransferMatching.Api.Models.Pledges
{
    public class GetApplicationsResponse
    {
        public Standard Standard { get; set; }
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

            public static implicit operator Application(GetApplicationsQueryResult.Application application)
            {
                return new Application
                {
                    Id = application.Id,
                    DasAccountName = application.DasAccountName,
                    PledgeId = application.PledgeId,
                    StandardDuration = application.StandardDuration,
                    StartDate = application.StartDate,
                    TotalAmount = application.TotalAmount,
                    Amount = application.Amount,
                    HasTrainingProvider = application.HasTrainingProvider,
                    CreatedOn = application.CreatedOn,
                    IsNamePublic = application.IsNamePublic,
                    Status = application.Status,
                    IsLocationMatch = application.IsLocationMatch,
                    IsSectorMatch = application.IsSectorMatch,
                    IsJobRoleMatch = application.IsJobRoleMatch,
                    IsLevelMatch = application.IsLevelMatch
                };
            }
        }
    }
}
