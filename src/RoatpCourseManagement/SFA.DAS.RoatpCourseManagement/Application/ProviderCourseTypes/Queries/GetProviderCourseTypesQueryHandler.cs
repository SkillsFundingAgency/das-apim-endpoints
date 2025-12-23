using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.Application.ProviderCourseTypes.Queries;
public class GetProviderCourseTypesQueryHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _courseManagementApiClient, ILogger<GetProviderCourseTypesQueryHandler> _logger) : IRequestHandler<GetProviderCourseTypesQuery, List<ProviderCourseTypeResult>>
{
    public async Task<List<ProviderCourseTypeResult>> Handle(GetProviderCourseTypesQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var courses = await _courseManagementApiClient.Get<List<ProviderCourseTypeResult>>(new GetProviderCourseTypesRequest(query.Ukprn));
            if (courses != null) return courses;

            _logger.LogInformation("Courses data not found for {Ukprn}", query.Ukprn);
            return null;

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred trying to retrieve Course types for Ukprn {Ukprn}", query.Ukprn);
            throw;
        }
    }
}
