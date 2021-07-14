using System.Collections.Generic;
using SFA.DAS.LevyTransferMatching.Models.ReferenceData;

namespace SFA.DAS.LevyTransferMatching.Api.Models
{
    public class GetCreateResponse
    {
        public IEnumerable<ReferenceDataItem> Levels { get; set; }
        public IEnumerable<ReferenceDataItem> Sectors { get; set; }
        public IEnumerable<ReferenceDataItem> JobRoles { get; set; }
    }
}
