using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.TrainingCourse;
public class GetDeleteTrainingCourseQueryHandler : IRequestHandler<GetDeleteTrainingCourseQuery, GetDeleteTrainingCourseQueryResult>
{
    private readonly ICandidateApiClient<CandidateApiConfiguration> _candidateApiClient;

    public GetDeleteTrainingCourseQueryHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
    {
        _candidateApiClient = candidateApiClient;
    }

    public async Task<GetDeleteTrainingCourseQueryResult> Handle(GetDeleteTrainingCourseQuery request, CancellationToken cancellationToken)
    {
        return await _candidateApiClient.Get<GetDeleteTrainingCourseResponse>(new GetDeleteTrainingCourseRequest(request.ApplicationId, request.CandidateId, request.TrainingCourseId));
    }
}
