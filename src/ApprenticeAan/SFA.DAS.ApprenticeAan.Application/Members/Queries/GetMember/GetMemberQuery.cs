using MediatR;

namespace SFA.DAS.ApprenticeAan.Application.Members.Queries.GetMember;

public record GetMemberQuery(Guid MemberId) : IRequest<GetMemberQueryResult>;
