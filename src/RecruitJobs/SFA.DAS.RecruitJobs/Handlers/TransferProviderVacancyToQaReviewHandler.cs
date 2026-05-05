using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.RecruitJobs.Domain;
using SFA.DAS.RecruitJobs.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Recruit.Services;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.RecruitJobs.Handlers;

public interface ITransferProviderVacancyToQaReviewHandler
{
    Task HandleAsync(Guid vacancyId, Guid userReference, CancellationToken cancellationToken);
}

public class TransferProviderVacancyToQaReviewHandler(
    IRecruitApiClient<RecruitApiConfiguration> recruitApiClient,
    IVacancyReviewService vacancyReviewService): ITransferProviderVacancyToQaReviewHandler
{
    public async Task HandleAsync(Guid vacancyId, Guid userReference, CancellationToken cancellationToken)
    {
        // patch the vacancy status to Submitted
        var patchDocument = new JsonPatchDocument<Vacancy>();
        patchDocument.Replace(x => x.Status, VacancyStatus.Submitted);
        patchDocument.Replace(x => x.SubmittedDate, DateTime.UtcNow);
        patchDocument.Replace(x => x.LastUpdatedDate, DateTime.UtcNow);
        patchDocument.Replace(x => x.SubmittedByUserId, userReference);

        var patchVacancyResponse = await recruitApiClient.PatchWithResponseCode(new PatchVacancyRequest(vacancyId, patchDocument));
        patchVacancyResponse.EnsureSuccessStatusCode();
        
        // create the vacancy review
        if (await vacancyReviewService.CreateAsync(vacancyId, cancellationToken) == null)
        {
            throw new Exception($"Failed to create vacancy review for vacancy {vacancyId}");   
        }
    }
}