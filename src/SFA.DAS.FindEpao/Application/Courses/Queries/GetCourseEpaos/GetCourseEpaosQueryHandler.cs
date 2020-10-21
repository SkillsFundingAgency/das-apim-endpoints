using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindEpao.InnerApi.Requests;
using SFA.DAS.FindEpao.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindEpao.Application.Courses.Queries.GetCourseEpaos
{
    public class GetCourseEpaosQueryHandler : IRequestHandler<GetCourseEpaosQuery, GetCourseEpaosResult>
    {
        private readonly IAssessorsApiClient<AssessorsApiConfiguration> _assessorsApiClient;
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;

        public GetCourseEpaosQueryHandler(
            IAssessorsApiClient<AssessorsApiConfiguration> assessorsApiClient,
            ICoursesApiClient<CoursesApiConfiguration> coursesApiClient)
        {
            _assessorsApiClient = assessorsApiClient;
            _coursesApiClient = coursesApiClient;
        }

        public async Task<GetCourseEpaosResult> Handle(GetCourseEpaosQuery request, CancellationToken cancellationToken)
        {
            var epaosTask = _assessorsApiClient.GetAll<GetCourseEpaoListItem>(
                new GetCourseEpaosRequest
                {
                    CourseId = request.CourseId
                });

            var courseTask = _coursesApiClient.Get<GetStandardsListItem>(
                new GetStandardRequest
                {
                    StandardId = request.CourseId
                });

            await Task.WhenAll(epaosTask, courseTask);

            return new GetCourseEpaosResult
            {
                Epaos = epaosTask.Result.OrderBy(item => item.Name),
                Course = courseTask.Result
            };
        }
    }
}