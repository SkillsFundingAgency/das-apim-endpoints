using System;
using SFA.DAS.Assessors.InnerApi.Responses;

namespace SFA.DAS.Assessors.Api.Models
{
    public class GetCourseListItem
    {
        public string StandardUId { get; set; }
        public string IfateReferenceNumber { get; set; }
        public int LarsCode { get; set; }
        public string Title { get; set; }
        public string Version { get; set; }
        public string Status { get; set; }
        public int Level { get; set; }
        public int MaxFunding { get; set; }
        public int TypicalDuration { get; set; }
        public bool IsActive { get; set; }
        public StandardDate StandardDates { get; set; }

        public static implicit operator GetCourseListItem(GetStandardsListItem source)
        {
            return new GetCourseListItem
            {
                StandardUId = source.StandardUId,
                IfateReferenceNumber = source.IfateReferenceNumber,
                LarsCode = source.LarsCode,
                Title = source.Title,
                Version = source.Version,
                Status = source.Status,
                Level = source.Level,
                MaxFunding = source.MaxFunding,
                TypicalDuration = source.TypicalDuration,
                IsActive = source.IsActive,
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
                if (source == null) return null;
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