using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.EmployerFinanceJobs.InnerApi.Requests;

public sealed record PutJobRequest(Guid Id, PutJobRequestData Payload) : IPutApiRequest
{
    public string PutUrl => $"api/jobs/{Id}";
    public object Data { get; set; } = Payload;
}

public sealed record PutJobRequestData
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
