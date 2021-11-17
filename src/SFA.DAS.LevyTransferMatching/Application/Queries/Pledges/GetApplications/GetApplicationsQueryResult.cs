using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.LevyTransferMatching.InnerApi.Responses;
using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.LevyTransferMatching.Models.ReferenceData;
using SFA.DAS.NServiceBus.ClientOutbox;
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
            public string EmployerAccountName { get; set; }
            public IEnumerable<GetApplicationResponse.ApplicationLocation> Locations { get; set; }
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

            public static Application BuildApplication(GetApplicationsResponse.Application application,
                IEnumerable<ReferenceDataItem> roles, Pledge pledgeResponse)
            {
                return new Application
                {
                    Id = application.Id,
                    DasAccountName = application.DasAccountName,
                    PledgeId = application.PledgeId,
                    StandardDuration = application.StandardDuration,
                    StartDate = application.StartDate,
                    Amount = application.Amount,
                    TotalAmount = application.TotalAmount,
                    HasTrainingProvider = application.HasTrainingProvider,
                    CreatedOn = application.CreatedOn,
                    IsNamePublic = application.IsNamePublic,
                    Status = application.Status,
                    EmployerAccountName = pledgeResponse.DasAccountName,
                    Details = application.Details,
                    EmailAddresses = application.EmailAddresses,
                    BusinessWebsite = application.BusinessWebsite,
                    PledgeRemainingAmount = pledgeResponse.RemainingAmount,
                    StandardMaxFunding = application.StandardMaxFunding,
                    JobRole = application.StandardTitle,
                    FirstName = application.FirstName,
                    Sectors = application.Sectors,
                    NumberOfApprentices = application.NumberOfApprentices,
                    LastName = application.LastName,
                    Locations = application.Locations.Select(o => new GetApplicationResponse.ApplicationLocation
                    {
                        Id = o.Id,
                        PledgeLocationId = o.PledgeLocationId
                    }),
                    IsJobRoleMatch = !pledgeResponse.JobRoles.Any() ||
                                     roles.Any(r => r.Description == application.StandardRoute),
                    IsLevelMatch = !pledgeResponse.Levels.Any() || pledgeResponse.Levels
                        .Select(x => char.GetNumericValue(x.Last())).Contains(application.StandardLevel),
                    IsLocationMatch = !pledgeResponse.Locations.Any() || application.Locations.Any(),
                    IsSectorMatch = !pledgeResponse.Sectors.Any() ||
                                    application.Sectors.Any(x => pledgeResponse.Sectors.Contains(x)),
                    StandardLevel = application.StandardLevel,
                    PledgeLocations = pledgeResponse.Locations,
                    SpecificLocation = application.SpecificLocation,
                    AdditionalLocations = application.AdditionalLocations
                };
            }

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
                    Status = x.Status,
                    EmployerAccountName = x.EmployerAccountName,
                    Details = x.Details,
                    EmailAddresses = x.EmailAddresses,
                    BusinessWebsite = x.BusinessWebsite,
                    PledgeRemainingAmount = x.PledgeRemainingAmount,
                    StandardMaxFunding = x.StandardMaxFunding,
                    JobRole = x.StandardTitle,
                    FirstName = x.FirstName,
                    Sectors = x.Sectors,
                    NumberOfApprentices = x.NumberOfApprentices,
                    LastName = x.LastName,
                    Locations = x.Locations.Select(o => new GetApplicationResponse.ApplicationLocation
                    {
                        Id = o.Id,
                        PledgeLocationId = o.PledgeLocationId
                    }),
                    IsJobRoleMatch = x.IsJobRoleMatch,
                    IsLevelMatch = x.IsLevelMatch,
                    IsLocationMatch = x.IsLocationMatch,
                    IsSectorMatch = x.IsSectorMatch,
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