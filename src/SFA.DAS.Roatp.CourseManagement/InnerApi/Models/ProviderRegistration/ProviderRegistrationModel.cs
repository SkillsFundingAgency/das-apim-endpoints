using System;

namespace SFA.DAS.Roatp.CourseManagement.InnerApi.Models.ProviderRegistration
{
    public class ProviderRegistrationModel
    {
        public int ukprn { get; set; }
        public int StatusId { get; set; }
        public DateTime StatusDate { get; set; }
        public int OrganisationTypeId { get; set; }
        public int ProviderTypeId { get; set; }
        public Feedback Feedback { get; set; }
        public string LegalName { get; set; }
    }
}