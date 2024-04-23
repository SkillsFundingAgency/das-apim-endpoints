using System;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.PublicSectorOrganisation
{
    public class PublicSectorOrganisation
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Source { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? AddressLine3 { get; set; }
        public string? Town { get; set; }
        public string? PostCode { get; set; }
        public string? Country { get; set; }
        public string? UPRN { get; set; }
        public string? OrganisationCode { get; set; }
        public string? OnsSector { get; set; }
        public bool Active { get; set; }
    }
}
