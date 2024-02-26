using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetJob
{
    public class GetJobQuery : IRequest<GetJobQueryResult>
    {
        public Guid CandidateId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid JobId { get; set; }
    }
}
