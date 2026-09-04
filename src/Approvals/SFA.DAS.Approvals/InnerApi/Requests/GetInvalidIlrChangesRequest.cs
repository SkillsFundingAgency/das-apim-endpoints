using System;
using System.Collections.Generic;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests;

public class GetInvalidIlrChangesRequest(long apprenticeshipId, long providerId) : IGetApiRequest
{
    public long ApprenticeshipId { get; } = apprenticeshipId;
    public long ProviderId { get; } = providerId;
    public string GetUrl => $"api/apprenticeships/{ApprenticeshipId}/invalid-ilr-changes?providerId={ProviderId}";
}

public class PostInvalidIlrChangesRequest(long apprenticeshipId, PostInvalidIlrChangesRequestData data) : IPostApiRequest
{
    public string PostUrl => $"api/apprenticeships/{apprenticeshipId}/invalid-ilr-changes";
    public object Data { get; set; } = data;
}

public class PostInvalidIlrChangesRequestData
{
    public long ProviderId { get; set; }
    public UserInfo UserInfo { get; set; }
    public List<InvalidIlrChangeAcknowledgement> Acknowledgements { get; set; } = [];
}

public class InvalidIlrChangeAcknowledgement
{
    public Guid ApprovalRequestId { get; set; }
    public bool? DeleteAlert { get; set; }
}
