using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.ManageApprenticeships.Application.Queries.GetPledges;

namespace SFA.DAS.ManageApprenticeships.Api.Models
{
    public class GetPledgesResponse
    {
        public IEnumerable<Pledge> Pledges { get; set; }
        public int TotalPledges { get; set; }

        public class Pledge
        {
            public int Id { get; set; }
            public long AccountId { get; set; }
            public int Amount { get; set; }
            public bool IsNamePublic { get; set; }
            public string DasAccountName { get; set; }
            public DateTime CreatedOn { get; set; }
            public IEnumerable<string> Sectors { get; set; }
            public IEnumerable<string> JobRoles { get; set; }
            public IEnumerable<string> Levels { get; set; }

            public static implicit operator Pledge(SharedOuterApi.InnerApi.Responses.GetPledgesResponse.Pledge pledge)
            {
                return new Pledge()
                {
                    AccountId = pledge.AccountId,
                    Amount = pledge.Amount,
                    CreatedOn = pledge.CreatedOn,
                    DasAccountName = pledge.DasAccountName,
                    Id = pledge.Id,
                    IsNamePublic = pledge.IsNamePublic,
                    JobRoles = pledge.JobRoles,
                    Levels = pledge.Levels,
                    Sectors = pledge.Sectors,
                };
            }
        }

        public static implicit operator GetPledgesResponse(GetPledgesQueryResult source)
        {
            return new GetPledgesResponse
            {
                TotalPledges = source.TotalPledges,
                Pledges = source.Pledges.Select(x => (Pledge)x),
            };
        }
    }
}