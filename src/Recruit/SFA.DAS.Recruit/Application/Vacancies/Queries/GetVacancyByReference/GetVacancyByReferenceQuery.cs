using MediatR;
using SFA.DAS.Recruit.Application.Vacancies.Queries.GetVacancyById;

namespace SFA.DAS.Recruit.Application.Vacancies.Queries.GetVacancyByReference;

public record GetVacancyByReferenceQuery(long VacancyReference) : IRequest<GetVacancyByReferenceQueryResult>;

