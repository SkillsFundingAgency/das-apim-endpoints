using SFA.DAS.Recruit.Domain.Vacancy;
using SFA.DAS.SharedOuterApi.Domain;
using System;

namespace SFA.DAS.Recruit.Domain;
public record VacancySummary
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public long? VacancyReference { get; set; }
    public string? LegalEntityName { get; set; }
    public DateTime? CreatedDate { get; set; }
    public VacancyStatus Status { get; set; }
    public int NoOfNewApplications { get; set; }
    public int NoOfSuccessfulApplications { get; set; }
    public int NoOfUnsuccessfulApplications { get; set; }
    public int NoOfApplications { get; set; }
    public int NoOfSharedApplications { get; set; }
    public int NoOfAllSharedApplications { get; set; }
    public int NoOfEmployerReviewedApplications { get; set; }
    public DateTime? ClosingDate { get; set; }
    public ApplicationMethod? ApplicationMethod { get; set; }
    public string? ProgrammeId { get; set; }
    public string? TrainingTitle { get; set; }
    public TrainingType TrainingType { get; set; }
    public DateTime? TransferInfoTransferredDate { get; set; }
    public bool IsTaskListCompleted { get; set; }
    public ApprenticeshipTypes? ApprenticeshipType { get; set; }
}