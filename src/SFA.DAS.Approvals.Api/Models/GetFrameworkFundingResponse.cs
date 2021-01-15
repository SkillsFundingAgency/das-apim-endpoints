using System;
using SFA.DAS.Approvals.InnerApi.Responses;

namespace SFA.DAS.Approvals.Api.Models
{
    public class GetFrameworkFundingResponse
    {
        public DateTime? EffectiveTo { get ; set ; }
        public DateTime EffectiveFrom { get ; set ; }
        public int MaxEmployerLevyCap { get; set; }
        public static implicit operator GetFrameworkFundingResponse(GetFrameworksListItem.FundingPeriod source)
        {
            return new GetFrameworkFundingResponse
            {
                EffectiveFrom = source.EffectiveFrom,
                EffectiveTo = source.EffectiveTo,
                MaxEmployerLevyCap = source.MaxEmployerLevyCap
            };
        }
    }
}