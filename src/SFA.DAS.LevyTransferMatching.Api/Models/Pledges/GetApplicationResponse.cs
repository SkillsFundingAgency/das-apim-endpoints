using SFA.DAS.LevyTransferMatching.Application.Queries.GetApplication;
using SFA.DAS.LevyTransferMatching.Models.ReferenceData;
using System;
using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Api.Models.Pledges
{
    public class GetApplicationResponse
    {
        public string Location { get; set; }
        public IEnumerable<string> Sector { get; set; }
        public string TypeOfJobRole { get; set; }
        public int Level { get; set; }
        public int NumberOfApprentices { get; set; }
        public int EstimatedDurationMonths { get; set; }
        public int MaxFunding { get; set; }
        public int Amount { get; set; }
        public DateTime StartBy { get; set; }
        public bool HasTrainingProvider { get; set; }
        public string AboutOpportunity { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IEnumerable<string> EmailAddresses { get; set; }
        public DateTime CreatedOn { get; set; }
        public string BusinessWebsite { get; set; }
        public string EmployerAccountName { get; set; }
        public IEnumerable<string> PledgeSectors { get; set; }
        public IEnumerable<string> PledgeLevels { get; set; }
        public IEnumerable<string> PledgeJobRoles { get; set; }
        public IEnumerable<string> PledgeLocations { get; set; }
        public int PledgeRemainingAmount { get; set; }
        public string Status { get; set; }
        public List<ReferenceDataItem> AllJobRoles { get; set; }
        public List<ReferenceDataItem> AllSectors { get; set; }
        public List<ReferenceDataItem> AllLevels { get; set; }
        public IEnumerable<string> Locations { get; set; }
        public string AdditionalLocation { get; set; }
        public string SpecificLocation { get; set; }
        public bool AutomaticApproval { get; set; }
        public bool IsLocationMatch { get; set; }
        public bool IsSectorMatch { get; set; }
        public bool IsJobRoleMatch { get; set; }
        public bool IsLevelMatch { get; set; }
        public int MatchPercentage { get; set; }

        public static implicit operator GetApplicationResponse(GetApplicationResult result)
        {
            return new GetApplicationResponse
            {
                AboutOpportunity = result.AboutOpportunity,
                BusinessWebsite = result.BusinessWebsite,
                EmailAddresses = result.EmailAddresses,
                CreatedOn = result.CreatedOn,
                EstimatedDurationMonths = result.EstimatedDurationMonths,
                MaxFunding = result.MaxFunding,
                Amount = result.Amount,
                FirstName = result.FirstName,
                HasTrainingProvider = result.HasTrainingProvider,
                LastName = result.LastName,
                Level = result.Level,
                Location = result.Location,
                NumberOfApprentices = result.NumberOfApprentices,
                Sector = result.Sector,
                StartBy = result.StartBy,
                TypeOfJobRole = result.TypeOfJobRole,
                EmployerAccountName = result.EmployerAccountName,
                PledgeSectors = result.PledgeSectors,
                PledgeLevels = result.PledgeLevels,
                PledgeJobRoles = result.PledgeJobRoles,
                PledgeLocations = result.PledgeLocations,
                PledgeRemainingAmount = result.PledgeRemainingAmount,
                Status = result.Status,
                AllJobRoles = result.AllJobRoles,
                AllSectors = result.AllSectors,
                AllLevels = result.AllLevels,
                Locations = result.Locations,
                AdditionalLocation = result.AdditionalLocation,
                SpecificLocation = result.SpecificLocation,
                AutomaticApproval = result.AutomaticApproval,
                IsJobRoleMatch = result.MatchJobRole,
                IsLevelMatch = result.MatchLevel,
                IsLocationMatch = result.MatchLocation,
                IsSectorMatch = result.MatchSector,
                MatchPercentage = result.MatchPercentage
            };
        }
    }
}
