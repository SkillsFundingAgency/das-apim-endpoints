namespace SFA.DAS.EmployerFinanceJobs.Api.Models.Requests;

public sealed record PostImportLearnersByAccountIdRequest
{
    public required long AccountId { get; set; }
}