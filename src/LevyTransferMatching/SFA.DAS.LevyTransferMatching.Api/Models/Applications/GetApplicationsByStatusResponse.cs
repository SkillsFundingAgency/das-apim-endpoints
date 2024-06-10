using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetApplicationsByStatus;
using SFA.DAS.LevyTransferMatching.Models;

namespace SFA.DAS.LevyTransferMatching.Api.Models.Applications
{
    public class GetApplicationsByStatusResponse
    {
        public IEnumerable<Application> Applications { get; set; }

        public static implicit operator GetApplicationsByStatusResponse(GetApplicationsByStatusResult source)
        {
            return new GetApplicationsByStatusResponse
            {
                Applications = source.Applications.Select(x => (Application)x)
            };
        }

        public class Application
        {
            public int Id { get; set; }
            public string DasAccountName { get; set; }
            public int PledgeId { get; set; }
            public int StandardDuration { get; set; }
            public DateTime StartDate { get; set; }
            public int Amount { get; set; }
            public int TotalAmount { get; set; }
            public int CurrentFinancialYearAmount { get; set; }
            public bool HasTrainingProvider { get; set; }
            public DateTime CreatedOn { get; set; }
            public bool IsNamePublic { get; set; }
            public string Status { get; set; }
            public bool IsLocationMatch { get; set; }
            public bool IsSectorMatch { get; set; }
            public bool IsJobRoleMatch { get; set; }
            public bool IsLevelMatch { get; set; }
            public int MatchPercentage { get; set; }
            public string EmployerAccountName { get; set; }
            public IEnumerable<InnerApi.Responses.GetApplicationResponse.ApplicationLocation> Locations { get; set; }
            public IEnumerable<string> Sectors { get; set; }
            public int NumberOfApprentices { get; set; }
            public string Details { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public IEnumerable<string> EmailAddresses { get; set; }
            public string BusinessWebsite { get; set; }
            public string JobRole { get; set; }
            public int PledgeRemainingAmount { get; set; }
            public int StandardMaxFunding { get; set; }
            public int StandardLevel { get; set; }
            public List<LocationDataItem> PledgeLocations { get; set; }
            public string AdditionalLocations { get; set; }
            public string SpecificLocation { get; set; }

            public static implicit operator Application(GetApplicationsByStatusResult.Application x)
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
                    Status = x.Status,
                    EmployerAccountName = x.EmployerAccountName,
                    Details = x.Details,
                    EmailAddresses = x.EmailAddresses,
                    BusinessWebsite = x.BusinessWebsite,
                    PledgeRemainingAmount = x.PledgeRemainingAmount,
                    StandardMaxFunding = x.StandardMaxFunding,
                    JobRole = x.JobRole,
                    FirstName = x.FirstName,
                    Sectors = x.Sectors,
                    NumberOfApprentices = x.NumberOfApprentices,
                    LastName = x.LastName,
                    Locations = x.Locations,
                    IsJobRoleMatch = x.IsJobRoleMatch,
                    IsLevelMatch = x.IsLevelMatch,
                    IsLocationMatch = x.IsLocationMatch,
                    IsSectorMatch = x.IsSectorMatch,
                    MatchPercentage = x.MatchPercentage,
                    StandardLevel = x.StandardLevel,
                    PledgeLocations = x?.PledgeLocations?.Select(o => new LocationDataItem
                    {
                        Id = x.PledgeId,
                        Name = o.Name
                    }).ToList(),
                    SpecificLocation = x.SpecificLocation,
                    AdditionalLocations = x.AdditionalLocations
                };
            }
        }
    }
}
