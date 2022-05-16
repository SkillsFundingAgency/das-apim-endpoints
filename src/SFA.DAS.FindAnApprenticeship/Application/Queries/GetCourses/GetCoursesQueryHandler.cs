using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.Domain;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.GetCourses
{
    public class GetCoursesQueryHandler : IRequestHandler<GetCoursesQuery, GetCoursesQueryResult>
    {
        private readonly ICourseService _courseService;

        public GetCoursesQueryHandler(ICourseService courseService)
        {
            _courseService = courseService;
        }
        public async Task<GetCoursesQueryResult> Handle(GetCoursesQuery request, CancellationToken cancellationToken)
        {
            var courseResponseTask = _courseService.GetActiveStandards<GetStandardsListResponse>(nameof(GetStandardsListResponse));
            var frameworkResponseTask = _courseService.GetAllFrameworks<GetFrameworksListResponse>(nameof(GetFrameworksListResponse));

            await Task.WhenAll(courseResponseTask, frameworkResponseTask);

            var trainingProgrammes = new List<TrainingProgramme>();
            trainingProgrammes.AddRange(courseResponseTask.Result.Standards.Select(c=>(TrainingProgramme)c).ToList());
            trainingProgrammes.AddRange(frameworkResponseTask.Result.Frameworks.Select(c=>(TrainingProgramme)c).ToList());
            
            return new GetCoursesQueryResult
            {
                TrainingProgrammes = trainingProgrammes
            };
        }   
    }
}