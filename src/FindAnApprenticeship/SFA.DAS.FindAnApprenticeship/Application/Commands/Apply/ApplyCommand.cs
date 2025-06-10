using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply;

public class ApplyCommand : IRequest<ApplyCommandResponse>
{
    public string VacancyReference { get; set; }
    public Guid CandidateId { get; set; }
}