using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Users.CheckAnswers
{
    public class UpdateCheckAnswersCommand : IRequest
    {
        public Guid CandidateId { get; set; }
    }
}
