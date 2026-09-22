using System;
using Esfa.Recruit.Vacancies.Client.Domain.Events;

namespace SFA.DAS.Recruit.Api.Models;

public record PostLiveVacancyUpdatedEventModel(Guid VacancyId, long VacancyReference, LiveUpdateKind UpdateKind);