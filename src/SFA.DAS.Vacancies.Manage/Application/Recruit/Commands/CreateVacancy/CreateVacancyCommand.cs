using System;
using MediatR;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Vacancies.Manage.InnerApi.Requests;

namespace SFA.DAS.Vacancies.Manage.Application.Recruit.Commands.CreateVacancy
{
    public class CreateVacancyCommand : IRequest<CreateVacancyCommandResponse>
    {
        public PostVacancyRequestData PostVacancyRequestData { get; set; }
        public Guid Id { get ; set ; }
        public AccountIdentifier AccountIdentifier { get ; set ; }
    }
}