using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using SFA.DAS.RecruitJobs.Domain;
using SFA.DAS.RecruitJobs.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using StrawberryShake;

namespace SFA.DAS.RecruitJobs.Handlers;

public interface ITransferProviderVacancyToLegalEntityHandler
{
    Task HandleAsync(Guid vacancyId, TransferReason transferReason, CancellationToken cancellationToken);
}

public class TransferProviderVacancyToLegalEntityHandler(
    ILogger<TransferProviderVacancyToLegalEntityHandler> logger,
    IRecruitGqlClient recruitGqlClient,
    IRecruitApiClient<RecruitApiConfiguration> recruitApiClient): ITransferProviderVacancyToLegalEntityHandler
{
    public async Task HandleAsync(Guid vacancyId, TransferReason transferReason, CancellationToken cancellationToken)
    {
        var response = await recruitGqlClient.GetProviderTransferableVacancyDetails.ExecuteAsync(vacancyId, cancellationToken);
        response.EnsureNoErrors();
        if (response is not { Data.Vacancies.Count: 1 })
        {
            logger.LogError("Could not find Vacancy '{VacancyId}' whilst handling vacancy transfer request.", vacancyId);
            return;
        }

        var vacancyDetails = response.Data.Vacancies[0];
        var patchDocument = new JsonPatchDocument<Vacancy>();
        var now = DateTime.UtcNow;

        VacancyReview? vacancyReview = null;
        switch (vacancyDetails.Status)
        {
            case VacancyStatus.Draft:
            case VacancyStatus.Referred:
            case VacancyStatus.Closed:
                break;
            case VacancyStatus.Submitted:
                var vacancyReviews = (await recruitApiClient.GetAll<VacancyReview>(new GetVacancyReviewsByVacancyReferenceRequest(vacancyDetails.VacancyReference!.Value)))?.ToList() ?? [];
                vacancyReview = vacancyReviews.OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                if (vacancyReview is not null && vacancyReview.Status is not ReviewStatus.UnderReview)
                {
                    patchDocument.Replace(x => x.Status, VacancyStatus.Draft);
                }
                break;
            case VacancyStatus.Live:
                patchDocument.Replace(x => x.Status, VacancyStatus.Closed);
                patchDocument.Replace(x => x.ClosedDate, now);
                patchDocument.Replace(x => x.ClosureReason, ClosureReason.TransferredByEmployer);
                break;
            case VacancyStatus.Approved:
                patchDocument.Replace(x => x.ApprovedDate, null);
                patchDocument.Replace(x => x.Status, VacancyStatus.Closed);
                patchDocument.Replace(x => x.ClosedDate, now);
                patchDocument.Replace(x => x.ClosureReason, ClosureReason.TransferredByEmployer);
                break;
            case VacancyStatus.Rejected:
            case VacancyStatus.Review:
                patchDocument.Replace(x => x.Status, VacancyStatus.Draft);
                break;
            default:
                 throw new ArgumentException($"{vacancyDetails.Status} is not a recognised '{nameof(VacancyStatus)}' the vacancy '{vacancyId}' can be transferred from.");
        }

        patchDocument.Replace(x => x.TransferInfo, new TransferInfo
        {
            Ukprn = vacancyDetails.Ukprn!.Value,
            ProviderName = vacancyDetails.TrainingProvider_Name,
            LegalEntityName = vacancyDetails.LegalEntityName,
            TransferredDate = now,
            Reason = transferReason,
        });

        patchDocument.Replace(x => x.OwnerType, OwnerType.Employer);
        patchDocument.Replace(x => x.Contact, null);
        patchDocument.Replace(x => x.SubmittedByUserId, null);
        patchDocument.Replace(x => x.ReviewRequestedByUserId, null);

        var patchRequest = new PatchVacancyRequest(vacancyId, patchDocument);
        var patchResponse = await recruitApiClient.PatchWithResponseCode(patchRequest);
        patchResponse.EnsureSuccessStatusCode();
        
        switch (vacancyDetails.Status)
        {
            case VacancyStatus.Submitted:
                switch (vacancyReview)
                {
                    case { Status: ReviewStatus.New or ReviewStatus.PendingReview }:
                    {
                        var vacancyReviewPatch = new JsonPatchDocument<VacancyReview>();
                        vacancyReviewPatch.Replace(x => x.ManualOutcome,
                            transferReason == TransferReason.BlockedByQa
                                ? nameof(ManualQaOutcome.Blocked)
                                : nameof(ManualQaOutcome.Transferred));
                        vacancyReviewPatch.Replace(x => x.Status, ReviewStatus.Closed);
                        vacancyReviewPatch.Replace(x => x.ClosedDate, now);

                        var vacancyReviewPatchResponse = await recruitApiClient.PatchWithResponseCode(new PatchVacancyReviewRequest(vacancyReview.Id, vacancyReviewPatch));
                        vacancyReviewPatchResponse.EnsureSuccessStatusCode();
                        break;
                    }
                    case { Status: ReviewStatus.UnderReview }:
                        logger.LogWarning("Latest vacancy review for vacancy '{VacancyReference}' that has been transferred is currently being reviewed.", vacancyReview.VacancyReference);
                        break;
                }
                break;
            // TODO: handle VacancyClosedEvent
            // case VacancyStatus.Approved:
            // case VacancyStatus.Live:
            //     await _messaging.PublishEvent(new VacancyClosedEvent
            //     {
            //         VacancyReference = vacancy.VacancyReference.Value,
            //         VacancyId = vacancy.Id
            //     });
            //     break;
        }
    }
}