using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.GetSettings
{
    public class GetSettingsQuery : IRequest<GetSettingsQueryResult>
    {
        public Guid CandidateId { get; set; }
        public string Email { get; set; }
    }
}
