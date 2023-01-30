using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Funding.Models;

namespace SFA.DAS.Funding.Api.Models
{
    public class LearnerDto
    {
        public string Uln { get; set; }
        public FundingType FundingType { get; set; }
        public List<OnProgrammeEarningDto> OnProgrammeEarnings { get; set; }
        public decimal TotalOnProgrammeEarnings { get; set; }

        public static implicit operator LearnerDto(Learner source)
        {
            return new LearnerDto
            {
                FundingType = (FundingType)Enum.Parse(typeof(FundingType), source.FundingType.ToString()),
                TotalOnProgrammeEarnings = source.TotalOnProgrammeEarnings,
                Uln = source.Uln,
                OnProgrammeEarnings = source.OnProgrammeEarnings.Select(x => (OnProgrammeEarningDto)x).ToList(),
            };
        }
    }
}
