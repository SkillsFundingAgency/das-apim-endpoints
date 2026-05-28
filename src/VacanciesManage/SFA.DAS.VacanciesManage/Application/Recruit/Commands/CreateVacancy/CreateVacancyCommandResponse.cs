using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.VacanciesManage.Application.Recruit.Commands.CreateVacancy;

public sealed record CreateVacancyCommandResponse([Required] string VacancyReference);