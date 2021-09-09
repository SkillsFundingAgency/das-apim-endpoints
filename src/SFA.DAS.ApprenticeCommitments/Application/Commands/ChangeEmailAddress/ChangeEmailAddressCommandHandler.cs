using System;

namespace SFA.DAS.ApprenticeCommitments.Application.Commands.ChangeEmailAddress
{
    public class ChangeEmailAddressCommand
    {
        public ChangeEmailAddressCommand(Guid apprenticeId, string email)
            => (ApprenticeId, Email) = (apprenticeId, email);

        public Guid ApprenticeId { get; }
        public string Email { get; }
    }
}