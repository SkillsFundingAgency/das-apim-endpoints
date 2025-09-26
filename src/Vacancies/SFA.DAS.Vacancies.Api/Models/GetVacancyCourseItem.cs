using SFA.DAS.Vacancies.InnerApi.Responses;

namespace SFA.DAS.Vacancies.Api.Models
{
    public class GetVacancyCourseItem
    {
        /// <summary>
        /// The code from the learning aim reference service (LARS) for the apprenticeship’s training course (almost known as a ‘standard’). Use `GET list of courses` to see all courses and their LARS codes.
        /// </summary>
        public int LarsCode { get; set; }
        /// <summary>
        /// The title of the apprenticeship training course and its level.
        /// </summary>
        /// <example>Furniture restorer (level 3)</example>
        public string Title { get; set; }
        /// <summary>
        /// What level the apprenticeship training course is.
        /// </summary>
        /// <example>3</example>
        public int Level { get; set; }
        /// <summary>
        /// Which route the apprenticeship training course is part of, using the routes from the Institute for Apprenticeships and Technical Education (IfATE). On Find an apprenticeship, we call these categories.
        /// </summary>
        /// <example>Creative and design</example>
        public string Route { get; set; }
        /// <summary>
        /// Will either be `apprenticeship` or `foundationApprenticeship`.
        /// </summary>
        public string Type { get; set; }

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
                Title = $"{source.CourseTitle} (level {source.CourseLevel})",
                Type = source.ApprenticeshipType
            };
        }
    }
}