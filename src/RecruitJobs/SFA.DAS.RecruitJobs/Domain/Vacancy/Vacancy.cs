using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Domain;
using DomainAddress = SFA.DAS.SharedOuterApi.Models.Address;

namespace SFA.DAS.RecruitJobs.Domain.Vacancy;

public class Vacancy
{
    public Guid Id { get; init; }
    public long? VacancyReference { get; init; }
    public long? AccountId { get; set; }
    public required VacancyStatus Status { get; set; }
    public ApprenticeshipTypes? ApprenticeshipType { get; init; }
    public string? Title { get; init; }
    public OwnerType? OwnerType { get; set; }
    public SourceOrigin? SourceOrigin { get; init; }
    public SourceType? SourceType { get; init; }
    public long? SourceVacancyReference { get; init; }
    public DateTime? ApprovedDate { get; init; }
    public DateTime? CreatedDate { get; init; }
    public DateTime? LastUpdatedDate { get; init; }
    public DateTime? SubmittedDate { get; init; }
    public DateTime? ReviewRequestedDate { get; init; }
    public DateTime? ClosedDate { get; set; }
    public DateTime? DeletedDate { get; init; }
    public DateTime? LiveDate { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? ClosingDate { get; init; }
    public int ReviewCount { get; init; }
    public string? ApplicationUrl { get; init; }
    public ApplicationMethod? ApplicationMethod { get; init; }
    public string? ApplicationInstructions { get; init; }
    public string? ShortDescription { get; init; }
    public string? Description { get; init; }
    public string? AnonymousReason { get; init; }
    public bool? DisabilityConfident { get; init; }
    public ContactDetail? Contact { get; set; }
    public string? EmployerDescription { get; init; }
    public List<DomainAddress>? EmployerLocations { get; set; }
    public AvailableWhere? EmployerLocationOption { get; set; }
    public string? EmployerLocationInformation { get; init; }
    public string? EmployerName { get; init; }
    public EmployerNameOption? EmployerNameOption { get; init; }
    public string? EmployerRejectedReason { get; init; }
    public string? LegalEntityName { get; init; }
    public string? EmployerWebsiteUrl { get; init; }
    public GeoCodeMethod? GeoCodeMethod { get; init; }
    public long? AccountLegalEntityId { get; init; }
    public int? NumberOfPositions { get; init; }
    public string? OutcomeDescription { get; init; }
    public string? ProgrammeId { get; set; }
    public List<string>? Skills { get; init; }
    public List<Qualification>? Qualifications { get; set; }
    public string? ThingsToConsider { get; init; }
    public string? TrainingDescription { get; init; }
    public string? AdditionalTrainingDescription { get; init; }
    public TrainingProvider? TrainingProvider { get; init; }
    public Wage? Wage { get; set; }
    public ClosureReason? ClosureReason { get; set; }
    public TransferInfo? TransferInfo { get; init; }
    public string? AdditionalQuestion1 { get; init; }
    public string? AdditionalQuestion2 { get; init; }
    public bool? HasSubmittedAdditionalQuestions { get; init; }
    public bool? HasChosenProviderContactDetails { get; init; }
    public bool? HasOptedToAddQualifications { get; init; }
    public List<ReviewFieldIndicator>? EmployerReviewFieldIndicators { get; init; }
    public List<ReviewFieldIndicator>? ProviderReviewFieldIndicators { get; init; }
    public Guid? SubmittedByUserId { get; set; }
    public Guid? ReviewRequestedByUserId { get; set; }
}