using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetEmployerAdditionalQuestionTwo;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications;

public class GetAdditionalQuestionTwoApiResponse
{
    public string QuestionTwo { get; set; }

    public static implicit operator GetAdditionalQuestionTwoApiResponse(GetEmployerAdditionalQuestionTwoQueryResult source)
    {
        return new GetAdditionalQuestionTwoApiResponse
        {
            QuestionTwo = source.QuestionTwo
        };
    }
}
