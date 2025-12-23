using MediatR;

namespace SFA.DAS.EmployerAccounts.Application.Queries.GetEmployerVacancies;

public class GetEmployerVacanciesQuery : IRequest<GetEmployerVacanciesResponse>
{
    public long AccountId { get; set; }
}