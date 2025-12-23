using MediatR;

namespace SFA.DAS.EmployerAccounts.Application.Queries.GetEmployerVacancies;

public class GetEmployerVacanciesQuery : IRequest<GetEmployerVacanciesQueryResponse>
{
    public long AccountId { get; set; }
}