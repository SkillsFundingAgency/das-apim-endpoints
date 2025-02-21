namespace SFA.DAS.Earnings.Api.LearnerData;

public class PagedResult {
    public List<Apprenticeship> Apprenticeships { get; set; } = [];
    public long TotalRecords { get; set; }   
    public int Page { get; set; }   
    public int PageSize { get; set; }   
    public int TotalPages { get; set; }
}