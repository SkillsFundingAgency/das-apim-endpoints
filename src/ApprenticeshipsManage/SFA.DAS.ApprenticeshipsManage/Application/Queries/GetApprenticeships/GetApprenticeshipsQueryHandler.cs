using MediatR;
using SFA.DAS.ApprenticeshipsManage.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Apprenticeships;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeshipsManage.Application.Queries.GetApprenticeships;

public class GetApprenticeshipsQueryHandler(
    IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration> apprenticeshipApiClient)
    : IRequestHandler<GetApprenticeshipsQuery, GetApprenticeshipsQueryResult>
{
    public async Task<GetApprenticeshipsQueryResult> Handle(GetApprenticeshipsQuery request, CancellationToken cancellationToken)
    {
        apprenticeshipApiClient.GenerateServiceToken("ApprenticeshipsManage");

        var applicationsResponse = await apprenticeshipApiClient.Get<PagedApprenticeshipsResponse>(
            new GetAllApprenticeshipsRequest(
                request.Ukprn,
                request.AcademicYear,
                request.Page,
                request.PageSize
                )
            );

        return (GetApprenticeshipsQueryResult)applicationsResponse;
    }
}