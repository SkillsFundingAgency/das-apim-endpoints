using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindEpao.InnerApi.Requests;
using SFA.DAS.FindEpao.InnerApi.Responses;
using SFA.DAS.FindEpao.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindEpao.Application.Courses.Queries.GetCourseEpaos
{
    public class GetCourseEpaosQueryHandler : IRequestHandler<GetCourseEpaosQuery, GetCourseEpaosResult>
    {
        private readonly ILogger<GetCourseEpaosQueryHandler> _logger;
        private readonly IAssessorsApiClient<AssessorsApiConfiguration> _assessorsApiClient;
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;
        private readonly ICourseEpaoIsValidFilterService _courseEpaoIsValidFilterService;

        public GetCourseEpaosQueryHandler(
            ILogger<GetCourseEpaosQueryHandler> logger,
            IAssessorsApiClient<AssessorsApiConfiguration> assessorsApiClient,
            ICoursesApiClient<CoursesApiConfiguration> coursesApiClient,
            ICourseEpaoIsValidFilterService courseEpaoIsValidFilterService)
        {
            _logger = logger;
            _assessorsApiClient = assessorsApiClient;
            _coursesApiClient = coursesApiClient;
            _courseEpaoIsValidFilterService = courseEpaoIsValidFilterService;
        }

        public async Task<GetCourseEpaosResult> Handle(GetCourseEpaosQuery request, CancellationToken cancellationToken)
        {
            var epaosTask = _assessorsApiClient.GetAll<GetCourseEpaoListItem>(
                new GetCourseEpaosRequest
                {
                    CourseId = request.CourseId
                });

            var courseTask = _coursesApiClient.Get<GetStandardsListItem>(
                new GetStandardRequest(request.CourseId));

            await Task.WhenAll(epaosTask, courseTask);
            _logger.LogDebug($"Found [{epaosTask.Result.Count()}] EPAOs for Course Id:[{request.CourseId}].");
            
            var filteredEpaos = epaosTask.Result
                .Where(_courseEpaoIsValidFilterService.IsValidCourseEpao)
                .ToList();

            List<Task<List<GetStandardsExtendedListItem>>> standardsTasks = new List<Task<List<GetStandardsExtendedListItem>>>();
            Parallel.ForEach(filteredEpaos, filtEPAO =>
            {
                standardsTasks.Add(_assessorsApiClient.Get<List<GetStandardsExtendedListItem>>(
                    new GetCourseEpaosStandardVersionsRequest(filtEPAO.EpaoId, request.CourseId)));
                
            });
            await Task.WhenAll(standardsTasks);

            for (int i = 0; i < standardsTasks.Count; i++)
            {
                if (standardsTasks[i].Result != null)
                    filteredEpaos[i].CourseEpaoDetails.standardVersions = standardsTasks[i].Result;
            }

            _logger.LogDebug($"Found [{filteredEpaos.Count}] EPAOs for Course Id:[{request.CourseId}] after filtering.");
            
            return new GetCourseEpaosResult
            {
                Epaos = filteredEpaos.OrderBy(item => item.Name),
                Course = courseTask.Result
            };
        }
    }
}