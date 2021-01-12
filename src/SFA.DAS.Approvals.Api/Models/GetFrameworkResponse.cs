using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Approvals.InnerApi.Responses;

namespace SFA.DAS.Approvals.Api.Models
{
    public class GetFrameworkResponse
    {
        public string Id { get ; set ; }
        public DateTime? EffectiveTo { get ; set ; }
        public DateTime EffectiveFrom { get ; set ; }
        public decimal CurrentFundingCap { get ; set ; }
        public int Duration { get ; set ; }
        public int PathwayCode { get ; set ; }
        public int ProgType { get ; set ; }
        public int FrameworkCode { get ; set ; }
        public int Level { get ; set ; }
        public string Title { get ; set ; }
        public string PathwayName { get ; set ; }
        public string FrameworkName { get ; set ; }
        public bool IsActiveFramework { get ; set ; }
        public List<GetFrameworkFundingResponse> FundingPeriods { get ; set ; }

        public static implicit operator GetFrameworkResponse(GetFrameworksListItem source)
        {
            return new GetFrameworkResponse
            {
                Id = source.Id,
                FrameworkName = source.FrameworkName,
                PathwayName = source.PathwayName,
                Title = source.Title,
                Level = source.Level,
                FrameworkCode = source.FrameworkCode,
                ProgType = source.ProgType,
                PathwayCode = source.PathwayCode,
                Duration = source.Duration,
                CurrentFundingCap = source.CurrentFundingCap,
                EffectiveFrom = source.EffectiveFrom,
                EffectiveTo = source.EffectiveTo,
                IsActiveFramework = source.IsActiveFramework,
                FundingPeriods = source.FundingPeriods.Select(c=>(GetFrameworkFundingResponse)c).ToList()
                
            };
        }
    }
}