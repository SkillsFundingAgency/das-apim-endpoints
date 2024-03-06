using System;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetAdditionalQuestion;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications;

public class GetAdditionalQuestionApiResponse
{
    public Guid Id { get; set; }
    public string QuestionText { get; set; }
    public string Answer { get; set; }
    public Guid ApplicationId { get; set; }

    public static implicit operator GetAdditionalQuestionApiResponse(GetAdditionalQuestionQueryResult source)
    {
        return new GetAdditionalQuestionApiResponse
        {
            QuestionText = source.QuestionText,
            Answer = source.Answer,
            Id = source.Id,
            ApplicationId = source.ApplicationId
        };
    }
}
