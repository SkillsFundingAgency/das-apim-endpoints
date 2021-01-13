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

namespace SFA.DAS.Recruit.Application.Queries.GetStandards
{
    public class GetStandardsQueryHandler : IRequestHandler<GetStandardsQuery, GetStandardsQueryResult>
    {
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;

        public GetStandardsQueryHandler (ICoursesApiClient<CoursesApiConfiguration> coursesApiClient)
        {
            _coursesApiClient = coursesApiClient;
        }
        public async Task<GetStandardsQueryResult> Handle(GetStandardsQuery request, CancellationToken cancellationToken)
        {
            var frameworks = await _coursesApiClient.Get<GetFrameworksListResponse>(new GetFrameworksRequest());
            var standards = await _coursesApiClient.Get<GetStandardsListResponse>(new GetAllStandardsListRequest());

            var trainingProgrammes = new List<TrainingProgramme>();
            trainingProgrammes.AddRange(frameworks.Frameworks?.Select(item => (TrainingProgramme)item) ?? Array.Empty<TrainingProgramme>());
            trainingProgrammes.AddRange(standards.Standards?.Select(item => (TrainingProgramme)item) ?? Array.Empty<TrainingProgramme>());
            
            return new GetStandardsQueryResult
            {
                TrainingProgrammes = trainingProgrammes
            };
        }
    }
}