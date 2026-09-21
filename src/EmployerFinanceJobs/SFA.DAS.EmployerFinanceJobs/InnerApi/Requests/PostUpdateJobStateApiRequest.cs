using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.EmployerFinanceJobs.InnerApi.Requests;

public sealed record PutUpdateJobStateApiRequest(Guid Id, PutImportJobStateRequestData Payload) : IPutApiRequest
{
    public string PutUrl => $"api/jobs/{Id}";
    public object? Data { get; set; } = Payload;
}

public sealed record PutImportJobStateRequestData
{
    public DateTime LastSuccessfulImportDate { get; set; }
    public DateTime LastAttemptedDate { get; set; }
    public bool LastAttemptSuccessful { get; set; }
    public int TotalRecordsLastRun { get; set; }
    public int FailedRecordsLastRun { get; set; }
}