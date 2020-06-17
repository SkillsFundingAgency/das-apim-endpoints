using SFA.DAS.FindApprenticeshipTraining.Application.Domain.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Api.Models
{
    public class GetTrainingCourseListItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
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

        public static implicit operator GetTrainingCourseListItem(GetStandardsListItem source)
        {
            return new GetTrainingCourseListItem
            {
                Id = source.Id,
                Title = source.Title,
                Level = source.Level,
                Version = source.Version,
                MaxFunding = source.MaxFunding,
                OverviewOfRole = source.OverviewOfRole,
                Keywords = source.Keywords,
                TypicalDuration = source.TypicalDuration,
                Route = source.Route,
                TypicalJobTitles = source.TypicalJobTitles,
                CoreSkillsCount = source.CoreSkillsCount,
                StandardPageUrl = source.StandardPageUrl,
                IntegratedDegree = source.IntegratedDegree
            };
        }
    }
}
