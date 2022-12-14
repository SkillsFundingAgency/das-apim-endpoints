using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

public class GetShortlistForUserIdRequest : IGetAllApiRequest
{
    private readonly Guid _shortlistUserId;
    public GetShortlistForUserIdRequest(Guid shortlistUserId)
    {
        _shortlistUserId = shortlistUserId;
    }

    public string GetAllUrl => $"api/shortlist/{_shortlistUserId}";
}