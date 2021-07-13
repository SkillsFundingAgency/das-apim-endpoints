using SFA.DAS.LevyTransferMatching.Models.ReferenceData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Api.Models
{
    public class GetSectorResponse
    {
        public IEnumerable<ReferenceDataItem> Sectors { get; set; }
        public PledgeDto Opportunity { get; set; }
    }
}
