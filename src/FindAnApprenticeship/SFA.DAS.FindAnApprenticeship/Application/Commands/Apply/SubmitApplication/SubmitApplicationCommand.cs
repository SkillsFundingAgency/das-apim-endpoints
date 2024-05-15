using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.SubmitApplication;

public class SubmitApplicationCommand : IRequest<bool>
{
    public Guid ApplicationId { get; set; }
    public Guid CandidateId { get; set; }
}