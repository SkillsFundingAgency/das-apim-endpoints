using System;
using System.Collections.Generic;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests;

public class GetInvalidIlrChangesRequest(long apprenticeshipId, long providerId, string path = "invalid-ilr-changes") : IGetApiRequest
{
    public const string InvalidIlrChangesPath = "invalid-ilr-changes";
    public const string DeclinedChangesPath = "declined-changes";

    public long ApprenticeshipId { get; } = apprenticeshipId;
    public long ProviderId { get; } = providerId;
    public string Path { get; } = path;
    public string GetUrl => $"api/apprenticeships/{ApprenticeshipId}/{Path}?providerId={ProviderId}";
}

public class PostInvalidIlrChangesRequest(long apprenticeshipId, PostInvalidIlrChangesRequestData data, string path = GetInvalidIlrChangesRequest.InvalidIlrChangesPath) : IPostApiRequest
{
    public string PostUrl => $"api/apprenticeships/{apprenticeshipId}/{path}";
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
