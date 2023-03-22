using MediatR;
using System;

namespace SFA.DAS.ApprenticePortal.Application.Commands.ApprenticeUpdate
{
    public class ApprenticeUpdateCommand : IRequest<Unit>
    {
        public Guid ApprenticeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}