using MediatR;

namespace SFA.DAS.EmployerAan.Application.Employer.Queries.GetEmployerMemberSummary;

public record GetEmployerMemberSummaryQuery(int employerAccontId) : IRequest<GetEmployerMemberSummaryQueryResult?>;
