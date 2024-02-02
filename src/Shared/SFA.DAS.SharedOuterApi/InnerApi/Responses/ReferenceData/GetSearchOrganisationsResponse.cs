using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.ReferenceData
{
    public class GetSearchOrganisationsResponse
    {
        public IEnumerable<Organisation> Organisations { get; set; }

        public class Organisation
        {
            public string Name { get; set; }
            public OrganisationType Type { get; set; }
            public OrganisationSubType SubType { get; set; }
            public string Code { get; set; }
            public DateTime? RegistrationDate { get; set; }
            public Address Address { get; set; }
            public string Sector { get; set; }
            public OrganisationStatus OrganisationStatus { get; set; }
        }
    }

    public enum OrganisationStatus
    {
        None = 0,
        Active = 1,
        Dissolved = 2,
        Liquidation = 3,
        Receivership = 4,
        Administration = 5,
        VoluntaryArrangement = 6,
        ConvertedClosed = 7,
        InsolvencyProceedings = 8
    }

    public enum OrganisationType
    {
        [Description("Listed on Companies House")]
        CompaniesHouse = 1,
        [Description("Charities")]
        Charities,
        [Description("Public Bodies")]
        PublicBodies,
        [Description("Other")]
        Other,
        [Description("Pensions Regulator")]
        PensionsRegulator
    }
    public enum OrganisationSubType
    {
        None,
        Ons,
        Nhs,
        Police
    }

    public class Address
    {
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }
        public string Line4 { get; set; }
        public string Line5 { get; set; }
        public string Postcode { get; set; }
    }
}
