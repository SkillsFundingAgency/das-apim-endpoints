using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Approvals.InnerApi.Responses;

namespace SFA.DAS.Approvals.Api.Models
{
    public class GetStandardResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Level { get ; set ; }
        public int Duration { get; set; }
        public int MaxFunding { get; set; }
        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public DateTime? LastDateForNewStarts { get ; set ; }
        public List<GetStandardFundingResponse> ApprenticeshipFunding { get; set; }


        public static implicit operator GetStandardResponse(GetStandardsListItem source)
        {
            return new GetStandardResponse
            {
                Id = source.Id,
                Title = source.Title,
                Level = source.Level,
                Duration = source.TypicalDuration,
                MaxFunding = source.MaxFunding,
                EffectiveFrom = source.StandardDates.EffectiveFrom,
                EffectiveTo = source.StandardDates.EffectiveTo,
                LastDateForNewStarts = source.StandardDates.LastDateStarts,
                ApprenticeshipFunding = source.ApprenticeshipFunding.Select(c=>(GetStandardFundingResponse)c).ToList()
                    
            };
        }
    }
}