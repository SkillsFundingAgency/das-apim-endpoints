using System;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Api.Models
{
    public class GetTrainingCourseListItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
        public string LevelEquivalent { get; set; }
        public decimal Version { get; set; }
        public long MaxFunding { get; set; }
        public string OverviewOfRole { get; set; }
        public string Keywords { get; set; }
        public int TypicalDuration { get; set; }
        public string Route { get; set; }
        public string TypicalJobTitles { get; set; }
        public string CoreSkillsCount { get; set; }
        public string StandardPageUrl { get; set; }
        public string IntegratedDegree { get; set; }
        public string SectorSubjectAreaTier2Description { get; set; }
        public decimal SectorSubjectAreaTier2 { get; set; }
        public bool OtherBodyApprovalRequired { get; set; }
        public StandardDate StandardDates { get; set; }


        public static implicit operator GetTrainingCourseListItem(GetStandardsListItem source)
        {
            return new GetTrainingCourseListItem
            {
                Id = source.Id,
                Title = source.Title,
                Level = source.Level,
                LevelEquivalent = source.LevelEquivalent,
                Version = source.Version,
                MaxFunding = source.MaxFunding,
                OverviewOfRole = source.OverviewOfRole,
                Keywords = source.Keywords,
                TypicalDuration = source.TypicalDuration,
                Route = source.Route,
                TypicalJobTitles = source.TypicalJobTitles,
                CoreSkillsCount = source.CoreSkillsCount,
                StandardPageUrl = source.StandardPageUrl,
                IntegratedDegree = source.IntegratedDegree,
                SectorSubjectAreaTier2Description = source.SectorSubjectAreaTier2Description,
                SectorSubjectAreaTier2 = source.SectorSubjectAreaTier2,
                OtherBodyApprovalRequired = source.OtherBodyApprovalRequired,
                StandardDates = source.StandardDates
            };
        }
    }

    public class StandardDate
    {
        public DateTime? LastDateStarts { get; set; }

        public DateTime? EffectiveTo { get; set; }

        public DateTime EffectiveFrom { get; set; }

        public static implicit operator StandardDate(InnerApi.Responses.StandardDate source)
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
