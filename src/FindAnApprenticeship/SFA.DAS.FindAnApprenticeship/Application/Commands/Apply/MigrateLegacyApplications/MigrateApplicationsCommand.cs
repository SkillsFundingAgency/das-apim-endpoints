using MediatR;
using System;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.MigrateLegacyApplications
{
    public record MigrateApplicationsCommand : IRequest<Unit>
    {
        public required Guid CandidateId { get; set; }
        public required string EmailAddress { get; set; }
    }
}
