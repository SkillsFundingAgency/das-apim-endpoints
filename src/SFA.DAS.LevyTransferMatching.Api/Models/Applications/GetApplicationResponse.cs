using System;
using System.Collections.Generic;
using SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetApplication;
using SFA.DAS.LevyTransferMatching.Models.ReferenceData;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.LevyTransferMatching.Api.Models.Applications
{
    public class GetApplicationResponse
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
        public Standard Standard { get; set; }
        public string PledgeEmployerAccountName { get; set; }
        public int PledgeAmount { get; set; }
        public long SenderEmployerAccountId { get; set; }
        public int AmountUsed { get; set; }
        public int NumberOfApprenticesUsed { get; set; }

        public static implicit operator GetApplicationResponse(GetApplicationResult result)
        {
            return new GetApplicationResponse()
            {
                AllJobRoles = result.AllJobRoles,
                AllLevels = result.AllLevels,
                AllSectors = result.AllSectors,
                Amount = result.Amount,
                PledgeAmount = result.PledgeAmount,
                EmployerAccountName = result.EmployerAccountName,
                PledgeEmployerAccountName = result.PledgeEmployerAccountName,
                IsNamePublic = result.IsNamePublic,
                JobRoles = result.JobRoles,
                Levels = result.Levels,
                PledgeLocations = result.PledgeLocations,
                NumberOfApprentices = result.NumberOfApprentices,
                RemainingAmount = result.RemainingAmount,
                Sectors = result.Sectors,
                StartBy = result.StartBy,
                Status = result.Status,
                OpportunityId = result.OpportunityId,
                Standard = result.Standard,
                SenderEmployerAccountId = result.SenderEmployerAccountId,
                AmountUsed = result.AmountUsed,
                NumberOfApprenticesUsed = result.NumberOfApprenticesUsed
            };
        }
    }
}