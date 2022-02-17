using MediatR;
using System;

namespace SFA.DAS.ApprenticeFeedback.Application.Commands.CreateApprentice
{
    public class CreateApprenticeCommand : IRequest
    {
        public Guid ApprenticeId { get; set; }
        public long ApprenticeshipId { get; set; }
    }
}
