using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.CreateEqualityQuestionsCommand;
using System;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications
{
    public class PostEqualityQuestionsApiResponse
    {
        public Guid Id { get; set; }

        public static implicit operator PostEqualityQuestionsApiResponse(UpsertAboutYouEqualityQuestionsCommandResult source)
        {
            return new PostEqualityQuestionsApiResponse
            {
                Id = source.Id
            };
        }
    }
}
