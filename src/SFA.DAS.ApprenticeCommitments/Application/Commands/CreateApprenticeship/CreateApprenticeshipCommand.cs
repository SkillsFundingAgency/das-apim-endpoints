using MediatR;
using System;

namespace SFA.DAS.ApprenticeCommitments.Apprenticeship.Commands
{
    public class CreateApprenticeshipCommand : IRequest
    {
        public Guid RegistrationId { get; }
        public long ApprenticeshipId { get; }
        public string Email { get; }

        public CreateApprenticeshipCommand(
            Guid registrationId,
            long apprenticeshipId,
            string email)
        {
            RegistrationId = registrationId;
            ApprenticeshipId = apprenticeshipId;
            Email = email;
        }
    }
}