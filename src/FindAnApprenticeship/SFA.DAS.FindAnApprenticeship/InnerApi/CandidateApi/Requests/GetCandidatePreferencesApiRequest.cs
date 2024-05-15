using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
public class GetCandidatePreferencesApiRequest : IGetApiRequest
{
    private readonly Guid _candidateId;

    public GetCandidatePreferencesApiRequest(Guid candidateId)
    {
        _candidateId = candidateId;
    }

    public string GetUrl => $"api/candidates/{_candidateId}/NotificationPreferences/";
}
