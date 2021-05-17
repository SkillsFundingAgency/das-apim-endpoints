using System;

namespace SFA.DAS.EmployerDemand.InnerApi.Responses
{
    public class GetEmployerDemandResponse
    {
        public Guid Id { get; set; }
        public bool EmailVerified { get ; set ; }
        public string ContactEmailAddress { get ; set ; }
        public string OrganisationName { get ; set ; }
        public EmployerDemandCourse Course { get ; set ; }
        
        public Location Location { get ; set ; }
        public int NumberOfApprentices { get ; set ; }
    }
}