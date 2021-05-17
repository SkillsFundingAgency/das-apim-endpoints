using SFA.DAS.EmployerDemand.Application.Demand.Commands.VerifyEmployerDemand;
using SFA.DAS.EmployerDemand.InnerApi.Responses;

namespace SFA.DAS.EmployerDemand.Api.Models
{
    public class GetCourseListItem
    {
        public string Title { get ; set ; }
        public int Level { get ; set ; }
        public int Id { get ; set ; }
        public string Sector { get; set; }

        public static implicit operator GetCourseListItem(GetStandardsListItem standard)
        {
            return new GetCourseListItem
            {
                Id = standard.LarsCode,
                Level = standard.Level,
                Title = standard.Title,
                Sector = standard.Route
            };
        }

        public static implicit operator GetCourseListItem(VerifyEmployerDemandCommandResult source)
        {
            return new GetCourseListItem
            {
                Id = source.EmployerDemand.CourseId,
                Title = source.EmployerDemand.CourseTitle,
                Level = source.EmployerDemand.CourseLevel,
                Sector = source.EmployerDemand.CourseRoute,
            };
        }
    }
}