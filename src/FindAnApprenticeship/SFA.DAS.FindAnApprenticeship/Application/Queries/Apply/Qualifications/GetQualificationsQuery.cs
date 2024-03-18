using MediatR;
using System;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.Qualifications
{
    public class GetQualificationsQuery : IRequest<GetQualificationsQueryResult>
    {
        public Guid CandidateId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
