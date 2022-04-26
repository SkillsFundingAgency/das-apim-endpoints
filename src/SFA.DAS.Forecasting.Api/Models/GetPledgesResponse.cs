using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Forecasting.Application.Pledges.Queries.GetPledges;

namespace SFA.DAS.Forecasting.Api.Models
{
    public class GetPledgesResponse
    {
        public IEnumerable<Pledge> Pledges { get; set; }
        public int TotalPledges { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }

        public class Pledge
        {
            public int Id { get; set; }
            public long AccountId { get; set; }
        }

        public static implicit operator GetPledgesResponse(GetPledgesQueryResult source)
        {
            return new GetPledgesResponse
            {
                Page = source.Page,
                PageSize = source.PageSize,
                TotalPages = source.TotalPages,
                TotalPledges = source.TotalPledges,
                Pledges = source.Pledges.Select(p => new Pledge
                {
                    Id = p.Id,
                    AccountId = p.AccountId
                })
            };
        }
    }
}
