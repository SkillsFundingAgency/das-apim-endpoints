using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.DeleteTrainingCourse;
public class GetDeleteTrainingCourseQueryHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
    : IRequestHandler<GetDeleteTrainingCourseQuery, GetDeleteTrainingCourseQueryResult>
{
    public async Task<GetDeleteTrainingCourseQueryResult> Handle(GetDeleteTrainingCourseQuery request, CancellationToken cancellationToken)
    {
        var result = await candidateApiClient.Get<GetTrainingCourseApiResponse>(new GetTrainingCourseApiRequest(request.ApplicationId, request.CandidateId, request.TrainingCourseId));
        return GetDeleteTrainingCourseQueryResult.From(result);
    }
}
