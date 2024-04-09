using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
public class PutCandidateApiRequest(Guid candidateId, PutCandidateApiRequestData data) : IPutApiRequest
{
    public object Data { get; set; } = data;

    public string PutUrl => $"/api/candidates/{candidateId}";
}
public class PutCandidateApiRequestData
{
    public string Email { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public short Status { get; set; }
}