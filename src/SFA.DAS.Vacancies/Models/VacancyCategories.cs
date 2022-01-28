using System.ComponentModel;

namespace SFA.DAS.Vacancies.Models
{
    public enum VacancyCategories
    {
        [Description("Health, Public Services and Care")]
        HealthPublicServicesAndCare = 1,
        [Description("Science and Mathematics")]        
        ScienceAndMathematics = 2,
        [Description("Agriculture, Horticulture and Animal Care")]
        AgricultureHorticultureAndAnimalCare = 3,
        [Description("Engineering and Manufacturing Technologies")]
        EngineeringAndManufacturingTechnologies = 4,
        [Description("Construction, Planning and the Built Environment")]
        ConstructionPlanningAndTheBuiltEnvironment = 5,
        [Description("Information and Communication Technology")]
        InformationAndCommunicationTechnology = 6,
        [Description("Retail and Commercial Enterprise")]
        RetailAndCommercialEnterprise = 7,
        [Description("Leisure, Travel and Tourism")]
        LeisureTravelAndTourism = 8,
        [Description("Arts, Media and Publishing")]
        ArtsMediaAndPublishing = 9,
        [Description("History, Philosophy and Theology")]
        HistoryPhilosophyAndTheology = 10,
        [Description("Social Sciences")]
        SocialSciences = 11,
        [Description("Languages, Literature and Culture")]
        LanguagesLiteratureAndCulture = 12,
        [Description("Education and Training")]
        EducationAndTraining = 13,
        [Description("Preparation for Life and Work")]
        PreparationForLifeAndWork = 14,
        [Description("Business Administration and Law")]
        BusinessAdministrationAndLaw = 15
    }
}