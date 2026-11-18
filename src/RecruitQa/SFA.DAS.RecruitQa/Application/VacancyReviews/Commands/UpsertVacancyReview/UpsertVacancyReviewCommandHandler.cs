using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.Recruit.Contracts.ApiRequests;
using SFA.DAS.Recruit.Contracts.ApiResponses;
using IRecruitApiClient = SFA.DAS.Recruit.Contracts.Client.IRecruitApiClient<SFA.DAS.Recruit.Contracts.Client.RecruitApiConfiguration>;

namespace SFA.DAS.RecruitQa.Application.VacancyReviews.Commands.UpsertVacancyReview;

public class UpsertVacancyReviewCommandHandler(IRecruitApiClient apiClient) : IRequestHandler<UpsertVacancyReviewCommand>
{
    public async Task Handle(UpsertVacancyReviewCommand request, CancellationToken cancellationToken)
    {
        var response = await apiClient.PutWithResponseCode<NullResponse>(new PutVacancyreviewsByIdApiRequest
        {
            Id = request.Id,
            Data = new PutVacancyReviewRequest
            {
                AccountId = request.VacancyReview.AccountId,
                AccountLegalEntityId = request.VacancyReview.AccountLegalEntityId,
                VacancyReference = request.VacancyReview.VacancyReference.ToString(),
                Status = Enum.Parse<ReviewStatus>(request.VacancyReview.Status),
                ManualOutcome = request.VacancyReview.ManualOutcome,
                CreatedDate = request.VacancyReview.CreatedDate,
                OwnerType = request.VacancyReview.OwnerType,
                AutomatedQaOutcome = request.VacancyReview.AutomatedQaOutcome,
                ClosedDate = request.VacancyReview.ClosedDate,
                ManualQaComment = request.VacancyReview.ManualQaComment,
                ReviewedByUserEmail = request.VacancyReview.ReviewedByUserEmail,
                VacancyTitle = request.VacancyReview.VacancyTitle,
                ReviewedDate = request.VacancyReview.ReviewedDate,
                AutomatedQaOutcomeIndicators = request.VacancyReview.AutomatedQaOutcomeIndicators,
                DismissedAutomatedQaOutcomeIndicators = request.VacancyReview.DismissedAutomatedQaOutcomeIndicators,
                ManualQaEditFieldIndicators = request.VacancyReview.ManualQaEditFieldIndicators,
                ManualQaFieldIndicators = request.VacancyReview.ManualQaFieldIndicators,
                UpdatedFieldIdentifiers = request.VacancyReview.UpdatedFieldIdentifiers,
                SubmissionCount = request.VacancyReview.SubmissionCount,
                VacancySnapshot = request.VacancyReview.VacancySnapshot,
                Ukprn = request.VacancyReview.Ukprn,
                SlaDeadLine = request.VacancyReview.SlaDeadLine,
                SubmittedByUserEmail = request.VacancyReview.SubmittedByUserEmail,
            }
        });
        response.EnsureSuccessStatusCode();
    }
}