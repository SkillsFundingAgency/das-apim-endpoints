using SFA.DAS.SharedOuterApi.Types.Domain.Recruit.Reviews;

namespace SFA.DAS.SharedOuterApi.Recruit.Services;

public interface IVacancyReviewService
{
    Task<VacancyReview> CreateAsync(Guid vacancyId, CancellationToken cancellationToken = default);
}

public class VacancyReviewService(
    IVacancySlaDeadlineService vacancySlaDeadlineService,
    IBankHolidaysService bankHolidaysService): IVacancyReviewService
{
    public Task<VacancyReview> CreateAsync(Guid vacancyId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();

        //var slaDeadline = await vacancySlaDeadlineService.GetSlaDeadlineAsync(vacancy.SubmittedDate.GetValueOrDefault(), cancellationToken);

        /* TODO: implement the following:
         
            logger.LogInformation("Creating vacancy review for vacancy {vacancyReference}.", message.VacancyReference);

            var vacancyTask = vacancyRepository.GetVacancyAsync(message.VacancyReference);
            var previousReviewsTask = vacancyReviewQuery.GetForVacancyAsync(message.VacancyReference);

            await Task.WhenAll(vacancyTask, previousReviewsTask);

            var vacancy = vacancyTask.Result;
            var previousReviews = previousReviewsTask.Result;

            //Defensive code, just in case the message is republished due to an error after creating the review.
            var activePreviousReview = previousReviews.FirstOrDefault(r => r.Status != ReviewStatus.Closed);
            if (activePreviousReview != null)
            {
                logger.LogWarning("Cannot create review for vacancy {MessageVacancyReference} as an active review {Guid} already exists.", message.VacancyReference, activePreviousReview.Id);
                return Unit.Value;
            }

            var slaDeadline = await slaService.GetSlaDeadlineAsync(vacancy.SubmittedDate.GetValueOrDefault());

            var updatedFields = GetUpdatedFields(vacancy, previousReviews);

            var review = BuildNewReview(vacancy, previousReviews.Count, slaDeadline, updatedFields, previousReviews.OrderByDescending(c=>c.SubmissionCount).FirstOrDefault());

            await vacancyReviewRepository.CreateAsync(review);

            return Unit.Value;
         */
    }
}