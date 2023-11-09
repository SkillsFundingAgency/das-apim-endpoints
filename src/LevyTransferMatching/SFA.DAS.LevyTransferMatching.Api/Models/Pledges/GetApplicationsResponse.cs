using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.LevyTransferMatching.Api.Models.Opportunity;
using SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetApplications;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LevyTransferMatching;

namespace SFA.DAS.LevyTransferMatching.Api.Models.Pledges
{
    public class GetApplicationsResponse
    {
        public IEnumerable<Application> Applications { get; set; }

        public string PledgeStatus { get; set; }
        public int PledgeRemainingAmount { get; set; }
        public int PledgeTotalAmount { get; set; }
        public AutomaticApprovalOption AutomaticApprovalOption { get; set; }

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
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public IEnumerable<string> EmailAddresses { get; set; }
            public string BusinessWebsite { get; set; }
            public string JobRole { get; set; }
            public int PledgeRemainingAmount { get; set; }
            public int MaxFunding { get; set; }
            public string Details { get; set; }
            public int Level { get; set; }
            public string EmployerAccountName { get; set; }
            public IEnumerable<GetApplyResponse.PledgeLocation> PledgeLocations { get; set; }
            public IEnumerable<InnerApi.Responses.GetApplicationResponse.ApplicationLocation> Locations { get; set; }
            public string AdditionalLocations { get; set; }
            public string SpecificLocation { get; set; }
            public int NumberOfApprentices { get; set; }
            public IEnumerable<string> Sectors { get; set; }
        }

        public static explicit operator GetApplicationsResponse(GetApplicationsQueryResult v)
        {
            return new GetApplicationsResponse
            {
                PledgeStatus = v.PledgeStatus,
                PledgeRemainingAmount = v.RemainingAmount,
                PledgeTotalAmount = v.TotalAmount,
                AutomaticApprovalOption = v.AutomaticApprovalOption,
                Applications = v.Applications.Select(application => new Application
                {
                    Id = application.Id,
                    DasAccountName = application.DasAccountName,
                    PledgeId = application.PledgeId,
                    StandardDuration = application.StandardDuration,
                    StartDate = application.StartDate,
                    TotalAmount = application.TotalAmount,
                    Amount = application.Amount,
                    CurrentFinancialYearAmount = application.CurrentFinancialYearAmount,
                    HasTrainingProvider = application.HasTrainingProvider,
                    CreatedOn = application.CreatedOn,
                    IsNamePublic = application.IsNamePublic,
                    Status = application.Status,
                    IsLocationMatch = application.IsLocationMatch,
                    IsSectorMatch = application.IsSectorMatch,
                    IsJobRoleMatch = application.IsJobRoleMatch,
                    IsLevelMatch = application.IsLevelMatch,
                    EmailAddresses = application.EmailAddresses,
                    JobRole = application.JobRole,
                    BusinessWebsite = application.BusinessWebsite,
                    PledgeRemainingAmount = application.PledgeRemainingAmount,
                    MaxFunding = application.StandardMaxFunding,
                    FirstName = application.FirstName,
                    LastName = application.LastName,
                    Details = application.Details,
                    Level = application.StandardLevel,
                    EmployerAccountName = application.EmployerAccountName,
                    PledgeLocations = application.PledgeLocations.Select(o => new GetApplyResponse.PledgeLocation
                    {
                        Id = o.Id,
                        Name = o.Name
                    }).ToList(),
                    Locations = application.Locations,
                    SpecificLocation = application.SpecificLocation,
                    AdditionalLocations = application.AdditionalLocations,
                    NumberOfApprentices = application.NumberOfApprentices,
                    Sectors = application.Sectors
                    })
            };
        }
    }
}
