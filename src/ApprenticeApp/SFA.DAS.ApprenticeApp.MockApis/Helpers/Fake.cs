using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using SFA.DAS.ApprenticeApp.InnerApi.CommitmentsV2.Responses;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.ApprenticeApp.Services;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.TrainingProviderService;

namespace SFA.DAS.ApprenticeApp.MockApis.Helpers
{
    [ExcludeFromCodeCoverage]
    public static class Fake
    {
        public static Apprentice Apprentice => new()
        {
            ApprenticeId = Guid.NewGuid(),
            FirstName = Faker.Name.First(),
            LastName = Faker.Name.Last(),
            TermsOfUseAccepted = global::Faker.Boolean.Random()
        };

        public static Apprenticeship ApprenticeshipForThisApprentice(Apprentice apprentice)
        {
            return new Apprenticeship
            {
                ApprenticeId = apprentice?.ApprenticeId ?? Guid.Empty,
                Id = Faker.RandomNumber.Next(),
                EmployerName = $"Employer {Faker.Company.Name()}",
                CourseName = $"Course {Faker.Lorem.Sentence(1)}",
                ApprovedOn = DateTime.UtcNow.AddMonths(-3),
                ConfirmedOn = null,
                StoppedReceivedOn = null,
                LastViewed = null,
                HasBeenConfirmedAtLeastOnce = false
            };
        }

        public static IEnumerable<Apprenticeship> ApprenticeshipsForThisApprentice(Apprentice apprentice)
        {
            for(var i = 0; i<=2; ++i)
            {
                yield return ApprenticeshipForThisApprentice(Apprentice);
            }
        }
        
        public static MyApprenticeshipData MyApprenticeship => new()
        {
            ApprenticeshipId = Faker.RandomNumber.Next(),
            Uln = Faker.RandomNumber.Next(10000000),
            EmployerName = Faker.Company.Name(),
            StartDate = DateTime.Today,
            EndDate = DateTime.Today.AddYears(1),
            TrainingProviderId = Faker.RandomNumber.Next(),
            TrainingProviderName = Faker.Company.Name(),
            TrainingCode = Faker.RandomNumber.Next().ToString(),
            StandardUId = Faker.RandomNumber.Next().ToString()
        };

        public static ApprenticeshipDetailsResponse CommitmentsApprenticeship => new()
        {
            Id = Faker.RandomNumber.Next(),
            AccountLegalEntityId = Faker.RandomNumber.Next(),
            CompletionDate = null,
            CourseCode = Faker.RandomNumber.Next().ToString(),
            CourseName = Faker.Finance.Ticker(),
            DateOfBirth = DateTime.Today.AddYears(-19),
            Email = Faker.NameFormats.Standard.ToString(),
            EmployerAccountId = Faker.RandomNumber.Next(),
            EmployerName = Faker.Company.Name(),
            StartDate = DateTime.Today,
            EndDate = DateTime.Today.AddYears(1)
        };

        public static TrainingProviderResponse Provider => new()
        {
            Id = Guid.NewGuid(),
            Ukprn = Faker.RandomNumber.Next(),
            LegalName = Faker.Company.Name()
        };

        public static StandardApiResponse StandardApiResponse => new()
        {
            Title = Faker.Lorem.Sentence(7)
        };
    }
}
