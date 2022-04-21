using System.Collections.Generic;

namespace SFA.DAS.Forecasting.Application.Pledges.Queries.GetPledges
{
    public class GetPledgesQueryResult
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
    }
}