using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
public class PutCandidateApiRequest : IPutApiRequest
{
    private readonly Guid _id;
    public object Data { get; set; }

    public PutCandidateApiRequest(Guid id, PutCandidateApiRequestData data)
    {
        _id = id;
        Data = data;
    }

    public string PutUrl => $"/api/candidates/{_id}";

    public class PutCandidateApiRequestData
    {
        public string Email { get; set; }
        public string GovUkIdentifier { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
