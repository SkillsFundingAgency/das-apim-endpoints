using System;
using MediatR;

namespace SFA.DAS.ApprenticeCommitments.Application.Commands.VerifyIdentityRegistration
{
    public class VerifyIdentityRegistrationCommand : IRequest
    {
        public Guid ApprenticeId { get; set; }
        public Guid UserIdentityId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}