using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Vacancies
{
    public class ApplyCommand : IRequest<ApplyCommandResponse>
    {
        public string VacancyReference { get; set; }
        public Guid CandidateId { get; set; }
    }
}
