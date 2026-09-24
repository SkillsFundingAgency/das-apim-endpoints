using System.Collections.Generic;
using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;

namespace SFA.DAS.Approvals.Application.InvalidIlrChanges.Commands;

public class AcknowledgeInvalidIlrChangesCommand : IRequest
{
    public long ProviderId { get; set; }
    public long ApprenticeshipId { get; set; }
    public UserInfo UserInfo { get; set; }
    public string InnerPath { get; set; } = GetInvalidIlrChangesRequest.InvalidIlrChangesPath;
    public List<InvalidIlrChangeAcknowledgement> Acknowledgements { get; set; } = [];
}
