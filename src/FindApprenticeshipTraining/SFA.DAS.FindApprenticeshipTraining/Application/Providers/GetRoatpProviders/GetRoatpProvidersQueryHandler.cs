using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RoatpV2;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.RoatpV2;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Providers.GetRoatpProviders;
public class GetRoatpProvidersQueryHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _roatpCourseManagementApiClient) : IRequestHandler<GetRoatpProvidersQuery, GetRoatpProvidersQueryResult>
{
    public async Task<GetRoatpProvidersQueryResult> Handle(GetRoatpProvidersQuery request, CancellationToken cancellationToken)
    {
        var result =
            await _roatpCourseManagementApiClient.GetWithResponseCode<GetProvidersResponse>(
                new GetRoatpProvidersRequest()
                {
                    Live = true
                }
        );

        var providers = result.Body.RegisteredProviders.Select(provider => (RoatpProvider)provider);
        return new GetRoatpProvidersQueryResult { Providers = providers };
    }
}