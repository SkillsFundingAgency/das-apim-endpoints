using System;
using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Api.Models
{
    public class PledgeDto
    {
        public int Id { get; set; }
        public long AccountId { get; set; }
        public int Amount { get; set; }
        public int RemainingAmount { get; set; }
        public bool IsNamePublic { get; set; }
        public string DasAccountName { get; set; }
        public DateTime CreatedOn { get; set; }
        public IEnumerable<string> JobRoles { get; set; }
        public IEnumerable<string> Levels { get; set; }
        public IEnumerable<string> Sectors { get; set; }

        public static implicit operator PledgeDto(LevyTransferMatching.Models.Pledge pledge)
        {
            return new PledgeDto()
            {
                Id = pledge.Id.Value,
                AccountId = pledge.AccountId,
                Amount = pledge.Amount,
                RemainingAmount = pledge.RemainingAmount,
                CreatedOn = pledge.CreatedOn,
                IsNamePublic = pledge.IsNamePublic,
                DasAccountName = pledge.IsNamePublic ? pledge.DasAccountName : "Opportunity",
                JobRoles = pledge.JobRoles,
                Levels = pledge.Levels,
                Sectors = pledge.Sectors,
            };
        }
    }
}