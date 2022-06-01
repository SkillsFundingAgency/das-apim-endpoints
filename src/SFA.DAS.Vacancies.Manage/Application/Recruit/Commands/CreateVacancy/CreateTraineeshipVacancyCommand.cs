using System;
using MediatR;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Vacancies.Manage.InnerApi.Requests;

namespace SFA.DAS.Vacancies.Manage.Application.Recruit.Commands.CreateVacancy
{
    public class CreateTraineeshipVacancyCommand : IRequest<CreateTraineeshipVacancyCommandResponse>
    {
        public PostTraineeshipVacancyRequestData PostVacancyRequestData { get; set; }
        public Guid Id { get; set; }
        public AccountIdentifier AccountIdentifier { get; set; }
        public bool IsSandbox { get; set; }
    }
}