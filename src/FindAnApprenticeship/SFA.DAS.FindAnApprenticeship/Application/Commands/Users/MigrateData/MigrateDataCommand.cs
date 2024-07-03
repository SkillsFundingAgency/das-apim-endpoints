using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Users.MigrateData
{
    public record MigrateDataCommand : IRequest<Unit>
    {
        public required Guid CandidateId { get; set; }
        public required string EmailAddress { get; set; }
    }
}