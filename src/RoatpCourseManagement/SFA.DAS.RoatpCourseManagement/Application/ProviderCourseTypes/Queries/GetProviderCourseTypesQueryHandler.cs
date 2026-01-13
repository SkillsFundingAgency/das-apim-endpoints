using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Application.ProviderCourseTypes.Queries;
public class GetProviderCourseTypesQueryHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _courseManagementApiClient, ILogger<GetProviderCourseTypesQueryHandler> _logger) : IRequestHandler<GetProviderCourseTypesQuery, List<ProviderCourseTypeResult>>
{
    public async Task<List<ProviderCourseTypeResult>> Handle(GetProviderCourseTypesQuery query, CancellationToken cancellationToken)
    {
        var courses = await _courseManagementApiClient.GetWithResponseCode<List<ProviderCourseTypeResult>>(new GetProviderCourseTypesRequest(query.Ukprn));

        _logger.LogInformation("Hnadling GetProviderCourseTypes request for {Ukprn}", query.Ukprn);

        if (courses.StatusCode != HttpStatusCode.OK)
        {
            _logger.LogError("Error occurred trying to retrieve Course types for Ukprn {Ukprn}", query.Ukprn);
            throw new InvalidOperationException();
        }

        return courses.Body;
    }
}