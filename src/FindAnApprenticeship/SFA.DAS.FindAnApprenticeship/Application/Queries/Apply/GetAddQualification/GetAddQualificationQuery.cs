using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetAddQualification;

public class GetAddQualificationQuery : IRequest<GetAddQualificationQueryResult>
{
    public Guid QualificationReferenceTypeId { get; set; }
}