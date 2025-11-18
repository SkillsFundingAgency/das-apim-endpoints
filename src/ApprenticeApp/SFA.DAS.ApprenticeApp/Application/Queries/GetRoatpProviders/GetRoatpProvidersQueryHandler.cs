using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RoatpV2;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.RoatpV2;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.Application.Queries.GetRoatpProviders;
public class GetRoatpProvidersQueryHandler: IRequestHandler<GetRoatpProvidersQuery, GetRoatpProvidersQueryResult>
{
    private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _roatpCourseManagementApiClient;

    public GetRoatpProvidersQueryHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> roatpCourseManagementApiClient)
        => _roatpCourseManagementApiClient = roatpCourseManagementApiClient;
    public async Task<GetRoatpProvidersQueryResult> Handle(GetRoatpProvidersQuery request, CancellationToken cancellationToken)
    {
        var result =
            await _roatpCourseManagementApiClient.GetWithResponseCode<GetProvidersResponse>(
                new GetRoatpProvidersRequest()
                {
                    Live = true
                }
        );
        var providers = result.Body.RegisteredProviders.Select(provider => new RoatpProvider
        {
            Name = provider.Name,
            Ukprn = provider.Ukprn,
        }).ToList();    

        return new GetRoatpProvidersQueryResult { Providers = providers };
    }
}