using MediatR;

namespace SFA.DAS.AdminAan.Application.Members.GetMemberProfile;

public record GetMemberProfileQuery(Guid MemberId, Guid RequestedByMemberId) : IRequest<GetMemberProfileQueryResult>;
