using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.WithdrawApplication;

public class WithdrawApplicationCommand : IRequest<bool>
{
    public Guid CandidateId { get; set; }
    public Guid ApplicationId { get; set; }
}