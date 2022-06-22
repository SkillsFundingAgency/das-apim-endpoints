using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Recruit.Domain;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.Application.Queries.GetTrainingProgrammes
{
    public class GetTrainingProgrammesQueryHandler : IRequestHandler<GetTrainingProgrammesQuery, GetTrainingProgrammesQueryResult>
    {
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;

        public GetTrainingProgrammesQueryHandler (ICoursesApiClient<CoursesApiConfiguration> coursesApiClient)
        {
            _coursesApiClient = coursesApiClient;
        }
        public async Task<GetTrainingProgrammesQueryResult> Handle(GetTrainingProgrammesQuery request, CancellationToken cancellationToken)
        {
            var frameworksTask = _coursesApiClient.Get<GetFrameworksListResponse>(new GetFrameworksRequest());
            var standardsTask = _coursesApiClient.Get<GetStandardsListResponse>(new GetActiveStandardsListRequest());

            await Task.WhenAll(frameworksTask, standardsTask);

            var trainingProgrammes = new List<TrainingProgramme>();
            trainingProgrammes.AddRange(frameworksTask.Result.Frameworks?.Select(item => (TrainingProgramme)item) ?? Array.Empty<TrainingProgramme>());
            trainingProgrammes.AddRange(standardsTask.Result.Standards?.Select(item => (TrainingProgramme)item) ?? Array.Empty<TrainingProgramme>());
            
            return new GetTrainingProgrammesQueryResult
            {
                TrainingProgrammes = trainingProgrammes
            };
        }
    }
}
