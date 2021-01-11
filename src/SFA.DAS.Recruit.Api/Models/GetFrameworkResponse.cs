﻿using SFA.DAS.Recruit.InnerApi.Responses;

namespace SFA.DAS.Recruit.Api.Models
{
    public class GetFrameworkResponse
    {
        public string Id { get; set; }
        public string FrameworkName { get; set; }
        public string PathwayName { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
        public int FrameworkCode { get; set; }
        public int ProgType { get; set; }
        public int PathwayCode { get; set; }
        public int MaxFunding { get; set; }
        public int Duration { get; set; }

        public static implicit operator GetFrameworkResponse(GetFrameworksListItem source)
        {
            return new GetFrameworkResponse
            {
                Id = source.Id,
                Level = source.Level,
                FrameworkName = source.FrameworkName,
                PathwayName = source.PathwayName,
                FrameworkCode = source.FrameworkCode,
                MaxFunding = source.CurrentFundingCap,
                Title = source.Title,
                PathwayCode = source.PathwayCode,
                ProgType = source.ProgType,
                Duration = source.Duration
            };
        }
    }
}