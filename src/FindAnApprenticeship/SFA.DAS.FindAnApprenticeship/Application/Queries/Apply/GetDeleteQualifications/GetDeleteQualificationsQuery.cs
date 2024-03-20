using MediatR;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.Qualifications;
using System;
using SFA.DAS.FindAnApprenticeship.Domain;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetDeleteQualifications;

public class GetDeleteQualificationsQuery : IRequest<GetDeleteQualificationsQueryResult>
{
    public Guid CandidateId { get; set; }
    public Guid ApplicationId { get; set; }
    public Guid QualificationReference { get; set; }
}