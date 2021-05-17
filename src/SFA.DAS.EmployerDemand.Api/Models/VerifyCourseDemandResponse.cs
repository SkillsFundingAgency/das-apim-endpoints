using System;
using SFA.DAS.EmployerDemand.Application.Demand.Commands.VerifyEmployerDemand;

namespace SFA.DAS.EmployerDemand.Api.Models
{
    public class VerifyCourseDemandResponse
    {
        public Guid Id { get; set; }
        public string OrganisationName { get; set; }
        public string ContactEmail { get; set; }
        public int NumberOfApprentices { get; set; }
        public GetCourseListItem TrainingCourse { get; set; }
        public GetLocationSearchResponseItem Location { get; set; }
        public bool EmailVerified { get ; set ; }

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
                TrainingCourse = source
            };
        }
    }
}