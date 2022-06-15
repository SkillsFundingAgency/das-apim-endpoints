using System;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Models.RegisteredProvider
{
    public class RegisteredProviderModel
    {
        public int Ukprn { get; set; }
        public int StatusId { get; set; }
        public DateTime StatusDate { get; set; }
        public int OrganisationTypeId { get; set; }
        public int ProviderTypeId { get; set; }
        public Feedback Feedback { get; set; }
        public string LegalName { get; set; }
    }
}