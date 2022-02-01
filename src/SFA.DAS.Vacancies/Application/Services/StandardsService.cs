using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Vacancies.InnerApi.Responses;
using SFA.DAS.Vacancies.Interfaces;
using SFA.DAS.Vacancies.Models;

namespace SFA.DAS.Vacancies.Application.Services
{
    public class StandardsService  : IStandardsService
    {
        private const int CourseCacheExpiryInHours = 4;
        private readonly ICacheStorageService _cacheStorageService;
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;

        public StandardsService (ICacheStorageService cacheStorageService, ICoursesApiClient<CoursesApiConfiguration> coursesApiClient)
        {
            _cacheStorageService = cacheStorageService;
            _coursesApiClient = coursesApiClient;
        }
        public async Task<GetStandardsListResponse> GetStandards()
        {
            var cachedCourses =
                await _cacheStorageService.RetrieveFromCache<GetStandardsListResponse>(
                    nameof(GetStandardsListResponse));

            if (cachedCourses != null)
            {
                return cachedCourses;
            }

            var apiCourses = await _coursesApiClient.Get<GetStandardsListResponse>(new GetActiveStandardsListRequest());

            await _cacheStorageService.SaveToCache(nameof(GetStandardsListResponse), apiCourses, CourseCacheExpiryInHours);

            return apiCourses;
        }

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
}