﻿using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Vacancies.DeleteSavedVacancy
{
    public record DeleteSavedVacancyCommand(Guid CandidateId, string VacancyReference) : IRequest<Unit>;
}