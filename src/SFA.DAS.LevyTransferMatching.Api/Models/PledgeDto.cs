using SFA.DAS.LevyTransferMatching.Models;
using System;
using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Api.Models
{
    public class PledgeDto : PledgeReferenceDto
    {
        public string EncodedAccountId { get; set; }

        public int Amount { get; set; }

        public bool IsNamePublic { get; set; }

        public DateTime CreatedOn { get; set; }

        public IEnumerable<string> JobRoles { get; set; }

        public IEnumerable<string> Levels { get; set; }

        public IEnumerable<string> Sectors { get; set; }

        public static implicit operator PledgeDto(Pledge pledge)
        {
            return new PledgeDto()
            {
                Amount = pledge.Amount,
                CreatedOn = pledge.CreatedOn,
                EncodedAccountId = pledge.EncodedAccountId,
                EncodedPledgeId = pledge.EncodedPledgeId,
                IsNamePublic = pledge.IsNamePublic,
                JobRoles = pledge.JobRoles,
                Levels = pledge.Levels,
                Sectors = pledge.Sectors,
            };
        }
    }
}