using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.Domain;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.TrainingCourses;
public class GetTrainingCoursesQueryHandler : IRequestHandler<GetTrainingCoursesQuery, GetTrainingCoursesQueryResult>
{
    private readonly ICandidateApiClient<CandidateApiConfiguration> _candidateApiClient;

    public GetTrainingCoursesQueryHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
    {
        _candidateApiClient = candidateApiClient;
    }
    public async Task<GetTrainingCoursesQueryResult> Handle(GetTrainingCoursesQuery request, CancellationToken cancellationToken)
    {
        var applicationTask = _candidateApiClient.Get<GetApplicationApiResponse>(new GetApplicationApiRequest(request.CandidateId, request.ApplicationId, false));
        var trainingCoursesTask = _candidateApiClient.Get<GetTrainingCoursesApiResponse>(new GetTrainingCoursesApiRequest(request.ApplicationId, request.CandidateId));

        await Task.WhenAll(applicationTask, trainingCoursesTask);

        var application = applicationTask.Result;
        var trainingCourses = trainingCoursesTask.Result;

        bool? isCompleted = application.TrainingCoursesStatus switch
        {
            Constants.SectionStatus.Incomplete => false,
            Constants.SectionStatus.Completed => true,
            _ => null
        };

        return new GetTrainingCoursesQueryResult
        {
            IsSectionCompleted = isCompleted,
            TrainingCourses = trainingCourses.TrainingCourses.Select(x =>
                new GetTrainingCoursesQueryResult.TrainingCourse
                {
                    ApplicationId = x.ApplicationId,
                    Id = x.Id,
                    CourseName = x.CourseName,
                    YearAchieved = x.YearAchieved

                }).ToList()
        };
    }
}
