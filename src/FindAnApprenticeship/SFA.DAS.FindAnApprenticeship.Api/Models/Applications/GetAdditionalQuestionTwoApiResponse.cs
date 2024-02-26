using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetAdditionalQuestionTwo;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications;

public class GetAdditionalQuestionTwoApiResponse
{
    public string QuestionTwo { get; set; }

    public static implicit operator GetAdditionalQuestionTwoApiResponse(GetAdditionalQuestionTwoQueryResult source)
    {
        return new GetAdditionalQuestionTwoApiResponse
        {
            QuestionTwo = source.QuestionTwo
        };
    }
}
