using MediatR;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RoatpV2;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.RoatpV2;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourses;

public sealed class GetCoursesQueryHandler(
    IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _roatpCourseManagementApiClient,
    ILocationLookupService _locationLookupService,
    ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient
) : IRequestHandler<GetCoursesQuery, GetCoursesQueryResult>
{
    public async Task<GetCoursesQueryResult> Handle(GetCoursesQuery query, CancellationToken cancellationToken)
    {
        LocationItem locationItem = await _locationLookupService.GetLocationInformation(query.Location, 0, 0);

        if (locationItem is null && !string.IsNullOrWhiteSpace(query.Location))
        {
            return GetEmptyResponse(query.PageSize);
        }

        var coursesStandardsResponse =
            await _coursesApiClient.GetWithResponseCode<GetStandardsListResponse>(
                new GetActiveStandardsListRequest()
                {
                    Keyword = query.Keyword ?? string.Empty,
                    OrderBy = query.OrderBy,
                    RouteIds = query.RouteIds,
                    Levels = query.Levels,
                    ApprenticeshipType = query.ApprenticeshipType
                }
        );

        coursesStandardsResponse.EnsureSuccessStatusCode();

        if (!coursesStandardsResponse.Body.Standards.Any())
        {
            return GetEmptyResponse(query.PageSize);
        }

        int maxPages = (int)Math.Ceiling((double)coursesStandardsResponse.Body.TotalFiltered / query.PageSize);

        if (query.Page > maxPages)
        {
            return GetEmptyResponse(query.PageSize);
        }

        var pagedStandards = coursesStandardsResponse.Body.Standards
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToArray();

        if (pagedStandards.Length < 1)
        {
            return GetEmptyResponse(query.PageSize);
        }

        var courseTrainingProvidersCountResponse =
            await _roatpCourseManagementApiClient.GetWithResponseCode<GetCourseTrainingProvidersCountResponse>(
                new GetCourseTrainingProvidersCountRequest(
                    pagedStandards.Select(a => a.LarsCode).ToArray(),
                    query.Distance,
                    locationItem?.Latitude,
                    locationItem?.Longitude
                )
        );

        courseTrainingProvidersCountResponse.EnsureSuccessStatusCode();

        var providerCounts = courseTrainingProvidersCountResponse.Body.Courses.ToDictionary(c => c.LarsCode);

        List<StandardModel> standardModels = new List<StandardModel>();

        for (int index = 0; index < pagedStandards.Count(); index++)
        {
            GetStandardsListItem standard = pagedStandards[index];

            var courseProviderCountModel = providerCounts.GetValueOrDefault(standard.LarsCode);

            standardModels.Add(
                StandardModel.CreateFrom(
                    standard,
                    index + 1,
                    courseProviderCountModel?.ProvidersCount ?? 0,
                    courseProviderCountModel?.TotalProvidersCount ?? 0
                )
            );
        }

        var response = new GetCoursesQueryResult()
        {
            Page = query.Page,
            PageSize = query.PageSize,
            TotalPages = CalculateTotalPages(query.PageSize, coursesStandardsResponse.Body.TotalFiltered),
            TotalCount = coursesStandardsResponse.Body.TotalFiltered,
            Standards = standardModels
        };

        return response;
    }

    private static int CalculateTotalPages(int pageSize, int totalItems)
    {
        if (pageSize <= 0)
        {
            throw new ArgumentException("Page size must be greater than zero.", nameof(pageSize));
        }

        if (totalItems < 0)
        {
            throw new ArgumentException("Total items cannot be negative.", nameof(totalItems));
        }

        return (int)Math.Ceiling((double)totalItems / pageSize);
    }

    private static GetCoursesQueryResult GetEmptyResponse(int pageSize)
    {
        return new GetCoursesQueryResult()
        {
            Page = 1,
            PageSize = pageSize,
            TotalPages = 0,
            TotalCount = 0,
            Standards = []
        };
    }
}