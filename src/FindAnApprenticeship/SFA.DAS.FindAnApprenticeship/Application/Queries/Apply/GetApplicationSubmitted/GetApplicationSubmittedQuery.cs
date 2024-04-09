using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetApplicationSubmitted;
public class GetApplicationSubmittedQuery : IRequest<GetApplicationSubmittedQueryResult>
{
    public Guid ApplicationId { get; set; }
    public Guid CandidateId { get; set; }
}
