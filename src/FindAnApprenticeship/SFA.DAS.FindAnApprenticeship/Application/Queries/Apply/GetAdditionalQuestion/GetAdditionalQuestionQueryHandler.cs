using MediatR;
using SFA.DAS.FindAnApprenticeship.Domain;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetAdditionalQuestion;
public class GetAdditionalQuestionQueryHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
    : IRequestHandler<GetAdditionalQuestionQuery, GetAdditionalQuestionQueryResult>
{
    public async Task<GetAdditionalQuestionQueryResult> Handle(GetAdditionalQuestionQuery request, CancellationToken cancellationToken)
    {
        var applicationRequest = new GetApplicationApiRequest(request.CandidateId, request.ApplicationId, false);
        var application = await candidateApiClient.Get<GetApplicationApiResponse>(applicationRequest);

        var sectionStatus = request.AdditionalQuestion switch
        {
            1 => application.AdditionalQuestion1Status,
            2 => application.AdditionalQuestion2Status,
            _ => string.Empty
        };

        bool? isCompleted = sectionStatus switch
        {
            Constants.SectionStatus.Incomplete => false,
            Constants.SectionStatus.Completed => true,
            _ => null
        };

        var additionalQuestion = await candidateApiClient.Get<GetAdditionalQuestionApiResponse>(new GetAdditionalQuestionApiRequest(request.ApplicationId, request.CandidateId, request.Id));

        return new GetAdditionalQuestionQueryResult
        {
            QuestionText = additionalQuestion.QuestionText,
            Answer = additionalQuestion.Answer,
            Id = additionalQuestion.Id,
            ApplicationId = additionalQuestion.ApplicationId,
            IsSectionCompleted = isCompleted
        };
    }
}
