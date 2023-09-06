using MediatR;
using SFA.DAS.EmployerAan.Common;

namespace SFA.DAS.EmployerAan.Application.Members.Queries.GetMembers;
public class GetMembersQuery : IRequest<GetMembersQueryResult>
{
    public Guid RequestedByMemberId { get; set; }
    public List<MemberUserType>? UserType { get; set; }
    public List<MembershipStatusType>? Status { get; set; }
    public bool? IsRegionalChair { get; set; }
    public List<int>? RegionIds { get; set; }
    public string? Keyword { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}
