﻿using System.Collections.Generic;
using SFA.DAS.LevyTransferMatching.Models.ReferenceData;

namespace SFA.DAS.LevyTransferMatching.Api.Models.Pledges
{
    public class GetLevelResponse
    {
        public IEnumerable<ReferenceDataItem> Levels { get; set; }
    }
}
