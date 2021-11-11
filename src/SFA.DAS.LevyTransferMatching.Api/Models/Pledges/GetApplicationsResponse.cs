using System;
using System.Collections.Generic;
using SFA.DAS.LevyTransferMatching.Application.Queries.Shared.GetApplications;
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
            public string Details { get; set; }
            public string StandardId { get; set; }
            public int NumberOfApprentices { get; set; }
            public DateTime StartDate { get; set; }
            public int Amount { get; set; }
            public bool HasTrainingProvider { get; set; }
            public IEnumerable<string> Sectors { get; set; }
            public string Postcode { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string BusinessWebsite { get; set; }
            public IEnumerable<string> EmailAddresses { get; set; }
            public DateTime CreatedOn { get; set; }
            public Standard Standard { get; set; }
            public bool IsNamePublic { get; set; }
            public string Status { get; set; }
            public bool IsLocationMatch { get; set; }
            public bool IsSectorMatch { get; set; }
            public bool IsJobRoleMatch { get; set; }
            public bool IsLevelMatch { get; set; }

            public static implicit operator Application(GetApplicationsQueryResultBase.Application application)
            {
                return new Application
                {
                    Id = application.Id,
                    DasAccountName = application.DasAccountName,
                    PledgeId = application.PledgeId,
                    Details = application.Details,
                    StandardId = application.StandardId,
                    NumberOfApprentices = application.NumberOfApprentices,
                    StartDate = application.StartDate,
                    Amount = application.Amount,
                    HasTrainingProvider = application.HasTrainingProvider,
                    Sectors = application.Sectors,
                    Postcode = application.Postcode,
                    FirstName = application.FirstName,
                    LastName = application.LastName,
                    BusinessWebsite = application.BusinessWebsite,
                    EmailAddresses = application.EmailAddresses,
                    CreatedOn = application.CreatedOn,
                    Standard = application.Standard,
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
