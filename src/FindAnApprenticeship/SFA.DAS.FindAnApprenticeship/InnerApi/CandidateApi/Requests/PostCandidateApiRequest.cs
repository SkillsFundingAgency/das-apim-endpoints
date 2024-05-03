using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
public class PostCandidateApiRequest(string govIdentifier, PostCandidateApiRequestData data) : IPostApiRequest
{
    public object Data { get; set; } = data;

    public string PostUrl => $"/api/candidates/{govIdentifier}";
}

public class PostCandidateApiRequestData
{
    public string Email { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}