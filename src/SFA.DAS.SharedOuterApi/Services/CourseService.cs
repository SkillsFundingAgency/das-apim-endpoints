using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Services
{
    public class CourseService : ICourseService
    {
     public List<string> MapRoutesToCategories(IReadOnlyList<string> routes)
        {

            if (routes == null)
            {
                return null;
            }

            var categories = new List<string>();


            if (routes.Contains("Agriculture, environmental and animal care", StringComparer.CurrentCultureIgnoreCase))
            {
                categories.AddRange(new List<string>
                {
                    VacancyCategories.ScienceAndMathematics.GetDescription(),
                    VacancyCategories.AgricultureHorticultureAndAnimalCare.GetDescription(),
                    VacancyCategories.EngineeringAndManufacturingTechnologies.GetDescription(),
                    VacancyCategories.BusinessAdministrationAndLaw.GetDescription()
                });
            }
            if (routes.Contains("Business and administration", StringComparer.CurrentCultureIgnoreCase))
            {
                categories.AddRange(new List<string>
                {
                    VacancyCategories.HealthPublicServicesAndCare.GetDescription(),
                    VacancyCategories.ScienceAndMathematics.GetDescription(),
                    VacancyCategories.EngineeringAndManufacturingTechnologies.GetDescription(),
                    VacancyCategories.ArtsMediaAndPublishing.GetDescription(),
                    VacancyCategories.EducationAndTraining.GetDescription(),
                    VacancyCategories.BusinessAdministrationAndLaw.GetDescription()
                });
            }
            if (routes.Contains("Care services", StringComparer.CurrentCultureIgnoreCase))
            {
                categories.AddRange(new List<string>
                {
                    VacancyCategories.HealthPublicServicesAndCare.GetDescription(),
                    VacancyCategories.HistoryPhilosophyAndTheology.GetDescription()
                });
            }
            if (routes.Contains("Catering and hospitality", StringComparer.CurrentCultureIgnoreCase))
            {
                categories.AddRange(new List<string>
                {
                    VacancyCategories.EngineeringAndManufacturingTechnologies.GetDescription(),
                    VacancyCategories.RetailAndCommercialEnterprise.GetDescription()
                });
            }
            if (routes.Contains("Construction", StringComparer.CurrentCultureIgnoreCase))
            {
                categories.AddRange(new List<string>
                {
                    VacancyCategories.AgricultureHorticultureAndAnimalCare.GetDescription(),
                    VacancyCategories.EngineeringAndManufacturingTechnologies.GetDescription(),
                    VacancyCategories.ConstructionPlanningAndTheBuiltEnvironment.GetDescription(),
                    VacancyCategories.RetailAndCommercialEnterprise.GetDescription(),
                    VacancyCategories.BusinessAdministrationAndLaw.GetDescription()
                });
            }
            if (routes.Contains("Creative and design", StringComparer.CurrentCultureIgnoreCase))
            {
                categories.AddRange(new List<string>
                {
                    VacancyCategories.HealthPublicServicesAndCare.GetDescription(),
                    VacancyCategories.EngineeringAndManufacturingTechnologies.GetDescription(),
                    VacancyCategories.InformationAndCommunicationTechnology.GetDescription(),
                    VacancyCategories.RetailAndCommercialEnterprise.GetDescription(),
                    VacancyCategories.LeisureTravelAndTourism.GetDescription(),
                    VacancyCategories.ArtsMediaAndPublishing.GetDescription(),
                    VacancyCategories.HistoryPhilosophyAndTheology.GetDescription(),
                    VacancyCategories.BusinessAdministrationAndLaw.GetDescription()
                });
            }
            if (routes.Contains("Digital", StringComparer.CurrentCultureIgnoreCase))
            {
                categories.AddRange(new List<string>
                {
                    VacancyCategories.EngineeringAndManufacturingTechnologies.GetDescription(),
                    VacancyCategories.InformationAndCommunicationTechnology.GetDescription(),
                    VacancyCategories.ArtsMediaAndPublishing.GetDescription()
                });
            }
            if (routes.Contains("Education and childcare", StringComparer.CurrentCultureIgnoreCase))
            {
                categories.AddRange(new List<string>
                {
                    VacancyCategories.HealthPublicServicesAndCare.GetDescription(),
                    VacancyCategories.EducationAndTraining.GetDescription()
                });
            }
            if (routes.Contains("Engineering and manufacturing", StringComparer.CurrentCultureIgnoreCase))
            {
                categories.AddRange(new List<string>
                {
                    VacancyCategories.HealthPublicServicesAndCare.GetDescription(),
                    VacancyCategories.AgricultureHorticultureAndAnimalCare.GetDescription(),
                    VacancyCategories.EngineeringAndManufacturingTechnologies.GetDescription(),
                    VacancyCategories.ConstructionPlanningAndTheBuiltEnvironment.GetDescription(),
                    VacancyCategories.RetailAndCommercialEnterprise.GetDescription(),
                    VacancyCategories.ArtsMediaAndPublishing.GetDescription(),
                    VacancyCategories.BusinessAdministrationAndLaw.GetDescription()
                });
            }
            if (routes.Contains("Hair and beauty", StringComparer.CurrentCultureIgnoreCase))
            {
                categories.AddRange(new List<string>
                {
                    VacancyCategories.HealthPublicServicesAndCare.GetDescription(),
                    VacancyCategories.RetailAndCommercialEnterprise.GetDescription()
                });
            }
            if (routes.Contains("Health and science", StringComparer.CurrentCultureIgnoreCase))
            {
                categories.AddRange(new List<string>
                {
                    VacancyCategories.HealthPublicServicesAndCare.GetDescription(),
                    VacancyCategories.ScienceAndMathematics.GetDescription(),
                    VacancyCategories.AgricultureHorticultureAndAnimalCare.GetDescription(),
                    VacancyCategories.EngineeringAndManufacturingTechnologies.GetDescription(),
                    VacancyCategories.RetailAndCommercialEnterprise.GetDescription(),
                    VacancyCategories.LeisureTravelAndTourism.GetDescription(),
                    VacancyCategories.BusinessAdministrationAndLaw.GetDescription()
                });
            }
            if (routes.Contains("Legal, finance and accounting", StringComparer.CurrentCultureIgnoreCase))
            {
                categories.AddRange(new List<string>
                {
                    VacancyCategories.EngineeringAndManufacturingTechnologies.GetDescription(),
                    VacancyCategories.SocialSciences.GetDescription(),
                    VacancyCategories.BusinessAdministrationAndLaw.GetDescription()
                });
            }
            if (routes.Contains("Protective services", StringComparer.CurrentCultureIgnoreCase))
            {
                categories.AddRange(new List<string>
                {
                    VacancyCategories.HealthPublicServicesAndCare.GetDescription(),
                    VacancyCategories.ConstructionPlanningAndTheBuiltEnvironment.GetDescription()
                });
            }
            if (routes.Contains("Sales, marketing and procurement", StringComparer.CurrentCultureIgnoreCase))
            {
                categories.AddRange(new List<string>
                {
                    VacancyCategories.HealthPublicServicesAndCare.GetDescription(),
                    VacancyCategories.EngineeringAndManufacturingTechnologies.GetDescription(),
                    VacancyCategories.InformationAndCommunicationTechnology.GetDescription(),
                    VacancyCategories.RetailAndCommercialEnterprise.GetDescription(),
                    VacancyCategories.LeisureTravelAndTourism.GetDescription(),
                    VacancyCategories.ArtsMediaAndPublishing.GetDescription(),
                    VacancyCategories.BusinessAdministrationAndLaw.GetDescription()
                });
            }
            if (routes.Contains("Transport and logistics", StringComparer.CurrentCultureIgnoreCase))
            {
                categories.AddRange(new List<string>
                {
                    VacancyCategories.AgricultureHorticultureAndAnimalCare.GetDescription(),
                    VacancyCategories.EngineeringAndManufacturingTechnologies.GetDescription(),
                    VacancyCategories.ConstructionPlanningAndTheBuiltEnvironment.GetDescription(),
                    VacancyCategories.RetailAndCommercialEnterprise.GetDescription(),
                    VacancyCategories.LeisureTravelAndTourism.GetDescription()
                });
            }
            return categories.Distinct().ToList(); 
        }   
    }
    
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