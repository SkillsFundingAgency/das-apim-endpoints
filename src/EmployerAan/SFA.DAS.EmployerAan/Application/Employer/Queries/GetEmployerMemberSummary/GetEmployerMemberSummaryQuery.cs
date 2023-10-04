using MediatR;

namespace SFA.DAS.EmployerAan.Application.Employer.Queries.GetEmployerMemberSummary;

public record GetEmployerMemberSummaryQuery(long EmployerAccountId) : IRequest<GetEmployerMemberSummaryQueryResult>;
