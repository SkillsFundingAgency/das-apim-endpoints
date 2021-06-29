using System.Collections.Generic;
using SFA.DAS.LevyTransferMatching.Models.Tags;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetLevels
{
    public class GetLevelsQueryResult
    {
        public IEnumerable<Tag> Tags { get; set; }
    }
}
