using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.Application.VacancyReview.Commands.UpsertVacancyReview;

public class UpsertVacancyReviewCommandHandler(IRecruitApiClient<RecruitApiConfiguration> apiClient) : IRequestHandler<UpsertVacancyReviewCommand>
{
    public async Task Handle(UpsertVacancyReviewCommand request, CancellationToken cancellationToken)
    {
        await apiClient.PutWithResponseCode<NullResponse>(
            new PutCreateVacancyReviewRequest(request.Id, request.VacancyReview));
    }
}