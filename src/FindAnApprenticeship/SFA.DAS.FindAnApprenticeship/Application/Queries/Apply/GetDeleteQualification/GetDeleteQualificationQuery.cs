using MediatR;
using System;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetDeleteQualification;

public class GetDeleteQualificationQuery : IRequest<GetDeleteQualificationQueryResult>
{
    public Guid CandidateId { get; set; }
    public Guid ApplicationId { get; set; }
    public Guid QualificationReference { get; set; }
    public Guid Id { get; set; }
}