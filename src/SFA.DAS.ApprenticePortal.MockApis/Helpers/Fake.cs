using System;
using System.Collections.Generic;
using SFA.DAS.ApprenticePortal.Models;

namespace SFA.DAS.ApprenticePortal.MockApis.Helpers
{
    public static class Fake
    {
        public static Apprentice Apprentice => new Apprentice
        {
            ApprenticeId = Guid.Empty,
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
                LastViewed = null
            };
        }

        public static IEnumerable<Apprenticeship> ApprenticeshipsForThisApprentice(Apprentice apprentice)
        {
            for(var i = 0; i<=2; ++i)
            {
                yield return ApprenticeshipForThisApprentice(Apprentice);
            }
        }
    }
}
