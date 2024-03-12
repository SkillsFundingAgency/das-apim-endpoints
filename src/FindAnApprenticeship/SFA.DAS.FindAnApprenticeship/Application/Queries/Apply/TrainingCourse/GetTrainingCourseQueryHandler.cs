using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.TrainingCourse;
public class GetTrainingCourseQueryHandler : IRequestHandler<GetTrainingCourseQuery, GetTrainingCourseQueryResult>
{
    private readonly ICandidateApiClient<CandidateApiConfiguration> _candidateApiClient;

    public GetTrainingCourseQueryHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
    {
        _candidateApiClient = candidateApiClient;
    }

    public async Task<GetTrainingCourseQueryResult> Handle(GetTrainingCourseQuery request, CancellationToken cancellationToken)
    {
        return await _candidateApiClient.Get<GetTrainingCourseApiResponse>(new GetTrainingCourseApiRequest(request.ApplicationId, request.CandidateId, request.TrainingCourseId));
    }
}
