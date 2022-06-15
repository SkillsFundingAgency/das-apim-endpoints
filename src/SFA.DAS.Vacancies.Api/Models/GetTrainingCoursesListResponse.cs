using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Vacancies.Application.TrainingCourses.Queries;
using SFA.DAS.Vacancies.InnerApi.Responses;

namespace SFA.DAS.Vacancies.Api.Models
{
    public class GetTrainingCoursesListResponse
    {
        public List<GetTrainingCoursesListResponseItem> TrainingCourses { get; set; }

        public static implicit operator GetTrainingCoursesListResponse(GetTrainingCoursesQueryResult source)
        {
            return new GetTrainingCoursesListResponse
            {
                TrainingCourses = source.TrainingCourses.Select(c => (GetTrainingCoursesListResponseItem)c).ToList()
            };
        }
    }

    public class GetTrainingCoursesListResponseItem
    {
        public int LarsCode { get; set; }
        public string Title { get; set; }
        public string Route { get; set; }


        public static implicit operator GetTrainingCoursesListResponseItem(GetStandardsListItem source)
        {
            return new GetTrainingCoursesListResponseItem
            {
                LarsCode = source.LarsCode,
                Title = $"{source.Title} (level {source.Level})",
                Route = source.Route
            };
        }
    }
}