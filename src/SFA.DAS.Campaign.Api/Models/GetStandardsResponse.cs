using System.Collections.Generic;
using SFA.DAS.Campaign.InnerApi.Responses;

namespace SFA.DAS.Campaign.Api.Models
{
    public class GetStandardsResponse
    {
        public IEnumerable<GetStandardsResponseItem> Standards { get; set; }
    }

    public class GetStandardsResponseItem
    {
        public int Id { get; set; }
        public int LarsCode { get; set; }
        public string StandardUId { get; set; }

        public static implicit operator GetStandardsResponseItem(GetStandardsListItem source)
        {
            return new GetStandardsResponseItem
            {
                Id = source.Id,
                LarsCode = source.LarsCode,
                StandardUId = source.StandardUId
                
            };
        }
    }
}