using MediatR;
using SFA.DAS.ApprenticeAan.Application.Common;

namespace SFA.DAS.ApprenticeAan.Application.Members.Queries.GetMembers;
public class GetMembersQuery : IRequest<GetMembersQueryResult>
{
    public List<MemberUserType>? UserType { get; set; }
    public bool? IsRegionalChair { get; set; }
    public List<int>? RegionIds { get; set; }
    public string? Keyword { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}
