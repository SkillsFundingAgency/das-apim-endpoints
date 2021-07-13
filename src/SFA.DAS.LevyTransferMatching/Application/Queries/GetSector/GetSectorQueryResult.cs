using SFA.DAS.LevyTransferMatching.Models.ReferenceData;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetSector
{
    public class GetSectorQueryResult
    {
        public IEnumerable<ReferenceDataItem> Sectors { get; set; }
    }
}
