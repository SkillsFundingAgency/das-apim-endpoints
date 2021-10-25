﻿using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Vacancies.Manage.Application.TrainingCourses.Queries;
using SFA.DAS.Vacancies.Manage.InnerApi.Responses;

namespace SFA.DAS.Vacancies.Manage.Api.Models
{
    public class GetTrainingCoursesListResponse
    {
        public List<GetTrainingCoursesListResponseItem> TrainingCourses { get; set; }

        public static implicit operator GetTrainingCoursesListResponse(GetTrainingCoursesQueryResult source)
        {
            return new GetTrainingCoursesListResponse
            {
                TrainingCourses = source.TrainingCourses.Select(c=>(GetTrainingCoursesListResponseItem)c).ToList()
            };
        }
    }

    public class GetTrainingCoursesListResponseItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Route { get ; set ; }

        public static implicit operator GetTrainingCoursesListResponseItem(GetStandardsListItem source)
        {
            return new GetTrainingCoursesListResponseItem
            {
                Id = source.LarsCode,
                Title = $"{source.Title} (level {source.Level})",
                Route = source.Route
            };
        }
    }
}