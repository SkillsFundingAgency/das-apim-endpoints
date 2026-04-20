using System;
using System.Collections.Generic;
using System.Text;
using SFA.DAS.Recruit.Contracts.ApiResponses;
using SFA.DAS.VacanciesManage.InnerApi.Requests;

namespace SFA.DAS.VacanciesManage.Models.Mapper;

public static class VacancyExtensions
{
    public static PostVacancyRequest ToPostVacancyRequest(this PostVacancyV2RequestData source)
    {
        if (source == null)
        {
            return null;
        }
        return new PostVacancyRequest
        {
            Title = source.Title,
            Description = source.Description,
            TrainingProvider = new TrainingProvider
            {
                Name = source.TrainingProvider?.Name,
                Ukprn = source.TrainingProvider?.Ukprn,
                Address = new TrainingProviderAddress
                {
                    AddressLine1 = source.TrainingProvider?.Address?.AddressLine1,
                    AddressLine2 = source.TrainingProvider?.Address?.AddressLine2,
                    AddressLine3 = source.TrainingProvider?.Address?.AddressLine3,
                    AddressLine4 = source.TrainingProvider?.Address?.AddressLine4,
                    Postcode = source.TrainingProvider?.Address?.Postcode,
                    Latitude = source.TrainingProvider?.Address?.Latitude,
                    Longitude = source.TrainingProvider?.Address?.Longitude
                }
            },
            LegalEntityName = source.LegalEntityName,
            Contact = source.Contact,
            VacancyType = source.VacancyType,
            ExpectedStartDate = source.ExpectedStartDate,
            ExpectedEndDate = source.ExpectedEndDate,
            NumberOfPositions = source.NumberOfPositions,
            ApplicationInstructions = source.ApplicationInstructions,
            ApplicationUrl = source.ApplicationUrl,
            Location = source.Location,
            WorkingWeekDescription = source.WorkingWeekDescription,
            DurationDescription = source.DurationDescription,
            WageDescription = source.WageDescription,
            EmployerName = source.EmployerName,
            Qualifications = source.Qualifications,
            AccountId = source.AccountId,
            AccountLegalEntityId = source.AccountLegalEntityId,
            AdditionalQuestion1 = source.AdditionalQuestion1,
            AdditionalQuestion2 = source.AdditionalQuestion2,
            AdditionalTrainingDescription = source.AdditionalTrainingDescription,
            AnonymousReason = source.AnonymousReason,
            ApplicationMethod = source.ApplicationMethod,
            ApprenticeshipType = source.ApprenticeshipType,
            SubmittedDate = source.SubmittedDate,
            ClosedDate = source.ClosingDate,
            ClosingDate = source.ClosingDate,
            ClosureReason = source.

        };
    }
}
