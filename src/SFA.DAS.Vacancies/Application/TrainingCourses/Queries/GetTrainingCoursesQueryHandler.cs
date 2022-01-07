using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Vacancies.Interfaces;

namespace SFA.DAS.Vacancies.Application.TrainingCourses.Queries
{
    public class GetTrainingCoursesQueryHandler : IRequestHandler<GetTrainingCoursesQuery, GetTrainingCoursesQueryResult>
    {
        private readonly IStandardsService _standardsService;

        public GetTrainingCoursesQueryHandler (IStandardsService standardsService, ICoursesApiClient<CoursesApiConfiguration> coursesApiClient)
        {
            
            _standardsService = standardsService;
            
        }
        public async Task<GetTrainingCoursesQueryResult> Handle(GetTrainingCoursesQuery request, CancellationToken cancellationToken)
        {
            var courses = await _standardsService.GetStandards();

            return new GetTrainingCoursesQueryResult
            {
                TrainingCourses = courses.Standards
            };
        }
    }
}