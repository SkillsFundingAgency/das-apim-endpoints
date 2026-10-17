using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.EmployerFinanceJobs.InnerApi.Requests;

public sealed record PostJobRequest(long Id, PostJobRequestData Payload) : IPostApiRequest
{
    public string PostUrl => $"api/jobs/{Id}";
    public object Data { get; set; } = Payload;
}

public sealed record PostJobRequestData
{
    public long AccountId { get; set; }
    public string JobName { get; set; } = string.Empty;
    public string JobState { get; set; } = string.Empty;
    public DateTime LastSuccessfulImportDate { get; set; }
    public DateTime LastAttemptedDate { get; set; }
    public bool LastAttemptSuccessful { get; set; }
    public int TotalRecordsLastRun { get; set; }
    public int FailedRecordsLastRun { get; set; }
}
