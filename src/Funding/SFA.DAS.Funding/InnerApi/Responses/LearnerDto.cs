using System.Collections.Generic;

namespace SFA.DAS.Funding.InnerApi.Responses
{
    public class LearnerDto
    {
        public string Uln { get; set; }
        public FundingType FundingType { get; set; }
        public List<OnProgrammeEarningDto> OnProgrammeEarnings { get; set; }
        public decimal TotalOnProgrammeEarnings { get; set; }
    }
}