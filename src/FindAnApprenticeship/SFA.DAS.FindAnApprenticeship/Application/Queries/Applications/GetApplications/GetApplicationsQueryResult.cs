using System;
using System.Collections.Generic;
using SFA.DAS.FindAnApprenticeship.Models;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Applications.GetApplications;

public class GetApplicationsQueryResult
{
    public List<Application> Applications { get; set; } = [];


    public class Application
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string VacancyReference { get; set; }
        public string EmployerName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public DateTime ClosingDate { get; set; }
        public DateTime? ResponseDate { get; set; }
        public string? ResponseNotes { get; set; }
        public ApplicationStatus Status { get; set; }
    }
}