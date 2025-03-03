using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.WithdrawApplication;

public class WithdrawApplicationQuery : IRequest<WithdrawApplicationQueryResult>
{
    public Guid ApplicationId { get; set; }
    public Guid CandidateId { get; set; }
}