using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Users.MigrateData
{
    public record MigrateDataQuery : IRequest<MigrateDataQueryResult>
    {
        public required string EmailAddress { get; set; }
        public required Guid CandidateId { get; set; }
    }
}
