﻿using SFA.DAS.FindApprenticeshipJobs.Domain.Models;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipJobs.Interfaces;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using System.Text.RegularExpressions;
using SFA.DAS.FindApprenticeshipJobs.Application.Shared;
using Address = SFA.DAS.FindApprenticeshipJobs.Application.Shared.Address;
using DisabilityConfident = SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses.DisabilityConfident;
using LiveVacancy = SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses.LiveVacancy;

namespace SFA.DAS.FindApprenticeshipJobs.Services
{
    public class LiveVacancyMapper : ILiveVacancyMapper
    {
        public Application.Shared.LiveVacancy Map(LiveVacancy source, GetStandardsListResponse standards)
        {
            var getStandardsListItem = standards.Standards.Single(s => s.LarsCode.ToString() == source.ProgrammeId);
            
            return new Application.Shared.LiveVacancy
            {
                Id = source.VacancyReference.ToString(),
                VacancyReference = source.VacancyReference.ToString(),
                VacancyId = source.VacancyId,
                Title = source.Title,
                PostedDate = source.LiveDate,
                StartDate = source.StartDate,
                ClosingDate = source.ClosingDate,
                Description = source.ShortDescription,
                NumberOfPositions = source.NumberOfPositions,
                EmployerName = source.EmployerName,
                ProviderName = source.TrainingProvider.Name,
                Ukprn = source.TrainingProvider.Ukprn,
                IsPositiveAboutDisability = false,
                
                IsEmployerAnonymous = source.IsAnonymous,
                VacancyLocationType = source.EmployerLocationOption == AvailableWhere.AcrossEngland ? "National" : "NonNational",
                ApprenticeshipLevel = GetApprenticeshipLevel(getStandardsListItem.Level),
                Wage = new Application.Shared.Wage
                {
                    Duration = source.Wage.Duration,
                    DurationUnit = source.Wage.DurationUnit,
                    FixedWageYearlyAmount = source.Wage.FixedWageYearlyAmount,
                    WageAdditionalInformation = source.Wage.WageAdditionalInformation,
                    WageType = source.Wage.WageType,
                    WeeklyHours = source.Wage.WeeklyHours,
                    WorkingWeekDescription = source.Wage.WorkingWeekDescription,
                    ApprenticeMinimumWage = source.Wage.ApprenticeMinimumWage,
                    Under18NationalMinimumWage = source.Wage.Under18NationalMinimumWage,
                    Between18AndUnder21NationalMinimumWage = source.Wage.Between18AndUnder21NationalMinimumWage,
                    Between21AndUnder25NationalMinimumWage = source.Wage.Between21AndUnder25NationalMinimumWage,
                    Over25NationalMinimumWage = source.Wage.Over25NationalMinimumWage,
                    WageText = source.Wage.WageText,
                    CompanyBenefitsInformation = source.Wage.CompanyBenefitsInformation
                },
                AnonymousEmployerName = source.IsAnonymous ? source.EmployerName: null,
                IsDisabilityConfident = source.DisabilityConfident == DisabilityConfident.Yes,
                AccountPublicHashedId = source.AccountPublicHashedId,
                AccountLegalEntityPublicHashedId = source.AccountLegalEntityPublicHashedId,
                ApplicationMethod = source.ApplicationMethod,
                ApplicationInstructions = source.ApplicationInstructions,
                ApplicationUrl = source.ApplicationUrl,
                LongDescription = source.Description,
                TrainingDescription = source.TrainingDescription,
                Skills = source.Skills,
                Qualifications = source.Qualifications.Select(q => new Application.Shared.Qualification
                {
                    QualificationType = q.QualificationType,
                    Subject = q.Subject,
                    Grade = q.Grade,
                    Weighting = q.Weighting
                }),
                OutcomeDescription = source.OutcomeDescription,
                EmployerContactName = source.EmployerContactName,
                EmployerContactEmail = source.EmployerContactEmail,
                EmployerContactPhone = source.EmployerContactPhone,
                ProviderContactEmail = source.ProviderContactEmail,
                ProviderContactName = source.ProviderContactName,
                ProviderContactPhone = source.ProviderContactPhone,
                EmployerDescription = source.EmployerDescription,
                EmployerWebsiteUrl = source.EmployerWebsiteUrl,
                Address = source.EmployerLocation is not null ? new Address
                {
                    AddressLine1 = source.EmployerLocation?.AddressLine1,
                    AddressLine2 = source.EmployerLocation?.AddressLine2,
                    AddressLine3 = source.EmployerLocation?.AddressLine3,
                    AddressLine4 = source.EmployerLocation?.AddressLine4,
                    Postcode = source.EmployerLocation?.Postcode,
                    Latitude = source.EmployerLocation?.Latitude ?? 0,
                    Longitude = source.EmployerLocation?.Longitude ?? 0,
                    Country = source.EmployerLocation?.Country
                } : null,
                EmploymentLocations = source.EmployerLocations?.Select(x => (Address)x).ToList() ?? [],
                EmploymentLocationInformation = source.EmployerLocationInformation,
                EmploymentLocationOption = source.EmployerLocationOption,
                Duration = source.Wage.Duration,
                DurationUnit = source.Wage.DurationUnit,
                ThingsToConsider = source.ThingsToConsider,
                ApprenticeshipTitle = getStandardsListItem.Title,
                Level = getStandardsListItem.Level,
                
                StandardLarsCode = getStandardsListItem.LarsCode,
                
                RouteCode = getStandardsListItem.RouteCode,  
                Route = getStandardsListItem?.Route ?? string.Empty,

                IsRecruitVacancy = true,
                TypicalJobTitles = getStandardsListItem.TypicalJobTitles == null ? "" : SortTypicalJobTitles(getStandardsListItem.TypicalJobTitles),
                AdditionalQuestion1 = source.AdditionalQuestion1,
                AdditionalQuestion2 = source.AdditionalQuestion2,
                AdditionalTrainingDescription = source.AdditionalTrainingDescription,
                SearchTags = string.Empty
            };
        }

