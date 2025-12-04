using Microsoft.Extensions.Logging;
using SFA.DAS.FindApprenticeshipJobs.Application.Shared;
using SFA.DAS.FindApprenticeshipJobs.Domain.Models;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipJobs.Interfaces;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using System.Text.RegularExpressions;
using SFA.DAS.SharedOuterApi.Models;
using LiveVacancy = SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses.LiveVacancy;

namespace SFA.DAS.FindApprenticeshipJobs.Services
{
    public class LiveVacancyMapper(ILogger<LiveVacancyMapper> logger) : ILiveVacancyMapper
    {
        public Application.Shared.LiveVacancy Map(LiveVacancy source, GetStandardsListResponse standards)
        {
            var getStandardsListItem =
                standards.Standards.SingleOrDefault(s => s.LarsCode.ToString() == source.ProgrammeId);

            if (getStandardsListItem == null)
            {
                logger.LogError("Standard not found {ProgrammeId}", source.ProgrammeId);
            }

            return new Application.Shared.LiveVacancy
            {
                Id = source.VacancyReference.ToString(),
                VacancyReference = source.VacancyReference.ToString(),
                VacancyId = source.Id,
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
                VacancyLocationType = source.EmployerLocationOption == AvailableWhere.AcrossEngland
                    ? "National"
                    : "NonNational",
                ApprenticeshipLevel = getStandardsListItem?.Level == null
                    ? ""
                    : GetApprenticeshipLevel(getStandardsListItem.Level),
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
                IsDisabilityConfident = source.DisabilityConfident,
                AccountId = source.AccountId,
                AccountLegalEntityId = source.AccountLegalEntityId,
                ApplicationMethod = source.ApplicationMethod,
                ApplicationInstructions = source.ApplicationInstructions,
                ApplicationUrl = source.ApplicationUrl,
                LongDescription = source.Description,
                TrainingDescription = source.TrainingDescription,
                Skills = source.Skills ?? [],
                Qualifications = source.Qualifications?.Select(q => new Application.Shared.Qualification
                {
                    QualificationType = q.QualificationType,
                    Subject = q.Subject,
                    Grade = q.Grade,
                    Weighting = q.Weighting
                }).ToList() ?? [],
                OutcomeDescription = source.OutcomeDescription,
                EmployerContactName = source.EmployerContactName,
                EmployerContactEmail = source.EmployerContactEmail,
                EmployerContactPhone = source.EmployerContactPhone,
                ProviderContactEmail = source.ProviderContactEmail,
                ProviderContactName = source.ProviderContactName,
                ProviderContactPhone = source.ProviderContactPhone,
                EmployerDescription = source.EmployerDescription,
                EmployerWebsiteUrl = source.EmployerWebsiteUrl,
                Address = source.Address,
                OtherAddresses = source.OtherAddresses,
                EmploymentLocations = source.EmployerLocations,
                EmploymentLocationInformation = source.EmployerLocationInformation,
                EmploymentLocationOption = source.EmployerLocationOption,
                Duration = source.Wage.Duration,
                DurationUnit = source.Wage.DurationUnit,
                ThingsToConsider = source.ThingsToConsider,
                ApprenticeshipTitle = getStandardsListItem?.Title,
                Level = getStandardsListItem?.Level ?? 0,

                StandardLarsCode = getStandardsListItem?.LarsCode ?? 0,

                RouteCode = getStandardsListItem?.RouteCode ?? 0,
                Route = getStandardsListItem?.Route ?? string.Empty,

                IsRecruitVacancy = true,
                TypicalJobTitles = getStandardsListItem?.TypicalJobTitles == null
                    ? ""
                    : SortTypicalJobTitles(getStandardsListItem.TypicalJobTitles),
                AdditionalQuestion1 = source.AdditionalQuestion1,
                AdditionalQuestion2 = source.AdditionalQuestion2,
                AdditionalTrainingDescription = source.AdditionalTrainingDescription,
                SearchTags = string.Empty,
                ApprenticeshipType = source.ApprenticeshipType,
            };
        }

