using SFA.DAS.SharedOuterApi.Types.Domain.Recruit;
using SFA.DAS.SharedOuterApi.Types.Models;

namespace SFA.DAS.SharedOuterApi.Recruit.UnitTests;

public class VacancyUtils
{
    public static Vacancy CreateVacancy()
    {
        return new Vacancy
        {
            Status = VacancyStatus.Submitted,
            VacancyReference = 123456,
            AccountId = 654321,
            ApplicationInstructions = "application instructions",
            ApplicationMethod = ApplicationMethod.ThroughExternalApplicationSite,
            ApplicationUrl = "application url",
            ClosingDate = new DateTime(),
            Description = "description",
            DisabilityConfident = false,
            Contact = new ContactDetail
            {
                Email = "employer contact email",
                Name = "employer contact name",
                Phone = "employer contact phone",
            },
            EmployerDescription = "employer description",
            EmployerLocations = [new Address
            {
                AddressLine1 = "address line 1",
                AddressLine2 = "address line 2",
                AddressLine3 = "address line 3",
                AddressLine4 = "address line 4",
                Postcode = "post code"
            }],
            EmployerLocationInformation = "employer location information",
            EmployerName = "employer name",
            EmployerWebsiteUrl = "employer website",
            NumberOfPositions = 5,
            OutcomeDescription = "outcome descriptions",
            ProgrammeId = "programme id",
            Qualifications = new List<Qualification>
            {
                new Qualification{QualificationType = "qualification type 1", Subject = "subject 1", Grade = "grade 1", Weighting = QualificationWeighting.Desired},
                new Qualification{QualificationType = "qualification type 2", Subject = "subject 2", Grade = "grade 2", Weighting = QualificationWeighting.Essential},
                new Qualification{QualificationType = "qualification type 3", Level = 1, Subject = "subject 2", Grade = "grade 2", Weighting = QualificationWeighting.Essential},
            },
            ShortDescription = "short description",
            Skills = new List<string> { "skill 1", "skill 2" },
            StartDate = new DateTime(),
            ThingsToConsider = "things to consider",
            Title = "title",
            TrainingDescription = "training description",
            TrainingProvider = new TrainingProvider { Ukprn = 1234 },
            Wage = new Wage
            {
                WeeklyHours = 35.5m,
                WorkingWeekDescription = "working week description",
                WageAdditionalInformation = "wage additional information",
                WageType = WageType.FixedWage,
                FixedWageYearlyAmount = 1000.00m,
                Duration = 1,
                DurationUnit = DurationUnit.Month
            },
            AdditionalQuestion1 = "Additional question",
            AdditionalQuestion2 = "Additional question 2",
        };
    }

    public static Vacancy CreateChangedVacancy()
    {
        return new Vacancy
        {
            Status = VacancyStatus.Submitted,
            VacancyReference = 1234567,
            AccountId = 9876854,
            ApplicationInstructions = "application instructions CHANGED",
            ApplicationMethod = ApplicationMethod.ThroughFindAnApprenticeship,
            ApplicationUrl = "application url CHANGED",
            ClosingDate = DateTime.MaxValue,
            Description = "description CHANGED",
            DisabilityConfident = true,
            Contact = new ContactDetail
            {
                Email = "employer contact email CHANGED",
                Name = "employer contact name CHANGED",
                Phone = "employer contact phone CHANGED",
            },
            EmployerDescription = "employer description CHANGED",
            EmployerLocations = [new Address
            {
                AddressLine1 = "address line 1 CHANGED",
                AddressLine2 = "address line 2 CHANGED",
                AddressLine3 = "address line 3 CHANGED",
                AddressLine4 = "address line 4 CHANGED",
                Postcode = "post code CHANGED"
            }],
            EmployerLocationInformation = "employer location information CHANGED",
            EmployerName = "employer name CHANGED",
            EmployerWebsiteUrl = "employer website CHANGED",
            NumberOfPositions = 6,
            OutcomeDescription = "outcome descriptions CHANGED",
            ProgrammeId = "programme id CHANGED",
            Qualifications = new List<Qualification>
            {
                new Qualification{QualificationType = "qualification type 1", Subject = "subject 1", Grade = "grade 1", Weighting = QualificationWeighting.Desired},
                new Qualification{QualificationType = "qualification type 2", Subject = "subject 2", Grade = "grade 2 CHANGED", Weighting = QualificationWeighting.Essential},
            },
            ShortDescription = "short description CHANGED",
            Skills = new List<string> { "skill 1", "skill 2 CHANGED" },
            StartDate = DateTime.MaxValue,
            ThingsToConsider = "things to consider CHANGED",
            Title = "title CHANGED",
            TrainingDescription = "training description CHANGED",
            TrainingProvider = new TrainingProvider { Ukprn = 12345 },
            Wage = new Wage
            {
                WeeklyHours = 36.5m,
                WorkingWeekDescription = "working week description CHANGED",
                WageAdditionalInformation = "wage additional information CHANGED",
                WageType = WageType.NationalMinimumWage,
                FixedWageYearlyAmount = 2000.00m,
                Duration = 2,
                DurationUnit = DurationUnit.Year
            },
            AdditionalQuestion1 = "Additional question CHANGED",
            AdditionalQuestion2 = "Additional question CHANGED",
        };
    }

    public static Vacancy CreateEmptyVacancy()
    {
        return new Vacancy
        {
            AccountId = null,
            AdditionalQuestion1 = null,
            AdditionalQuestion2 = null,
            ApplicationInstructions = null,
            ApplicationMethod = null,
            ApplicationUrl = null,
            ClosingDate = null,
            Contact = null,
            Description = null,
            DisabilityConfident = true,
            EmployerDescription = null,
            EmployerLocationInformation = null,
            EmployerLocations = null,
            EmployerName = null,
            EmployerWebsiteUrl = null,
            NumberOfPositions = null,
            OutcomeDescription = null,
            ProgrammeId = null,
            Qualifications = null,
            ShortDescription = null,
            StartDate = null,
            Status = VacancyStatus.Submitted,
            ThingsToConsider = null,
            Title = null,
            TrainingDescription = null,
            TrainingProvider = null,
            VacancyReference = null,
            Wage = null,
        };
    }
}