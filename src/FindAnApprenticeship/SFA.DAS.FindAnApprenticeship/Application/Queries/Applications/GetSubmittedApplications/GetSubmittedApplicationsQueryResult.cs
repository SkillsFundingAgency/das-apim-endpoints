using SFA.DAS.FindAnApprenticeship.Domain.Models;
using System;
using System.Collections.Generic;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Applications.GetSubmittedApplications
{
    public record GetSubmittedApplicationsQueryResult
    {
        public List<Application> SubmittedApplications { get; set; } = [];

        public class Application
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public string VacancyReference { get; set; }
            public string EmployerName { get; set; }
            public string City { get; set; }
            public string Postcode { get; set; }
            public DateTime CreatedDate { get; set; }
            public DateTime? SubmittedDate { get; set; }
            public DateTime ClosingDate { get; set; }
            public ApplicationStatus Status { get; set; }
        }
    }
}
