using System.Collections.Generic;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using Microsoft.Extensions.Logging;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.Recruit.Contracts.ApiRequests;
using SFA.DAS.Recruit.Contracts.ApiResponses;
using SFA.DAS.RecruitJobs.GraphQL;
using SFA.DAS.RecruitJobs.InnerApi.Requests;
using StrawberryShake;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ClosureReason = SFA.DAS.Recruit.Contracts.ApiResponses.ClosureReason;
using OwnerType = SFA.DAS.Recruit.Contracts.ApiResponses.OwnerType;
using TransferInfo = SFA.DAS.Recruit.Contracts.ApiResponses.TransferInfo;
using VacancyReview = SFA.DAS.Recruit.Contracts.ApiResponses.VacancyReview;
using VacancyStatus = SFA.DAS.Recruit.Contracts.ApiResponses.VacancyStatus;

namespace SFA.DAS.RecruitJobs.Handlers;

public interface ITransferProviderVacancyToLegalEntityHandler
{
    Task HandleAsync(Guid vacancyId, TransferReason transferReason, CancellationToken cancellationToken);
}

public class TransferProviderVacancyToLegalEntityHandler(
    ILogger<TransferProviderVacancyToLegalEntityHandler> logger,
    IRecruitGqlClient recruitGqlClient,
    Recruit.Contracts.Client.IRecruitApiClient<Recruit.Contracts.Client.RecruitApiConfiguration> recruitApiClient) : ITransferProviderVacancyToLegalEntityHandler
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
            case GraphQL.VacancyStatus.Draft:
            case GraphQL.VacancyStatus.Referred:
            case GraphQL.VacancyStatus.Closed:
                break;
            case GraphQL.VacancyStatus.Submitted:
                var vacancyReviews = await recruitApiClient.Get<List<VacancyReview>>(
                    new GetVacanciesByVacancyReferenceReviewsApiRequest(
                        vacancyDetails.VacancyReference.GetValueOrDefault().ToString(), null, null, null));
                vacancyReview = vacancyReviews.OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                if (vacancyReview is not null && vacancyReview.Status is not ReviewStatus.UnderReview)
                {
                    patchDocument.Replace(x => x.Status, VacancyStatus.Draft);
                }
                break;
            case GraphQL.VacancyStatus.Live:
                patchDocument.Replace(x => x.Status, VacancyStatus.Closed);
                patchDocument.Replace(x => x.ClosedDate, now);
                patchDocument.Replace(x => x.ClosureReason, ClosureReason.TransferredByEmployer);
                break;
            case GraphQL.VacancyStatus.Approved:
                patchDocument.Replace(x => x.ApprovedDate, null);
                patchDocument.Replace(x => x.Status, VacancyStatus.Closed);
                patchDocument.Replace(x => x.ClosedDate, now);
                patchDocument.Replace(x => x.ClosureReason, ClosureReason.TransferredByEmployer);
                break;
            case GraphQL.VacancyStatus.Rejected:
            case GraphQL.VacancyStatus.Review:
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

        var patchRequest = new PatchVacanciesByVacancyIdApiRequest
        {
            VacancyId = vacancyId,
            Data = patchDocument
        };
        var patchResponse = await recruitApiClient.PatchWithResponseCode(patchRequest);
        patchResponse.EnsureSuccessStatusCode();

        if (vacancyDetails.Status is GraphQL.VacancyStatus.Submitted)
        {
            switch (vacancyReview)
            {
                case { Status: ReviewStatus.New or ReviewStatus.PendingReview }:
                    {
                        var vacancyReviewPatch = new JsonPatchDocument<VacancyReview>();
                        vacancyReviewPatch.Replace(x => x.ManualOutcome,
                            transferReason == TransferReason.BlockedByQa
                                ? nameof(Domain.ManualQaOutcome.Blocked)
                                : nameof(Domain.ManualQaOutcome.Transferred));
                        vacancyReviewPatch.Replace(x => x.Status, ReviewStatus.Closed);
                        vacancyReviewPatch.Replace(x => x.ClosedDate, now);
                        var vacancyReviewPatchResponse = await recruitApiClient.PatchWithResponseCode(new PatchVacancyreviewsByIdApiRequest
                        {
                            Id = vacancyReview.Id,
                            Data = vacancyReviewPatch
                        });
                        vacancyReviewPatchResponse.EnsureSuccessStatusCode();
                        break;
                    }
                case { Status: ReviewStatus.UnderReview }:
                    logger.LogWarning("Latest vacancy review for vacancy '{VacancyReference}' that has been transferred is currently being reviewed.", vacancyReview.VacancyReference);
                    break;
            }
        }
    }
}