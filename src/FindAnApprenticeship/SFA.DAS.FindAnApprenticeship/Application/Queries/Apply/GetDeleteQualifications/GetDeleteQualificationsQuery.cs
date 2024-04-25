using MediatR;
using System;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetDeleteQualifications;

public class GetDeleteQualificationsQuery : IRequest<GetDeleteQualificationsQueryResult>
{
    public Guid CandidateId { get; set; }
    public Guid ApplicationId { get; set; }
    public Guid QualificationReference { get; set; }
}