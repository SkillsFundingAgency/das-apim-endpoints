using MediatR;
using System;

namespace SFA.DAS.ApprenticeApp.Application.Commands.Cmad
{
    public class CreateApprenticeshipFromRegistrationCommand : IRequest<Unit>
    {
        public Guid RegistrationId { get; set; }
        public Guid ApprenticeId { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
