using System;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetRestartEmployerDemand;
using SFA.DAS.EmployerDemand.InnerApi.Responses;

namespace SFA.DAS.EmployerDemand.Api.Models
{
    public class GetRestartCourseDemandResponse : GetCourseDemandResponse
    {
        public GetCourseListItem TrainingCourse { get; set; }
        public GetLocationSearchResponseItem Location { get; set; }
        public bool RestartDemandExists { get; set; }
        
        public static implicit operator GetRestartCourseDemandResponse(GetRestartEmployerDemandQueryResult source)
        {
            return new GetRestartCourseDemandResponse
            {
                Id = source.EmployerDemand.Id,
                ContactEmail = source.EmployerDemand.ContactEmailAddress,
                OrganisationName = source.EmployerDemand.OrganisationName,
                EmailVerified = source.EmployerDemand.EmailVerified,
                NumberOfApprentices = source.EmployerDemand.NumberOfApprentices,
                RestartDemandExists = source.RestartDemandExists,
                TrainingCourse = BuildCourse(source.EmployerDemand.Course, source.LastStartDate),
                Location = source.EmployerDemand.Location
                    
            };
        }

        private static GetCourseListItem BuildCourse(EmployerDemandCourse course, DateTime? lastStartDate)
        {
            return new GetCourseListItem
            {
                Id = course.Id,
                Level = course.Level,
                Route = course.Route,
                Title = course.Title,
                LastStartDate = lastStartDate
            };
        }
    }
}