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
        public int LarsCode { get; set; }
        public string StandardUId { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }

        public static implicit operator GetStandardsResponseItem(GetStandardsListItem source)
        {
            return new GetStandardsResponseItem
            {
                LarsCode = source.LarsCode,
                StandardUId = source.StandardUId,
                Title = source.Title,
                Level = source.Level
            };
        }
    }
}