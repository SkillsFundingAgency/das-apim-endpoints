using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.TrainingCourse;
public class GetTrainingCourseQueryHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
    : IRequestHandler<GetTrainingCourseQuery, GetTrainingCourseQueryResult>
{
    public async Task<GetTrainingCourseQueryResult> Handle(GetTrainingCourseQuery request, CancellationToken cancellationToken)
    {
        var result = await candidateApiClient.Get<GetTrainingCourseApiResponse>(new GetTrainingCourseApiRequest(request.ApplicationId, request.CandidateId, request.TrainingCourseId));
        return GetTrainingCourseQueryResult.From(result);
    }
}
