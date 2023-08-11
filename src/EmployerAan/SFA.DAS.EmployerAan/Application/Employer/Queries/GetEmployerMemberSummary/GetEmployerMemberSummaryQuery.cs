using MediatR;

namespace SFA.DAS.EmployerAan.Application.Employer.Queries.GetEmployerMemberSummary;

public record GetEmployerMemberSummaryQuery(int EmployerAccountId) : IRequest<GetEmployerMemberSummaryQueryResult?>;
