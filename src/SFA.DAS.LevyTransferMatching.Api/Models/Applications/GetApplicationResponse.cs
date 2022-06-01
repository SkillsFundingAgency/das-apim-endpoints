using System;
using System.Collections.Generic;
using SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetApplication;
using SFA.DAS.LevyTransferMatching.Models.ReferenceData;

namespace SFA.DAS.LevyTransferMatching.Api.Models.Applications
{
    public class GetApplicationResponse
    {
        public string StandardTitle { get; set; }
        public int StandardLevel { get; set; }
        public int StandardDuration { get; set; }
        public int StandardMaxFunding { get; set; }
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
        public int NumberOfApprentices { get; set; }
        public int Amount { get; set; }
        public int TotalAmount { get; set; }
        public DateTime StartBy { get; set; }
        public int OpportunityId { get; set; }
        public string PledgeEmployerAccountName { get; set; }
        public int PledgeAmount { get; set; }
        public long SenderEmployerAccountId { get; set; }
        public int AmountUsed { get; set; }
        public int NumberOfApprenticesUsed { get; set; }
        public bool AutomaticApproval { get; set; }
        public bool IsWithdrawableAfterAcceptance { get; set; }

        public static implicit operator GetApplicationResponse(GetApplicationResult result)
        {
            return new GetApplicationResponse
            {
                StandardTitle = result.StandardTitle,
                StandardLevel = result.StandardLevel,
                StandardDuration = result.StandardDuration,
                StandardMaxFunding = result.StandardMaxFunding,
                AllJobRoles = result.AllJobRoles,
                AllLevels = result.AllLevels,
                AllSectors = result.AllSectors,
                Amount = result.Amount,
                TotalAmount = result.TotalAmount,
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
                SenderEmployerAccountId = result.SenderEmployerAccountId,
                AmountUsed = result.AmountUsed,
                NumberOfApprenticesUsed = result.NumberOfApprenticesUsed,
                AutomaticApproval = result.AutomaticApproval,
                IsWithdrawableAfterAcceptance = result.IsWithdrawableAfterAcceptance
            };
        }
    }
}