        public Application.Shared.LiveVacancy Map(GetNhsJobApiDetailResponse source, GetLocationsListResponse locations, GetRoutesListItem route)
        {
            var location = source.Locations.FirstOrDefault().Location.Split(",");
            var locationLookup = locations.Locations.FirstOrDefault(c =>
                c.Postcode.Replace(" ","").Equals(location[1].Replace(" ","").Trim(), StringComparison.CurrentCultureIgnoreCase));
            return new Application.Shared.LiveVacancy
            {
                Route = route.Name,
                RouteCode = route.Id,
                Title = source.Title,
                Description = source.Description,
                Id = source.Id,
                EmployerName = source.Employer,
                VacancyReference = source.Reference,
                Wage = GetWage(source.Salary),
                ApplicationUrl = source.Url,
                ClosingDate = DateTime.Parse(source.CloseDate),
                PostedDate = DateTime.Parse(source.PostDate),
                Address = new Address
                {
                    AddressLine4 = location[0].Trim(),
                    Postcode = location[1].Trim(),
                    Latitude = locationLookup?.Location?.GeoPoint?.FirstOrDefault() ?? 0,
                    Longitude = locationLookup?.Location?.GeoPoint?.LastOrDefault() ?? 0,
                    Country = locationLookup?.Country
                },
                Qualifications = [],
                Skills = [],
                SearchTags = "NHS National Health Service Health Medical Hospital",
            };
        }

        private static string GetApprenticeshipLevel(int level)
        {
            return level switch
            {
                0 => "Unknown",
                2 => "Intermediate",
                3 => "Advanced",
                4 => "Higher",
                5 => "Higher",
                6 => "Degree",
                7 => "Degree",
                _ => ""
            };
        }

        private static string SortTypicalJobTitles(string typicalJobTitles)
        {
            var orderedJobTitles = typicalJobTitles.Split("|").OrderBy(s => s);
            return string.Join("|", orderedJobTitles);
        }

        public static Application.Shared.Wage GetWage(string wageText)
        {
            // Regex to match numbers with decimals
            var matches = Regex.Matches(wageText, @"\d+\.\d{2}");

            if (matches.Count == 2)
            {
                var lowerBound = decimal.Parse(matches[0].Value);
                var upperBound = decimal.Parse(matches[1].Value);
                var middleBound = upperBound / 1.33M;
                return new Application.Shared.Wage
                {
                    WageType = WageType.FixedWage.ToString(),
                    WageText = wageText,
                    ApprenticeMinimumWage = lowerBound,
                    Under18NationalMinimumWage = lowerBound,
                    Between18AndUnder21NationalMinimumWage = decimal.Round(middleBound, 2, MidpointRounding.AwayFromZero),
                    Between21AndUnder25NationalMinimumWage = upperBound,
                    Over25NationalMinimumWage = upperBound,
                };
            }

            if(decimal.TryParse(wageText.TrimStart('£'), out var fixedWage))
            {
                return new Application.Shared.Wage
                {
                    WageType = WageType.FixedWage.ToString(),
                    WageText = wageText,
                    ApprenticeMinimumWage = fixedWage,
                    Under18NationalMinimumWage = fixedWage,
                    Between18AndUnder21NationalMinimumWage = fixedWage,
                    Between21AndUnder25NationalMinimumWage = fixedWage,
                    Over25NationalMinimumWage = fixedWage,
                    FixedWageYearlyAmount = fixedWage,
                };
            }

            return new Application.Shared.Wage
            {
                WageType = WageType.CompetitiveSalary.ToString(),
                WageText = wageText,
            };
        }
    }
}
