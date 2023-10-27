using MediatR;

namespace SFA.DAS.ApprenticeAan.Application.Employer.Queries.GetEmployerMemberSummary;

public record GetEmployerMemberSummaryQuery(long EmployerAccountId) : IRequest<GetEmployerMemberSummaryQueryResult>;
