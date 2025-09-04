using System;
using MediatR;

namespace SFA.DAS.Recruit.Application.Vacancies.Queries.GetVacancyById;

public record GetVacancyByIdQuery(Guid Id) : IRequest<GetVacancyByIdQueryResult>;