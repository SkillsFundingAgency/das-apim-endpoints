using System;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

public class GetShortlistForUserIdRequest : IGetAllApiRequest
{
    private readonly Guid _shortlistUserId;
    public GetShortlistForUserIdRequest(Guid shortlistUserId)
    {
        _shortlistUserId = shortlistUserId;
    }
    public Guid ShortlistUserId => _shortlistUserId;

    public string GetAllUrl => $"api/shortlist/{_shortlistUserId}";
}