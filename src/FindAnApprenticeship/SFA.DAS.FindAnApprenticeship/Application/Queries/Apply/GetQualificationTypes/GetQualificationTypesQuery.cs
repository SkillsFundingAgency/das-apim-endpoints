using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetQualificationTypes;

public class GetQualificationTypesQuery : IRequest<GetQualificationTypesQueryResult>
{
    public Guid CandidateId { get; set; }
    public Guid ApplicationId { get; set; }
}