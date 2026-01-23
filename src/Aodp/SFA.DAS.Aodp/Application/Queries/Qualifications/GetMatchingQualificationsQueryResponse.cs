namespace SFA.DAS.Aodp.Application.Queries.Qualifications;

public class GetMatchingQualificationsQueryResponse
{
    public int TotalRecords { get; set; } = 0;
    public int? Skip { get; set; } = 0;
    public int? Take { get; set; } = 0;
    public List<GetMatchingQualificationsQueryItem> Qualifications { get; set; } = new();
}

public class GetMatchingQualificationsQueryItem
{
    public Guid Id { get; set; }
    public string Qan { get; set; } = null!;
    public string? QualificationName { get; set; }
    public string? Status { get; set; }
}