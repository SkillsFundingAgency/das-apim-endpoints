using MediatR;
using SFA.DAS.RecruitQa.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderCourses;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitQa.Application.Provider.GetProvider;

public class GetProviderQueryHandler(
    IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> roatpCourseManagementApiClient)
    : IRequestHandler<GetProviderQuery, GetProviderQueryResult>
{
    public async Task<GetProviderQueryResult> Handle(GetProviderQuery request, CancellationToken cancellationToken)
    {
        var response = await roatpCourseManagementApiClient.Get<GetProvidersListItem>(new GetProviderRequest(request.Ukprn));

        if (response == null)
            return new GetProviderQueryResult();

        return new GetProviderQueryResult
        {
            Provider = response
        };
    }
}