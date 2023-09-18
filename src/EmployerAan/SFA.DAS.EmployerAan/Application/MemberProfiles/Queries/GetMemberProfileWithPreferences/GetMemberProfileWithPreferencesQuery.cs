using MediatR;

namespace SFA.DAS.EmployerAan.Application.MemberProfiles.Queries.GetMemberProfileWithPreferences;
public record GetMemberProfileWithPreferencesQuery(Guid MemberId, Guid RequestedByMemberId, bool IsPublicView) : IRequest<GetMemberProfileWithPreferencesQueryResult>;
