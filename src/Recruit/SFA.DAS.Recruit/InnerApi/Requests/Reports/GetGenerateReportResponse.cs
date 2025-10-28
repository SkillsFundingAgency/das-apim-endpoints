using SFA.DAS.Recruit.Enums;
using SFA.DAS.SharedOuterApi.Domain;
using System;
using System.Collections.Generic;

namespace SFA.DAS.Recruit.InnerApi.Requests.Reports;
public record GetGenerateReportResponse
{
    public List<ApplicationReviewReport> ApplicationReviewReports { get; set; } = [];

    public record ApplicationReviewReport
    {
        public Guid ApplicationId { get; set; }
        public Guid CandidateId { get; set; }
        public long VacancyReference { get; set; }
        public string? VacancyTitle { get; set; }
        public string? EmployerName { get; set; }
        public string? TrainingProviderName { get; set; }
        public int ProgrammeId { get; set; }
        public DateTime? VacancyClosingDate { get; set; }
        public DateTime? ApplicationSubmittedDate { get; set; }
        public AvailableWhere? AvailableWhere { get; set; }
        public ApplicationReviewStatus ApplicationStatus { get; set; }
        public ApprenticeshipTypes ApprenticeshipType { get; set; }
        public int? NumberOfDaysApplicationAtThisStatus { get; set; }
    }
}