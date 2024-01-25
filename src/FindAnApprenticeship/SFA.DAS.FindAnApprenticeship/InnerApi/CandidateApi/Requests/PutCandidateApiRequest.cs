using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
public class PutCandidateApiRequest : IPutApiRequest
{
    private readonly string _govIdentifier;
    public object Data { get; set; }

    public PutCandidateApiRequest(string govIdentifier, PutCandidateApiRequestData data)
    {
        _govIdentifier = govIdentifier;
        Data = data;
    }

    public string PutUrl => $"/api/candidates/{_govIdentifier}";

    public class PutCandidateApiRequestData
    {
        public string Email { get; set; }
        public DateTime DateOfBirth => DateTime.UtcNow; // TODO 
    }
}
