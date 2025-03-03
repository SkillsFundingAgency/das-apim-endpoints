using System;
using MediatR;

namespace SFA.DAS.Recruit.Application.Candidates.Commands.CandidateApplicationStatus;

public class CandidateApplicationStatusCommand : IRequest<Unit>
{
    public Guid ApplicationId { get; set; }
    public Guid CandidateId { get; set; }
    public string Feedback { get; set; }
    public string Outcome { get; set; }
    public long VacancyReference { get; set; }
    
    public string VacancyTitle { get; set; }
    public string VacancyCity { get; set; }
    public string VacancyPostcode { get; set; }
    public string VacancyEmployerName { get; set; }
}