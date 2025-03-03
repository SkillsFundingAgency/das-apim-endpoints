using MediatR;
using System;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Applications.GetApplication
{
    public record GetApplicationViewQuery : IRequest<GetApplicationViewQueryResult>
    {
        public Guid ApplicationId { get; set; }
        public Guid CandidateId { get; set; }
    }
}