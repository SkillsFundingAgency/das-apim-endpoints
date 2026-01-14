using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Application.ProviderCourseTypes.Queries;
public class GetProviderCourseTypesQueryHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _courseManagementApiClient, ILogger<GetProviderCourseTypesQueryHandler> _logger) : IRequestHandler<GetProviderCourseTypesQuery, List<ProviderCourseTypeResult>>
{
    public async Task<List<ProviderCourseTypeResult>> Handle(GetProviderCourseTypesQuery query, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetProviderCourseTypes request for {Ukprn}", query.Ukprn);

        var response = await _courseManagementApiClient.GetWithResponseCode<List<ProviderCourseTypeResult>>(new GetProviderCourseTypesRequest(query.Ukprn));

        response.EnsureSuccessStatusCode();

        return response.Body;
    }
}