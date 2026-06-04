using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.RecruitJobs.Domain;
using System.Collections.Generic;
using ContactDetail = SFA.DAS.RecruitJobs.Domain.ContactDetail;
using Qualification = SFA.DAS.RecruitJobs.Domain.Qualification;
using ReviewFieldIndicator = SFA.DAS.RecruitJobs.Domain.ReviewFieldIndicator;
using TrainingProvider = SFA.DAS.RecruitJobs.Domain.TrainingProvider;
using TransferInfo = SFA.DAS.RecruitJobs.Domain.TransferInfo;
using Wage = SFA.DAS.RecruitJobs.Domain.Wage;

namespace SFA.DAS.RecruitJobs.InnerApi.Requests.Vacancy;

public record PutVacancyRequest(Guid Id, PutVacancyRequestData Payload) : IPutApiRequest
{
    public string PutUrl => $"api/vacancies/{Id}";
    public object Data { get; set; } = Payload;
}

public class PutVacancyRequestData
{
    public long? VacancyReference { get; init; }
    public long? AccountId { get; init; }
    public required SharedOuterApi.Types.Domain.Recruit.VacancyStatus Status { get; init; }
    public SharedOuterApi.Types.Domain.ApprenticeshipTypes? ApprenticeshipType { get; init; }
    public string? Title { get; init; }
    public SharedOuterApi.Types.Domain.Recruit.OwnerType? OwnerType { get; init; }
    public SharedOuterApi.Types.Domain.Recruit.SourceOrigin? SourceOrigin { get; init; }
    public SharedOuterApi.Types.Domain.Recruit.SourceType? SourceType { get; init; }
    public SharedOuterApi.Types.Domain.Recruit.ArchiveType? ArchiveType { get; init; }
    public long? SourceVacancyReference { get; init; }
    public DateTime? ApprovedDate { get; init; }
    public DateTime? CreatedDate { get; init; }
    public DateTime? LastUpdatedDate { get; init; }
    public DateTime? SubmittedDate { get; init; }
    public DateTime? ReviewRequestedDate { get; init; }
    public DateTime? ClosedDate { get; init; }
    public DateTime? DeletedDate { get; init; }
    public DateTime? LiveDate { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? ClosingDate { get; init; }
    public DateTime? ArchivedDate { get; init; }
    public int ReviewCount { get; init; }
    public string? ApplicationUrl { get; init; }
    public SharedOuterApi.Types.Domain.Recruit.ApplicationMethod? ApplicationMethod { get; init; }
    public string? ApplicationInstructions { get; init; }
    public string? ShortDescription { get; init; }
    public string? Description { get; init; }
    public string? AnonymousReason { get; init; }
    public bool? DisabilityConfident { get; init; }
    public ContactDetail? Contact { get; set; }
    public string? EmployerDescription { get; init; }
    public List<Address>? EmployerLocations { get; set; }
    public SharedOuterApi.Types.Domain.AvailableWhere? EmployerLocationOption { get; init; }
    public string? EmployerLocationInformation { get; init; }
    public string? EmployerName { get; init; }
    public SharedOuterApi.Types.Domain.Recruit.EmployerNameOption? EmployerNameOption { get; init; }
    public string? EmployerRejectedReason { get; init; }
    public string? LegalEntityName { get; init; }
    public string? EmployerWebsiteUrl { get; init; }
    public SharedOuterApi.Types.Domain.Recruit.GeoCodeMethod? GeoCodeMethod { get; init; }
    public long? AccountLegalEntityId { get; init; }
    public int? NumberOfPositions { get; init; }
    public string? OutcomeDescription { get; init; }
    public string? ProgrammeId { get; init; }
    public List<string>? Skills { get; init; }
    public List<Qualification>? Qualifications { get; set; }
    public string? ThingsToConsider { get; init; }
    public string? TrainingDescription { get; init; }
    public string? AdditionalTrainingDescription { get; init; }
    public TrainingProvider? TrainingProvider { get; init; }
    public Wage? Wage { get; set; }
    public SharedOuterApi.Types.Domain.Recruit.ClosureReason? ClosureReason { get; init; }
    public TransferInfo? TransferInfo { get; init; }
    public string? AdditionalQuestion1 { get; init; }
    public string? AdditionalQuestion2 { get; init; }
    public bool? HasSubmittedAdditionalQuestions { get; init; }
    public bool? HasChosenProviderContactDetails { get; init; }
    public bool? HasOptedToAddQualifications { get; init; }
    public List<ReviewFieldIndicator>? EmployerReviewFieldIndicators { get; init; }
    public List<ReviewFieldIndicator>? ProviderReviewFieldIndicators { get; init; }
    public string? SubmittedByUserId { get; init; }
    public string? ReviewRequestedByUserId { get; init; }
    public string? ArchivedByUserId { get; init; }
}