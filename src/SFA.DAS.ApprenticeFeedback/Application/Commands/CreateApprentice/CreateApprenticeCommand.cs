using MediatR;
using System;

namespace SFA.DAS.ApprenticeFeedback.Application.Commands.CreateApprentice
{
    public class CreateApprenticeCommand : IRequest
    {
        public Guid ApprenticeId { get; set; }
        public Guid ApprenticeshipId { get; set; }
        public string Status { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
    }
}
