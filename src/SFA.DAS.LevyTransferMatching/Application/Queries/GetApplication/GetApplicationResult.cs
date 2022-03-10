using System;
using System.Collections.Generic;
using SFA.DAS.LevyTransferMatching.Models.ReferenceData;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetApplication
{
    public class GetApplicationResult
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
        public string SenderEmployerAccountName { get; set; }
        public IEnumerable<string> Locations { get; set; }
        public string AdditionalLocation { get; set; }
        public string SpecificLocation { get; set; }
        public IEnumerable<string> PledgeSectors { get; set; }
        public IEnumerable<string> PledgeLevels { get; set; }
        public IEnumerable<string> PledgeJobRoles { get; set; }
        public IEnumerable<string> PledgeLocations { get; set; }
        public int PledgeRemainingAmount { get; set; }
        public string Status { get; set; }
        public List<ReferenceDataItem> AllJobRoles { get; set; }
        public List<ReferenceDataItem> AllSectors { get; set; }
        public List<ReferenceDataItem> AllLevels { get; set; }
        public bool AutomaticApproval { get; set; }
        public bool MatchSector { get; set; }
        public bool MatchJobRole { get; set; }
        public bool MatchLevel { get; set; }
        public bool MatchLocation { get; set; }
        public int MatchPercentage { get; set; }
    }
}