using SFA.DAS.Vacancies.InnerApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.Api.Models
{
    public class GetVacancyCourseItem
    {
        public int LarsCode { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
        public string Route { get; set; }
        public static implicit operator GetVacancyCourseItem (GetVacanciesListItem source)
        {
            if (source.StandardLarsCode == null)
            {
                return new GetVacancyCourseItem();
            }
            return new GetVacancyCourseItem
            {
                LarsCode = source.StandardLarsCode.Value,
                Level = source.CourseLevel,
                Route = source.Route,
                Title = $"{source.CourseTitle} (level {source.CourseLevel})"
            };
        }
    }
}