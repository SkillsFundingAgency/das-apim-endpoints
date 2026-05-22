using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using Microsoft.Extensions.Logging;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.RecruitJobs.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Recruit.Services;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Domain.Recruit;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Recruit;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Recruit;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using Vacancy = SFA.DAS.RecruitJobs.Domain.Vacancy;

namespace SFA.DAS.RecruitJobs.Handlers;

public interface ITransferProviderVacancyToQaReviewHandler
{
    Task HandleAsync(Guid vacancyId, Guid userReference, string userEmailAddress, CancellationToken cancellationToken);
}

public class TransferProviderVacancyToQaReviewHandler(
    ILogger<TransferProviderVacancyToQaReviewHandler> logger,
    IRecruitApiClient<RecruitApiConfiguration> recruitApiClient,
    IVacancyReviewService vacancyReviewService): ITransferProviderVacancyToQaReviewHandler
{
    public async Task HandleAsync(Guid vacancyId, Guid userReference, string userEmailAddress, CancellationToken cancellationToken)
    {
        // fetch the vacancy
        var response = await recruitApiClient.GetWithResponseCode<GetVacancyResponse>(new GetVacancyRequest(vacancyId));
        response.EnsureSuccessStatusCode();
        var vacancy = response.Body;

        if (vacancy is not { Status: VacancyStatus.Review })
        {
            logger.LogInformation("Vacancy {VacancyId} is not in review status, skipping transfer to QA review", vacancyId);
            return;
        }
        
        // patch the vacancy status to Submitted
        var patchDocument = new JsonPatchDocument<Vacancy>();
        patchDocument.Replace(x => x.Status, Domain.VacancyStatus.Submitted);
        patchDocument.Replace(x => x.SubmittedDate, DateTime.UtcNow);
        patchDocument.Replace(x => x.LastUpdatedDate, DateTime.UtcNow);
        patchDocument.Replace(x => x.SubmittedByUserId, userReference);

        var patchVacancyResponse = await recruitApiClient.PatchWithResponseCode(new InnerApi.Requests.PatchVacancyRequest(vacancyId, patchDocument));
        patchVacancyResponse.EnsureSuccessStatusCode();
        
        // create the vacancy review
        if (await vacancyReviewService.CreateAsync(vacancy, userEmailAddress, cancellationToken) == null)
        {
            throw new Exception($"Failed to create vacancy review for vacancy {vacancyId}");   
        }
    }
}