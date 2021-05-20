using SFA.DAS.EmployerDemand.Application.Demand.Commands.VerifyEmployerDemand;

namespace SFA.DAS.EmployerDemand.Api.Models
{
    public class VerifyCourseDemandResponse : GetCourseDemandResponse
    {
        public GetCourseListItem TrainingCourse { get; set; }
        public GetLocationSearchResponseItem Location { get; set; }
        
        public static implicit operator VerifyCourseDemandResponse(VerifyEmployerDemandCommandResult source)
        {
            if (source?.EmployerDemand == null)
            {
                return null;
            }
            return new VerifyCourseDemandResponse
            {
                Id = source.EmployerDemand.Id,
                OrganisationName = source.EmployerDemand.OrganisationName,
                ContactEmail = source.EmployerDemand.ContactEmailAddress,
                NumberOfApprentices = source.EmployerDemand.NumberOfApprentices,
                EmailVerified = source.EmployerDemand.EmailVerified,
                Location = source.EmployerDemand.Location,
                TrainingCourse = source.EmployerDemand.Course
            };
        }
    }
}