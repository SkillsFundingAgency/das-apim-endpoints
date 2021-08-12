﻿using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Models
{
    public class Standard
    {
        public string StandardUId { get; set; }
        public int LarsCode { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
        public IEnumerable<ApprenticeshipFunding> ApprenticeshipFunding { get; set; }
    }
}
