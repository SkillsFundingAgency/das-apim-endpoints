using System;
using MediatR;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.VacanciesManage.InnerApi.Requests;

namespace SFA.DAS.VacanciesManage.Application.Recruit.Commands.CreateVacancy;

public class CreateVacancyCommand : IRequest<CreateVacancyCommandResponse>
{
    public PostVacancyV2RequestData PostVacancyRequest { get; set; }
    public Guid Id { get ; set ; }
    public AccountIdentifier AccountIdentifier { get ; set ; }
    public bool IsSandbox { get; set; }
}