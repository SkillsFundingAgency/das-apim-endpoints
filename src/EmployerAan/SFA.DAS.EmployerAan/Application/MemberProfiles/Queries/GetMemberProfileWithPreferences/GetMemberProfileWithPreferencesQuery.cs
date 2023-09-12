using MediatR;

namespace SFA.DAS.EmployerAan.Application.MemberProfiles.Queries.GetMemberProfileWithPreferences;
public record GetMemberProfileWithPreferencesQuery(Guid MemberId, bool IsPublicView) : IRequest<GetMemberProfileWithPreferencesQueryResult>
{
}
