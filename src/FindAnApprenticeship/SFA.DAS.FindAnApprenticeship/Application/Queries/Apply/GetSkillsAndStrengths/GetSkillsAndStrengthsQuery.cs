using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetSkillsAndStrengths;
public class GetSkillsAndStrengthsQuery : IRequest<GetSkillsAndStrengthsQueryResult>
{
    public Guid CandidateId { get; set; }
    public Guid ApplicationId { get; set; }
}
