using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Recruit.Domain;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.Application.Queries.GetTrainingProgrammes;

public class GetTrainingProgrammesQueryHandler(ICoursesApiClient<CoursesApiConfiguration> coursesApiClient)
    : IRequestHandler<GetTrainingProgrammesQuery, GetTrainingProgrammesQueryResult>
{
    public async Task<GetTrainingProgrammesQueryResult> Handle(GetTrainingProgrammesQuery request, CancellationToken cancellationToken)
    {
        var standards = await coursesApiClient.Get<GetStandardsListResponse>(new GetActiveStandardsListRequest());
        var trainingProgrammes = new List<TrainingProgramme>();
        trainingProgrammes.AddRange(standards.Standards?
            .Where(c => request.IncludeFoundationApprenticeships || c.ApprenticeshipType.Equals("Apprenticeship", StringComparison.CurrentCultureIgnoreCase))
            .Select(item => (TrainingProgramme)item) ?? []);
            
        return new GetTrainingProgrammesQueryResult
        {
            TrainingProgrammes = trainingProgrammes
        };
    }
}