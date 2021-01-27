using System;
using SFA.DAS.Assessors.InnerApi.Responses;

namespace SFA.DAS.Assessors.Api.Models
{
    public class GetCourseListItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
        public int MaxFunding { get; set; }
        public int TypicalDuration { get; set; }
        public StandardDate StandardDates { get; set; }

        public static implicit operator GetCourseListItem(GetStandardsListItem source)
        {
            return new GetCourseListItem
            {
                Id = source.Id,
                Title = source.Title,
                Level = source.Level,
                MaxFunding = source.MaxFunding,
                TypicalDuration = source.TypicalDuration,
                StandardDates = source.StandardDates
            };
        }

        public class StandardDate
        {
            public DateTime? LastDateStarts { get; set; }

            public DateTime? EffectiveTo { get; set; }

            public DateTime EffectiveFrom { get; set; }

            public static implicit operator StandardDate(SharedOuterApi.InnerApi.Responses.StandardDate source)
            {
                return new StandardDate
                {
                    EffectiveFrom = source.EffectiveFrom,
                    EffectiveTo = source.EffectiveTo,
                    LastDateStarts = source.LastDateStarts
                };
            }
        }
    }
}