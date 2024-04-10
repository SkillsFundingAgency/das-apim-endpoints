using System;
using SFA.DAS.EmployerDemand.InnerApi.Responses;

namespace SFA.DAS.EmployerDemand.Api.Models
{
    public class GetCourseListItem
    {
        public string Title { get ; set ; }
        public int Level { get ; set ; }
        public int Id { get ; set ; }
        public string Route { get; set; }
        public DateTime? LastStartDate { get; set; }

        public static implicit operator GetCourseListItem(GetStandardsListItem standard)
        {
            return new GetCourseListItem
            {
                Id = standard.LarsCode,
                Level = standard.Level,
                Title = standard.Title,
                Route = standard.Route,
                LastStartDate = standard.StandardDates.LastDateStarts
            };
        }

        public static implicit operator GetCourseListItem(EmployerDemandCourse source)
        {
            return new GetCourseListItem
            {
                Id = source.Id,
                Title = source.Title,
                Level = source.Level,
                Route = source.Route,
            };
        }
    }
}