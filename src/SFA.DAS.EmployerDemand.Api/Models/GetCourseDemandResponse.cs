using System;
using SFA.DAS.EmployerDemand.InnerApi.Responses;

namespace SFA.DAS.EmployerDemand.Api.Models
{
    public class GetCourseDemandResponse 
    {
        public Guid Id { get; set; }
        public string OrganisationName { get; set; }
        public string ContactEmail { get; set; }
        public int NumberOfApprentices { get; set; }
        public bool EmailVerified { get ; set ; }

        public static implicit operator GetCourseDemandResponse(GetEmployerDemandResponse source)
        {
            if (source == null)
            {
                return null;
            }
            return new GetCourseDemandResponse
            {
                Id = source.Id,
                ContactEmail = source.ContactEmailAddress,
                EmailVerified = source.EmailVerified,
                OrganisationName = source.OrganisationName,
                NumberOfApprentices = source.NumberOfApprentices
            };
        }
    }
}