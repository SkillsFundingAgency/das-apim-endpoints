namespace SFA.DAS.EmployerAan.Application.Members.Queries.GetMembers;
public class GetMembersQueryResult
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
    public List<MemberSummary> Members { get; set; } = new List<MemberSummary>();
}
