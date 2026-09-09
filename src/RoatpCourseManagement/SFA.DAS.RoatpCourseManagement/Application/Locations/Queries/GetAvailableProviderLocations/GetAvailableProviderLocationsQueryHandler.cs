using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.RoatpCourseManagement.Application.Locations.Queries.GetAllProviderLocations;
using SFA.DAS.RoatpCourseManagement.InnerApi.Models;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.RoatpCourseManagement.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.Application.Locations.Queries.GetAvailableProviderLocations;

public class GetAvailableProviderLocationsQueryHandler : IRequestHandler<GetAvailableProviderLocationsQuery, GetAvailableProviderLocationsQueryResult>
{
    private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _courseManagementApiClient;

    public GetAvailableProviderLocationsQueryHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> courseManagementApiClient)
    {
        _courseManagementApiClient = courseManagementApiClient;
    }

    public async Task<GetAvailableProviderLocationsQueryResult> Handle(GetAvailableProviderLocationsQuery request, CancellationToken cancellationToken)
    {
        var allProviderLocationsTask = _courseManagementApiClient.Get<List<ProviderLocationModel>>(new GetAllProviderLocationsQuery(request.Ukprn));
        var allProviderCourseLocationsTask = _courseManagementApiClient.Get<List<GetProviderCourseLocationsResponse>>(new GetProviderCourseLocationsRequest(request.Ukprn, request.LarsCode));

        await Task.WhenAll(allProviderLocationsTask, allProviderCourseLocationsTask);
        var allProviderLocations = allProviderLocationsTask.Result.FindAll(a => a.LocationType == LocationType.Provider);
        var allProviderCourseLocations = allProviderCourseLocationsTask.Result;

        var availableProviderLocations = new List<ProviderLocationModel>();
        foreach (var l in allProviderLocations)
        {
            if (!allProviderCourseLocations.Exists(a => a.LocationName == l.LocationName))
            {
                availableProviderLocations.Add(l);
            }
        }

        return new GetAvailableProviderLocationsQueryResult() { AvailableProviderLocations = availableProviderLocations };
    }
}