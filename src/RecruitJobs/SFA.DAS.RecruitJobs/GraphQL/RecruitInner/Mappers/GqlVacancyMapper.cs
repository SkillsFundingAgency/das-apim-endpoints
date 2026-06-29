using System.Collections.Generic;
using System.Text.Json;

namespace SFA.DAS.RecruitJobs.GraphQL.RecruitInner.Mappers;

// Note this class cannot be moved to the Shared assemblies as it
// relies upon the dynamically generated interface IAllVacancyFields. 
public static class GqlVacancyMapper
{
    private static T DeserializeOrNull<T>(string value) where T : class
    {
        return string.IsNullOrWhiteSpace(value) ? null : JsonSerializer.Deserialize<T>(value, Global.JsonSerializerOptions);
    }
    
    private static T? NullOrEnum<T>(string value) where T: struct
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        return Enum.TryParse<T>(value, true, out var result) 
            ? result 
            : throw new InvalidOperationException($"Could not convert {value} to type {typeof(T)}");
    }

    public static Recruit.Contracts.ApiResponses.Vacancy From(IAllVacancyFields source)
    {
        var employerLocations = DeserializeOrNull<List<Recruit.Contracts.ApiResponses.Address>>(source.EmployerLocations) ?? [];

        return new Recruit.Contracts.ApiResponses.Vacancy
        {
            AccountId = source.AccountId,
            AccountLegalEntityId = source.AccountLegalEntityId,
            AdditionalQuestion1 = source.AdditionalQuestion1,
            AdditionalQuestion2 = source.AdditionalQuestion2,
            AdditionalTrainingDescription = source.AdditionalTrainingDescription,
            AnonymousReason = source.AnonymousReason,
            ApplicationInstructions = source.ApplicationInstructions,
            ApplicationMethod = NullOrEnum<Recruit.Contracts.ApiResponses.ApplicationMethod>(source.ApplicationMethod?.ToString()),
            ApplicationUrl = source.ApplicationUrl,
            ApprenticeshipType = NullOrEnum<Recruit.Contracts.ApiResponses.ApprenticeshipTypes>(source.ApprenticeshipType?.ToString()),
            ApprovedDate = source.ApprovedDate?.UtcDateTime,
            ArchiveType = NullOrEnum<Recruit.Contracts.ApiResponses.ArchiveType>(source.ArchiveType?.ToString()),
            ArchivedDate = source.ArchivedDate?.UtcDateTime,
            ArchivedByUserId = source.ArchivedByUserId,
            CreatedDate = source.CreatedDate?.UtcDateTime,
            ClosedDate = source.ClosedDate?.UtcDateTime,
            ClosingDate = source.ClosingDate?.UtcDateTime,
            ClosureReason = NullOrEnum<Recruit.Contracts.ApiResponses.ClosureReason>(source.ClosureReason?.ToString()),
            Contact = source is { ContactName: null, ContactEmail: null, ContactPhone: null }
                ? null
                : new Recruit.Contracts.ApiResponses.ContactDetail
                {
                    Email = source.ContactEmail,
                    Name = source.ContactName!,
                    Phone = source.ContactPhone!,
                },
            DeletedDate = source.DeletedDate?.UtcDateTime,
            Description = source.Description,
            DisabilityConfident = source.DisabilityConfident,
            EmployerDescription = source.EmployerDescription,
            EmployerLocationInformation = source.EmployerLocationInformation,
            EmployerLocationOption = MapEmployerLocationOption(source, employerLocations),
            EmployerLocations = employerLocations,
            EmployerName = source.EmployerName,
            EmployerNameOption = NullOrEnum<Recruit.Contracts.ApiResponses.EmployerNameOption>(source.EmployerNameOption?.ToString()),
            EmployerRejectedReason = source.EmployerRejectedReason,
            EmployerReviewFieldIndicators = DeserializeOrNull<List<Recruit.Contracts.ApiResponses.ReviewFieldIndicator>>(source.EmployerReviewFieldIndicators),
            EmployerWebsiteUrl = source.EmployerWebsiteUrl,
            GeoCodeMethod = NullOrEnum<Recruit.Contracts.ApiResponses.GeoCodeMethod>(source.GeoCodeMethod?.ToString()),
            HasChosenProviderContactDetails = source.HasChosenProviderContactDetails,
            HasOptedToAddQualifications = source.HasOptedToAddQualifications,
            HasSubmittedAdditionalQuestions = source.HasSubmittedAdditionalQuestions,
            Id = source.Id,
            LastUpdatedDate = source.LastUpdatedDate?.UtcDateTime,
            LegalEntityName = source.LegalEntityName,
            LiveDate = source.LiveDate?.UtcDateTime,
            NumberOfPositions = source.NumberOfPositions,
            OutcomeDescription = source.OutcomeDescription,
            OwnerType = Enum.Parse<Recruit.Contracts.ApiResponses.OwnerType>(source.OwnerType?.ToString()),
            ProgrammeId = source.ProgrammeId,
            ProviderReviewFieldIndicators = DeserializeOrNull<List<Recruit.Contracts.ApiResponses.ReviewFieldIndicator>>(source.ProviderReviewFieldIndicators),
            Qualifications = DeserializeOrNull<List<Recruit.Contracts.ApiResponses.Qualification>>(source.Qualifications) ?? [],
            ReviewCount = source.ReviewCount,
            ReviewRequestedByUserId = source.ReviewRequestedByUserId,
            ReviewRequestedDate = source.ReviewRequestedDate?.UtcDateTime,
            ShortDescription = source.ShortDescription,
            Skills = DeserializeOrNull<List<string>>(source.Skills) ?? [],
            SourceOrigin = NullOrEnum<Recruit.Contracts.ApiResponses.SourceOrigin>(source.SourceOrigin?.ToString()),
            SourceType = NullOrEnum<Recruit.Contracts.ApiResponses.SourceType>(source.SourceType?.ToString()),
            SourceVacancyReference = source.SourceVacancyReference,
            StartDate = source.StartDate?.UtcDateTime,
            Status = Enum.Parse<Recruit.Contracts.ApiResponses.VacancyStatus>(source.Status.ToString()),
            SubmittedByUserId = source.SubmittedByUserId,
            SubmittedDate = source.SubmittedDate?.UtcDateTime,
            ThingsToConsider = source.ThingsToConsider,
            Title = source.Title,
            TrainingDescription = source.TrainingDescription,
            TrainingProvider = source.Ukprn is null
                ? null
                : new Recruit.Contracts.ApiResponses.TrainingProvider
                {
                    Ukprn = source.Ukprn,
                    Name = source.TrainingProvider_Name!,
                    Address = DeserializeOrNull<Recruit.Contracts.ApiResponses.TrainingProviderAddress>(source.TrainingProvider_Address)!,
                },
            TransferInfo = DeserializeOrNull<Recruit.Contracts.ApiResponses.TransferInfo>(source.TransferInfo),
            VacancyReference = source.VacancyReference,
            Wage = source.Wage_WageType is null && source.Wage_DurationUnit is null
                ? null
                : new Recruit.Contracts.ApiResponses.Wage
                {
                    CompanyBenefitsInformation = source.Wage_CompanyBenefitsInformation,
                    Duration = source.Wage_Duration,
                    DurationUnit = source.Wage_DurationUnit != null ? Enum.Parse<Recruit.Contracts.ApiResponses.DurationUnit>(source.Wage_DurationUnit.ToString()!) : null,
                    FixedWageYearlyAmount = (double?)source.Wage_FixedWageYearlyAmount,
                    WageAdditionalInformation = source.Wage_WageAdditionalInformation,
                    WageType = source.Wage_WageType != null ? Enum.Parse<Recruit.Contracts.ApiResponses.WageType>(source.Wage_WageType.ToString()!) : null,
                    WeeklyHours = (double?)source.Wage_WeeklyHours,
                    WorkingWeekDescription = source.Wage_WorkingWeekDescription,
                },
        };
    }

    private static Recruit.Contracts.ApiResponses.AvailableWhere? MapEmployerLocationOption(IAllVacancyFields source, List<Recruit.Contracts.ApiResponses.Address> employerLocations)
    {
        if (source is { EmployerLocationOption: not null })
        {
            return NullOrEnum<Recruit.Contracts.ApiResponses.AvailableWhere>(source.EmployerLocationOption.ToString());
        }

        return source.Status switch
        {
            // draft vacancy that hasn't progressed to setting this property yet
            VacancyStatus.Draft => null,

            // field should be set by now, so guesstimate it based on the locations
            _ => employerLocations switch
            {
                { Count: 1 } => Recruit.Contracts.ApiResponses.AvailableWhere.OneLocation,
                { Count: > 1 } => Recruit.Contracts.ApiResponses.AvailableWhere.MultipleLocations,
                _ => Recruit.Contracts.ApiResponses.AvailableWhere.AcrossEngland
            }
        };
    }
}