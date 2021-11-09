using System;
using System.Collections.Generic;
using System.Text;
using SFA.DAS.LevyTransferMatching.Models.ReferenceData;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetApplicationsForDownload
{
    public class GetApplicationsForDownloadQueryResult
    {
        public IEnumerable<ApplicationForDownloadModel> Applications { get; set; }
    }
    
    public class ApplicationForDownloadModel
    {
        public int PledgeId { get; set; }
        public long ApplicationId { get; set; }
        public DateTime DateApplied { get; set; }
        public string Status { get; set; }
        public string EmployerAccountName { get; set; }
        public IEnumerable<string> Locations { get; set; }
        public IEnumerable<string> Sectors { get; set; }
        public int NumberOfApprentices { get; set; }
        public DateTime StartBy { get; set; }
        public bool HasTrainingProvider { get; set; }
        public string AboutOpportunity { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IEnumerable<string> EmailAddresses { get; set; }
        public string BusinessWebsite { get; set; }
        public bool IsLocationMatch { get; set; }
        public bool IsSectorMatch { get; set; }
        public bool IsJobRoleMatch { get; set; }
        public bool IsLevelMatch { get; set; }
        public Standard Standard { get; set; }
        public int EstimatedDurationMonths { get; set; }
        public int MaxFunding { get; set; }
        public int PledgeRemainingAmount { get; set; }
        public int Amount { get; set; }
        public string JobRole { get; set; }
    }
}
