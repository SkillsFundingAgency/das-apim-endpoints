using System.Text.Json;
using Microsoft.Extensions.Logging;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Domain.Recruit;
using SFA.DAS.SharedOuterApi.Types.Domain.Recruit.Reviews;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Recruit;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Recruit;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.SharedOuterApi.Recruit.Services;

public interface IVacancyReviewService
{
    Task<VacancyReview?> CreateAsync(Guid vacancyId, string submittingUserEmailAddress, CancellationToken cancellationToken = default);
    Task<VacancyReview?> CreateAsync(Vacancy vacancy, string submittingUserEmailAddress, CancellationToken cancellationToken = default);
}

public class VacancyReviewService(
    ILogger<VacancyReviewService> logger,
    IRecruitApiClient<RecruitApiConfiguration> recruitApiClient,
    IVacancySlaDeadlineService vacancySlaDeadlineService,
    IVacancyComparerService vacancyComparerService): IVacancyReviewService
{
    public async Task<VacancyReview?> CreateAsync(Guid vacancyId, string submittingUserEmailAddress, CancellationToken cancellationToken = default)
    {
        // fetch the vacancy
        var vacancyResponse = await recruitApiClient.GetWithResponseCode<GetVacancyResponse>(new GetVacancyRequest(vacancyId));
        vacancyResponse.EnsureSuccessStatusCode();
        Vacancy vacancy = vacancyResponse.Body;
        
        return await CreateAsync(vacancy, submittingUserEmailAddress, cancellationToken);
    }

    public async Task<VacancyReview?> CreateAsync(Vacancy vacancy, string submittingUserEmailAddress, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Creating vacancy review for vacancy {VacancyId}.", vacancy.Id);
        
        // get the reviews
        var vacancyReviews = (await recruitApiClient.GetAll<VacancyReview>(new GetVacancyReviewsByVacancyReferenceRequest(vacancy.VacancyReference!.Value))).ToList() ?? [];

        // defensive code, just in case the message is republished due to an error after creating the review
        var activePreviousReview = vacancyReviews.FirstOrDefault(r => r.Status != ReviewStatus.Closed);
        if (activePreviousReview is not null)
        {
            logger.LogWarning("Cannot create review for vacancy {VacancyReference} as an active review {Guid} already exists.", vacancy.VacancyReference, activePreviousReview.Id);
            return null;
        }
        
        // get info for the review
        var slaDeadline = await vacancySlaDeadlineService.GetSlaDeadlineAsync(vacancy.SubmittedDate.GetValueOrDefault(), cancellationToken);
        var updatedFields = GetUpdatedFields(vacancy, vacancyReviews);
        var previousReview = vacancyReviews.OrderByDescending(x => x.SubmissionCount).FirstOrDefault();
        
        // create the review
        var vacancyReview = new VacancyReview
        {
            Id = Guid.NewGuid(),
            VacancyReference = vacancy.VacancyReference.GetValueOrDefault(),
            VacancyTitle = vacancy.Title,
            Status = ReviewStatus.New,    
            CreatedDate = TimeProvider.System.GetUtcNow().DateTime,
            SubmittedByUserEmail = submittingUserEmailAddress,
            SubmissionCount = vacancyReviews.Count + 1,
            SlaDeadLine = slaDeadline,
            VacancySnapshot = JsonSerializer.Serialize(vacancy),
            UpdatedFieldIdentifiers = updatedFields,
            DismissedAutomatedQaOutcomeIndicators = previousReview?.DismissedAutomatedQaOutcomeIndicators.Except(updatedFields).ToList()
        };

        var createVacancyReviewResponse = await recruitApiClient.PutWithResponseCode<NullResponse>(new PutVacancyReviewRequest(vacancyReview.Id, vacancyReview));
        createVacancyReviewResponse.EnsureSuccessStatusCode();
        return vacancyReview;
    }

    private List<string> GetUpdatedFields(Vacancy vacancy, IEnumerable<VacancyReview> allReviewsForVacancy)
    {
        var previousReview = GetPreviousReferredVacancyReview(allReviewsForVacancy);
        if(previousReview == null)
        {
            return [];
        }

        var snapshot = JsonSerializer.Deserialize<Vacancy>(previousReview.VacancySnapshot)!;
        var comparison = vacancyComparerService.Compare(vacancy, snapshot);
        return comparison.Fields
            .Where(f => !f.AreEqual)
            .Select(f => f.FieldName)
            .ToList();
    }

    private static VacancyReview? GetPreviousReferredVacancyReview(IEnumerable<VacancyReview> allReviewsForVacancy)
    {
        return allReviewsForVacancy
            .Where(r => r is { Status: ReviewStatus.Closed, ManualOutcome: ManualQaOutcome.Referred })
            .OrderByDescending(r => r.ClosedDate)
            .FirstOrDefault();
    }
}