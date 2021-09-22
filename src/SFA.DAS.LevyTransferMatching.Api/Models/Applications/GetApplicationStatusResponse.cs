using System;
using System.Collections.Generic;
using SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetApplicationStatus;
using SFA.DAS.LevyTransferMatching.Models.ReferenceData;

namespace SFA.DAS.LevyTransferMatching.Api.Models.Applications
{
    public class GetApplicationStatusResponse
    {
        public IEnumerable<string> Sectors { get; set; }
        public IEnumerable<string> Levels { get; set; }
        public IEnumerable<string> JobRoles { get; set; }
        public IEnumerable<string> PledgeLocations { get; set; }
        public List<ReferenceDataItem> AllSectors { get; set; }
        public List<ReferenceDataItem> AllLevels { get; set; }
        public List<ReferenceDataItem> AllJobRoles { get; set; }
        public int RemainingAmount { get; set; }
        public bool IsNamePublic { get; set; }
        public string EmployerAccountName { get; set; }
        public string Status { get; set; }
        public string JobRole { get; set; }
        public int Level { get; set; }
        public int NumberOfApprentices { get; set; }
        public int Amount { get; set; }
        public DateTime StartBy { get; set; }
        public int OpportunityId { get; set; }

        public static implicit operator GetApplicationStatusResponse(GetApplicationStatusResult result)
        {
            return new GetApplicationStatusResponse()
            {
                AllJobRoles = result.AllJobRoles,
                AllLevels = result.AllLevels,
                AllSectors = result.AllSectors,
                Amount = result.Amount,
                EmployerAccountName = result.EmployerAccountName,
                IsNamePublic = result.IsNamePublic,
                JobRole = result.JobRole,
                JobRoles = result.JobRoles,
                Level = result.Level,
                Levels = result.Levels,
                PledgeLocations = result.PledgeLocations,
                NumberOfApprentices = result.NumberOfApprentices,
                RemainingAmount = result.RemainingAmount,
                Sectors = result.Sectors,
                StartBy = result.StartBy,
                Status = result.Status,
                OpportunityId = result.OpportunityId,
            };
        }
    }
}