namespace SFA.DAS.EmployerFinanceJobs.InnerApi.Responses;

public sealed record GetAllLearnersApiResponse
{
    public int BatchNumber { get; set; }
    public int BatchSize { get; set; }
    public int TotalNumberOfBatches { get; set; }
    public List<Learner> Learners { get; set; } = [];
}