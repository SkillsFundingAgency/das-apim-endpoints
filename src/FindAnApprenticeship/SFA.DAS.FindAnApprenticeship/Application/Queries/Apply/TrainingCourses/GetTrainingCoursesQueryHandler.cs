using System.Threading;
using System.Threading.Tasks;
using MediatR;
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
        return await _candidateApiClient.Get<GetTrainingCoursesApiResponse>(new GetTrainingCoursesApiRequest(request.ApplicationId, request.CandidateId));
}
}
