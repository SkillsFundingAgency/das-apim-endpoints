using System;
using SFA.DAS.EmployerDemand.InnerApi.Responses;

namespace SFA.DAS.EmployerDemand.Domain.Models
{
    public class EmployerDemand
    {
        public Guid Id { get; set; }
        public string OrganisationName { get; set; }
        public string ContactEmail { get; set; }
        public bool EmailVerified { get; set; }
        public int NumberOfApprentices { get; set; }
        public GetStandardsListItem Standard { get; set; }
        public LocationItem Location { get; set; }
    }
}