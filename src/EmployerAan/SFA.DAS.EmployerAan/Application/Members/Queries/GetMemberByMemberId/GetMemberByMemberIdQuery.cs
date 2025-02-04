using MediatR;

namespace SFA.DAS.EmployerAan.Application.Members.Queries.GetMemberByMemberId;

public class GetMemberByIdQuery : IRequest<GetMemberByIdQueryResult>
{
    public Guid MemberId { get; set; }
}