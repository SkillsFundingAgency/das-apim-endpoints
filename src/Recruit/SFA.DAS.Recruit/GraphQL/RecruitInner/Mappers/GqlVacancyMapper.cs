using System;
using System.Collections.Generic;
using System.Text.Json;
using SFA.DAS.Recruit.Domain.Vacancy;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Recruit.GraphQL.RecruitInner.Mappers;

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

    public static Vacancy From(IAllVacancyFields source)
    {
        var employerLocations = DeserializeOrNull<List<Address>>(source.EmployerLocations) ?? [];

        return new Vacancy
        {
            AccountId = source.AccountId,
            AccountLegalEntityId = source.AccountLegalEntityId,
            AdditionalQuestion1 = source.AdditionalQuestion1,
            AdditionalQuestion2 = source.AdditionalQuestion2,
            AdditionalTrainingDescription = source.AdditionalTrainingDescription,
            AnonymousReason = source.AnonymousReason,
            ApplicationInstructions = source.ApplicationInstructions,
            ApplicationMethod = NullOrEnum<Domain.Vacancy.ApplicationMethod>(source.ApplicationMethod?.ToString()),
            ApplicationUrl = source.ApplicationUrl,
            ApprenticeshipType = NullOrEnum<SharedOuterApi.Domain.ApprenticeshipTypes>(source.ApprenticeshipType?.ToString()),
            ApprovedDate = source.ApprovedDate?.UtcDateTime,
            CreatedDate = source.CreatedDate?.UtcDateTime,
            ClosedDate = source.ClosedDate?.UtcDateTime,
            ClosingDate = source.ClosingDate?.UtcDateTime,
            ClosureReason = NullOrEnum<Domain.Vacancy.ClosureReason>(source.ClosureReason?.ToString()),
            Contact = source is { ContactName: null, ContactEmail: null, ContactPhone: null }
                ? null
                : new ContactDetail
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
            EmployerNameOption = NullOrEnum<Domain.Vacancy.EmployerNameOption>(source.EmployerNameOption?.ToString()),
            EmployerRejectedReason = source.EmployerRejectedReason,
            EmployerReviewFieldIndicators = DeserializeOrNull<List<ReviewFieldIndicator>>(source.EmployerReviewFieldIndicators),
            EmployerWebsiteUrl = source.EmployerWebsiteUrl,
            GeoCodeMethod = NullOrEnum<Domain.Vacancy.GeoCodeMethod>(source.GeoCodeMethod?.ToString()),
            HasChosenProviderContactDetails = source.HasChosenProviderContactDetails,
            HasOptedToAddQualifications = source.HasOptedToAddQualifications,
            HasSubmittedAdditionalQuestions = source.HasSubmittedAdditionalQuestions,
            Id = source.Id,
            LastUpdatedDate = source.LastUpdatedDate?.UtcDateTime,
            LegalEntityName = source.LegalEntityName,
            LiveDate = source.LiveDate?.UtcDateTime,
            NumberOfPositions = source.NumberOfPositions,
            OutcomeDescription = source.OutcomeDescription,
            OwnerType = NullOrEnum<Domain.Vacancy.OwnerType>(source.OwnerType?.ToString()),
            ProgrammeId = source.ProgrammeId,
            ProviderReviewFieldIndicators = DeserializeOrNull<List<ReviewFieldIndicator>>(source.ProviderReviewFieldIndicators),
            Qualifications = DeserializeOrNull<List<Qualification>>(source.Qualifications) ?? [],
            ReviewCount = source.ReviewCount,
            ReviewRequestedByUserId = source.ReviewRequestedByUserId,
            ReviewRequestedDate = source.ReviewRequestedDate?.UtcDateTime,
            ShortDescription = source.ShortDescription,
            Skills = DeserializeOrNull<List<string>>(source.Skills) ?? [],
            SourceOrigin = NullOrEnum<Domain.Vacancy.SourceOrigin>(source.SourceOrigin?.ToString()),
            SourceType = NullOrEnum<Domain.Vacancy.SourceType>(source.SourceType?.ToString()),
            SourceVacancyReference = source.SourceVacancyReference,
            StartDate = source.StartDate?.UtcDateTime,
            Status = Enum.Parse<Domain.Vacancy.VacancyStatus>(source.Status.ToString()),
            SubmittedByUserId = source.SubmittedByUserId,
            SubmittedDate = source.SubmittedDate?.UtcDateTime,
            ThingsToConsider = source.ThingsToConsider,
            Title = source.Title,
            TrainingDescription = source.TrainingDescription,
            TrainingProvider = source.Ukprn is null
                ? null
                : new TrainingProvider
                {
                    Ukprn = source.Ukprn,
                    Name = source.TrainingProvider_Name!,
                    Address = DeserializeOrNull<Address>(source.TrainingProvider_Address)!,
                },
            TransferInfo = DeserializeOrNull<TransferInfo>(source.TransferInfo),
            VacancyReference = source.VacancyReference,
            Wage = source.Wage_WageType is null && source.Wage_DurationUnit is null 
                ? null
                : new Wage
                {
                    CompanyBenefitsInformation = source.Wage_CompanyBenefitsInformation,
                    Duration = source.Wage_Duration,
                    DurationUnit = source.Wage_DurationUnit != null ?  Enum.Parse<Domain.Vacancy.DurationUnit>(source.Wage_DurationUnit.ToString()!) : null,
                    FixedWageYearlyAmount = source.Wage_FixedWageYearlyAmount,
                    WageAdditionalInformation = source.Wage_WageAdditionalInformation,
                    WageType = source.Wage_WageType != null ? Enum.Parse<Domain.Vacancy.WageType>(source.Wage_WageType.ToString()!) : null,
                    WeeklyHours = source.Wage_WeeklyHours,
                    WorkingWeekDescription = source.Wage_WorkingWeekDescription,
                },
        };
    }

    private static SharedOuterApi.Domain.AvailableWhere? MapEmployerLocationOption(IAllVacancyFields source, List<Address> employerLocations)
    {
        if (source is { EmployerLocationOption: not null })
        {
            return NullOrEnum<SharedOuterApi.Domain.AvailableWhere>(source.EmployerLocationOption.ToString());
        }

        return source.Status switch
        {
            // draft vacancy that hasn't progressed to setting this property yet
            VacancyStatus.Draft => null,
            
            // field should be set by now, so guesstimate it based on the locations
            _ => employerLocations switch
            {
                { Count: 1 } => SharedOuterApi.Domain.AvailableWhere.OneLocation,
                { Count: > 1 } => SharedOuterApi.Domain.AvailableWhere.MultipleLocations,
                _ => SharedOuterApi.Domain.AvailableWhere.AcrossEngland
            }
        };
    }
}