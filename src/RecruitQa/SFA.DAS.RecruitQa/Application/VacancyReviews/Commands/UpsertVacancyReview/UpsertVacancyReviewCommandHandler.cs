using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.RecruitQa.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.RecruitQa.Application.VacancyReviews.Commands.UpsertVacancyReview;

public class UpsertVacancyReviewCommandHandler(IRecruitApiClient<RecruitApiConfiguration> apiClient) : IRequestHandler<UpsertVacancyReviewCommand>
{
    public async Task Handle(UpsertVacancyReviewCommand request, CancellationToken cancellationToken)
    {
        var response = await apiClient.PutWithResponseCode<NullResponse>(new PutCreateVacancyReviewRequest(request.Id, request.VacancyReview));
        response.EnsureSuccessStatusCode();
    }
}