using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.RoatpCourseManagement.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetAllProviderCourses
{
    public class GetAllProviderCoursesQueryHandler : IRequestHandler<GetAllProviderCoursesQuery, List<GetAllProviderCoursesQueryResult>>
    {
        private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _courseManagementApiClient;
        private readonly ILogger<GetAllProviderCoursesQueryHandler> _logger;

        public GetAllProviderCoursesQueryHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> courseManagementApiClient, ILogger<GetAllProviderCoursesQueryHandler> logger)
        {
            _courseManagementApiClient = courseManagementApiClient;
            _logger = logger;
        }
        public async Task<List<GetAllProviderCoursesQueryResult>> Handle(GetAllProviderCoursesQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get Course request received for Ukprn number {Ukprn}", request.Ukprn);
            try
            {
                var courses = await _courseManagementApiClient.Get<List<GetAllProviderCoursesResponse>>(new GetAllCoursesRequest(request.Ukprn));
                if (courses == null)
                {
                    _logger.LogInformation("Courses data not found for {ukprn}", request.Ukprn);
                    return null;
                }
                var results = new List<GetAllProviderCoursesQueryResult>();
                foreach (var c in courses)
                {
                    var course = new GetAllProviderCoursesQueryResult
                    {
                        ProviderCourseId = c.ProviderCourseId,
                        CourseName = c.CourseName,
                        Level = c.Level,
                        IsImported = c.IsImported,
                        LarsCode = c.LarsCode,
                        ApprovalBody = c.ApprovalBody,
                        Version = c.Version,
                        IsApprovedByRegulator = c.IsApprovedByRegulator
                    };
                    results.Add(course);
                }
                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred trying to retrieve Courses for Ukprn {request.Ukprn}");
                throw;
            }
        }
    }
}
