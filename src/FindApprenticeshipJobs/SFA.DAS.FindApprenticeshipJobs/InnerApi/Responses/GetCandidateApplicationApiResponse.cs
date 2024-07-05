using Newtonsoft.Json;

namespace SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;

public class GetCandidateApplicationApiResponse
{
    [JsonProperty("candidates")]
    public List<CandidateElement> Candidates { get; set; }
}

public class CandidateElement
{
    [JsonProperty("applicationId")]
    public Guid ApplicationId { get; set; }

    [JsonProperty("candidate")]
    public CandidateCandidate Candidate { get; set; }

    [JsonProperty("applicationCreatedDate")]
    public DateTime ApplicationCreatedDate { get; set; }
}

public class CandidateCandidate
{

    [JsonProperty("lastName")]
    public string LastName { get; set; }

    [JsonProperty("firstName")]
    public string FirstName { get; set; }

    [JsonProperty("email")]
    public string Email { get; set; }

    [JsonProperty("id")]
    public Guid Id { get; set; }
}
