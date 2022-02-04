using SFA.DAS.ApprenticeFeedback.InnerApi.Responses;
using System.Collections.Generic;

namespace SFA.DAS.ApprenticeFeedback.Api.Models
{
    public class CacheStandardsCommand
    {
        public IEnumerable<Standard> Standards { get; set; }
    }

    public class Standard
    {
        public string StandardUId { get; set; }
        public string StandardReference { get; set; }
        public string StandardName { get; set; }
        public int LarsCode { get; set; }

        public static implicit operator Standard(GetStandardsListItem source)
        {
            return new Standard
            {
                StandardUId = source.StandardUId,
                StandardReference = source.StandardReference,
                StandardName = source.StandardName,
                LarsCode= source.LarsCode
            };
        }
    }
}
