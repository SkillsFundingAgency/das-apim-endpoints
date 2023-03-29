using MediatR;

namespace SFA.DAS.EmployerAan.Application.Employer.Queries.GetEmployerMember;
public record GetEmployerMemberQuery(Guid UserRef) : IRequest<GetEmployerMemberQueryResult>;
