using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Api.Models
{
    public class GetTrainingCourseListItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
        public string LevelEquivalent { get; set; }
        public string Version { get; set; }
        public int MaxFunding { get; set; }
        public string OverviewOfRole { get; set; }
        public string Keywords { get; set; }
        public int TypicalDuration { get; set; }
        public string Route { get; set; }
        public List<string> TypicalJobTitles { get; set; }
        public List<string> CoreSkills { get; set; }
        public string StandardPageUrl { get; set; }
        public string IntegratedDegree { get; set; }
        public string SectorSubjectAreaTier2Description { get; set; }
        public decimal SectorSubjectAreaTier2 { get; set; }
        public bool OtherBodyApprovalRequired { get; set; }
        public string ApprovalBody { get; set; }
        public StandardDate StandardDates { get; set; }


        public static implicit operator GetTrainingCourseListItem(GetStandardsListItem source)
        {
            return new GetTrainingCourseListItem
            {
                Id = source.LarsCode,
                Title = source.Title,
                Level = source.Level,
                LevelEquivalent = source.LevelEquivalent,
                Version = source.Version,
                MaxFunding = source.MaxFunding,
                OverviewOfRole = source.OverviewOfRole,
                Keywords = source.Keywords,
                TypicalDuration = source.TypicalDuration,
                Route = source.Route,
                TypicalJobTitles = source.TypicalJobTitles.Split('|').Length <=1? new List<string>() : source.TypicalJobTitles.Split('|').OrderBy(x => x).ToList(),
                CoreSkills = source.CoreSkills,
                StandardPageUrl = source.StandardPageUrl,
                IntegratedDegree = source.IntegratedDegree,
                SectorSubjectAreaTier2Description = source.SectorSubjectAreaTier2Description,
                SectorSubjectAreaTier2 = source.SectorSubjectAreaTier2,
                OtherBodyApprovalRequired = source.OtherBodyApprovalRequired,
                ApprovalBody = source.ApprovalBody,
                StandardDates = source.StandardDates
            };
        }
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
