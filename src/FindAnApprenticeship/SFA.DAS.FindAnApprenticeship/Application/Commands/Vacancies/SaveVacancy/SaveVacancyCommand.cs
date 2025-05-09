using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Vacancies.SaveVacancy
{
    public record SaveVacancyCommand(Guid CandidateId, string VacancyId) : IRequest<SaveVacancyCommandResult>;
}