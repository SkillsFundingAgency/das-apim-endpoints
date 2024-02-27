using System;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetAdditionalQuestion;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications;

public class GetAdditionalQuestionApiResponse
{
    public string Question { get; set; }
    public string Answer { get; set; }
    public Guid Id { get; set; }

    public static implicit operator GetAdditionalQuestionApiResponse(GetAdditionalQuestionQueryResult source)
    {
        return new GetAdditionalQuestionApiResponse
        {
            Question = source.Question,
            Answer = source.Answer,
            Id = source.Id
        };
    }
}
