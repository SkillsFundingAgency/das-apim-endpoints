using System;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetCandidateAdditionalQuestionTwo;
public class GetCandidateAdditionalQuestionTwoQueryResult
{
    public string QuestionTwo { get; set; }

    public static implicit operator GetCandidateAdditionalQuestionTwoQueryResult(GetCandidateAdditionalQuestionTwoApiResponse source)
    {
        return new GetCandidateAdditionalQuestionTwoQueryResult
        {
            QuestionTwo = source.QuestionTwo
        };
    }
}
