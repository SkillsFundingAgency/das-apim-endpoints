using MediatR;
using System;


namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.WorkHistory.DeleteJob
{
    public class GetDeleteJobQuery : IRequest<GetDeleteJobQueryResult>
    {
        public Guid CandidateId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid JobId { get; set; }
    }
}
