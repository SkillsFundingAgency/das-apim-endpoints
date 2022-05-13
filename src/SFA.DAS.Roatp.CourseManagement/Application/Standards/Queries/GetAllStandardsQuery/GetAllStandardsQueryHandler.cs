using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Roatp.CourseManagement.Application.Standards.Queries.GetAllCoursesQuery;
using SFA.DAS.Roatp.CourseManagement.Application.Standards.Queries.GetStandardQuery;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Requests;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Roatp.CourseManagement.Application.Standards.Queries.GetAllStandardsQuery
{
    public class GetAllStandardsQueryHandler : IRequestHandler<GetAllStandardsQuery, GetAllStandardsResult>
    {
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;
        private readonly ILogger<GetAllStandardsQueryHandler> _logger;

        public GetAllStandardsQueryHandler(ICoursesApiClient<CoursesApiConfiguration> coursesApiClient, ILogger<GetAllStandardsQueryHandler> logger)
        {
            _coursesApiClient = coursesApiClient;
            _logger = logger;
        }


        public async Task<GetAllStandardsResult> Handle(GetAllStandardsQuery request, CancellationToken cancellationToken)
        {
             _logger.LogInformation("Get all standards request received");
             try
             {
                 var standards =
                     await _coursesApiClient.Get<GetAllStandardsResponse>(new GetAllStandardsRequest());
                 if (standards == null)
                 {
                     _logger.LogInformation("Standards not retrieved");
                     return null;
                 }

                 var standardResults = standards.Standards.Select(standard => new GetStandardResult
                     {
                         LarsCode = standard.LarsCode,
                         ApprovalBody = standard.ApprovalBody,
                         IfateReferenceNumber = standard.IfateReferenceNumber,
                         Level = standard.Level,
                         StandardUId = standard.StandardUId,
                         Title = standard.Title,
                         Version = standard.Version,
                         Route = standard.Route
                     })
                     .ToList();

                 return new GetAllStandardsResult { Standards = standardResults };

             }
             catch (Exception ex)
             {
                 _logger.LogError(ex, $"Error occurred trying to retrieve standards");
                 throw;
             }
        }
    }
}
