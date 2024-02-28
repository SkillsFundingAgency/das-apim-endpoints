using System;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.UpdateAdditionalQuestion;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications;

public class PostAdditionalQuestionApiResponse
{
    public Guid Id { get; set; }

    public static implicit operator PostAdditionalQuestionApiResponse(UpdateAdditionalQuestionQueryResult source)
    {
        return new PostAdditionalQuestionApiResponse
        {
            Id = source.Id
        };
    }
}
