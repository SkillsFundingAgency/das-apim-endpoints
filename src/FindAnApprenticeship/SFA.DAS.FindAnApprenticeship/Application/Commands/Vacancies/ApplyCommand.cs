using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Vacancies
{
    public class ApplyCommand : IRequest<ApplyCommandResponse>
    {
        public string VacancyReference { get; set; }
        public Guid CandidateId { get; set; }
    }
}
