using System;
using System.Collections.Generic;
using SFA.DAS.Recruit.Domain.Vacancy;
using SFA.DAS.SharedOuterApi.Domain;

namespace SFA.DAS.Recruit.Api.Models.Vacancies.Responses;

public class PostVacancyResponse
{
    public Guid Id { get; set; }
    public string? EmployerAccountId { get; set; }
    public long? VacancyReference { get; set; }
    public VacancyStatus? Status { get; set; }
    public OwnerType OwnerType { get; set; }
    public SourceOrigin? SourceOrigin { get; set; }
    public SourceType? SourceType { get; set; }
    public long? SourceVacancyReference { get; set; }
    public DateTime? ClosedDate { get; set; }
    public VacancyUser? ClosedByUser { get; set; }
    public DateTime? CreatedDate { get; set; }
    public VacancyUser? CreatedByUser { get; set; }
    public DateTime? SubmittedDate { get; set; }
    public VacancyUser? SubmittedByUser { get; set; }
    public DateTime? ReviewDate { get; set; }
    public VacancyUser? ReviewByUser { get; set; }
    public int ReviewCount { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public DateTime? LiveDate { get; set; }
    public DateTime? LastUpdatedDate { get; set; }
    public VacancyUser? LastUpdatedByUser { get; set; }
    public bool? IsDeleted { get; set; }
    public DateTime? DeletedDate { get; set; }
    public VacancyUser? DeletedByUser { get; set; }
    public string? AnonymousReason { get; set; }
    public string? ApplicationInstructions { get; set; }
    public ApplicationMethod? ApplicationMethod { get; set; }
    public string? ApplicationUrl { get; set; }
    public DateTime? ClosingDate { get; set; }
    public string? Description { get; set; }
    public DisabilityConfident? DisabilityConfident { get; set; }
    public ContactDetail? EmployerContact { get; set; }
    public string? EmployerDescription { get; set; }
    public Address? EmployerLocation { get; set; }
    public List<Address>? EmployerLocations { get; set; }
    public AvailableWhere? EmployerLocationOption { get; set; }
    public string? EmployerLocationInformation { get; set; }
    public string? EmployerName { get; set; }
    public EmployerNameOption? EmployerNameOption { get; set; }
    public List<EmployerReviewFieldIndicator>? EmployerReviewFieldIndicators { get; set; }
    public string? EmployerRejectedReason { get; set; }
    public string? LegalEntityName { get; set; }
    public string? EmployerWebsiteUrl { get; set; }
    public GeoCodeMethod? GeoCodeMethod { get; set; }
    public string? AccountLegalEntityPublicHashedId { get; set; }
    public int? NumberOfPositions { get; set; }
    public string? OutcomeDescription { get; set; }
    public ContactDetail? ProviderContact { get; set; }
    public List<ProviderReviewFieldIndicator>? ProviderReviewFieldIndicators { get; set; }
    public string? ProgrammeId { get; set; }
    public bool? HasOptedToAddQualifications { get; set; }
    public List<Qualification>? Qualifications { get; set; }
    public ApprenticeshipTypes? ApprenticeshipType { get; set; }
    public string? ShortDescription { get; set; }
    public List<string>? Skills { get; set; }
    public DateTime? StartDate { get; set; }
    public string? ThingsToConsider { get; set; }
    public string? Title { get; set; }
    public string? TrainingDescription { get; set; }
    public string? AdditionalTrainingDescription { get; set; }
    public TrainingProvider? TrainingProvider { get; set; }
    public Wage? Wage { get; set; }
    public ClosureReason? ClosureReason { get; set; }
    public string? ClosureExplanation { get; set; }
    public TransferInfo? TransferInfo { get; set; }
    public string? AdditionalQuestion1 { get; set; }
    public string? AdditionalQuestion2 { get; set; }
    public bool? HasSubmittedAdditionalQuestions { get; set; }
    public bool? HasChosenProviderContactDetails { get; set; }
}