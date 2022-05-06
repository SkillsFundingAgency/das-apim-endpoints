using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Requests;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Roatp.CourseManagement.Application.Standards.Queries.GetStandardQuery
{
    public class GetStandardQueryHandler : IRequestHandler<GetStandardQuery, GetStandardResult>
    {
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;
        private readonly ILogger<GetStandardQueryHandler> _logger;

        public GetStandardQueryHandler(ICoursesApiClient<CoursesApiConfiguration> coursesApiClient,
            ILogger<GetStandardQueryHandler> logger)
        {
            _coursesApiClient = coursesApiClient;
            _logger = logger;
        }

        public async Task<GetStandardResult> Handle(GetStandardQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get Standard request received for Lars code {LarsCode}", request.LarsCode);
            try
            {
                var standard =
                    await _coursesApiClient.Get<GetStandardResponse>(new GetStandardRequest(request.LarsCode));
                if (standard == null)
                {
                    _logger.LogInformation("Standar data not found for Lars Code: {LarsCode}", request.LarsCode);
                    return null;
                }

                var result = new GetStandardResult
                {
                    LarsCode = standard.LarsCode,
                    ApprovalBody = standard.ApprovalBody,
                    IfateReferenceNumber = standard.IfateReferenceNumber,
                    Level = standard.Level,
                    StandardUId = standard.StandardUId,
                    Title = standard.Title,
                    Version = standard.Version
                };

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred trying to retrieve stanard for Lars code: {request.LarsCode}");
                throw;
            }
        }
    }
}

// public class GetAllCoursesQueryHandler : IRequestHandler<GetAllCoursesQuery, List<GetAllCoursesResult>>
    // {
    //     private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _courseManagementApiClient;
    //     private readonly ILogger<GetAllCoursesQueryHandler> _logger;
    //
    //     public GetAllCoursesQueryHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> courseManagementApiClient, ILogger<GetAllCoursesQueryHandler> logger)
    //     {
    //         _courseManagementApiClient = courseManagementApiClient;
    //         _logger = logger;
    //     }
    //     public async Task<List<GetAllCoursesResult>> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken)
    //     {
    //         _logger.LogInformation("Get Course request received for Ukprn number {Ukprn}", request.Ukprn);
    //         try
    //         {
    //             var courses = await _courseManagementApiClient.Get<List<GetAllCoursesResponse>>(new GetAllCoursesRequest(request.Ukprn));
    //             if (courses == null)
    //             {
    //                 _logger.LogInformation("Courses data not found for {ukprn}", request.Ukprn);
    //                 return null;
    //             }
    //             var results = new List<GetAllCoursesResult>();
    //             foreach (var c in courses)
    //             {
    //                 var course = new GetAllCoursesResult
    //                 {
    //                     ProviderCourseId = c.ProviderCourseId,
    //                     CourseName = c.CourseName,
    //                     Level = c.Level,
    //                     IsImported = c.IsImported
    //                 };
    //                 results.Add(course);
    //             }
    //             return results;
    //         }
    //         catch (Exception ex)
    //         {
    //             _logger.LogError(ex, $"Error occurred trying to retrieve Courses for Ukprn {request.Ukprn}");
    //             throw;
    //         }
    //     }
   //}
//}
