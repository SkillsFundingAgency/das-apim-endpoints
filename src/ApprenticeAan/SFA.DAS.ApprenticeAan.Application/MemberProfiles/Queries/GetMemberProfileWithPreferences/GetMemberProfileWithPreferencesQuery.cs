using MediatR;

namespace SFA.DAS.ApprenticeAan.Application.MemberProfiles.Queries.GetMemberProfileWithPreferences;
public record GetMemberProfileWithPreferencesQuery(Guid MemberId, bool IsPublicView) : IRequest<GetMemberProfileWithPreferencesQueryResult>;