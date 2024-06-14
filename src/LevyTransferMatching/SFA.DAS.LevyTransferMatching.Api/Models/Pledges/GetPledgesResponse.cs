using System;
using System.Collections.Generic;
using SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetApplications;
using SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetPledges;

namespace SFA.DAS.LevyTransferMatching.Api.Models.Pledges
{
    public class GetPledgesResponse
    {
        public IEnumerable<Pledge> Pledges { get; set; }
        public decimal? StartingTransferAllowance { get; set; }
        public IEnumerable<PledgeApplication> AcceptedAndApprovedApplications { get; set; }

        public static implicit operator GetPledgesResponse(GetPledgesQueryResult source)
        {
            return new GetPledgesResponse
            {
                AcceptedAndApprovedApplications = source.AcceptedAndApprovedApplications
            };
        }

        public class Pledge
        {
            public int Id { get; set; }
            public long AccountId { get; set; }
            public int Amount { get; set; }
            public int RemainingAmount { get; set; }
            public int ApplicationCount { get; set; }
            public bool IsNamePublic { get; set; }
            public string DasAccountName { get; set; }
            public DateTime CreatedOn { get; set; }
            public IEnumerable<string> JobRoles { get; set; }
            public IEnumerable<string> Levels { get; set; }
            public IEnumerable<string> Sectors { get; set; }
            public string Status { get; set; }

            public static implicit operator Pledge(SharedOuterApi.InnerApi.Responses.GetPledgesResponse.Pledge pledge)
            {
                return new Pledge()
                {
                    Id = pledge.Id,
                    AccountId = pledge.AccountId,
                    Amount = pledge.Amount,
                    CreatedOn = pledge.CreatedOn,
                    IsNamePublic = pledge.IsNamePublic,
                    DasAccountName = pledge.IsNamePublic ? pledge.DasAccountName : "Opportunity",
                    JobRoles = pledge.JobRoles,
                    Levels = pledge.Levels,
                    Sectors = pledge.Sectors,
                    Status = pledge.Status,
                };
            }
        }
    }
}