        public Application.Shared.LiveVacancy Map(GetNhsJobApiDetailResponse source, GetLocationsListResponse locations,
            GetRoutesListItem route)
        {
            var location = source.Locations.FirstOrDefault().Location.Split(",");
            var locationLookup = locations.Locations.FirstOrDefault(c =>
                c.Postcode.Replace(" ", "").Equals(location[1].Replace(" ", "").Trim(),
                    StringComparison.CurrentCultureIgnoreCase));
            return new Application.Shared.LiveVacancy
            {
                Route = route.Name,
                RouteCode = route.Id,
                Title = source.Title,
                Description = source.Description,
                Id = source.Id,
                EmployerName = source.Employer,
                VacancyReference = source.Reference,
                Wage = GetNhsJobsWage(source.Salary),
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

        public Application.Shared.LiveVacancy Map(GetCivilServiceJobsApiResponse.Job source, GetRoutesListItem route)
        {
            return new Application.Shared.LiveVacancy
            {
                Route = route.Name,
                RouteCode = route.Id,
                Title = source.JobTitle.En ?? string.Empty,
                Description = string.Empty,
                Id = source.JobCode ?? string.Empty,
                EmployerName = source.Department.En,
                VacancyReference = source.JobReference ?? string.Empty,
                Wage = GetCivilServiceJobWage(source.SalaryMinimum, source.SalaryMinimum),
                ApplicationUrl = source.JobUrl,
                ClosingDate = source.KeyTimes.ClosingTime,
                PostedDate = source.KeyTimes.PublishedTime,
                Address = new Address
                {
                    Latitude = source.LocationGeoCoordinates.Count > 0 ? source.LocationGeoCoordinates.FirstOrDefault()?.Lat : 0,
                    Longitude = source.LocationGeoCoordinates.Count > 0 ? source.LocationGeoCoordinates.FirstOrDefault()?.Lon : 0,
                },
                OtherAddresses = GetCivilServiceJobOtherAddresses(source.LocationGeoCoordinates),
                Qualifications = [],
                Skills = [],
                SearchTags = "Civil Service Civil Servant Public Sector Whitehall",
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

        public static Application.Shared.Wage GetNhsJobsWage(string wageText)
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
                    WageType = WageType.FixedWage,
                    WageText = wageText,
                    ApprenticeMinimumWage = lowerBound,
                    Under18NationalMinimumWage = lowerBound,
                    Between18AndUnder21NationalMinimumWage =
                        decimal.Round(middleBound, 2, MidpointRounding.AwayFromZero),
                    Between21AndUnder25NationalMinimumWage = upperBound,
                    Over25NationalMinimumWage = upperBound,
                };
            }

            if (decimal.TryParse(wageText.TrimStart('£'), out var fixedWage))
            {
                return new Application.Shared.Wage
                {
                    WageType = WageType.FixedWage,
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
                WageType = WageType.CompetitiveSalary,
                WageText = wageText,
            };
        }

        private static Application.Shared.Wage GetCivilServiceJobWage(decimal lowerBand, decimal upperBand)
        {
            var middleBand = upperBand / 1.33M;
            return new Application.Shared.Wage
            {
                WageType = WageType.FixedWage,
                WageText = $"{lowerBand} - {upperBand}",
                ApprenticeMinimumWage = lowerBand,
                Under18NationalMinimumWage = lowerBand,
                Between18AndUnder21NationalMinimumWage = decimal.Round(middleBand, 2, MidpointRounding.AwayFromZero),
                Between21AndUnder25NationalMinimumWage = upperBand,
                Over25NationalMinimumWage = upperBand
            };
        }

        private static List<Address> GetCivilServiceJobOtherAddresses(
            List<GetCivilServiceJobsApiResponse.LocationGeoCoordinate>? source)
        {
            if (source is null || source.Count == 0) return [];
            
            return source.Skip(1).Select(location => new Address
            {
                Latitude = location.Lat,
                Longitude = location.Lon
            }).ToList();
        }
    }
}