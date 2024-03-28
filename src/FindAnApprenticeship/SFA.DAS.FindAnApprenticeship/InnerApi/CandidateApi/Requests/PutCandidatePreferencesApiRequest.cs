using System;
using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
public class PutCandidatePreferencesApiRequest : IPutApiRequest
{
    private readonly Guid _candidateId;
    public object Data { get; set; }

    public PutCandidatePreferencesApiRequest(Guid candidateId, PutCandidatePreferencesRequestData data)
    {
        _candidateId = candidateId;
        Data = data;
    }

    public string PutUrl => $"api/candidates/{_candidateId}/NotificationPreferences/";
}

public class PutCandidatePreferencesRequestData
{
    public List<CandidatePreference> CandidatePreferences { get; set; }

    public class CandidatePreference
    {
        public Guid PreferenceId { get; set; }
        public string ContactMethod { get; set; }
        public bool Status { get; set; }
    }
}
