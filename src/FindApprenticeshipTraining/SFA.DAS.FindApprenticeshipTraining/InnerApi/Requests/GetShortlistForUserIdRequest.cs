using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

//MFCMFC new
public class GetShortlistForUserIdRequest : IGetAllApiRequest
{
    private readonly Guid _shortlistUserId;
    public GetShortlistForUserIdRequest(Guid shortlistUserId)
    {
        _shortlistUserId = shortlistUserId;
    }

    //public Guid ShortlistUserId => _shortlistUserId;

    //public string GetUrl => $"api/shortlist/{_shortlistUserId}";
    public string GetAllUrl => $"api/shortlist/{_shortlistUserId}";
}