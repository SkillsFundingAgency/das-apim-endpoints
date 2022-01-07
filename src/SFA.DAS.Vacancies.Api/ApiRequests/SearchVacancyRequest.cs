using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.Vacancies.Api
{
    public class SearchVacancyRequest
    {
        
        [FromQuery]
        public int PageNumber { get; set; } = 1;
        [FromQuery]
        public int PageSize { get; set; } = 10;
        [FromQuery]
        public string AccountLegalEntityPublicHashedId { get; set; } = null;
        [FromQuery]
        public int? Ukprn { get; set; } = null;
        [FromQuery]
        public Route? Route { get; set; } = null;
        [FromQuery]
        public double? Lat { get ; set ; } = null;
        [FromQuery]
        public double? Lon { get ; set ; } = null;
        [FromQuery]
        public VacancySort? Sort { get ; set ; } = null;
        [FromQuery]
        public uint? DistanceInMiles { get ; set ; } = null;
        [FromQuery]
        public bool? NationWideOnly { get ; set ; } = null;
        [FromQuery]
        public int? StandardsLarsCode { get ; set ; } = null;
        [FromQuery]
        public uint? PostedInLastNumberOfDays { get ; set ; } = null;
        [FromQuery]
        public string StandardLarsCode { get ; set ; }
    }

    public enum Route
    {
        [Description("Agriculture, Environmental and Animal Care")]
        AgricultureEnvironmentalAndAnimalCare,
        [Description("Business and Administration")]        
        BusinessAndAdministration,
        [Description("Catering and Hospitality")]
        CateringAndHospitality,
        [Description("Education and Childcare")]
        EducationAndChildcare,
        [Description("Construction")]
        Construction,
        [Description("Creative and Design")]
        CreativeAndDesign,
        [Description("Digital")]
        Digital,
        [Description("Engineering and Manufacturing")]
        EngineeringAndManufacturing,
        [Description("Hair and Beauty")]
        HairAndBeauty,
        [Description("Health and Science")]
        HealthAndScience,
        [Description("Legal, Finance and Accounting")]
        LegalFinanceAndAccounting,
        [Description("Design, Surveying and Planning")]
        DesignSurveyingAndPlanning,
        [Description("Building Services Engineering")]
        BuildingServicesEngineering,
        [Description("Onsite Construction")]
        OnsiteConstruction,
        [Description("Digital Production, Design and Development")]
        DigitalProductionDesignAndDevelopment,
        [Description("Digital Support and Services")]
        DigitalSupportAndServices,
        [Description("Digital Business Services")]
        DigitalBusinessServices
 
    }
    public enum VacancySort
    {
        AgeDesc,
        AgeAsc,
        DistanceDesc,
        DistanceAsc,
        ExpectedStartDateDesc,
        ExpectedStartDateAsc
    }
}