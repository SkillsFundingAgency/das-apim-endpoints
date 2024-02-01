using System.ComponentModel;

namespace SFA.DAS.EmployerAccounts.Application.Queries.SearchOrganisations
{
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
}
