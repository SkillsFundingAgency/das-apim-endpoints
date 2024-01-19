using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipJobs.Interfaces;

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
                VacancyReference = source.VacancyReference,
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
                VacancyLocationType = "NonNational",
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
                    WageText = source.Wage.WageText
                },
                AnonymousEmployerName = source.IsAnonymous ? source.EmployerName: null,
                IsDisabilityConfident = source.DisabilityConfident == DisabilityConfident.Yes,
                AccountPublicHashedId = source.AccountPublicHashedId,
                AccountLegalEntityPublicHashedId = source.AccountLegalEntityPublicHashedId,
                ApplicationMethod = source.ApplicationMethod,
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
                EmployerContactName = source.EmployerContact?.EmployerContactName,
                EmployerContactEmail = source.EmployerContact?.EmployerContactEmail,
                EmployerContactPhone = source.EmployerContact?.EmployerContactPhone,
                EmployerDescription = source.EmployerDescription,
                EmployerWebsiteUrl = source.EmployerWebsiteUrl,
                Address = new Application.Shared.Address
                {
                    AddressLine1 = source.EmployerLocation?.AddressLine1,
                    AddressLine2 = source.EmployerLocation?.AddressLine2,
                    AddressLine3 = source.EmployerLocation?.AddressLine3,
                    AddressLine4 = source.EmployerLocation?.AddressLine4,
                    Postcode = source.EmployerLocation?.Postcode,
                    Latitude = source.EmployerLocation?.Latitude ?? 0,
                    Longitude = source.EmployerLocation?.Longitude ?? 0,
                },
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
                AdditionalQuestion2 = source.AdditionalQuestion2
            };
        }

        private string GetApprenticeshipLevel(int level)
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

        private string SortTypicalJobTitles(string typicalJobTitles)
        {
            var orderedJobTitles = typicalJobTitles.Split("|").OrderBy(s => s);
            return string.Join("|", orderedJobTitles);
        }
    }
}
