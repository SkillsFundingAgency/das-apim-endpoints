using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetAddQualification;

public class GetAddQualificationQuery : IRequest<GetAddQualificationQueryResult>
{
    public Guid QualificationReferenceTypeId { get; set; }
    public Guid ApplicationId { get; set; }
    public Guid CandidateId { get; set; }
    public Guid? Id { get; set; }
}