using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetAdditionalQuestion;
public class GetAdditionalQuestionQueryHandler : IRequestHandler<GetAdditionalQuestionQuery, GetAdditionalQuestionQueryResult>
{
    private readonly ICandidateApiClient<CandidateApiConfiguration> _candidateApiClient;

    public GetAdditionalQuestionQueryHandler(
        ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
    {
        _candidateApiClient = candidateApiClient;
    }

    public async Task<GetAdditionalQuestionQueryResult> Handle(GetAdditionalQuestionQuery request, CancellationToken cancellationToken)
    {
        var additionalQuestion = await _candidateApiClient.Get<GetAdditionalQuestionApiResponse>(new GetAdditionalQuestionApiRequest(request.CandidateId, request.ApplicationId, request.QuestionId));

        return new GetAdditionalQuestionQueryResult
        {
            Question = additionalQuestion.Question,
            Answer = additionalQuestion.Answer,
            Id = additionalQuestion.Id
        };
    }
}
