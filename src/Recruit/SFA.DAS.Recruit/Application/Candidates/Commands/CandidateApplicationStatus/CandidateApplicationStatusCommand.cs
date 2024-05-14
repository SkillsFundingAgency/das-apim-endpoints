using System;
using MediatR;

namespace SFA.DAS.Recruit.Application.Candidates.Commands.CandidateApplicationStatus;

public class CandidateApplicationStatusCommand : IRequest<Unit>
{
    public Guid ApplicationId { get; set; }
    public Guid CandidateId { get; set; }
    public string Feedback { get; set; }
    public string Outcome { get; set; }
}