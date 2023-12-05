using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipJobs.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.Services
{
    public class LiveVacancyMapper : ILiveVacancyMapper
    {
        private readonly ICourseService _courseService;

        public LiveVacancyMapper(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public async Task<Application.Shared.LiveVacancy> Map(LiveVacancy source)
        {
            var standards = await _courseService.GetActiveStandards<GetStandardsListResponse>(nameof(GetStandardsListResponse));

            return new Application.Shared.LiveVacancy
            {
                VacancyId = source.VacancyId,
                VacancyReference = source.VacancyReference,
                VacancyTitle = source.Title,
                NumberOfPositions = source.NumberOfPositions,
                ApprenticeshipTitle = standards.Standards.SingleOrDefault(s => s.LarsCode.ToString() == source.ProgrammeId)?.Title ?? string.Empty,
                Level = standards.Standards.SingleOrDefault(s => s.LarsCode.ToString() == source.ProgrammeId)?.Level ?? 0,
                Description = source.Description,
                EmployerName = source.EmployerName,
                ProviderName = source.TrainingProvider.Name,
                ProviderId = source.TrainingProvider.Ukprn,
                LiveDate = source.LiveDate,
                ClosingDate = source.ClosingDate,
                ProgrammeId = source.ProgrammeId,
                ProgrammeType = source.ProgrammeType,
                StartDate = source.StartDate,
                Route = standards.Standards.SingleOrDefault(s => s.LarsCode.ToString() == source.ProgrammeId)?.Route ?? string.Empty,
                EmployerLocation = new Application.Shared.Address
                {
                    AddressLine1 = source.EmployerLocation?.AddressLine1,
                    AddressLine2 = source.EmployerLocation?.AddressLine2,
                    AddressLine3 = source.EmployerLocation?.AddressLine3,
                    AddressLine4 = source.EmployerLocation?.AddressLine4,
                    Postcode = source.EmployerLocation?.Postcode,
                    Latitude = source.EmployerLocation?.Latitude ?? 0,
                    Longitude = source.EmployerLocation?.Longitude ?? 0,
                },
                Wage = source.Wage == null ? null : new FindApprenticeshipJobs.Application.Shared.Wage
                {
                    Duration = source.Wage.Duration,
                    DurationUnit = source.Wage.DurationUnit,
                    FixedWageYearlyAmount = source.Wage.FixedWageYearlyAmount,
                    WageAdditionalInformation = source.Wage.WageAdditionalInformation,
                    WageType = source.Wage.WageType,
                    WeeklyHours = source.Wage.WeeklyHours,
                    WorkingWeekDescription = source.Wage.WorkingWeekDescription
                },
                OutcomeDescription = source.OutcomeDescription,
                //LongDescription =
                TrainingDescription = source.TrainingDescription,
                Skills = source.Skills,
                Qualifications = source.Qualifications.Select(q => new Application.Shared.Qualification
                {
                    QualificationType = q.QualificationType,
                    Subject = q.Subject,
                    Grade = q.Grade,
                    Weighting = (Application.Shared.QualificationWeighting)q.Weighting
                }),
                ThingsToConsider = source.ThingsToConsider,
                Id = source.Id,
                IsDisabilityConfident = (Application.Shared.DisabilityConfident)source.DisabilityConfident,
                IsEmployerAnonymous = source.IsAnonymous,
                EmployerDescription = source.EmployerDescription,
                EmployerWebsiteUrl = source.EmployerWebsiteUrl,
                IsRecruitVacancy = true,
                //AnonymousEmployerName = 
                //Category = 
                //CategoryCode = 
                //IsPositiveAboutDisability = 
                //SubCategory = 
                //SubCategoryCode = 
                //VacancyLocationType =
                //WageAmountLowerBand =
                //WageAmountUpperBand = 
                //ExpectedDuration = 
                //Distance = 
                //Score = 
                EmployerContactName = source.EmployerContact == null ? null : source.EmployerContact.EmployerContactName,
                EmployerContactEmail = source.EmployerContact == null ? null : source.EmployerContact.EmployerContactEmail,
                EmployerContactPhone = source.EmployerContact == null ? null : source.EmployerContact.EmployerContactPhone
            };
        }
    }
}
