using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetCandidateAdditionalQuestionTwo;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications;

public class GetCandidateAdditionalQuestionTwoApiResponse
{
    public string QuestionTwo { get; set; }

    public static implicit operator GetCandidateAdditionalQuestionTwoApiResponse(GetCandidateAdditionalQuestionTwoQueryResult source)
    {
        return new GetCandidateAdditionalQuestionTwoApiResponse
        {
            QuestionTwo = source.QuestionTwo
        };
    }
}
