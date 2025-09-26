using System;
using System.Collections.Generic;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.SharedOuterApi.Domain;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Users.GetAccountDeletionQuery
{
    public record GetAccountDeletionQueryResult
    {
        public List<Application> SubmittedApplications { get; set; } = [];

        public class Application
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public string VacancyReference { get; set; }
            public string EmployerName { get; set; }
            public DateTime CreatedDate { get; set; }
            public DateTime? SubmittedDate { get; set; }
            public DateTime ClosingDate { get; set; }
            public ApplicationStatus Status { get; set; }
            public Address Address { get; set; }
            public List<Address> OtherAddresses { get; set; } = [];
            public string? EmploymentLocationInformation { get; set; }
            public AvailableWhere? EmployerLocationOption { get; set; }
            public ApprenticeshipTypes ApprenticeshipType { get; set; }
        }
    }
}
