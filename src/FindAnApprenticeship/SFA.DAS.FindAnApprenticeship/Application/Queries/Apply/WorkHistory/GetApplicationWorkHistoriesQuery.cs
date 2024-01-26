using MediatR;
using System;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.WorkHistory
{
    public class GetApplicationWorkHistoriesQuery : IRequest<GetApplicationWorkHistoriesQueryResult>
    {
        public Guid CandidateId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
