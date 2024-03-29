﻿using System.Collections.Generic;

namespace SFA.DAS.Funding.Models
{
    public class Learner
    {
        public string Uln { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public FundingType FundingType { get; set; }
        public List<OnProgrammeEarning> OnProgrammeEarnings { get; set; }
        public decimal TotalOnProgrammeEarnings { get; set; }
    }
}
